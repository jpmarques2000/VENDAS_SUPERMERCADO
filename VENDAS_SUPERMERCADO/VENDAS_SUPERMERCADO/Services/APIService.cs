using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VENDAS_SUPERMERCADO.Services
{
    public class APIService
    {
        public async Task<List<T>> Get<T>(string controller)
        {
            try
            {

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://products-mart-api.herokuapp.com/products"),

                    Headers =
                    {
                    { "Authorization", "super-secret" },
                    },
                };
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    return null;

                }

                var result = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<List<T>>(result);
                return list;

            }
            catch
            {
                return null;
            }
        }
    }
}
