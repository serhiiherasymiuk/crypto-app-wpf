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
        private readonly CryptoCurrency _cryptoCurrency;

        internal CryptoCurrencyPage(CryptoCurrency cryptoCurrency)
        {
            InitializeComponent();

            _cryptoCurrency = cryptoCurrency;

            // You can now use the cryptoCurrency parameter as needed
            // For example, you can set the DataContext of the page to the cryptoCurrency object
            DataContext = cryptoCurrency;
        }
    }
}
