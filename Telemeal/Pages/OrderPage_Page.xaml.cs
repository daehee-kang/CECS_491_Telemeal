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
    /// Interaction logic for OrderPage_Page.xaml
    /// </summary>
    public partial class OrderPage_Page : Page
    {
        List<Food> foods = new List<Food>();
        List<Food> cart = new List<Food>();
        List<Grid> grids = new List<Grid>();
        double tax = 0.1;
        double total = 0;

        public OrderPage_Page()
        {
            InitializeComponent();

            List<Food> categories = new List<Food>();
            dbConnection conn = new dbConnection();

            categories.Add(new Food() { SubCtgr = Sub_Category.Drink });
            categories.Add(new Food() { SubCtgr = Sub_Category.Appetizer });
            categories.Add(new Food() { SubCtgr = Sub_Category.Main });
            categories.Add(new Food() { SubCtgr = Sub_Category.Dessert });

            SQLiteDataReader reader = conn.ViewTable("Food");
            while (reader.Read())
            {
                foods.Add(new Food()
                {
                    //FoodID = ((int)reader["id"]),
                    Name = ((string)reader["name"]),
                    Price = ((double)reader["price"]),
                    Description = ((string)reader["desc"]),
                    Img = ((string)reader["img"]),
                    SubCtgr = categories[(int)reader["subctgr"]].SubCtgr
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
            this.NavigationService.Navigate(new PaymentOption_Page(total * (1 + tax), cart));
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
            Food f = foods.Where(x => x.Name == (foodGrid.Tag.ToString())).First();

            cart.Add(f);
            ItemCart.Items.Add(f);
            PriceCart.Items.Add(f);

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


        private void lbx2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer _listboxScrollViewer1 = GetDescendantByType(ItemCart, typeof(ScrollViewer)) as ScrollViewer;
            ScrollViewer _listboxScrollViewer2 = GetDescendantByType(PriceCart, typeof(ScrollViewer)) as ScrollViewer;
            _listboxScrollViewer1.ScrollToVerticalOffset(_listboxScrollViewer2.VerticalOffset);
        }
    }
}
