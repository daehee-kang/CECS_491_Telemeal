using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telemeal.Model;

namespace Telemeal.Pages
{
    /// <summary>
    /// Helper class for item cart
    /// </summary>
    public class CartItems
    {
        public int Qty { get; set; }
        public String Name { get; set; }
        public double Price { get; set; }
    }

    /// <summary>
    /// This page will show menu containing food items. Each food item will be described by the name, price, description, image, and user-defined category
    /// </summary>
    public partial class OrderPage_Page : Page
    {
        //menu will be the storage of all the food objects that are stored in the database and are shown in the menu.
        List<Food> menu = new List<Food>();
        //itemGrids defines the layout of the each menu object 
        List<Grid> itemGrids = new List<Grid>();
        //items will be the copy of the item cart with appropriate data binding format
        List<CartItems> cartItems = new List<CartItems>();
        //order will track the information of the food items which user put in the cart
        Order order = new Order { OrderID = 1, SalesTax = 0.1, Total = 0, Foods = new List<Food>(), IsTakeOut = false, OrderDateTime = new DateTime() };


        public OrderPage_Page()
        {
            InitializeComponent();
            //adding scroll to the menu
            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //list of categories for comparison
            List<Sub_Category> categories = new List<Sub_Category> { Sub_Category.Drink, Sub_Category.Appetizer, Sub_Category.Main, Sub_Category.Dessert };
            //open the connection
            dbConnection conn = new dbConnection();
            //binds items to the cart
            itemCart.ItemsSource = cartItems;

            //get the data from the database
            SQLiteDataReader reader = conn.ViewTable("Food");
            //iterate and initialize food item as adding to the menu
            //after full iterations, menu will be full of food items retrieved from the database
            while (reader.Read())
            {
                //each iteration, single food item will be created and added to the menu
                menu.Add(new Food()
                {
                    Name = ((string)reader["name"]),
                    Price = ((double)reader["price"]),
                    Description = ((string)reader["desc"]),
                    Img = ((string)reader["img"]),
                    SubCtgr = categories[int.Parse(reader["subctgr"].ToString())]
                });
            }

            //update order information; initially, cart will be empty and total and subtotal will be zero
            RefreshSubTotal();

            //create grid item holder for each food item in the menu
            foreach (Food f in menu)
            {
                ChangeMenu(f);
            }

            //close connection
            conn.Close();
        }

        /// <summary>
        /// Button click event for "Check Out" button
        /// This method will pass the order information to the next page which is PaymentOption_Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            //order time is based on the time of check out. Thus, update the OrderDateTime property.
            order.OrderDateTime = DateTime.Now;
            //proceed to the next page which user can choose method of payment
            this.NavigationService.Navigate(new PaymentOption_Page(order));
        }

        /// <summary>
        /// Helper method for updating receipt information
        /// </summary>
        private void RefreshSubTotal()
        {
            //set each field with matching order information in format of decimal with precision 2 
            this.totalTBox.Text = string.Format("{0:F2}", order.Total);
            this.taxTBox.Text = string.Format("{0:F2}", order.Total * order.SalesTax);
            this.subtotalTBox.Text = string.Format("{0:F2}", order.SubTotal());
        }

        /// <summary>
        /// Helper method for filtering the menu according to the category which the user selects
        /// </summary>
        /// <param name="category"></param>
        private void FilterMenuByCategory(Sub_Category category)
        {
            //Clear menu placeholders
            itemGrids.Clear();
            //Clear menu
            Menu.Children.Clear();

            //Filter the menu with matching condition and repopulate menu as iterating
            foreach (Food f in menu.Where(x => x.SubCtgr == category))
                ChangeMenu(f);
        }

        /// <summary>
        /// Button click event for user clicking "Appetizer" button
        /// This method will show all the food items in the Appetizer category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Appetizer_Click(object sender, RoutedEventArgs e)
        {
            FilterMenuByCategory(Sub_Category.Appetizer);  
        }

        /// <summary>
        /// Button click event for user clicking "Main" button
        /// This method will show all the food items in the Main category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Click(object sender, RoutedEventArgs e)
        {
            FilterMenuByCategory(Sub_Category.Main);
        }

        /// <summary>
        /// Button click event for user clicking "Dessert" button
        /// This method will show all the food items in the Dessert category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dessert_Click(object sender, RoutedEventArgs e)
        {
            FilterMenuByCategory(Sub_Category.Dessert);
        }

        /// <summary>
        /// Button click event for user clicking "Drinks" button
        /// This method will show all the food items in the Drink category 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Drinks_Click(object sender, RoutedEventArgs e)
        {
            FilterMenuByCategory(Sub_Category.Drink);
        }

        /// <summary>
        /// Button click event for user clicking "All" button
        /// This method will show all the food items in the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void All_Click(object sender, RoutedEventArgs e)
        {
            //Clear food item placeholders and menu
            itemGrids.Clear();
            Menu.Children.Clear();
            //repopulate food items in menu
            foreach (Food f in menu)
                ChangeMenu(f);
        }

        /// <summary>
        /// Button click event for user clicking "Clear All" button
        /// This method will empty the item carts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            cartItems.Clear();
            order.Foods.Clear();
            itemCart.Items.Refresh();
            order.Total = 0;
            RefreshSubTotal();
        }

        /// <summary>
        /// Button double click event for item in the cart
        /// This method will remove item existing in the cart and update receipt information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //obtain selected item information from the cart
            CartItems selected = itemCart.SelectedItem as CartItems;
            //item selection validator
            if (itemCart.SelectedItem != null)
            {
                //reduce quantity of selected item
                cartItems.Where(x => x.Name == selected.Name).First().Qty--;
                //remove first appearance of same named item
                order.Foods.Remove(order.Foods.Where(x => x.Name == selected.Name).First());
                //if quantity is 0, remove from the cart
                cartItems.RemoveAll(x => x.Qty == 0);
                //deduct the total amount by the price of removing item
                order.Total -= selected.Price;
            }

            //update the cart
            itemCart.Items.Refresh();
            //update the receipt information
            RefreshSubTotal();
        }

        /// <summary>
        /// Helper method for creating a placeholder for the food item and adding it to the menu
        /// </summary>
        /// <param name="f"></param>
        private void ChangeMenu(Food f)
        {
            //create the placeholder for a single food item
            Grid grid = new Grid();
            //tag will be used for matcher
            grid.Tag = f.Name;
            grid.Height = 100;
            //add button click event for clicking the placeholder
            grid.MouseDown += new MouseButtonEventHandler(FoodClick);

            //column 1 will be a placeholder for the image
            //column 2 will be a placeholder for the information of an item in text format
            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol1.Width = new GridLength(100, GridUnitType.Pixel);
            gridCol2.Width = new GridLength(525, GridUnitType.Pixel);
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);

            //create image object get the path from the food object
            Image image = new Image
            {
                Source = new BitmapImage(new Uri(f.Img, UriKind.RelativeOrAbsolute))
            };
            //add image to the first column of the placeholder
            image.Stretch = Stretch.Fill;
            Grid.SetColumn(image, 0);
            grid.Children.Add(image);

            //create dock panel for custom layout of the item information
            DockPanel dp = new DockPanel();

            //create text block contains price information
            TextBlock price = new TextBlock
            {
                Text = f.Price.ToString(),
                Width = 30
            };
            //dock to the right
            DockPanel.SetDock(price, Dock.Right);
            //add to the dock panel
            dp.Children.Add(price);

            //create text block contains name information
            TextBlock name = new TextBlock
            {
                Text = f.Name
            };
            name.TextAlignment = TextAlignment.Left;
            //dock to the top
            DockPanel.SetDock(name, Dock.Top);
            //add to the dock panel
            dp.Children.Add(name);

            //create text block contains category information
            TextBlock category = new TextBlock
            {
                Text = f.SubCtgr.ToString()
            };
            category.TextAlignment = TextAlignment.Left;
            //dock to the top (below name)
            DockPanel.SetDock(category, Dock.Top);
            //add to the dock panel
            dp.Children.Add(category);

            //create text block contains description information
            TextBlock desc = new TextBlock
            {
                Text = f.Description
            };
            desc.TextAlignment = TextAlignment.Left;
            //add to the dock panel (rest of the space is given for description)
            dp.Children.Add(desc);

            //add dock panel to the second column of placeholder
            Grid.SetColumn(dp, 1);
            grid.Children.Add(dp);

            //add item placeholder (filled) to the menu
            itemGrids.Add(grid);
            Menu.Children.Add(grid);
        }

        /// <summary>
        /// Button click event for user selecting the food item
        /// This method will add food item in the cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoodClick(object sender, MouseButtonEventArgs e)
        {
            //get the grid information which user clicked on.
            Grid foodGrid = sender as Grid;
            //get the food information which matches with grid user clicked.
            Food f = menu.Where(x => x.Name == foodGrid.Tag.ToString()).First();
            //create food item appropriate for adding to the cart
            CartItems i = new CartItems { Qty = 1, Name = f.Name, Price = f.Price };

            //add food item into the order
            order.Foods.Add(f);
            //if cart already has item with same information, increase the quantity
            if (cartItems.Select(x => x.Name).Contains(i.Name))
            {
                cartItems.Where(x => x.Name == f.Name).First().Qty++;
            }
            //otherwise, newly add item to the cart
            else
                cartItems.Add(i);

            //update the cart
            itemCart.Items.Refresh();

            //update the receipt information
            order.Total += f.Price;
            RefreshSubTotal();
        }

        public Visual GetDescendantByType(Visual element, Type type)
        {
            if (element == null) return null;
            if (element.GetType() == type) return element;
            Visual foundElement = null;
            if (element is FrameworkElement)
            {
                (element as FrameworkElement).ApplyTemplate();
            }
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }
    }
}
