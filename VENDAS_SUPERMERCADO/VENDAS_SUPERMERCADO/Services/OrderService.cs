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

        public OrderFirebase CreateOrder(Order orderApi, List<ItemsOrder> itemsOrderList)
        {
            var order = new OrderFirebase
            {
                email = orderApi.email,
                bairro = orderApi.bairro,
                cep = orderApi.cep,
                data = orderApi.data,
                cliente = orderApi.cliente,
                observacao = orderApi.observacao,
                rua = orderApi.rua,
                valor_total_pedido = orderApi.valor_total_pedido,
                telefone = orderApi.telefone,
                pagamento = orderApi.pagamento,
                data_entrega = orderApi.data_entrega,
                cpf = orderApi.cpf,
                dateFB = orderApi.dateFB,
                complemento = orderApi.complemento,
                ItemsOrder = itemsOrderList
                
            };
            return order;
        }

       

        public async Task<bool> InserteNewOrder(OrderFirebase order)
        {
            var response = await client.Child("Order").PostAsync(order);
            return true;
        }
       

        public async Task<List<OrderFirebase>> GetAllOrders()
        {
            var lista =  (await client
                .Child("Order")
                .OnceAsync<OrderFirebase>()).Select(item => new OrderFirebase
                {
                    data_entrega = item.Object.data_entrega,
                    cliente = item.Object.cliente,
                    numeroPedido = item.Object.numeroPedido,
                    observacao = item.Object.observacao,
                    pagamento = item.Object.pagamento,
                    valor_total_pedido = item.Object.valor_total_pedido,
                    data = item.Object.data,
                    email = item.Object.email,
                    ItemsOrder = item.Object.ItemsOrder
                }).ToList();
            return lista;
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
