using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;

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

        public static async Task<bool> Post(Order order)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var client = new RestClient("https://products-mart-api.herokuapp.com/sales");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "super-secret");
                request.AddHeader("Content-Type", "application/json");
                var body = json;
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode != HttpStatusCode.Created) 
                {
                    throw new Exception("Erro ao gravar os dados na api ! Tente novamente");
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<Horarios>> GetSchedules()
        {
            try
            {
                var client = new RestClient("https://shrouded-plains-66019.herokuapp.com");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = await client.ExecuteAsync(request);
                var result = response.Content;
                var list = JsonConvert.DeserializeObject<List<Horarios>>(result);
                return list;

            }
            catch
            {
                return null;
            }
        }
    }
}
