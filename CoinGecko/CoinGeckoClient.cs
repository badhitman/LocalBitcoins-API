using MetadataEntityModel.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CoinGeckoApi
{
    public class CoinGeckoClient
    {
        private string BaseUrl = "https://api.coingecko.com/api/v3/";
        public async Task<CoinGeckoSimplePriceModel> getPrice(string vs_currencies = "rub", string ids = "bitcoin")
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(BaseUrl + "simple/price?vs_currencies=" + vs_currencies + "&ids=" + ids))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseBody)))
                        {
                            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CoinGeckoSimplePriceModel));
                            CoinGeckoSimplePriceModel bsObj2 = (CoinGeckoSimplePriceModel)deserializer.ReadObject(ms);
                            bsObj2.time = DateTime.Now;
                            responseBody = null;
                            return bsObj2;
                        }
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
            return null;
        }
    }
}
