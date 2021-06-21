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
    public class FirebaseService
    {
        FirebaseClient firebase = new FirebaseClient("https://tcc-vendas-supermercado-default-rtdb.firebaseio.com/");

        //public async Task AddProduto(int produtoid, string nome, double preco, bool promocao, double precopromocao, decimal estoque)
        //{
        //    await firebase.Child("Produtos").PostAsync(new Products() { Produtoid = produtoid, Nome = nome, Preco = preco, Promocao = promocao, Precopromocao = precopromocao, Estoque = estoque });
        //}

        //public async Task<List<Products>> GetProdutos()
        //{

        //    return (await firebase.Child("Produtos")
        //        .OnceAsync<Products>()).Select(item => new Products
        //        {
        //            Produtoid = item.Object.Produtoid,
        //            Nome = item.Object.Nome,
        //            Preco = item.Object.Preco,
        //            Promocao = item.Object.Promocao,
        //            Precopromocao = item.Object.Precopromocao,
        //            Estoque = item.Object.Estoque


        //        }).ToList();

        //}

        
    }
}
