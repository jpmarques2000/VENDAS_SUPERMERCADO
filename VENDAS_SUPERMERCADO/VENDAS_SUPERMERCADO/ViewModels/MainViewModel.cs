using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class MainViewModel
    {
        private NetService netService;

        private APIService apiService;
        public ObservableCollection<ProductItemViewModel> Products { get; set; }

        private static MainViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            instance = this;
            apiService = new APIService();
            netService = new NetService();
            Products = new ObservableCollection<ProductItemViewModel>();
            //Customers = new ObservableCollection<CustomerItemViewModel>();
            //CurrentCustomer = new CustomerItemViewModel();

            //NewLogin = new LoginViewModel();
            //UserLoged = new UserViewModel();
            LoadProducts();
            //LoadCustomers();
        }
        public async void LoadProducts()
        {
           
            var products = new List<Products>();

            //if (netService.IsConnected())
            //{
            //    products = await apiService.Get<Products>("products");

            //}
            //else
            //{
            //    // Jogar mensagem "Necessario conexao com a internet para carregar produtos"
            //}
            products = await apiService.Get<Products>("products");

            ReloadProducts(products);
        }
        public void ReloadProducts(List<Products> products)
        {
            Products.Clear();

            foreach (var product in products.OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_nome = product.pro_nome,
                    Preco = product.Preco,
                });
            }
        }
    }
}
