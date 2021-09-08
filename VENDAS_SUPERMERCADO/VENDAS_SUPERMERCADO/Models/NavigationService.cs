using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.ViewModels;
using Xamarin.Forms;
using static VENDAS_SUPERMERCADO.Models.User;

namespace VENDAS_SUPERMERCADO.Views
{
    public class NavigationService : INavigationService
    {
        public NavigationService()
        {
            _googleManager = DependencyService.Get<IGoogleManager>();
        }
        private readonly IGoogleManager _googleManager;

        public async Task NavigateToLogin()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginView());
        }

        public async Task NavigateToMenu()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MenuView());
        }

        public async Task NavigateToMyCart()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MyCartPage());
        }

        public async Task NavigateToMyOrder()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MyOrderPage());
        }

        public async Task NavigateToProducts()
        {
            await App.Current.MainPage.Navigation.PushAsync(new ProductsPage());
        }

        public async Task NavigateToUserProfile()
        {
            await App.Current.MainPage.Navigation.PushAsync(new UserProfile());
        }
        public async Task userLogout()
        {
            
            Task.Run(async () =>
            {
                 _googleManager.Logout();

            }).Wait();

            Application.Current.MainPage = new NavigationPage(new LoginView());

        }
        public async Task NavigateToFilterView()
        {
            await App.Current.MainPage.Navigation.PushAsync(new FilterProductsView());
        }
    }
}
