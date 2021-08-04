using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using VENDAS_SUPERMERCADO.Services;
using Xamarin.Essentials;
using VENDAS_SUPERMERCADO.Views;
using VENDAS_SUPERMERCADO.Models;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class LoginViewModel : User
    {
        private readonly IMessageService _messageService;
        private readonly INavigationService _navigationService;

        public Command LoginCommand { get; set; }
        public Command RegisterCommand { get; set; }

        public LoginViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();

            LoginCommand = new Command(async () => await LoginCommandAsync());
            RegisterCommand = new Command(async () => await RegisterCommandAsync());
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
                    await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuario Registrado", "Ok");
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
                    // await Application.Current.MainPage.Navigation.PushAsync(new MainShell());
                    var mainViewModel = MainViewModel.GetInstance();
                   // mainViewModel.LoadUser();
                //    Application.Current.MainPage = new MainShell();
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


    }
}
