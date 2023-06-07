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
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string rank;

        public string Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        private string priceUsd;

        public string PriceUsd
        {
            get { return priceUsd; }
            set { priceUsd = value; }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public CryptoCurrency Clone()
        {
            CryptoCurrency cryptoCurrency = (CryptoCurrency)this.MemberwiseClone();
            cryptoCurrency.Id = (string)this.Id;
            cryptoCurrency.Name = (string)this.Name;
            cryptoCurrency.Rank = (string)this.Rank;
            cryptoCurrency.PriceUsd = (string)this.PriceUsd;
            return cryptoCurrency;
        }
    }
}
