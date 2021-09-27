using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<bool> CreateNewOrder(string emailUser, string bairro, string cep, string data, string nome, 
            string observacao, string rua, double valorTotal, string telefone, string pagamento, 
            string dataEntrega, string cpf, DateTime dateFB)
        {
            await client.Child("Order")
                 .PostAsync(new Order()
                 {
                     email = emailUser,
                     bairro = bairro,
                     cep = cep,
                     data = data,
                     cliente = nome,
                     observacao = observacao,
                     rua = rua,
                     valor_total_pedido = valorTotal,
                     telefone = telefone,
                     pagamento = pagamento,
                     data_entrega = dataEntrega,
                     cpf = cpf,
                     dateFB = dateFB
                 });
            return true;
        }
        public async Task<bool> SaveItemsOrder(List<ItemsOrder> itemsOrderList, string dataPedido)
        {
            foreach(var itemsOrder in itemsOrderList)
            {
                await client.Child("ItemsOrder")
                 .PostAsync(new ItemsOrder()
                 {
                     codigoProduto = itemsOrder.codigoProduto,
                     qtde = itemsOrder.qtde,
                     unitario = itemsOrder.unitario,
                     valorTotal = itemsOrder.qtde * itemsOrder.unitario,
                     data = dataPedido,
                     custo = itemsOrder.custo,
                     id = itemsOrder.id,
                     pro_nome = itemsOrder.pro_nome
                     
                 });
            }
            return true;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return (await client
                .Child("Order")
                .OnceAsync<Order>()).Select(item => new Order
                {
                    data_entrega = item.Object.data_entrega,
                    cliente = item.Object.cliente,
                    numeroPedido = item.Object.numeroPedido,
                    observacao = item.Object.observacao,
                    pagamento = item.Object.pagamento,
                    valor_total_pedido = item.Object.valor_total_pedido,
                    data = item.Object.data,
                    email = item.Object.email
                }).ToList();
        }
        //public async Task<Order> GetOrderList(string username)
        //{
        //    var allOrders = await GetAllOrders();
        //    /*
        //    await client
        //      .Child("Users")
        //      .OnceAsync<User>();
        //    */
        //    var umOrder = allOrders.Where(a => a.email == username)
        //        .ToList();

        //    return umOrder;
        //}

        public async Task<List<ItemsOrder>> GetOrderDetails()
        {
            return (await client
                .Child("ItemsOrder")
                .OnceAsync<ItemsOrder>()).Select(item => new ItemsOrder
                {
                    codigoProduto = item.Object.codigoProduto,
                    custo = item.Object.custo,
                    data = item.Object.data,
                    desconto = item.Object.desconto,
                    id = item.Object.id,
                    qtde = item.Object.qtde,
                    unitario = item.Object.unitario,
                    valorTotal = item.Object.valorTotal,
                    pro_nome = item.Object.pro_nome
                }).ToList();
        }

    }
}
