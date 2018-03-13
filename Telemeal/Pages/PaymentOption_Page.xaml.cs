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
using PayPal;
using Telemeal.Model;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Data;

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for PaymentOption_Page.xaml
    /// </summary>
    public partial class PaymentOption_Page : Page
    {
        Order mOrder;
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
            mOrder = o;
            List<CartItems> items = new List<CartItems>();
            foreach (Food f in o.Foods)
            {
                CartItems i = new CartItems { Qty = 1, Name = f.Name, Price = f.Price };
                if (items.Select(x => x.Name).Contains(i.Name))
                {
                    items.Where(x => x.Name == f.Name).First().Qty++;
                }
                else
                    items.Add(i);
            }
            itemCart.ItemsSource = items;
            AmountDue.Text = o.SubTotal().ToString("F2");
            MessageBox.Show(ConvertJSON());
        }

        private string ConvertJSON()
        {
            return JsonConvert.SerializeObject(mOrder);
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(OrderPath(Environment.CurrentDirectory));
            this.NavigationService.GoBack();
        }

        private void Cash_Click(object sender, RoutedEventArgs e)
        {
            //byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(ConvertJSON()));
            //WebRequest request = WebRequest.Create("ftp://18.216.172.183");
            //request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //request.Credentials = new NetworkCredential("cecs327", "cecs327");

            WriteFile(OrderPath(Environment.CurrentDirectory));
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential("cecs327", "cecs327");
                client.UploadFile("ftp://18.216.172.183/Order/order.txt", "STOR", OrderPath(Environment.CurrentDirectory));

            }
            /*using (var resp = (FtpWebResponse)request.GetResponse())
            {
                MessageBox.Show(resp.StatusCode.ToString());
              
            }*/
            byte[] bytes = sendMessage(System.Text.Encoding.Unicode.GetBytes(ConvertJSON()));
            this.NavigationService.Navigate(new CashPmt_Page());
        }

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

        private string OrderPath(string path)
        {
            string relPath = "";
            int counter = 0;
            bool pathFound = false;
            String[] split = path.Split('\\');

            for (int i = 0; i < split.Length; i++)
            {
                if (split[i] == "CECS_491_Telemeal")
                {
                    counter = i;
                    pathFound = true;
                    break;
                }
            }

            if (pathFound)
            {
                for (int i = 0; i <= counter; i++)
                {
                    if (i != 0)
                    {
                        relPath += "/";
                    }
                    relPath += split[i];
                    if (i == counter)
                    {
                        relPath += "/";
                        relPath += "order.txt";
                    }
                }
            }

            return relPath;
        }

        private void WriteFile(string path)
        {
            StreamWriter output = new StreamWriter(path);

            //Writes the beginning statement to the specified file in output
            //deletes output object when done
            using (output)
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(output, ConvertJSON());
            }
        }
    }
}
