using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private NetService netService;
        public User UserLoged { get; set; }
        public ObservableCollection<User> userDB { get; set; }

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
            username = UserLoggedIn.UserName;

            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();

            GravarCommand = new Command(async () => await GravarCommandAsync());
            netService = new NetService();
            userDB = new ObservableCollection<User>();
            var userService = new UserService();

            Task.Run(async () =>
            {
                await Buscar();

            }).Wait();

            if (UserLoged !=null)
            {
                LoadUser();
            }

        }

        private async Task Buscar()
        {
            var userService = new UserService();
            UserLoged = await userService.GetUser(username);

        }

        private async Task GravarCommandAsync()
        {
            if (netService.IsConnected())
            { 
                if (IsBusy)
                    return;
                try
                {
                    IsBusy = true;
                    var userService = new UserService();
                    await userService.UpdateUser(username, dataNascimento, telefone, cep, rua, bairro, numero, nome );
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
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }

        public void LoadUser()
        {
            nome = UserLoged.nome;
            dataNascimento = UserLoged.dataNascimento;
            telefone = UserLoged.telefone;
            cep = UserLoged.cep;
            rua = UserLoged.rua;
            bairro = UserLoged.bairro;
            numero = UserLoged.numero;
        }

    }
}
