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
    /// Interaction logic for ManagerOptions_Page.xaml
    /// </summary>
    public partial class ManagerOptions_Page : Page
    {
        public ManagerOptions_Page()
        {
            InitializeComponent();
        }

        private void Edit_Employee_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new EmployeeDBWindow_Page());
        }

        private void Edit_Menu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FoodDBWindow_Page());
        }
    }
}
