using System;
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
        //private NetService netService;

        //private APIService apiService;
        //public ObservableCollection<ProductItemViewModel> Products { get; set; }

        //public event PropertyChangedEventHandler PropertyChanged;
        public ProductsPage()
        {
            //apiService = new APIService();
            //netService = new NetService();
            //Products = new ObservableCollection<ProductItemViewModel>();
            //LoadProducts();
            InitializeComponent();
            BindingContext = new MainViewModel();

        }
        //public async void LoadProducts()
        //{

        //    var productsAPI = new List<Products>();

        //    productsAPI = await apiService.Get<Products>("products");

        //    ReloadProducts(productsAPI);
        //}
        //public void ReloadProducts(List<Products> products)
        //{
        //    Products.Clear();

        //    foreach (var product in products.OrderBy(p => p.pro_nome))
        //    {
        //        Products.Add(new ProductItemViewModel
        //        {
        //            pro_nome = product.pro_nome,
        //            Preco = product.Preco,
        //        });
        //    }
        //}
    }
}