using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppWpf.Model
{
    public class Links
    {
        public List<string> homepage { get; set; }
    }
    public class CoinData
    {
        public CryptoCurrency item { get; set; }
    }

    public class CryptoCurrencyApiResponse
    {
        public List<CoinData> coins { get; set; }
        public List<object> nfts { get; set; }
        public List<object> exchanges { get; set; }
    }

    public class CryptoCurrencyInfoResponse
    {
        public MarketData market_data { get; set; }
        public int? market_cap_rank { get; set; }
        public Links links { get; set; }
        public List<TickerInfo> tickers { get; set; }
    }

    public class MarketData
    {
        public Dictionary<string, decimal> current_price { get; set; }
        public decimal? price_change_percentage_24h { get; set; }
        public decimal? price_change_percentage_7d { get; set; }
        public decimal? price_change_percentage_30d { get; set; }
        public decimal? price_change_percentage_1y { get; set; }
    }

    public class CryptoCurrencySearchResult
    {
        public string name { get; set; }
        public string symbol { get; set; }
        public string id { get; set; }
        public string large { get; set; }
    }

    public class CryptoCurrencySearchResponse
    {
        public List<CryptoCurrencySearchResult> coins { get; set; }
    }
    public class TickerInfo
    {
        public decimal last { get; set; }
        public decimal volume { get; set; }
        public string trust_score { get; set; }
        public MarketInfo market { get; set; }
    }
    public class MarketInfo
    {
        public string name { get; set; }
        public string identifier { get; set; }
        public bool has_trading_incentive { get; set; }
    }
}
