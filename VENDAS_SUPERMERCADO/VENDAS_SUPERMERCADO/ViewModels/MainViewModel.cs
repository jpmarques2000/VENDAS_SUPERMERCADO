using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.Views;
using static System.Net.Mime.MediaTypeNames;

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

        public UserViewModel UserLoged { get; set; }

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
            UserLoged = new UserViewModel();
            //Customers = new ObservableCollection<CustomerItemViewModel>();
            //CurrentCustomer = new CustomerItemViewModel();

            //NewLogin = new LoginViewModel();
            //UserLoged = new UserViewModel();
            //LoadCustomers();
        }
        public async Task LoadProducts()
        {
           
            var products = new List<Products>();

            if (netService.IsConnected())
            {
                products = await apiService.Get<Products>("products");

                if (products != null)
                {
                    ReloadProducts(products);
                }

            }
            else
            {
               
            }
            //products = await apiService.Get<Products>("products");

            //if (products != null)
            //{
            //    ReloadProducts(products);
            //}

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
            if (products != null)
            {
                filterProducts(products, ProductFilter);
            }
            
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

        public void LoadUser(User user)
        {
            UserLoged.id = user.id;
            UserLoged.nome = user.nome;
            UserLoged.bairro = user.bairro;
            UserLoged.cep = user.cep;
            UserLoged.dataNascimento = user.dataNascimento;
            UserLoged.email = user.email;
            UserLoged.numero = user.numero;
            UserLoged.rua = user.rua;
            UserLoged.telefone = user.telefone;
        }


    }
}
