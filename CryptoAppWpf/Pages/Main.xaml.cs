using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using CryptoAppWpf.Model;

namespace CryptoAppWpf.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        CryptoViewModel viewModel;

        public Main()
        {
            InitializeComponent();
            viewModel = new CryptoViewModel();
            DataContext = viewModel;
        }
        public Main(CryptoViewModel sharedViewModel)
        {
            InitializeComponent();
            viewModel = sharedViewModel;
            DataContext = viewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCryptoCurrency = (sender as ListBox).SelectedItem as CryptoCurrency;

            if (selectedCryptoCurrency != null)
            {
                NavigationService?.Navigate(new CryptoCurrencyPage(selectedCryptoCurrency));
            }

            (sender as ListBox).SelectedItem = null;
        }

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {
            var selectedCryptoCurrency = (sender as ListBox).SelectedItem as CryptoCurrency;

            if (selectedCryptoCurrency != null)
            {
                NavigationService?.Navigate(new CryptoCurrencyPage(selectedCryptoCurrency));
            }
        }
    }
}
