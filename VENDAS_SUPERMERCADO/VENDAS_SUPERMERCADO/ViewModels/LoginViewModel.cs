using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using VENDAS_SUPERMERCADO.Services;
using Xamarin.Essentials;
using VENDAS_SUPERMERCADO.Views;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Interfaces;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class LoginViewModel : User
    {
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IGoogleManager _googleManager;

        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }
        public Command GoogleLoginCommand { get; set; }
        public Command GoogleLogoutCommand { get; set; }
        private NetService netService;

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
        public bool IsLogedIn { get; set; }

        User userGoogle = new User();

        public LoginViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();
            _googleManager = DependencyService.Get<IGoogleManager>();

            LoginCommand = new Command(async () => await LoginCommandAsync());
            RegisterCommand = new Command(async () => await RegisterCommandAsync());
            GoogleLoginCommand = new Command( async () => await GoogleLoginCommandAsync());
            GoogleLogoutCommand = new Command( () =>  GoogleLogoutCommandAsync());
            netService = new NetService();
            //    CheckUserLoggedIn();
        }

        private async Task RegisterCommandAsync()
        {
            if (IsBusy)
                return;
            try
            {
                IsBusy = true;
                var userService = new UserService();
                Result = await userService.RegisterUser(username, password);
                if (Result)
                {
                    
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuario Registrado", "Ok");
                   

                }
                
                else
                    await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao registrar usuario", "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoginCommandAsync()
        {
            if (netService.IsConnected())
            {
                if (IsBusy)
                    return;
                try
                {
                    IsBusy = true;
                    var userService = new UserService();
                    Result = await userService.LoginUser(username, password);
                    if (Result)
                    {
                        Preferences.Set("Username", username);
                        await MainViewModel.GetInstance().LoadProducts();

                        // mainViewModel.LoadUser();
                        //  Application.Current.MainPage = new MainShell();
                        await this._navigationService.NavigateToMenu();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Usuario/Senha invalido(s)", "Ok");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
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
        private async Task GoogleLoginCommandAsync()
        {
            if (netService.IsConnected())
            { 
                _googleManager.Login(OnLoginComplete);
                await MainViewModel.GetInstance().LoadProducts();
                await this._navigationService.NavigateToMenu();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }

        private void OnLoginComplete(User user, string message)
        {
            if (user != null)
            {
                userGoogle = user;
                username = userGoogle.email;
                password = userGoogle.password;
                IsLogedIn = true;

            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Message", message, "Ok");
            }
        }

        private void GoogleLogoutCommandAsync()
        {
            _googleManager.Logout();
            IsLogedIn = false;
            username = "";
            password = "";
        }
        private void CheckUserLoggedIn()
        {
            if (netService.IsConnected())
            { 
                _googleManager.Login(OnLoginComplete);
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }

    }
}
