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
using Newtonsoft.Json;

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for PaymentOption_Page.xaml
    /// </summary>
    public partial class PaymentOption_Page : Page
    {
        Order mOrder;
        public PaymentOption_Page(Order o)
        {
            InitializeComponent();
            mOrder = o;
            List<CartItems> items = new List<CartItems>();
            foreach (Food f in o.Foods)
            {
                CartItems i = new CartItems { Qty = 1, Name = f.Name, Price = f.Price };
                if (items.Select(x => x.Name).Contains(i.Name))
                {
                    items.Where(x => x.Name == f.Name).First().Qty++;
                }
                else
                    items.Add(i);
            }
            itemCart.ItemsSource = items;
            AmountDue.Text = o.SubTotal().ToString("F2");
            MessageBox.Show(ConvertJSON());
        }

        private string ConvertJSON()
        {
            return JsonConvert.SerializeObject(mOrder);
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
