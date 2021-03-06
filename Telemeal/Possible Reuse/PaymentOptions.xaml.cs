﻿using System;
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
using System.Windows.Shapes;
using Telemeal.Model;

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for PaymentOptions.xaml
    /// </summary>
    public partial class PaymentOptions : Window
    {
        Order mOrder;
        public PaymentOptions(Order o)
        {
            InitializeComponent();
            mOrder = o;
            List<CartItems> items = new List<CartItems>();
            foreach (Food f in o.Foods)
            {
                CartItems i = new CartItems { Qty = 1, Name = f.Name, Price = f.Price };
                if (items.Select(x => x.Name).Contains(i.Name)) {
                    items.Where(x => x.Name == f.Name).First().Qty++;
                }
                else
                    items.Add(i);
            }
            itemCart.ItemsSource = items;
            AmountDue.Text = o.SubTotal().ToString("F2");
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            this.Close();
        }

        private void Cash_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var cashpmt = new CashPmt();
            cashpmt.Closed += Window_Closed;
            cashpmt.Show();
            this.Hide();
        }

        private void Paypal_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            var ppalpmt = new PaypalPmt();
            ppalpmt.Closed += Window_Closed;
            ppalpmt.Show();
            this.Hide();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Show();
        }
    }
}
