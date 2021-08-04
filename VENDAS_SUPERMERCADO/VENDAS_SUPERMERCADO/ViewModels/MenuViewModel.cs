using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class MenuViewModel
    {
        public ICommand RegistroCommand { get; private set; }

        private readonly INavigationService _navigationService;

        public MenuViewModel()
        {
            _navigationService = DependencyService.Get<INavigationService>();
            RegistroCommand = new Command(RegistroCmd);
        }

        private void RegistroCmd()
        {
            this._navigationService.NavigateToRegister();
        }
    }
}
