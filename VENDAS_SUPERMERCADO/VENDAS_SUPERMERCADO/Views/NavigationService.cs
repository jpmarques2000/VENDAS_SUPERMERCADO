using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.ViewModels;

namespace VENDAS_SUPERMERCADO.Views
{
    public class NavigationService : INavigationService
    {
        public async Task NavigateToLogin()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginView());
        }

        public async Task NavigateToMenu()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MenuView());
        }

        public async Task NavigateToRegister()
        {
            await App.Current.MainPage.Navigation.PushAsync(new LoginView());
        }

        public async Task NavigateToShell()
        {
            await App.Current.MainPage.Navigation.PushAsync(new MainShell());
        }
    }
}
