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
//using System.Windows.Shapes;
using Telemeal.Model;
using System.Data.SQLite;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows.Markup;
using System.Data;

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for FoodDBTestWindow.xaml
    /// </summary>



    public partial class FoodDBTestWindow : Window
    {
        private static string ERROR_CODE_MRF = "Required Fields: Name, Price, Category -- ";
        private static string ERROR_CODE_IDT = "Invalid Datatype: ";
        dbConnection conn = new dbConnection();
        List<Food> lFood = new List<Food>();
        Sub_Category[] sub_list = new Sub_Category[] { Sub_Category.Drink, Sub_Category.Appetizer, Sub_Category.Main, Sub_Category.Dessert };

        public FoodDBTestWindow()
        {
            InitializeComponent();

            foreach (Sub_Category s in sub_list)
            {
                cbAddCategory.Items.Add(s);
                cbEditCategory.Items.Add(s);
            }

            PopulateCBEditFoodID();
        }

        private void PopulateCBEditFoodID()
        {
            SQLiteDataReader reader = conn.ViewTable("Food");
            while (reader.Read())
            {
                Console.WriteLine($"{reader["id"].ToString()}, {reader["name"].ToString()}");
                int id = int.Parse(reader["id"].ToString());
                string name = (string)reader["name"];
                double price = (double)reader["price"];
                string desc = (string)reader["desc"];
                string image = (string)reader["img"];
                Main_Category main = (Main_Category) Enum.Parse(typeof(Main_Category), reader["mainctgr"].ToString());
                Sub_Category sub = (Sub_Category) Enum.Parse(typeof(Sub_Category), reader["subctgr"].ToString());
                Food food = new Food
                {
                    Name = name,
                    Price = price,
                    Description = desc,
                    Img = image,
                    MainCtgr = main,
                    SubCtgr = sub
                };

                lFood.Add(food);
                cbEditName.Items.Add(food.Name);
            }
            reader.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
            conn.Close();
        }

        private void bAddFoodItem_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string fName = "";
            double fPrice;
            string fDesc = "";
            string fImg = "";
            Main_Category fMain = Main_Category.All;
            Sub_Category fSub;
            try
            {
                if ((fName = tbAddName.Text) == "")
                    throw new ArgumentNullException("Missing Name");
                if (tbAddPrice.Text == "")
                    throw new ArgumentNullException("Missing Price");
                fPrice = double.Parse(tbAddPrice.Text);
                fDesc = tbAddDesc.Text;
                fImg = TelemealPath(tbAddImage.Text);
                if (cbAddCategory.Text == "")
                    throw new ArgumentNullException("Missing Category");
                fSub = (Sub_Category)Enum.Parse(typeof(Sub_Category), cbAddCategory.Text);

                Food food = new Food
                {
                    Name = fName,
                    Price = fPrice,
                    Description = fDesc,
                    Img = fImg,
                    MainCtgr = fMain,
                    SubCtgr = fSub
                };
                conn.InsertFood(food);
                lFood.Add(food);

                tbAddName.Clear();
                tbAddImage.Clear();
                tbAddDesc.Clear();
                tbAddPrice.Clear();
                cbAddCategory.SelectedIndex = -1;

                cbEditName.Items.Add(food.Name);
            }
            catch (ArgumentNullException ex)
            {
                MessageBox.Show(ERROR_CODE_MRF + ex.ParamName);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ERROR_CODE_IDT);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bAddImage_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string imgFile = "";
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../"));
            ofd.Filter = "PNG files (*.png)|*.png|JPEG files (*.jpeg)|*.jpeg|JPG files (*.jpg)|*.jpg|All files (*.*)|*.*";
            ofd.FilterIndex = 4;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    imgFile = ofd.FileName;
                    tbAddImage.Text = imgFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void bViewFoodItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = sender as Button;
                ViewDB viewDB = new ViewDB();
                viewDB.Show();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cBox = sender as ComboBox;
            string name = "";
            if (cBox.SelectedIndex != -1)
            {
                name = cBox.SelectedItem as string;
                Food food = lFood.Where(v => v.Name == name).First();
                tbEditPrice.Text = food.Price.ToString();
                tbEditDesc.Text = food.Description;
                tbEditImage.Text = food.Img;
                cbEditCategory.Text = food.SubCtgr.ToString();
            }
        }

        private void bEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = cbEditName.Text;
                double price = double.Parse(tbEditPrice.Text);
                string desc = tbEditDesc.Text;
                string img = TelemealPath(tbEditImage.Text);
                Main_Category main = Main_Category.All;
                Sub_Category sub = (Sub_Category)Enum.Parse(typeof(Sub_Category), cbEditCategory.Text);
                Food food = new Food
                {
                    Name = name,
                    Price = price,
                    Description = desc,
                    Img = img,
                    MainCtgr = main,
                    SubCtgr = sub
                };
                conn.UpdateFood(food);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void bEditImage_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            string imgFile = "";
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../"));
            ofd.Filter = "PNG files (*.png)|*.png|JPEG files (*.jpeg)|*.jpeg|JPG files (*.jpg)|*.jpg|All files (*.*)|*.*";
            ofd.FilterIndex = 4;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    imgFile = ofd.FileName;
                    tbEditImage.Text = imgFile;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = cbEditName.Text;
                double price = double.Parse(tbEditPrice.Text);
                conn.DeleteFoodByNameAndPrice(name, price);

                ClearEditFields();

                PopulateCBEditFoodID();
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ClearEditFields()
        {
            cbEditName.SelectedIndex = -1;
            tbEditImage.Clear();
            tbEditDesc.Clear();
            tbEditPrice.Clear();
            cbEditCategory.SelectedIndex = -1;

            cbEditName.Items.Clear();
            lFood.Clear();
        }

        private string TelemealPath(string path)
        {
            string relPath = "";
            int counter = 0;
            bool pathFound = false;
            String[] split = path.Split('\\');

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i] == "Telemeal")
                {
                    counter = i;
                    pathFound = true;
                    break;
                }
            }

            if (pathFound)
            {
                for (int i = counter; i < split.Length; i++)
                {
                    relPath += "/";
                    relPath += split[i];
                    if (i == counter)
                    {
                        relPath += ";component";
                    }
                }
            }

            return relPath;
        }
    }
}

