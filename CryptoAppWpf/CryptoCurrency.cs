using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppWpf
{
    internal class CryptoCurrency : INotifyPropertyChanged
    {
        public string symbol { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string large { get; set; }
        public decimal price_btc { get; set; }
        public decimal PriceUSD { get; set; }
        public decimal price_change_percentage_24h { get; set; }
        public decimal price_change_percentage_7d { get; set; }
        public decimal price_change_percentage_30d { get; set; }
        public decimal price_change_percentage_1y { get; set; }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
