using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Telemeal.Model
{
    class dbConnection
    {
        private static string ConnectionString;
        public SQLiteConnection sqlite_conn;
        public SQLiteCommand sqlite_cmd;
        public SQLiteDataReader sqlite_dr;

        /// <summary>
        /// constructor
        /// </summary>
        public dbConnection()
        {
            //Connection string
            string v = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../"));
            ConnectionString = string.Format($@"Data Source={v}sqliteTMDB.db;Version=3;New=false;Compress=True");
            //initialize member data
            sqlite_conn = new SQLiteConnection(ConnectionString);
            //open the connection
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
        }

        /// <summary>
        /// This method will create new food table in the database
        /// id: primary key
        /// name, price, category: unique key (sub-key)
        /// </summary>
        public void CreateFoodTable()
        {
            string cmd = $"CREATE TABLE Food " +
                $"(food_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"name VARCHAR(50) NOT NULL, " +
                $"price DOUBLE NOT NULL, " +
                $"desc VARCHAR(200), " +
                $"img VARCHAR(100), " +
                $"mainctgr INTEGER, " +
                $"subctgr INTEGER NOT NULL," +
                $"CONSTRAINT name_price_ctgr_unique_key UNIQUE (name, price, subctgr))";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will create new employee table in the database
        /// id, name: primary key
        /// </summary>
        public void CreateEmployeeTable()
        {
            string cmd = $"CREATE TABLE Employee " +
                $"(employee_id INTEGER NOT NULL, " +
                $"name VARCHAR(50) NOT NULL, " +
                $"position VARCHAR(50), " +
                $"privilege BOOL, " +
                $"PRIMARY KEY(id, name))";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// This method will create new order table in the database
        /// id: primary key
        /// total, datetime: unique key
        /// </summary>
        public void OrderTable()
        {
            string cmd = $"CREATE TABLE Order " +
                $"(order_id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"total DOUBLE NOT NULL, " +
                $"tax DOUBLE, " +
                $"datetime DATETIME NOT NULL, " +
                $"takeout BOOL, " +
                $"CONSTRAINT total_datetime_unique_key UNIQUE (total, datetime))";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void FoodOrderTable()
        {
            string cmd = $"CREATE TABLE FoodOrder " +
                $"(order_id INTEGER NOT NULL, " +
                $"food_id INTEGER NOT NULL, " +
                $"quantity INTEGER NOT NULL, " +
                $"FOREIGN KEY (order_id, food_id) REFERENCES Order(order_id), Food(food_id))";
        }

        /// <summary>
        /// this method will insert new food item into the food table
        /// </summary>
        /// <param name="food">Food object to be added to the table</param>
        public void InsertFood(Food food)
        {
            string name = food.Name;
            double price = food.Price;
            string desc = food.Description;
            string img = food.Img;
            int mainCtr = (int) food.MainCtgr;
            int subCtr = (int) food.SubCtgr;
            string cmd = $"INSERT INTO Food (name, price, desc, img, mainctgr, subctgr) VALUES (@name, @price, @desc, @img, @mainCtr, @subCtr)";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.Parameters.AddWithValue("@name", name);
            sqlite_cmd.Parameters.AddWithValue("@price", price);
            sqlite_cmd.Parameters.AddWithValue("@desc", desc);
            sqlite_cmd.Parameters.AddWithValue("@img", img);
            sqlite_cmd.Parameters.AddWithValue("@mainCtr", mainCtr);
            sqlite_cmd.Parameters.AddWithValue("@subCtr", subCtr);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will insert new employee object into the employee table
        /// </summary>
        /// <param name="employee">Employee object to be added to the table</param>
        public void InsertEmployee(Employee employee)
        {
            int employeeID = employee.ID;
            string employeeName = employee.name;
            string employeePosition = employee.position;
            bool employeePrivilege = employee.privilege;
            string cmd = $"INSERT INTO Employee (id, name, position, privilege) VALUES ({employeeID}, '{employeeName}', '{employeePosition}', '{employeePrivilege}')";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will update the change made in any object of same name
        /// </summary>
        /// <param name="food">Food object which has same name but different properties to be updated</param>
        public void UpdateFood(Food food) {
            string name = food.Name;
            double price = food.Price;
            string desc = food.Description;
            string img = food.Img;
            int mainCtr = (int)food.MainCtgr;
            int subCtr = (int)food.SubCtgr;
            
            string cmd = $"UPDATE Food " +
                $"SET price = @price, desc = @desc, img = @img, mainctgr = @mainCtr, subctgr = @subCtr " +
                $"WHERE name = @name";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);

            sqlite_cmd.Parameters.AddWithValue("@name", name);
            sqlite_cmd.Parameters.AddWithValue("@price", price);
            sqlite_cmd.Parameters.AddWithValue("@desc", desc);
            sqlite_cmd.Parameters.AddWithValue("@img", img);
            sqlite_cmd.Parameters.AddWithValue("@mainCtr", mainCtr);
            sqlite_cmd.Parameters.AddWithValue("@subCtr", subCtr);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will remove data with matching name and price from the table
        /// </summary>
        /// <param name="name">Name of the food to be deleted</param>
        /// <param name="price">Price of the food to be deleted</param>
        public void DeleteFoodByNameAndPrice(string name, double price)
        {
            string cmd = $"DELETE FROM Food WHERE name = '{name}' AND price = {price}";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will remove data with matching name
        /// </summary>
        /// <param name="name">Name of the food to be deleted</param>
        public void DeleteFoodByName(string name)
        {
            string cmd = $"DELETE FROM Food WHERE name = '{name}'";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will remove data with matching name from the table
        /// </summary>
        /// <param name="name">Name of employee to be deleted</param>
        public void DeleteEmployeeByName(string name)
        {
            string cmd = $"DELETE FROM Employee WHERE name = '{name}'";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will delete the table with matching name
        /// </summary>
        /// <param name="name">Name of the table to be deleted</param>
        public void DeleteTable(string name)
        {
            string cmd = $"DROP TABLE {name}";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// this method will return SQLiteDataReader object which contains data from the table
        /// </summary>
        /// <param name="tableName">Name of the table to look up</param>
        /// <returns>SQLiteDataObject containing data instances</returns>
        public SQLiteDataReader ViewTable(string tableName)
        {
            string cmd = $"SELECT * FROM {tableName} order by id";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_dr = sqlite_cmd.ExecuteReader();
            return sqlite_dr;
        }

        public void CreateInvoiceTable()
        {
            string cmd = $"CREATE TABLE Invoice " +
               $"(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
               $"invoiceDate DATE NOT NULL, " +
               $"privilege BOOL, " +
               $"PRIMARY KEY(id))";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void Close()
        {
            sqlite_conn.Close();
        }
    }
}
