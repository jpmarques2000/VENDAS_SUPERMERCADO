using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using Xamarin.Forms;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class ItemsOrderViewModel : ItemsOrder
    {
        public User UserLoged { get; set; }

        private readonly IMessageService _messageService;

        private readonly INavigationService _navigationService;

        private NetService netService;

        public Command AlterarDadosCommand { get; set; }

        public ItemsOrderViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();

            var userService = new UserService();

            AlterarDadosCommand = new Command(async () => await AlterarDadosAsync());

            Task.Run(async () =>
            {
                await Buscar();

            }).Wait();

            if (UserLoged != null)
            {
                LoadUser();
            }
        }

        private async Task Buscar()
        {
            var userService = new UserService();
            UserLoged = await userService.GetUser(UserLoggedIn.UserName);

        }

        public void LoadUser()
        {
            UserLoged.nome = UserLoged.nome;
            UserLoged.telefone = UserLoged.telefone;
            UserLoged.cep = "Cep:  " + UserLoged.cep;
            UserLoged.rua = "Rua:  " + UserLoged.rua + "  Num." + UserLoged.numero;
            UserLoged.bairro = "Bairro:  " + UserLoged.bairro;
          //  UserLoged.numero = UserLoged.numero;
        }

        private async Task AlterarDadosAsync()
        {
             this._navigationService.NavigateToUserProfile();
        }

    }
}
