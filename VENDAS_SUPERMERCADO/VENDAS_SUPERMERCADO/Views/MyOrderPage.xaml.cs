using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VENDAS_SUPERMERCADO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrderPage : ContentPage
    {
        private readonly INavigationService _navigationService;
        public MyOrderPage()
        {
            InitializeComponent();
            _navigationService = DependencyService.Get<INavigationService>();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var obj = ((Button)sender).BindingContext;
            var pedido = (OrderFirebase)obj;
            await ItemsOrderViewModel.GetInstance().LoadDetailsPage(pedido);
        }
    }
}