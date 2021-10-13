﻿using System;
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
        public Command TermsCommand { get; set; }

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

        private bool aguarde;

        public bool Aguarde
        {
            get { return aguarde;}
            set
            {
                aguarde = value;
                OnPropertyChanged();
            }
        }


        public bool IsLogedIn { get; set; }

        User userGoogle = new User();

        public LoginViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();
            _googleManager = DependencyService.Get<IGoogleManager>();

            //LoginCommand = new Command(async () => await LoginCommandAsync());
            //RegisterCommand = new Command(async () => await RegisterCommandAsync());
            GoogleLoginCommand = new Command( async () => await GoogleLoginCommandAsync());
            GoogleLogoutCommand = new Command( () =>  GoogleLogoutCommandAsync());
            TermsCommand = new Command(TermsOfServiceCmd);
            netService = new NetService();
            Filter.showOffer = true;
            CheckUserLoggedIn();
        }

        private void TermsOfServiceCmd()
        {
            this._navigationService.NavitaToTermsPage();
        }

        //private async Task RegisterCommandAsync()
        //{
        //    if (IsBusy)
        //        return;
        //    try
        //    {
        //        IsBusy = true;
        //        var userService = new UserService();
        //        Result = await userService.RegisterUser(username, password);
        //        if (Result)
        //        {

        //            await Application.Current.MainPage.DisplayAlert("Sucesso", "Usuario Registrado", "Ok");


        //        }

        //        else
        //            await Application.Current.MainPage.DisplayAlert("Erro", "Falha ao registrar usuario", "Ok");
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        //private async Task LoginCommandAsync()
        //{
        //    if (netService.IsConnected())
        //    {
        //        if (IsBusy)
        //            return;
        //        try
        //        {
        //            IsBusy = true;
        //            var userService = new UserService();
        //            Result = await userService.LoginUser(username, password);
        //            if (Result)
        //            {
        //                Preferences.Set("Username", username);
        //                UserLoggedIn.UserName = username;

        //                await MainViewModel.GetInstance().LoadProducts();
        //                // await this._navigationService.NavigateToMenu();
        //                Application.Current.MainPage = new NavigationPage(new MenuView());
        //            }
        //            else
        //            {
        //                await Application.Current.MainPage.DisplayAlert("Erro", "Usuario/Senha invalido(s)", "Ok");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Erro", ex.Message, "Ok");
        //        }
        //        finally
        //        {
        //            IsBusy = false;
        //        }
        //    }
        //    else
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
        //    }
        //}
        private async Task GoogleLoginCommandAsync()
        {
            Aguarde = true;

            if (netService.IsConnected())
            {
                if(ChkTerms == true)
                { 
                    _googleManager.Login(OnLoginComplete);
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Aviso", "Necessário concordar com os termos de uso, para realizar o login", "Ok");
                    Aguarde = false;
                }

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
                Aguarde = false;
            }
            
        }

        private void OnLoginComplete(User user, string message)
        {
            if (user != null)
            {
                var userService = new UserService();
                userGoogle = user;
                UserLoggedIn.UserName = userGoogle.email;
                username = userGoogle.email;
                imagem = userGoogle.imagem;
                IsLogedIn = true;

                Task.Run(async () =>
                {
                    await userService.RegisterUserGoogle(username);
                    await loadMenu();
                }).Wait();
                
            }
            else
            {
                // Application.Current.MainPage.DisplayAlert("Usuário não selecionado ou ocorreu um erro", message, "Ok");
            }
            Aguarde = false;
        }

        private async Task loadMenu()
        {
            await MainViewModel.GetInstance().LoadProductsOffer();
            Application.Current.MainPage =  new NavigationPage( new MenuView());
            
        }

        private void GoogleLogoutCommandAsync()
        {
            _googleManager.Logout();
            IsLogedIn = false;
            username = "";
            UserLoggedIn.UserName = "";
        }
        private void CheckUserLoggedIn()
        {
            if (netService.IsConnected())
            {
                Aguarde = true;
                _googleManager.Login(OnLoginComplete);
            }
            else
            {
                Aguarde = false;
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }

        bool _chkterms;
        public bool ChkTerms
        {
            get
            {
                return _chkterms;
            }
            set
            {
                _chkterms = value;
                OnPropertyChanged();
            }
        }

    }
}
