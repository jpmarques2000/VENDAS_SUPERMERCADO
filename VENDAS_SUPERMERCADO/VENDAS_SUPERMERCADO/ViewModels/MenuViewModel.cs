using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using Xamarin.Forms;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class MenuViewModel
    {
        public ICommand RegisterCommand { get; private set; }
        public ICommand ProductCommand { get; private set; }
        public ICommand MyCartCommand { get; private set; }
        public ICommand MyOrderCommand { get; private set; }
        public ICommand UserProfileCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }

        private readonly INavigationService _navigationService;
        private NetService netService;

        public MenuViewModel()
        {
            var usuarioLogado = UserLoggedIn.UserName;
            _navigationService = DependencyService.Get<INavigationService>();
            ProductCommand = new Command(ProductCmd);
            MyCartCommand = new Command(MyCartCmd);
            MyOrderCommand = new Command(MyOrderCmd);
            UserProfileCommand = new Command(UserProfileCmd);
            LogoutCommand = new Command(LogoutCmd);
            netService = new NetService();
        }

        private void ProductCmd()
        {
            if (netService.IsConnected())
            { 
                this._navigationService.NavigateToProducts();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }
        private void MyCartCmd()
        {
            if (netService.IsConnected())
            {
                this._navigationService.NavigateToMyCart();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }
        private void MyOrderCmd()
        {
            if (netService.IsConnected())
            {
                this._navigationService.NavigateToMyOrder();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }
        private void UserProfileCmd()
        {
            if (netService.IsConnected())
            {
                this._navigationService.NavigateToUserProfile();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }
        private void LogoutCmd()
        {
            if (netService.IsConnected())
            {
             //   MeuCarrinho.Lista.Clear();
                this._navigationService.userLogout();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }
    }
}
