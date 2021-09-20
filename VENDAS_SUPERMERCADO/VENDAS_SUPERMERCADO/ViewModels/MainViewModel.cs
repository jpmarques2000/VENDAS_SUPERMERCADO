using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.Views;
using Xamarin.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private NetService netService;

        private readonly INavigationService _navigationService;

        private APIService apiService;

     //   ListDepartaments = new ObservableCollection<string>();
        public ObservableCollection<ProductItemViewModel> Products { get; set; }

        private static MainViewModel instance;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string productFilter;
        public ICommand SearchProductCommand { get { return new RelayCommand(SearchProduct); } }
        public ICommand MyCartCommand { get; private set; }

        public ICommand DepartamentCommand { get; private set; }

        public ICommand FilterCommand { get; set; }

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
            _navigationService = DependencyService.Get<INavigationService>();
            MyCartCommand = new Command(MyCartCmd);
            DepartamentCommand = new Command(DepartamentsCmd);
            FilterCommand = new Command(MyFilterCmd);
            ListDepartaments = new ObservableCollection<string>();
            ListOrderBy = new ObservableCollection<string>();
            loadFilter();
            //Customers = new ObservableCollection<CustomerItemViewModel>();
            //CurrentCustomer = new CustomerItemViewModel();

            //NewLogin = new LoginViewModel();
            //UserLoged = new UserViewModel();
            //LoadCustomers();
        }

        private async void MyFilterCmd()
        {
            Filter.filterDepartament = Departaments;
            Filter.filterOrder = OrderBy;
            if (Filter.filterDepartament != null)
            {
                if (Filter.filterDepartament != "Todos")
                {
                    var products = new List<Products>();
                    products = await apiService.Get<Products>("products");
                    filterDepartaments(products, Filter.filterDepartament);
                    await this._navigationService.NavitaToProductFilter();
                }
                else
                {
                    await LoadProducts();
                    await this._navigationService.NavitaToProductFilter();
                }
            }
            else if(Filter.filterOrder != "")
            {
                var products = new List<Products>();
                products = await apiService.Get<Products>("products");
                FilterOrders(products, Filter.filterOrder);
                await this._navigationService.NavitaToProductFilter();
            }
            else
            {
                await LoadProducts();
                await this._navigationService.NavitaToProductFilter();
            }
            
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

        }
        public void ReloadProducts(List<Products> products)
        {

            Products.Clear();
            ListDepartaments.Clear();

            foreach (var product in products.OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_codigo = product.pro_codigo,
                    pro_nome = product.pro_nome,
                    preco = product.preco,
                    desconto = product.desconto,
                    categoria = product.categoria,
                    preco_desconto = product.preco_desconto,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipo_embalagem = product.tipo_embalagem,
                    departamento = product.departamento,
                    id = product.id
                });

            }
            ListDepartaments.Add("Todos");
            foreach (var produto in products)
            {
                if (ListDepartaments.Contains(produto.departamento))
                    continue;
                if(produto.departamento != "ACOGUE")
                    ListDepartaments.Add(produto.departamento);
            }

            
        }

        public void FilterOrders(List<Products> products, string filter)
        {

            Products.Clear();

            foreach (var product in products.OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_codigo = product.pro_codigo,
                    pro_nome = product.pro_nome,
                    preco = product.preco,
                    desconto = product.desconto,
                    categoria = product.categoria,
                    preco_desconto = product.preco_desconto,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipo_embalagem = product.tipo_embalagem,
                    departamento = product.departamento,
                    id = product.id
                });

            }

            if(filter != "Preço Crescente")
            {
                products = products.OrderByDescending(p => p.preco).ToList();
            }
            else
            {
                products = products.OrderBy(p => p.preco).ToList();
            }

        }

        private async void SearchProduct()
        {
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
                    preco = product.preco,
                    desconto = product.desconto,
                    categoria = product.categoria,
                    preco_desconto = product.preco_desconto,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipo_embalagem = product.tipo_embalagem,
                    departamento = product.departamento,
                    id = product.id
                });
            }

            products = products.OrderByDescending(p => p.preco).ToList();

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

        private void MyCartCmd()
        {

            this._navigationService.NavigateToMyCart();

        }

        private void DepartamentsCmd()
        {

            this._navigationService.NavigateToFilterView();

        }

        ObservableCollection<string> _listDepartaments;
        public ObservableCollection<string> ListDepartaments
        {
            get
            {
                return _listDepartaments;
            }
            set
            {
                _listDepartaments = value;
                OnPropertyChanged();
            }
        }
        string _departaments;
        public string Departaments
        {
            get
            {
                return _departaments;
            }
            set
            {
                _departaments = value;
                OnPropertyChanged();
            }
        }
        string _departamentSelected;
        public string DepartamentSelected
        {
            get
            {
                return _departamentSelected;
            }
            set
            {
                _departamentSelected = value;
                OnPropertyChanged();
            }
        }

        ObservableCollection<string> _listOrderBy;
        public ObservableCollection<string> ListOrderBy
        {
            get
            {
                return _listOrderBy;
            }
            set
            {
                _listOrderBy = value;
                OnPropertyChanged();
            }
        }
        string _orderBy;
        public string OrderBy
        {
            get
            {
                return _orderBy;
            }
            set
            {
                _orderBy = value;
                OnPropertyChanged();
            }
        }
        string _orderBySelected;
        public string OrderBySelected
        {
            get
            {
                return _orderBySelected;
            }
            set
            {
                _orderBySelected = value;
                OnPropertyChanged();
            }
        }

        bool _chkpromocao;
        public bool chkPromocao
        {
            get
            {
                return _chkpromocao;
            }
            set
            {
                _chkpromocao = value;
                OnPropertyChanged();
            }
        }

        private void loadFilter()
        {
            
            //ListDepartaments.Add("HORTI FRUTI");
            //ListDepartaments.Add("BEBIDAS");
            //ListDepartaments.Add("PADARIA");
            //ListDepartaments.Add("FRIOS");
            ListOrderBy.Add("Nome");
            ListOrderBy.Add("Preço Crescente");
            ListOrderBy.Add("Preço Decrescente");
        }

        public void filterDepartaments(List<Products> products, string filter)
        {
            Products.Clear();
            foreach (var product in products.Where(p => p.departamento.ToUpper().Contains(filter.ToUpper())).OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_codigo = product.pro_codigo,
                    pro_nome = product.pro_nome,
                    preco = product.preco,
                    desconto = product.desconto,
                    categoria = product.categoria,
                    preco_desconto = product.preco_desconto,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipo_embalagem = product.tipo_embalagem,
                    departamento = product.departamento,
                    id = product.id
                });
            }
        }
        public void LoadOffers(List<Products> products)
        {

            Products.Clear();

            foreach(var product in products.Where(p => p.preco_desconto > 0).OrderBy(p => p.pro_nome))
            {
                Products.Add(new ProductItemViewModel
                {
                    pro_codigo = product.pro_codigo,
                    pro_nome = product.pro_nome,
                    preco = product.preco,
                    desconto = product.desconto,
                    categoria = product.categoria,
                    preco_desconto = product.preco_desconto,
                    custo = product.custo,
                    ean = product.ean,
                    secao = product.secao,
                    tipo_embalagem = product.tipo_embalagem,
                    departamento = product.departamento,
                    id = product.id

                });

            }
        }


    }
}
