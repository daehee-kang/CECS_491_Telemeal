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
using System.Data.SQLite;

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for EmployeeDBTestWindow.xaml
    /// </summary>
    public partial class EmployeeDBTestWindow : Window
    {
        private static string ERROR_CODE_MRF = "Required Fields: Name, Price, Category -- ";
        private static string ERROR_CODE_IDT = "Invalid Datatype: ";
        dbConnection conn = new dbConnection();
        public EmployeeDBTestWindow()
        {
            InitializeComponent();
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            try
            {
                if (eID.Text == "")
                    throw new ArgumentNullException("Missing ID");
                if (eName.Text == "")
                    throw new ArgumentNullException("Missing Name");
                Employee employee = new Employee
                {
                    ID = int.Parse(eID.Text),
                    name = eName.Text,
                    position = ePosition.Text,
                    privilege = (bool)ePrivilege.IsChecked
                };
                conn.InsertEmployee(employee);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ERROR_CODE_MRF + ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ERROR_CODE_IDT);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            conn.Close();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            SQLiteDataReader reader = conn.ViewTable("Employee");
            ShowData.Text = "";
            while (reader.Read())
            {
                ShowData.Text += string.Format($"Name: {reader["name"]}, Position: {reader["position"]}, Is Admin: {reader["privilege"]}\n");
            }
        }

        private void eID_SelectionChanged(object sender, RoutedEventArgs e)
        {
            hintEmployeeID.Visibility = Visibility.Visible;
            if (eID.Text.Length > 0)
            {
                hintEmployeeID.Visibility = Visibility.Hidden;
            }
        }

        private void eName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            hintEmployeeName.Visibility = Visibility.Visible;
            if (eName.Text.Length > 0)
            {
                hintEmployeeName.Visibility = Visibility.Hidden;
            }
        }

        private void ePosition_SelectionChanged(object sender, RoutedEventArgs e)
        {
            hintEmployeePosition.Visibility = Visibility.Visible;
            if (ePosition.Text.Length > 0)
            {
                hintEmployeePosition.Visibility = Visibility.Hidden;
            }
        }
    }
}
