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
        private static string ConnectionString;// = @"Data Source=sqliteTMDB.db;Version=3;New=false;Compress=True";
        public SQLiteConnection sqlite_conn;
        public SQLiteCommand sqlite_cmd;
        public SQLiteDataReader sqlite_dr;

        public dbConnection()
        {
            string v = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../"));
            ConnectionString = string.Format($@"Data Source={v}sqliteTMDB.db;Version=3;New=false;Compress=True");
            sqlite_conn = new SQLiteConnection(ConnectionString);
            sqlite_conn.Open();
            sqlite_cmd = sqlite_conn.CreateCommand();
        }

        public void CreateFoodTable()
        {
            string cmd = $"CREATE TABLE Food " +
                $"(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
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

        public void CreateEmployeeTable()
        {
            string cmd = $"CREATE TABLE Employee " +
                $"(id INTEGER NOT NULL, " +
                $"name VARCHAR(50) NOT NULL, " +
                $"position VARCHAR(50), " +
                $"privilege BOOL, " +
                $"PRIMARY KEY(id, name))";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

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

        public void UpdateFood(Food food) {
            string name = food.Name;
            double price = food.Price;
            string desc = food.Description;
            string img = food.Img;
            int mainCtr = (int)food.MainCtgr;
            int subCtr = (int)food.SubCtgr;
            
            string cmd = $"UPDATE Food " +
                $"SET name = @name, price = @price, desc = @desc, img = @img, mainctgr = @mainCtr, subctgr = @subCtr " +
                $"WHERE id = @foodID";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);

            sqlite_cmd.Parameters.AddWithValue("@name", name);
            sqlite_cmd.Parameters.AddWithValue("@price", price);
            sqlite_cmd.Parameters.AddWithValue("@desc", desc);
            sqlite_cmd.Parameters.AddWithValue("@img", img);
            sqlite_cmd.Parameters.AddWithValue("@mainCtr", mainCtr);
            sqlite_cmd.Parameters.AddWithValue("@subCtr", subCtr);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void DeleteFoodByNameAndPrice(string name, double price)
        {
            string cmd = $"DELETE FROM Food WHERE name = '{name}' AND price = {price}";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void DeleteFoodByName(string name)
        {
            string cmd = $"DELETE FROM Food WHERE name = '{name}'";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void DeleteEmployeeByName(string name)
        {
            string cmd = $"DELETE FROM Employee WHERE name = '{name}'";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void DeleteTable(string name)
        {
            string cmd = $"DROP TABLE {name}";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public SQLiteDataReader ViewTable(string tableName)
        {
            string cmd = $"SELECT * FROM {tableName} order by id";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_dr = sqlite_cmd.ExecuteReader();
            return sqlite_dr;
        }

        public void Close()
        {
            sqlite_conn.Close();
        }
    }
}
