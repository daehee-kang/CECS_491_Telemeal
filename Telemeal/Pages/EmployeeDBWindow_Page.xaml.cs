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

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for EmployeeDBWindow_Page.xaml
    /// </summary>
    public partial class EmployeeDBWindow_Page : Page
    {
        dbConnection conn = new dbConnection();
        public EmployeeDBWindow_Page()
        {
            InitializeComponent();
        }

        private void CreateTable_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string name = TableName.Text;
            conn.CreateEmployeeTable(name);
        }

        private void AddEmployee_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string tableName = eTable.Text;
            Employee employee = new Employee
            {
                ID = int.Parse(eID.Text),
                name = eName.Text,
                position = ePosition.Text,
                privilege = (bool)ePrivilege.IsChecked
            };
            conn.InsertEmployee(tableName, employee);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            conn.Close();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            string name = viewTableName.Text;
            SQLiteDataReader reader = conn.ViewTable(name);
            while (reader.Read())
            {
                ShowData.Text += string.Format($"Name: {reader["name"]}, Position: {reader["position"]}, Is Admin: {reader["privilege"]}\n");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            conn.DeleteTable(NameDelete.Text);
        }

        private void eTable_SelectionChanged(object sender, RoutedEventArgs e)
        {
            hintTableName.Visibility = Visibility.Visible;
            if (eTable.Text.Length > 0)
            {
                hintTableName.Visibility = Visibility.Hidden;
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

        private void viewTableName_SelectionChanged(object sender, RoutedEventArgs e)
        {
            hintViewTableName.Visibility = Visibility.Visible;
            if (viewTableName.Text.Length > 0)
            {
                hintViewTableName.Visibility = Visibility.Hidden;
            }
        }

        private void NameDelete_SelectionChanged(object sender, RoutedEventArgs e)
        {
            hintTableDelete.Visibility = Visibility.Visible;
            if (NameDelete.Text.Length > 0)
            {
                hintTableDelete.Visibility = Visibility.Hidden;
            }
        }
    }
}
