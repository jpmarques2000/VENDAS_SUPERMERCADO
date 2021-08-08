using Newtonsoft.Json;
using RestSharp;
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

                var client = new RestClient("https://products-mart-api.herokuapp.com/products");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "super-secret");
                IRestResponse response = await client.ExecuteAsync(request);
                var result = response.Content;
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
