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
using System.Windows.Shapes;
using Telemeal.Model;

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeLogin.xaml
    /// </summary>
    public partial class EmployeeLogin : Window
    {
        private static string ADMINID = "";
        private static string ADMINNAME = "";
        private StringBuilder id = new StringBuilder();
        private string pw;

        public EmployeeLogin()
        {
            InitializeComponent();
            pw = EmployeeID.Password;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            if (id.Length > 0)
            {
                id.Remove(id.Length - 1, 1);
            }
        }
        
        private void Enter_Click(object sender, RoutedEventArgs e)
        {
            dbConnection conn = new dbConnection();
            Button b = sender as Button;
            SQLiteDataReader reader = conn.ViewTable("Employee");
            bool key = false;
            bool admin = false;
            while(reader.Read())
            {
                ADMINID = reader["ID"].ToString();
                ADMINNAME = ((string)reader["name"]);
                if (EmployeeID.Password.Equals(ADMINID) && EmployeeName.Text.Equals(ADMINNAME))
                {
                    key = true;
                    admin = ((bool)reader["privilege"]);
                    break;
                }
            }
            conn.Close();
            if(key)
            {
                if(admin)
                {
                    var manOption = new ManagerOptions();
                    manOption.Closed += Window_Closed;
                    manOption.Show();
                    this.Hide();
                }
                else
                {
                    var foodDB = new FoodDBTestWindow();
                    foodDB.Closed += Window_Closed;
                    foodDB.Show();
                    this.Hide();
                }
            }
            else
            {
                MessageBox.Show("Invalid ID or Name. Try again.");
            }
            ClearFields();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }

        private void EmployeeID_PasswordChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void ClearFields()
        {
            EmployeeName.Clear();
            EmployeeID.Clear();
        }
       
    }
}
