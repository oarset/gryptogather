using Application.Models;
using Application.Models.Currencies;
using Elasticsearch.Net;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Application
{
    public class Gatherer : BackgroundService
    {
        private static readonly ConnectionConfiguration _esSettings = new ConnectionConfiguration(new Uri("http://localhost:9200")).RequestTimeout(TimeSpan.FromMinutes(2));
        private static HttpClient Client = new HttpClient();        
        private static ElasticLowLevelClient EsClient = new ElasticLowLevelClient(_esSettings);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await GatherBTCAsync(cancellationToken);
                await GatherAsync(cancellationToken, "ETH");
                await GatherAsync(cancellationToken, "IOT");

                await Task.Delay(TimeSpan.FromSeconds(60), cancellationToken);
            }
        }

        private async Task GatherBTCAsync(CancellationToken cancellationToken)
        {
            var resultBTC = await Client.GetAsync("https://api.bitfinex.com/v2/ticker/tBTCUSD");
            var test = new PricesResponse();
            if (resultBTC.IsSuccessStatusCode)
            {
                // var response = JsonConvert.DeserializeObject<PricesResponse>(resultETH.Content.ReadAsStringAsync().Result);
                var response = resultBTC.Content.ReadAsStringAsync().Result;
                var data = Array.ConvertAll(response.Trim('[', ']').Split(','), s => float.Parse(s));
                var ticker = ParseTicker(data);

                var asyncIndexResponseBTC = await EsClient.IndexAsync<string>("btc", "Currency", Guid.NewGuid().ToString(), ticker);
                string responseString = asyncIndexResponseBTC.Body;
            }
        }

        private async Task GatherAsync(CancellationToken cancellationToken, string currency)
        {
            var url = "https://api.bitfinex.com/v2/ticker/t" + currency +"BTC";
            var result = await Client.GetAsync(url);
            var test = new PricesResponse();
            if (result.IsSuccessStatusCode)
            {
                // var response = JsonConvert.DeserializeObject<PricesResponse>(resultETH.Content.ReadAsStringAsync().Result);
                var response = result.Content.ReadAsStringAsync().Result;
                var data = Array.ConvertAll(response.Trim('[', ']').Split(','), s => float.Parse(s));
                var ticker = ParseTicker(data);

                var asyncIndexResponse = await EsClient.IndexAsync<string>(currency.ToLower(), "Currency", Guid.NewGuid().ToString(), ticker);
                string responseString = asyncIndexResponse.Body;
            }
        }

        private TickerResponse ParseTicker(float[] array)
        {
            var ticker = new TickerResponse();
            ticker.TimeStamp = DateTime.Now;
            ticker.Bid = array[0];
            ticker.BidSize = array[1];
            ticker.Ask = array[2];
            ticker.AskSize = array[3];
            ticker.DailyChange = array[4];
            ticker.DailyChangePerc = array[5];
            ticker.LastPrize = array[6];
            ticker.Volume = array[7];
            ticker.High = array[8];
            ticker.Low = array[9];

            return ticker;
        }
    }
}
