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
    /// Interaction logic for CashPmt.xaml
    /// </summary>
    public partial class CashPmt : Window
    {
        public CashPmt()
        {
            InitializeComponent();
        }

        private void Intro_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var login = new LogIn();
            login.Closed += Window_Closed;
            login.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
