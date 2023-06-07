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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CryptoAppWpf.Pages
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        ViewModel viewModel = new ViewModel();
        public Main()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCryptoCurrency = (sender as ListBox).SelectedItem as CryptoCurrency;

            if (selectedCryptoCurrency != null)
            {
                NavigationService.Navigate(new CryptoCurrencyPage(selectedCryptoCurrency));
            }
        }
    }
}
