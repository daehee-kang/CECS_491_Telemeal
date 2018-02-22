using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Telemeal.Model;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace Telemeal.Windows
{
    public partial class OrderPage : Window
    { 
        List<Food> foods = new List<Food>();
        List<Food> cart = new List<Food>();
        List<Grid> grids = new List<Grid>();
        double tax = 0.1;
        double total = 0;

        public OrderPage()
        {
            InitializeComponent();
            List<Food> categories = new List<Food>();
            dbConnection conn = new dbConnection();

            categories.Add(new Food() {SubCtgr = Sub_Category.Drink });
            categories.Add(new Food() {SubCtgr = Sub_Category.Appetizer });
            categories.Add(new Food() {SubCtgr = Sub_Category.Main });
            categories.Add(new Food() {SubCtgr = Sub_Category.Dessert });

            SQLiteDataReader reader = conn.ViewTable("Food");
            while (reader.Read())
            {
                foods.Add(new Food() { Name = ((string)reader["name"]), Price = ((double)reader["price"]),
                    Description = ((string)reader["desc"]), Img = ((string)reader["img"]),SubCtgr = categories[int.Parse(reader["subctgr"].ToString())].SubCtgr});
            }

            this.totalTBox.Text = total.ToString();
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));

            //foods.Add(new Food() { FoodID = 1, Name = "Hamburger", Price = 2.50, Img = "/Telemeal;component/Images/hamburger.png", SubCtgr = Sub_Category.Main });
            //foods.Add(new Food() { FoodID = 2, Name = "Cheeseburger", Price = 3.00, Img = "/Telemeal;component/Images/cheeseburger.png", SubCtgr = Sub_Category.Main });
            //foods.Add(new Food() { FoodID = 3, Name = "Double Double Burger", Price = 4.00, Img = "/Telemeal;component/Images/doubleburger.png", SubCtgr = Sub_Category.Main });
            //foods.Add(new Food() { FoodID = 4, Name = "French Fries", Price = 1.50, Img = "/Telemeal;component/Images/fries.jpg", SubCtgr = Sub_Category.Appetizer });
            //foods.Add(new Food() { FoodID = 5, Name = "Animal Fries", Price = 3.00, Img = "/Telemeal;component/Images/animal.jpg", SubCtgr = Sub_Category.Appetizer });
            //foods.Add(new Food() { FoodID = 6, Name = "sm Drink", Price = 1.50, Img = "", SubCtgr = Sub_Category.Drink });
            //foods.Add(new Food() { FoodID = 7, Name = "lg Drink", Price = 2.00, Img = "", SubCtgr = Sub_Category.Drink });
            //foods.Add(new Food() { FoodID = 8, Name = "Milkshake", Price = 3.00, Img = "/Telemeal;component/Images/milkshake.jpg", SubCtgr = Sub_Category.Dessert });
            //foods.Add(new Food() { FoodID = 9, Name = "Cookie", Price = 1.00, Img = "", SubCtgr = Sub_Category.Dessert });

            foreach (Food f in foods)
            {
                ChangeMenu(f);
            }

            /*
            DataAccess db = new DataAccess();
            foods = db.GetFoods();

            Menu.ItemsSource = foods;
            Menu.DisplayMemberPath = "Name"; 
            */
            conn.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            Order order = new Order
            {
                
            };

            var pmtscr = new PaymentOptions(order);
            pmtscr.Closed += Window_Closed;
            pmtscr.Show();
            this.Hide();
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
            ItemCart.Items.Clear();
            PriceCart.Items.Clear();
            cart.Clear();
            total = 0;
            this.totalTBox.Text = string.Format("{0:F2}", total);
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));
        }

        private void ItemCart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Food selected = ItemCart.SelectedItem as Food;
            int index = ItemCart.SelectedIndex;
            if (ItemCart.SelectedItem != null)
            {
                ItemCart.Items.RemoveAt(index);
                PriceCart.Items.RemoveAt(index);
                cart.RemoveAt(index);
                total -= selected.Price;
            }
            this.totalTBox.Text = string.Format("{0:F2}", total);
            this.taxTBox.Text = string.Format("{0:F2}", total * tax);
            this.subtotalTBox.Text = string.Format("{0:F2}", (total + Double.Parse(taxTBox.Text)));
        }

        private void PriceCart_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Food selected = PriceCart.SelectedItem as Food;
            int index = PriceCart.SelectedIndex;
            if (PriceCart.SelectedItem != null)
            {
                ItemCart.Items.RemoveAt(index);
                PriceCart.Items.RemoveAt(index);
                cart.RemoveAt(index);
                total -= selected.Price;
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
            ItemCart.DisplayMemberPath = "Name";
            PriceCart.DisplayMemberPath = "Price";

            Grid foodGrid = sender as Grid;
            Food f = foods.Where(x => x.Name == foodGrid.Tag.ToString()).First();

            cart.Add(f);
            ItemCart.Items.Add(f);
            PriceCart.Items.Add(f);
            
            total += f.Price;
            this.totalTBox.Text = string.Format("{0:F2}", total);
            this.taxTBox.Text = string.Format("{0:F2}", total* tax);
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


        private void lbx2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(ItemCart, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(PriceCart, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);
        }
    }
}
