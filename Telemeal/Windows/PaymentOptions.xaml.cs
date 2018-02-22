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

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for PaymentOptions.xaml
    /// </summary>
    public partial class PaymentOptions : Window
    {
        private double dueAmount;
        private List<Food> foods = new List<Food>();

        public PaymentOptions(double due)
        {
            dueAmount = due;
            InitializeComponent();
            AmountDue.Text = "$" + string.Format("{0:F2}", dueAmount);
        }

        public PaymentOptions(double due, List<Food> f) : this(due)
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

            foreach(Food food in foods)
            {
                Cart.Items.Add(food);
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            this.Close();
        }

        private void Cash_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var cashpmt = new CashPmt();
            cashpmt.Closed += Window_Closed;
            cashpmt.Show();
            this.Hide();
        }

        private void Paypal_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var ppalpmt = new PaypalPmt();
            ppalpmt.Closed += Window_Closed;
            ppalpmt.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
