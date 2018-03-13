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
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using Newtonsoft.Json;
using Telemeal.Model;
using System.Threading;
using System.IO;

namespace Server
{
    public class OrderItem
    {
        public int OrderItemID { get; set; }
        public String FoodName { get; set; }
        public DateTime TimePurchased { get; set; }
        public Boolean IsTakeOut { get; set; }

        public OrderItem(int id, string name, DateTime time, Boolean takeout)
        {
            OrderItemID = id;
            FoodName = name;
            TimePurchased = time;
            IsTakeOut = takeout;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<OrderItem> items = new List<OrderItem>();

        public MainWindow()
        {
            InitializeComponent();
            itemCart.ItemsSource = items;
            //IsBackground makes sure thread does not keep running after window has been closed
            //new Thread(() => WaitForResponse()) { IsBackground = true }.Start() ;
        }

        public void WaitForResponse()
        {
            /*
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234);
            TcpListener listener = new TcpListener(ep);
            
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream nwStream = client.GetStream();
            */

            while (true)
            {
                /*
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                // Read the message, and perform different actions  
                message = cleanMessage(buffer);
                Order order = JsonConvert.DeserializeObject<Order>(message);

                foreach(Food f in order.Foods)
                {
                    OrderItem item = new OrderItem(order.OrderID, f.Name, order.OrderDateTime, order.IsTakeOut);
                    items.Add(item);
                }*/
                bool received = GetOrder();
                if (received)
                {
                    Order order = ReadFile();
                    foreach (Food f in order.Foods)
                    {
                        OrderItem item = new OrderItem(order.OrderID, f.Name, order.OrderDateTime, order.IsTakeOut);
                        items.Add(item);
                    }
                    MessageBox.Show("Received");
                }

                this.Dispatcher.Invoke(() =>
                {
                    itemCart.Items.Refresh();
                });


            }
        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = System.Text.Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private static bool GetOrder()
        {
            var checkReq = (FtpWebRequest)WebRequest.Create("ftp://18.216.172.183/Order/order.txt");
            checkReq.Credentials = new NetworkCredential("cecs327", "cecs327");
            checkReq.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)checkReq.GetResponse();

                using (WebClient req = new WebClient())
                {
                    req.Credentials = new NetworkCredential("cecs327", "cecs327");
                    byte[] fileData = req.DownloadData("ftp://18.216.172.183/Order/order.txt");
                    MessageBox.Show(OrderPath(Environment.CurrentDirectory));

                    using (FileStream file = File.Create(OrderPath(Environment.CurrentDirectory)))
                    {
                        file.Write(fileData, 0, fileData.Length);
                        file.Close();
                    }
                }

                FtpWebRequest delReq = (FtpWebRequest)WebRequest.Create("ftp://18.216.172.183/Order/order.txt");

                //If you need to use network credentials
                delReq.Credentials = new NetworkCredential("cecs327", "cecs327");
                //additionally, if you want to use the current user's network credentials, just use:
                //System.Net.CredentialCache.DefaultNetworkCredentials

                delReq.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse delResp = (FtpWebResponse)delReq.GetResponse();
                delResp.Close();
            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }
            }

            return true;
        }

        private static string OrderPath(string path)
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
                        relPath += "getOrder.txt";
                    }
                }
            }

            return relPath;
        }

        private static Order ReadFile()
        {
            String jsonText = File.ReadAllText(OrderPath(Environment.CurrentDirectory));
            var charsToRemove = new string[] { "\\" };
            foreach (var c in charsToRemove)
            {
                jsonText = jsonText.Replace(c, string.Empty);
            }
            jsonText = jsonText.Substring(1);
            jsonText = jsonText.Substring(0, jsonText.Length - 1);
            MessageBox.Show(jsonText);

            return JsonConvert.DeserializeObject<Order>(jsonText);
        }
    }
}

