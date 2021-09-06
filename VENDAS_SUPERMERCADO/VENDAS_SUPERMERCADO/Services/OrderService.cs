using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;

namespace VENDAS_SUPERMERCADO.Services
{
    public class OrderService
    {
        FirebaseClient client;

        public OrderService()
        {
            client =
                new FirebaseClient("https://tcc-vendas-supermercado-default-rtdb.firebaseio.com/");
        }

        public async Task<bool> CreateNewOrder(string emailUser, string bairro, string cep, DateTime data, string nome, 
            string observacao, string rua, double valorTotal, string telefone, string pagamento, string dataEntrega)
        {
            await client.Child("Order")
                 .PostAsync(new Order()
                 {
                     email = emailUser,
                     bairro = bairro,
                     cep = cep,
                     data = data,
                     nome = nome,
                     observacao = observacao,
                     rua = rua,
                     valorTotalDB = valorTotal,
                     telefone = telefone,
                     pagamento = pagamento,
                     dataEntrega = dataEntrega,
                 });
            return true;
        }

    }
}
