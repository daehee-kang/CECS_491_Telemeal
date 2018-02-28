using System.Windows;
using DotNetBrowser;
using DotNetBrowser.WPF;

namespace Telemeal.Windows
{
    /// <summary>
    /// Interaction logic for PaypalPmt.xaml
    /// </summary>
    public partial class PaypalPmt : Window
    {
        BrowserView webView;
        public PaypalPmt()
        {
            InitializeComponent();
            webView = new WPFBrowserView(BrowserFactory.Create());
            mainLayout.Children.Add((UIElement)webView.GetComponent());
            webView.Browser.LoadURL("http://www.google.com");
        }
    }
}
