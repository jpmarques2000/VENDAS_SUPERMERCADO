﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VENDAS_SUPERMERCADO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public string departamentoFiltro { get; set; }

        private Products productSelected;

        public ProductsPage()
        {
            
            InitializeComponent();

        }

        private void pckDPT_SelectedIndexChanged(object sender, EventArgs e)
        {
            departamentoFiltro = pckDPT.Items[pckDPT.SelectedIndex];
            DisplayAlert(departamentoFiltro, "Departamento selecionado", "OK");
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
           int.TryParse(((Button)sender).BindingContext.ToString(), out var codigo);

            
            if (codigo == 0)
                return;
            if (MeuCarrinho.Lista == null) 
                MeuCarrinho.Lista = new List<ItemsOrder>();
            
            var produtoVM = MainViewModel.GetInstance().Products.AsQueryable().Where(x => x.pro_codigo == codigo).FirstOrDefault();

            var produto = MeuCarrinho.Lista.Find(x => x.codigoProduto == codigo);
            if (produto == null)
            {
                var novoProduto = new ItemsOrder();
                novoProduto.codigoProduto = codigo;
                novoProduto.qtde = 1;
                novoProduto.pro_nome = produtoVM.pro_nome;
                novoProduto.unitario = produtoVM.Preco;
                novoProduto.valorTotal = produtoVM.Preco;
                novoProduto.custo = produtoVM.custo;
                MeuCarrinho.Lista.Add(novoProduto);
                return;
            }
            MeuCarrinho.Lista.Find(x => x.codigoProduto == codigo).qtde++;
        }

        private void OnButtonClicked2(object sender, EventArgs e)
        {
            string data = ((Button)sender).BindingContext as string;
        //    productSelected = sender;
            int.TryParse(((Button)sender).BindingContext.ToString(), out var codigo);

            if (codigo == 0)
                return;
            if (MeuCarrinho.Lista == null)
                return;
            var produto = MeuCarrinho.Lista.Find(x => x.codigoProduto == codigo);
            if (produto == null)
                return;
            var item = MeuCarrinho.Lista.FindIndex(x => x.codigoProduto == codigo);
            if (produto.qtde == 1 )
            {
                MeuCarrinho.Lista.RemoveAt(item);
            }
            else
            {
                MeuCarrinho.Lista.Find(x => x.codigoProduto == codigo).qtde--;
            }
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
          

        }
    }
}