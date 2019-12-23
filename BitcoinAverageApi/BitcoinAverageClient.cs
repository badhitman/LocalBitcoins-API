using BitcoinAverageApi.Model;
using MetadataEntityModel.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinAverageApi
{
    public class BitcoinAverageClient
    {
        private string BaseUrl = "https://apiv2.bitcoinaverage.com/";
        public async Task<BitcoinAverageConvertModel> getConvert(double amount, string from = "RUB", string to = "BTC", SymbolSetsEnum set_symbol = SymbolSetsEnum.global)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    using (HttpResponseMessage response = await client.GetAsync(BaseUrl + "convert/" + set_symbol.ToString() + "?from=" + from + "&to=" + to + "&amount=" + amount))
                    {
                        response.EnsureSuccessStatusCode();
                        string responseBody = await response.Content.ReadAsStringAsync();
                        using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(responseBody)))
                        {
                            DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(BitcoinAverageConvertModel));
                            BitcoinAverageConvertModel bsObj2 = (BitcoinAverageConvertModel)deserializer.ReadObject(ms);
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
