using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.Diagnostics;
using CryptoAppWpf;
using System;
using System.Linq;

namespace CryptoAppWpf
{
    internal class ViewModel
    {
        private RelayCommand fetchCommand;
        private RelayCommand searchCommand;
        public string SearchText { get; set; }
        private ObservableCollection<CryptoCurrency> cryptoCurrencies = new ObservableCollection<CryptoCurrency>();
        public IEnumerable<CryptoCurrency> CryptoCurrencies => cryptoCurrencies;
        public CryptoCurrency CurrentCryptoCurrency { get; set; }
        public CryptoCurrency SelectedCryptoCurrency { get; set; }
        public ViewModel()
        {
            fetchCommand = new RelayCommand(async (o) => await FetchCryptoCurrenciesAsync());
            searchCommand = new RelayCommand(async (o) => await SearchCryptoCurrenciesAsync());
            CurrentCryptoCurrency = new CryptoCurrency();
            FetchCryptoCurrenciesAsync();
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
                    HttpResponseMessage usdPriceResponse = await client.GetAsync($"https://api.coingecko.com/api/v3/coins/{currency.id}");
                    usdPriceResponse.EnsureSuccessStatusCode();
                    string usdPriceResponseBody = await usdPriceResponse.Content.ReadAsStringAsync();
                    CryptoCurrencyPriceResponse priceResponse = JsonSerializer.Deserialize<CryptoCurrencyPriceResponse>(usdPriceResponseBody);

                    if (priceResponse.market_data != null)
                    {
                        MarketData marketData = priceResponse.market_data;

                        if (marketData.current_price != null && marketData.current_price.ContainsKey("usd"))
                        {
                            decimal usdPrice = marketData.current_price["usd"];
                            currency.PriceUSD = usdPrice;
                            Trace.WriteLine($"Price (USD): {usdPrice}");
                        }
                        currency.price_change_percentage_24h = marketData.price_change_percentage_24h;
                        currency.price_change_percentage_7d = marketData.price_change_percentage_7d;
                        currency.price_change_percentage_30d = marketData.price_change_percentage_30d;
                        currency.price_change_percentage_1y = marketData.price_change_percentage_1y;
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
internal class CoinData
{
    public CryptoCurrency item { get; set; }
}

internal class CryptoCurrencyApiResponse
{
    public List<CoinData> coins { get; set; }
    public List<object> nfts { get; set; }
    public List<object> exchanges { get; set; }
}

internal class CryptoCurrencyPriceResponse
{
    public MarketData market_data { get; set; }
}

internal class MarketData
{
    public Dictionary<string, decimal> current_price { get; set; }
    public decimal price_change_percentage_24h { get; set; }
    public decimal price_change_percentage_7d { get; set; }
    public decimal price_change_percentage_30d { get; set; }
    public decimal price_change_percentage_1y { get; set; }
}

internal class CryptoCurrencySearchResult
{
    public string name { get; set; }
    public string symbol { get; set; }
    public string id { get; set; }
    public string large { get; set; }
}

internal class CryptoCurrencySearchResponse
{
    public List<CryptoCurrencySearchResult> coins { get; set; }
}