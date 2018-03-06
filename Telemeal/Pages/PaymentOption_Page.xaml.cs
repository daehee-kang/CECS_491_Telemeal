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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telemeal.Model;
using Newtonsoft.Json;
using System.Net.Sockets;

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for PaymentOption_Page.xaml
    /// </summary>
    public partial class PaymentOption_Page : Page
    {
        //order information is passed from the OrderPage
        Order order;
        /// <summary>
        /// Constructor for PaymentOption_Page
        /// This constructor will initialize the component in the view, and show cart and price information on the screen
        /// </summary>
        /// <param name="o">Order object passed for visualization of cart items and price information</param>
        public PaymentOption_Page(Order o)
        {
            InitializeComponent();
            order = o;
            //Convert order into user friendly cart item object
            List<CartItem> items = new List<CartItem>();
            //iterate through the food items in the order
            foreach (Food f in order.Foods)
            {
                //convert into cartItems object
                CartItem i = new CartItem { Qty = 1, Name = f.Name, Price = f.Price };
                //if cart item already exist in the cart, increase quantity
                if (items.Select(x => x.Name).Contains(i.Name))
                {
                    items.Where(x => x.Name == f.Name).First().Qty++;
                }
                //otherwise add new item
                else
                    items.Add(i);
            }
            //bind item to the cart
            itemCart.ItemsSource = items;
            //bind subtotal to the AmountDue
            AmountDue.Text = order.SubTotal().ToString("F2");
            MessageBox.Show(ConvertJSON());
        }

        /// <summary>
        /// Convert order object into JSON object to communicate with the printing method or payment method
        /// </summary>
        /// <returns></returns>
        private string ConvertJSON()
        {
            return JsonConvert.SerializeObject(order);
        }

        /// <summary>
        /// Button click event for "Menu" button
        /// This method will go back to previous screen with all the information saved
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        /// <summary>
        /// Button click event for "Cash" button
        /// This method will lead to CashPmt_Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cash_Click(object sender, RoutedEventArgs e)
        {
            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(ConvertJSON()));
            this.NavigationService.Navigate(new CashPmt_Page());
        }

        /// <summary>
        /// Button click event for "Paypal" button
        /// This method will lead to PaypalPmt_Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Paypal_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PaypalPmt_Page());
        }

        private static byte[] sendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 1024;
            try // Try connecting and send the message bytes  
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient("127.0.0.1", 1234); // Create a new connection  
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes  
                messageBytes = new byte[bytesize]; // Clear the message   

                // Clean up  
                stream.Dispose();
                client.Close();
            }
            catch (Exception e) // Catch exceptions  
            {
                MessageBox.Show(e.Message);
            }

            return messageBytes; // Return response  
        }
    }
}
