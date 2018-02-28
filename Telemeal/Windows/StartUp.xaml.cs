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
    /// Interaction logic for StartUp.xaml
    /// </summary>
    public partial class StartUp : Window
    {
        public StartUp()
        {
            InitializeComponent();
            Loaded += StartUp_Loaded;
        }

        /// <summary>
        /// Loads initial page as LogIn_Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartUp_Loaded(Object sender, RoutedEventArgs e)
        {
            Main.NavigationService.Navigate(new LogIn_Page());
        }
    }
}
