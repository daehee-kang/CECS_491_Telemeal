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
        public PaymentOptions(Order o)
        {
            InitializeComponent();
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
