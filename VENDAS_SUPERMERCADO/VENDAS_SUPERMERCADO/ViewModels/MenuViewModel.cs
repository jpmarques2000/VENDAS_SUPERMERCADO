using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
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

        private readonly INavigationService _navigationService;

        public MenuViewModel()
        {
            _navigationService = DependencyService.Get<INavigationService>();
            RegisterCommand = new Command(RegisterCmd);
            ProductCommand = new Command(ProductCmd);
            MyCartCommand = new Command(MyCartCmd);
            MyOrderCommand = new Command(MyOrderCmd);
            UserProfileCommand = new Command(UserProfileCmd);
        }

        private void RegisterCmd()
        {
            this._navigationService.NavigateToRegister();
        }
        private void ProductCmd()
        {
            this._navigationService.NavigateToProducts();
        }
        private void MyCartCmd()
        {
            this._navigationService.NavigateToMyCart();
        }
        private void MyOrderCmd()
        {
            this._navigationService.NavigateToMyOrder();
        }
        private void UserProfileCmd()
        {
            this._navigationService.NavigateToUserProfile();
        }
    }
}
