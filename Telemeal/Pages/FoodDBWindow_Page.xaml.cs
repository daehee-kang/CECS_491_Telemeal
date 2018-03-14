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
using Telemeal.Model;
using System.Data.SQLite;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Windows.Markup;
using System.Data;
using Telemeal.Windows;

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for FoodDBTestWindow.xaml
    /// </summary>
    public partial class FoodDBWindow_Page : Page
    {
        //Error Message for Missing Required Fields
        private static string ERROR_CODE_MRF = "Required Fields: Name, Price, Category -- ";
        //Error Meesage for Invalid Data Type
        private static string ERROR_CODE_IDT = "Invalid Datatype: ";
        //open the connection to database
        dbConnection conn = new dbConnection();
        //list of food for viewing the database and listing to the edit/remove by name selector
        List<Food> lFood = new List<Food>();
        //list of category for listing to the edit/add category to the food item
        Sub_Category[] sub_list = new Sub_Category[] { Sub_Category.Drink, Sub_Category.Appetizer, Sub_Category.Main, Sub_Category.Dessert };

        /// <summary>
        /// Constructor
        /// </summary>
        public FoodDBWindow_Page()
        {
            InitializeComponent();

            //fill categories into the comboboxes for adding and editing category of the food
            foreach (Sub_Category s in sub_list)
            {
                cbAddCategory.Items.Add(s);
                cbEditCategory.Items.Add(s);
            }

            //populate food name in the combobox for edit/remove food item
            PopulateCBEditFoodName();
        }

        /// <summary>
        /// Helper method for populating food item and updating the combobox in the edit/remove by name
        /// </summary>
        private void PopulateCBEditFoodName()
        {
            //get the data from the table
            SQLiteDataReader reader = conn.ViewTable("Food");
            //iterate through the data retrieved
            while (reader.Read())
            {
                //get data fields from the reader
                int id = int.Parse(reader["id"].ToString());
                string name = (string)reader["name"];
                double price = (double)reader["price"];
                string desc = (string)reader["desc"];
                string image = (string)reader["img"];
                Main_Category main = (Main_Category)Enum.Parse(typeof(Main_Category), reader["mainctgr"].ToString());
                Sub_Category sub = (Sub_Category)Enum.Parse(typeof(Sub_Category), reader["subctgr"].ToString());
                
                //make a class object
                Food food = new Food
                {
                    Name = name,
                    Price = price,
                    Description = desc,
                    Img = image,
                    MainCtgr = main,
                    SubCtgr = sub
                };

                //add data into list to bind to comboboxes
                lFood.Add(food);
                cbEditName.Items.Add(food.Name);
            }
            reader.Close();
        }

        /// <summary>
        /// Button click event for clicking "Add Food Item" button
        /// This method will add food class object into the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAddFoodItem_Click(object sender, RoutedEventArgs e)
        {
            //data holder
            string fName = "";
            double fPrice;
            string fDesc = "";
            string fImg = "";
            Main_Category fMain = Main_Category.All;
            Sub_Category fSub;
            //input validator using try/catch blocks
            try
            {
                //no input in name throws ArgumentNullException instead empty string
                if ((fName = tbAddName.Text) == "")
                    throw new ArgumentNullException("Missing Name");
                //no input in price throws same exception
                if (tbAddPrice.Text == "")
                    throw new ArgumentNullException("Missing Price");
                fPrice = double.Parse(tbAddPrice.Text);
                //description can be null
                fDesc = tbAddDesc.Text;
                //image can be null
                fImg = TelemealPath(tbAddImage.Text);
                //no selection in category throws ArgumentNullException
                if (cbAddCategory.Text == "")
                    throw new ArgumentNullException("Missing Category");
                fSub = (Sub_Category)Enum.Parse(typeof(Sub_Category), cbAddCategory.Text);

                //if there is no exception occurs, we know that non-nullable data are filled
                //thus, create object and insert into the database
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

                //after operation is executed, clear all the fields
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Button click event for "Browse" button of image
        /// This will open the window dialog box to navigate directory system to find the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bAddImage_Click(object sender, RoutedEventArgs e)
        {
            string imgFile = "";
            //create file dialog object
            OpenFileDialog ofd = new OpenFileDialog();

            //set initial path to be Telemeal folder
            ofd.InitialDirectory = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"../../"));
            //filter image files
            ofd.Filter = "PNG files (*.png)|*.png|JPEG files (*.jpeg)|*.jpeg|JPG files (*.jpg)|*.jpg|All files (*.*)|*.*";
            ofd.FilterIndex = 4;
            ofd.RestoreDirectory = true;

            //click on "open" button of dialog will select the file and set path to image file in relative path format
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

        /// <summary>
        /// Button click event for clicking "View Food Items" button
        /// This method will open up new window to show data store in the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bViewFoodItem_Click(object sender, RoutedEventArgs e)
        {
            //open new window
            try
            {
                ViewDB viewDB = new ViewDB();
                viewDB.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// ComboBox click event handler
        /// This method will auto-fill each field with corresponding name of the food
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cBox = sender as ComboBox;
            string name = "";
            //input validator
            if (cBox.SelectedIndex != -1)
            {
                name = cBox.SelectedItem as string;
                //find the data from the list
                Food food = lFood.Where(v => v.Name == name).First();
                //set each field with the data
                tbEditPrice.Text = food.Price.ToString();
                tbEditDesc.Text = food.Description;
                tbEditImage.Text = food.Img;
                cbEditCategory.Text = food.SubCtgr.ToString();
            }
        }

        /// <summary>
        /// Button click event for clicking "Edit Food item" button
        /// This method will update the change made from the edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bEdit_Click(object sender, RoutedEventArgs e)
        {
            //exception checker
            try
            {
                string name = cbEditName.Text;
                double price = double.Parse(tbEditPrice.Text);
                string desc = tbEditDesc.Text;
                string img = TelemealPath(tbEditImage.Text);
                Main_Category main = Main_Category.All;
                Sub_Category sub = (Sub_Category)Enum.Parse(typeof(Sub_Category), cbEditCategory.Text);
                //make new object with same name
                Food food = new Food
                {
                    Name = name,
                    Price = price,
                    Description = desc,
                    Img = img,
                    MainCtgr = main,
                    SubCtgr = sub
                };
                //and call function update of dbConnection class
                conn.UpdateFood(food);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            ClearEditFields();
        }

        /// <summary>
        /// Button click event for "Browse" button of image
        /// This will open the window dialog box to navigate directory system to find the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bEditImage_Click(object sender, RoutedEventArgs e)
        {
            //same for bAddImage_Click Method
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

        /// <summary>
        /// Button click event for clicking "Delete Food Item" button
        /// This method will remove data from the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //execute delete by name and price as they are primary keys
                string name = cbEditName.Text;
                double price = double.Parse(tbEditPrice.Text);
                conn.DeleteFoodByNameAndPrice(name, price);

                //clear all the fields
                ClearEditFields();

                //reload data from the database
                PopulateCBEditFoodName();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Helper method for clearing all the fields in edit/delete box
        /// </summary>
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

        /// <summary>
        /// Helper method for changing absolute path into relative path
        /// </summary>
        /// <param name="path">full absolute path</param>
        /// <returns></returns>
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

