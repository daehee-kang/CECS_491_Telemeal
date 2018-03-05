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
            new Thread(() => WaitForResponse()) { IsBackground = true }.Start() ;
        }

        public void WaitForResponse()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 1234);
            TcpListener listener = new TcpListener(ep);
            listener.Start();

            while (true)
            {
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
    }
}

