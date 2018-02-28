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
using Telemeal.Pages;

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for LogIn_Page.xaml
    /// </summary>
    public partial class LogIn_Page : Page
    {
        public LogIn_Page()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Button click event for clicking "Menu" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuestProceed_Click(object sender, RoutedEventArgs e)
        {
            //loads Order Page where user can see the menu
            this.NavigationService.Navigate(new OrderPage_Page());
        }

        /// <summary>
        /// Button click event for clicking "Other" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            //loads EmployeeLogin Page where user can edit the data relate to menu/employee
            this.NavigationService.Navigate(new EmployeeLogin_Page());
        }
    }
}
