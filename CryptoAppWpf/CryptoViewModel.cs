using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.Diagnostics;
using System;
using System.Linq;
using CryptoAppWpf.Model;
using System.Reflection.Metadata;

namespace CryptoAppWpf
{
    public class CryptoViewModel
    {
        private RelayCommand fetchCommand;
        private RelayCommand searchCommand;
        public string SearchText { get; set; }
        static private ObservableCollection<CryptoCurrency> cryptoCurrencies = new ObservableCollection<CryptoCurrency>();
        public IEnumerable<CryptoCurrency> CryptoCurrencies => cryptoCurrencies;

        public CryptoViewModel()
        {
            fetchCommand = new RelayCommand(async (o) => await FetchCryptoCurrenciesAsync());
            searchCommand = new RelayCommand(async (o) => await SearchCryptoCurrenciesAsync());
        }
        public ICommand FetchCommand => fetchCommand;        
        public ICommand SearchCommand => searchCommand;

        public async Task FetchCryptoCurrenciesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    cryptoCurrencies.Clear();
                    HttpResponseMessage response = await client.GetAsync("https://api.coingecko.com/api/v3/search/trending");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    CryptoCurrencyApiResponse apiResponse = JsonSerializer.Deserialize<CryptoCurrencyApiResponse>(responseBody);

                    foreach (CoinData coinData in apiResponse.coins)
                    {
                        CryptoCurrency currency = coinData.item;
                        AddCryptoInfo(currency);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Trace.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        public async Task SearchCryptoCurrenciesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    cryptoCurrencies.Clear();
                    string searchQuery = SearchText;
                    HttpResponseMessage response = await client.GetAsync($"https://api.coingecko.com/api/v3/search?query={searchQuery}");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    CryptoCurrencySearchResponse searchResponse = JsonSerializer.Deserialize<CryptoCurrencySearchResponse>(responseBody);

                    foreach (CryptoCurrencySearchResult result in searchResponse.coins)
                    {
                        CryptoCurrency currency = new CryptoCurrency
                        {
                            name = result.name,
                            symbol = result.symbol,
                            id = result.id,
                            large = result.large,
                        };

                        AddCryptoInfo(currency);
                    }
                }
                catch (HttpRequestException ex)
                {
                    Trace.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        private async void AddCryptoInfo(CryptoCurrency currency)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync($"https://api.coingecko.com/api/v3/coins/{currency.id}");
                    response.EnsureSuccessStatusCode();
                    string usdPriceResponseBody = await response.Content.ReadAsStringAsync();
                    CryptoCurrencyInfoResponse infoResponse = JsonSerializer.Deserialize<CryptoCurrencyInfoResponse>(usdPriceResponseBody);
                    if (infoResponse.market_cap_rank != null)
                    {
                        currency.market_cap_rank = infoResponse.market_cap_rank;
                    }
                    if (infoResponse.tickers != null)
                    {
                        currency.tickers = infoResponse.tickers;
                    }
                    if (infoResponse.market_data != null)
                    {
                        MarketData marketData = infoResponse.market_data;

                        if (marketData.current_price != null && marketData.current_price.ContainsKey("usd"))
                        {
                            decimal usdPrice = marketData.current_price["usd"];
                            currency.PriceUSD = usdPrice;
                        }
                        currency.price_change_percentage_24h = marketData.price_change_percentage_24h;
                        currency.price_change_percentage_7d = marketData.price_change_percentage_7d;
                        currency.price_change_percentage_30d = marketData.price_change_percentage_30d;
                        currency.price_change_percentage_1y = marketData.price_change_percentage_1y;
                    }

                    if (infoResponse.links != null && infoResponse.links.homepage != null && infoResponse.links.homepage.Count > 0)
                    {
                        currency.homepage = infoResponse.links.homepage[0];
                    }

                    cryptoCurrencies.Add(currency);
                }
                catch (HttpRequestException ex)
                {
                    Trace.WriteLine($"Error: {ex.Message}");
                }
            }
        }
    }
}