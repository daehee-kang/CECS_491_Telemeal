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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telemeal.Model;

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for PaymentOption_Page.xaml
    /// </summary>
    public partial class PaymentOption_Page : Page
    {
        private double dueAmount;
        private List<Food> foods = new List<Food>();

        public PaymentOption_Page(double due)
        {
            dueAmount = due;
            InitializeComponent();
            AmountDue.Text = "$" + string.Format("{0:F2}", dueAmount);
        }

        public PaymentOption_Page(double due, List<Food> f):this(due)
        {
            var grid = new GridView();

            Cart.View = grid;

            grid.Columns.Add(new GridViewColumn
            {
                Header = "Name",
                DisplayMemberBinding = new Binding("Name")
            });
            grid.Columns.Add(new GridViewColumn
            {
                Header = "Price",
                DisplayMemberBinding = new Binding("Price")
            });


            foreach (Food food in f)
            {
                foods.Add(food);
            }

            foreach (Food food in foods)
            {
                Cart.Items.Add(food);
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Cash_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CashPmt_Page());
        }

        private void Paypal_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PaypalPmt_Page());
        }
    }
}
