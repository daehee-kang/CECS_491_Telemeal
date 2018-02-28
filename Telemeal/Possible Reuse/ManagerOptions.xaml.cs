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
    /// Interaction logic for ManagerOptions.xaml
    /// </summary>
    public partial class ManagerOptions : Window
    {
        public ManagerOptions()
        {
            InitializeComponent();
        }

        private void Edit_Employee_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var employDB = new EmployeeDBTestWindow();
            employDB.Closed += Window_Closed;
            employDB.Show();
            this.Hide();
        }

        private void Edit_Menu_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var foodDB = new FoodDBTestWindow();
            foodDB.Closed += Window_Closed;
            foodDB.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
