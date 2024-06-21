using Common.Models;
using Newtonsoft.Json;

namespace Common.Client
{
    public class StockClient
    {
        private readonly HttpClient _httpClient;
        public StockClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<StockPriceResponse> GetDataForTicker(string ticker)
        {
            var tickerDataString = await _httpClient.GetStringAsync($"?function=TIME_SERIES_INTRADAY&symbol={ticker}&interval=5min&apikey=UOB842P4MLGQKO10");

            var tickerData = JsonConvert.DeserializeObject<StockData>(tickerDataString);

            return new StockPriceResponse(tickerData.MetaData.Symbol, tickerData.TimeSeries.FirstOrDefault().Value);
        }
    }
}
