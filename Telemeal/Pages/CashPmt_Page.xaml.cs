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

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for CashPmt_Page.xaml
    /// </summary>
    public partial class CashPmt_Page : Page
    {
        public CashPmt_Page()
        {
            InitializeComponent();
        }

        private void Intro_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < 3; i++)
            {
                this.NavigationService.GoBack();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            
        }
    }
}
