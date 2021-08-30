using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using VENDAS_SUPERMERCADO.ViewModels;

namespace VENDAS_SUPERMERCADO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailView : ContentPage
    {
        public ProductDetailView()
        {
            InitializeComponent();
            this.BindingContext = new ProductDetailViewModel();
        }
        private void BackTapped(object sender, EventArgs e)
        {
            this.Navigation.PopAsync();
        }
    }
}