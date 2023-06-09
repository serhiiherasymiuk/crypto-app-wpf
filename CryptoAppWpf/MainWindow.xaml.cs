using CryptoAppWpf.Pages;
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

namespace CryptoAppWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CryptoViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new CryptoViewModel();
            viewModel.FetchCryptoCurrenciesAsync();
            navframe.Navigate(new Main(viewModel));
            sidebar.SelectedItem = sidebar.Items.Cast<NavButton>().FirstOrDefault(btn => btn.NavLink.ToString() == "/Pages/Main.xaml");
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = sidebar.SelectedItem as NavButton;
            Trace.WriteLine(selected.NavLink.ToString());
            if (selected.NavLink.ToString() == "/Pages/Main.xaml")
            {
                navframe.Navigate(new Main(viewModel));
            }
            else
            {
                navframe.Navigate(selected.NavLink);
            }
        }
    }
}
