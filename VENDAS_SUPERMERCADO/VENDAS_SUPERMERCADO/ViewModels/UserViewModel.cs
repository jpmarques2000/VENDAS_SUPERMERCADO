using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class UserViewModel : User
    {
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        public Command GravarCommand { get; set; }

        private bool _Result;
        public bool Result
        {
            set
            {
                this._IsBusy = value;
                OnPropertyChanged();
            }
            get
            {
                return this._IsBusy;
            }
        }
        private bool _IsBusy;
        public bool IsBusy
        {
            set
            {
                this._Result = value;
                OnPropertyChanged();
            }
            get
            {
                return this._Result;
            }
        }

        public UserViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();

            GravarCommand = new Command(async () => await GravarCommandAsync());
        }

        private async Task GravarCommandAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var userService = new UserService();
                await userService.UpdateUser(username, dataNascimento, telefone, cep, rua, bairro, numero, password );
                await Application.Current.MainPage.DisplayAlert("Sucesso", "Dados atualizados", "Ok");

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro ao atualizar dados do usuário", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
