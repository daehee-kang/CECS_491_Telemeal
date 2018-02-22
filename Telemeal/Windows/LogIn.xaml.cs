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

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void GuestProceed_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var orderscr = new OrderPage();
            orderscr.Closed += Window_Closed;
            orderscr.Show();
            this.Hide();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var empLogin = new EmployeeLogin();
            empLogin.Closed += Window_Closed;
            empLogin.Show();
            this.Hide();
        }
    }
}
