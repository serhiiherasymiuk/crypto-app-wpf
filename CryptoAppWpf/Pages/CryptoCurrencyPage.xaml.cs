using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using CryptoAppWpf.Model;

namespace CryptoAppWpf.Pages
{
    /// <summary>
    /// Interaction logic for CryptoCurrencyPage.xaml
    /// </summary>
    public partial class CryptoCurrencyPage : Page
    {
        public CryptoCurrencyPage()
        {
            InitializeComponent();
        }

        internal CryptoCurrencyPage(CryptoCurrency cryptoCurrency)
        {
            InitializeComponent();
            DataContext = cryptoCurrency;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var destinationurl = ((Hyperlink)sender).CommandParameter.ToString();
            var sInfo = new ProcessStartInfo(destinationurl)
            {
                UseShellExecute = true,
            };
            Process.Start(sInfo);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }

}
