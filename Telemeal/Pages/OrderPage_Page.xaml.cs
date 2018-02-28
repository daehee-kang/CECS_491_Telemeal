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
        //foods will be the storage of all the food objects that are stored in the database and are shown in the menu.
        List<Food> foods = new List<Food>();
        //cart will be the storage for the food items that the user put in the item cart
        List<Food> cart = new List<Food>();
        //grids defines the layout of the each menu object 
        List<Grid> grids = new List<Grid>();
        //items will be the copy of the item cart with appropriate data binding format
        List<CartItems> items = new List<CartItems>();
        double tax = 0.1;
        double total = 0;

        public OrderPage_Page()
        {
            InitializeComponent();
            List<Food> categories = new List<Food>();
            dbConnection conn = new dbConnection();
            itemCart.ItemsSource = items;

            categories.Add(new Food() { SubCtgr = Sub_Category.Drink });
            categories.Add(new Food() { SubCtgr = Sub_Category.Appetizer });
            categories.Add(new Food() { SubCtgr = Sub_Category.Main });
            categories.Add(new Food() { SubCtgr = Sub_Category.Dessert });

            SQLiteDataReader reader = conn.ViewTable("Food");
            while (reader.Read())
            {
                foods.Add(new Food()
                {
                    Name = ((string)reader["name"]),
                    Price = ((double)reader["price"]),
                    Description = ((string)reader["desc"]),
                    Img = ((string)reader["img"]),
                    SubCtgr = categories[int.Parse(reader["subctgr"].ToString())].SubCtgr
                });
            }

            this.totalTBox.Text = total.ToString();
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));

            foreach (Food f in foods)
            {
                ChangeMenu(f);
            }

            conn.Close();
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Order order = new Order
            {
                OrderID = 1,
                Total = total,
                SalesTax = tax,
                OrderDateTime = DateTime.Now,
                IsTakeOut = false,
                Foods = cart
            };
            this.NavigationService.Navigate(new PaymentOption_Page(order));
        }

        private void Appetizer_Click(object sender, RoutedEventArgs e)
        {
            grids.Clear();
            Menu.Children.Clear();

            foreach (Food f in foods)
            {
                if (f.SubCtgr == Sub_Category.Appetizer)
                {
                    ChangeMenu(f);
                }
            }
        }

        private void Main_Click(object sender, RoutedEventArgs e)
        {
            grids.Clear();
            Menu.Children.Clear();

            foreach (Food f in foods)
            {
                if (f.SubCtgr == Sub_Category.Main)
                {
                    ChangeMenu(f);
                }
            }
        }

        private void Dessert_Click(object sender, RoutedEventArgs e)
        {
            grids.Clear();
            Menu.Children.Clear();

            foreach (Food f in foods)
            {
                if (f.SubCtgr == Sub_Category.Dessert)
                {
                    ChangeMenu(f);
                }
            }
        }

        private void Drinks_Click(object sender, RoutedEventArgs e)
        {
            grids.Clear();
            Menu.Children.Clear();

            foreach (Food f in foods)
            {
                if (f.SubCtgr == Sub_Category.Drink)
                {
                    ChangeMenu(f);
                }

            }
        }

        private void All_Click(object sender, RoutedEventArgs e)
        {
            grids.Clear();
            Menu.Children.Clear();
            foreach (Food f in foods)
            {
                ChangeMenu(f);
            }
        }

        private void ClearAll_Click(object sender, RoutedEventArgs e)
        {
            items.Clear();
            cart.Clear();
            itemCart.Items.Refresh();
            total = 0;
            this.totalTBox.Text = string.Format("{0:F2}", total);
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {

            CartItems selected = itemCart.SelectedItem as CartItems;
            if (itemCart.SelectedItem != null)
            {
                int qty = items.Where(x => x.Name == selected.Name).First().Qty;
                cart.RemoveAll(x => x.Name == selected.Name);
                items.Remove(items.Where(x => x.Name == selected.Name).First());
                total -= selected.Price * qty;
                itemCart.Items.Refresh();
            }

            this.totalTBox.Text = string.Format("{0:F2}", total);
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));
        }

        private void ChangeMenu(Food f)
        {
            ScrollViewer viewer = new ScrollViewer();
            viewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            Grid grid = new Grid();
            grid.Tag = f.Name;
            grid.Height = 100;
            grid.Background = new SolidColorBrush(Colors.AntiqueWhite);
            grid.MouseDown += new MouseButtonEventHandler(FoodClick);

            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            gridCol1.Width = new GridLength(100, GridUnitType.Pixel);
            gridCol2.Width = new GridLength(525, GridUnitType.Pixel);
            grid.ColumnDefinitions.Add(gridCol1);
            grid.ColumnDefinitions.Add(gridCol2);

            Image image = new Image
            {
                Source = new BitmapImage(new Uri(f.Img, UriKind.RelativeOrAbsolute))
            };
            image.Stretch = Stretch.Fill;
            Grid.SetColumn(image, 0);
            grid.Children.Add(image);

            DockPanel dp = new DockPanel();

            TextBlock price = new TextBlock
            {
                Text = f.Price.ToString(),
                Width = 30
            };
            DockPanel.SetDock(price, Dock.Right);
            dp.Children.Add(price);

            TextBlock name = new TextBlock
            {
                Text = f.Name
            };
            name.TextAlignment = TextAlignment.Left;
            DockPanel.SetDock(name, Dock.Top);
            dp.Children.Add(name);

            TextBlock category = new TextBlock
            {
                Text = f.SubCtgr.ToString()
            };
            category.TextAlignment = TextAlignment.Left;
            DockPanel.SetDock(category, Dock.Top);
            dp.Children.Add(category);

            TextBlock desc = new TextBlock
            {
                Text = f.Description
            };
            desc.TextAlignment = TextAlignment.Left;
            dp.Children.Add(desc);

            Grid.SetColumn(dp, 1);
            grid.Children.Add(dp);

            grids.Add(grid);

            Menu.Children.Add(grid);
        }

        private void FoodClick(object sender, MouseButtonEventArgs e)
        {
            Grid foodGrid = sender as Grid;
            Food f = foods.Where(x => x.Name == foodGrid.Tag.ToString()).First();
            CartItems i = new CartItems { Qty = 1, Name = f.Name, Price = f.Price };

            cart.Add(f);
            if (items.Select(x => x.Name).Contains(i.Name))
            {
                items.Where(x => x.Name == f.Name).First().Qty++;
            }
            else
                items.Add(i);

            itemCart.Items.Refresh();

            total += f.Price;
            this.totalTBox.Text = string.Format("{0:F2}", total);
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));
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
