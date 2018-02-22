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

        public void CreateFoodTable(string tableName)
        {
            string cmd = $"CREATE TABLE {tableName} (id INT, name VARCHAR(50), price DOUBLE, desc VARCHAR(200), img VARCHAR(100), mainctgr INT, subctgr INT)";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void CreateEmployeeTable(string tableName)
        {
            string cmd = $"CREATE TABLE {tableName} (id INT, name VARCHAR(50), position VARCHAR(50), privilege BOOL)";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void InsertFood(string tableName, Food food)
        {
            int foodID = food.FoodID;
            string name = food.Name;
            double price = food.Price;
            string desc = food.Description;
            string img = food.Img;
            int mainCtr = (int) food.MainCtgr;
            int subCtr = (int) food.SubCtgr;
            //string cmd = $"INSERT INTO {tableName} (id, name, price, desc, img, mainctgr, subctgr) VALUES ({foodID}, '{name}', {price}, '{desc}', '{img}', {mainCtr}, {subCtr})";
            string cmd = $"INSERT INTO {tableName} (id, name, price, desc, img, mainctgr, subctgr) VALUES (@foodID, @name, @price, @desc, @img, @mainCtr, @subCtr)";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.Parameters.AddWithValue("@foodID", foodID);
            sqlite_cmd.Parameters.AddWithValue("@name", name);
            sqlite_cmd.Parameters.AddWithValue("@price", price);
            sqlite_cmd.Parameters.AddWithValue("@desc", desc);
            sqlite_cmd.Parameters.AddWithValue("@img", img);
            sqlite_cmd.Parameters.AddWithValue("@mainCtr", mainCtr);
            sqlite_cmd.Parameters.AddWithValue("@subCtr", subCtr);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void InsertEmployee(string tableName, Employee employee)
        {
            int employeeID = employee.ID;
            string employeeName = employee.name;
            string employeePosition = employee.position;
            bool employeePrivilege = employee.privilege;
            string cmd = $"INSERT INTO {tableName} (id, name, position, privilege) VALUES ({employeeID}, '{employeeName}', '{employeePosition}', '{employeePrivilege}')";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void UpdateFood(string tableName, Food food) {
            int foodID = food.FoodID;
            string name = food.Name;
            double price = food.Price;
            string desc = food.Description;
            string img = food.Img;
            int mainCtr = (int)food.MainCtgr;
            int subCtr = (int)food.SubCtgr;
            /**
            string cmd = $"UPDATE {tableName} " +
                $"SET name = '{name}', price = {price}, desc = '{desc}', img = '{img}', mainctgr = {mainCtr}, subctgr = {subCtr} " + 
                $"WHERE id = {foodID}";*/
            
            string cmd = $"UPDATE {tableName} " +
                $"SET name = @name, price = @price, desc = @desc, img = @img, mainctgr = @mainCtr, subctgr = @subCtr " +
                $"WHERE id = @foodID";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.Parameters.AddWithValue("@foodID", foodID);
            sqlite_cmd.Parameters.AddWithValue("@name", name);
            sqlite_cmd.Parameters.AddWithValue("@price", price);
            sqlite_cmd.Parameters.AddWithValue("@desc", desc);
            sqlite_cmd.Parameters.AddWithValue("@img", img);
            sqlite_cmd.Parameters.AddWithValue("@mainCtr", mainCtr);
            sqlite_cmd.Parameters.AddWithValue("@subCtr", subCtr);
            sqlite_cmd.ExecuteNonQuery();
        }

        public void DeleteFoodByID(string tableName, int id)
        {
            string cmd = $"DELETE FROM {tableName} WHERE id = @id";
            sqlite_cmd = new SQLiteCommand(cmd, sqlite_conn);
            sqlite_cmd.Parameters.AddWithValue("@id", id);
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
