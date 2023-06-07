using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.Http;
using System.Diagnostics;
using CryptoAppWpf;
using System;

namespace CryptoAppWpf
{
    internal class ViewModel
    {
        private RelayCommand fetchCommand;
        private ObservableCollection<CryptoCurrency> cryptoCurrencies = new ObservableCollection<CryptoCurrency>();
        public IEnumerable<CryptoCurrency> CryptoCurrencies => cryptoCurrencies;
        public CryptoCurrency CurrentCryptoCurrency { get; set; }
        public CryptoCurrency SelectedCryptoCurrency { get; set; }
        public ViewModel()
        {
            fetchCommand = new RelayCommand(async (o) => await FetchCryptoCurrenciesAsync());
            CurrentCryptoCurrency = new CryptoCurrency();
            FetchCryptoCurrenciesAsync();
        }
        public ICommand FetchCommand => fetchCommand;
        public async Task FetchCryptoCurrenciesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("https://api.coincap.io/v2/assets");
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Trace.WriteLine(responseBody);
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseBody, options);
                        List<CryptoCurrency> fetchedCryptoCurrencies = apiResponse?.Data;
                        if (fetchedCryptoCurrencies != null)
                        {
                            foreach (CryptoCurrency currency in fetchedCryptoCurrencies)
                            {
                                Trace.WriteLine($"ID: {currency.Id}");
                                Trace.WriteLine($"Name: {currency.Name}");
                                Trace.WriteLine($"Rank: {currency.Rank}");
                                Trace.WriteLine($"Price USD: {currency.PriceUsd}");
                            }
                        }
                        else
                        {
                            Trace.WriteLine("Failed to deserialize the API response.");
                        }
                        if (fetchedCryptoCurrencies != null)
                        {
                            foreach (CryptoCurrency currency in fetchedCryptoCurrencies)
                            {
                                cryptoCurrencies.Add(currency);
                            }
                        }
                    }
                    else
                    {
                        Trace.WriteLine("Request failed with status code: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
internal class ApiResponse
{
    public List<CryptoCurrency> Data { get; set; }
}