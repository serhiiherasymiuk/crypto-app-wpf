using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.Diagnostics;
using CryptoAppWpf.Model;
using System.Linq;
using System.Text.Json;

namespace CryptoAppWpf
{
    public class ConvertViewModel : INotifyPropertyChanged
    {
        private string amount;
        private string cryptoName;
        private string convertTo;
        private string conversionResult;

        public ICommand ConvertCommand { get; }

        public ConvertViewModel()
        {
            ConvertCommand = new RelayCommand(async (o) => await ConvertCryptoCurrenciesAsync());
        }

        public string Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public string CryptoName
        {
            get { return cryptoName; }
            set
            {
                cryptoName = value;
                OnPropertyChanged();
            }
        }

        public string ConvertTo
        {
            get { return convertTo; }
            set
            {
                convertTo = value;
                OnPropertyChanged();
            }
        }

        public string ConversionResult
        {
            get { return conversionResult; }
            set
            {
                conversionResult = value;
                OnPropertyChanged();
            }
        }

        public async Task ConvertCryptoCurrenciesAsync()
        {
            decimal cryptoPrice = await GetCurrencyPrice(CryptoName);
            decimal convertToPrice = await GetCurrencyPrice(ConvertTo);

            if (decimal.TryParse(Amount, out decimal amount) && amount > 0)
            {
                if (cryptoPrice != 0 && convertToPrice != 0)
                {
                    decimal convertedAmount = (amount / convertToPrice) * cryptoPrice;
                    ConversionResult = convertedAmount.ToString("F3");
                }
                else
                {
                    ConversionResult = "Crypto not found or API limit reached";
                }
            }
            else
            {
                ConversionResult = "Invalid amount";
            }
        }


        public async Task<decimal> GetCurrencyPrice(string currencyId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.coingecko.com/api/v3/search?query={currencyId}");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    CryptoCurrencySearchResponse searchResponse = JsonSerializer.Deserialize<CryptoCurrencySearchResponse>(responseBody);

                    if (searchResponse.coins.Count == 0)
                        return 0;

                    HttpResponseMessage getResponse = await client.GetAsync($"https://api.coingecko.com/api/v3/coins/{searchResponse.coins.First().id}");
                    getResponse.EnsureSuccessStatusCode();
                    string usdPriceResponseBody = await getResponse.Content.ReadAsStringAsync();
                    CryptoCurrencyInfoResponse infoResponse = JsonSerializer.Deserialize<CryptoCurrencyInfoResponse>(usdPriceResponseBody);

                    if (infoResponse.market_data != null)
                    {
                        MarketData marketData = infoResponse.market_data;
                        if (marketData.current_price != null && marketData.current_price.ContainsKey("usd"))
                        {
                            return marketData.current_price["usd"];
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    Trace.WriteLine($"Error: {ex.Message}");
                }
            }
            return 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
