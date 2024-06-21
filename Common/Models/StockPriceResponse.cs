using Newtonsoft.Json;

namespace Common.Models
{
    public class MetaData
    {
        [JsonProperty("1. Information")]
        public string Information { get; set; }

        [JsonProperty("2. Symbol")]
        public string Symbol { get; set; }

        [JsonProperty("3. Last Refreshed")]
        public DateTime LastRefreshed { get; set; }

        [JsonProperty("4. Interval")]
        public string Interval { get; set; }

        [JsonProperty("5. Output Size")]
        public string OutputSize { get; set; }

        [JsonProperty("6. Time Zone")]
        public string TimeZone { get; set; }
    }

    public class TimeSeriesEntry
    {
        [JsonProperty("1. open")]
        public decimal Open { get; set; }

        [JsonProperty("2. high")]
        public decimal High { get; set; }

        [JsonProperty("3. low")]
        public decimal Low { get; set; }

        [JsonProperty("4. close")]
        public decimal Close { get; set; }

        [JsonProperty("5. volume")]
        public int Volume { get; set; }
    }

    public class StockData
    {
        [JsonProperty("Meta Data")]
        public MetaData MetaData { get; set; }

        [JsonProperty("Time Series (5min)")]
        public Dictionary<string, TimeSeriesEntry> TimeSeries { get; set; }
    }
    public class StockPriceResponse
    {
        public StockPriceResponse(string symbol, TimeSeriesEntry entry)
        {
            Symbol = symbol;
            Price = entry;
        }
        public string Symbol { get; private set; }
        public TimeSeriesEntry Price { get; private set; }
    }
}
