using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for EmployeeLogin_Page.xaml
    /// </summary>
    public partial class EmployeeLogin_Page : Page
    {
        private static string ADMINID = "";
        private static string ADMINNAME = "";
        private StringBuilder id = new StringBuilder();
        private string pw;

        public EmployeeLogin_Page()
        {
            InitializeComponent();
            pw = EmployeeID.Password;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            dbConnection conn = new dbConnection();
            Button b = sender as Button;
            SQLiteDataReader reader = conn.ViewTable("Employee");
            bool key = false;
            bool admin = false;
            while (reader.Read())
            {
                ADMINID = ((int)reader["ID"]).ToString();
                ADMINNAME = ((string)reader["name"]);
                if (EmployeeID.Password.Equals(ADMINID) && EmployeeName.Text.Equals(ADMINNAME))
                {
                    //var foodDB = new FoodDBTestWindow();
                    //foodDB.Closed += Window_Closed;
                    //foodDB.Show();
                    key = true;
                    admin = ((bool)reader["privilege"]);
                    break;
                }
            }
            conn.Close();
            if (key)
            {
                if (admin)
                {
                    this.NavigationService.Navigate(new ManagerOptions_Page());
                }
                else
                {
                    this.NavigationService.Navigate(new FoodDBWindow_Page());
                }
            }
        }

        private void EmployeeID_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
