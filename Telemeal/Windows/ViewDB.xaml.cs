using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for ViewDB.xaml
    /// </summary>
    public partial class ViewDB : Window
    {
        dbConnection conn = new dbConnection();
        public ViewDB()
        {
            InitializeComponent();
            List<FoodwID> foods = new List<FoodwID>();
            SQLiteDataReader reader = conn.ViewTable("Food");
            while (reader.Read())
            {
                foods.Add(new FoodwID
                {
                    id = int.Parse(reader["id"].ToString()),
                    name = reader["name"].ToString(),
                    price = (double)reader["price"],
                    desc = (string)reader["desc"],
                    img = (string)reader["img"],
                    subctgr = (Sub_Category) Enum.Parse(typeof(Sub_Category), reader["subctgr"].ToString())
                });
            }
            dgFoods.ItemsSource = foods;
        }
    }
    public class FoodwID {
        public int id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string desc { get; set; }
        public string img { get; set; }
        public Sub_Category subctgr { get; set; }
    }
}
