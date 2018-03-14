using System.Windows;
using System.Windows.Controls;
using DotNetBrowser;
using DotNetBrowser.WPF;
using PayPal.Api;
using System.Collections.Generic;

namespace Telemeal.Pages
{
    /// <summary>
    /// Interaction logic for PaypalPmt_Page.xaml
    /// </summary>
    public partial class PaypalPmt_Page : Page
    {
        BrowserView webView;
        public PaypalPmt_Page()
        {
            InitializeComponent();
            webView = new WPFBrowserView(BrowserFactory.Create());
            mainLayout.Children.Add((UIElement)webView.GetComponent());
            webView.Browser.LoadURL("http://web.csulb.edu/~phuynh/cecs491b/index.html");
        }

        private void Intro_Click(object sender, RoutedEventArgs e)
        {
            while (this.NavigationService.CanGoBack)
            {
                this.NavigationService.GoBack();
            }
        }
    }
}
