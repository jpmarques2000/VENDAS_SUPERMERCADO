using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.Views;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private NetService netService;

        private APIService apiService;
        public ObservableCollection<ProductItemViewModel> Products { get; set; }

        private static MainViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged;

        List<string> Departments = new List<string>();

        private string productFilter;
        public ICommand SearchProductCommand { get { return new RelayCommand(SearchProduct); } }

        public ProductsPage productDPT;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        public string ProductFilter
        {
            set
            {
                if (productFilter != value)
                {
                    productFilter = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ProductFilter"));
                }

                if (string.IsNullOrEmpty(productFilter))
                {
                    LoadProducts();
                }
            }
            get
            {
                return productFilter;
            }
        }

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

            //    ReloadProducts(products);

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
            Departments.Clear();

            foreach (var product in products.OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_codigo = product.pro_codigo,
                    pro_nome = product.pro_nome,
                    Preco = product.Preco,
                    Promocao = product.Promocao,
                    categoria = product.categoria,
                    Precopromocao = product.Precopromocao,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipoEmbalagem = product.tipoEmbalagem
                });

                Departments.Add(product.departamento.ToString());
            }
        }

        private async void SearchProduct()
        {
      //      var dptSelecionado = productDPT.departamentoFiltro();
            var products = new List<Products>();
            products = await apiService.Get<Products>("products");
            filterProducts(products, ProductFilter);

        }

        public void filterProducts(List<Products> products, string filter)
        {
            Products.Clear();

            foreach (var product in products.Where(p => p.pro_nome.ToUpper().Contains(filter.ToUpper())) .OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_codigo = product.pro_codigo,
                    pro_nome = product.pro_nome,
                    Preco = product.Preco,
                    Promocao = product.Promocao,
                    categoria = product.categoria,
                    Precopromocao = product.Precopromocao,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipoEmbalagem = product.tipoEmbalagem
                });
            }
        }


    }
}
