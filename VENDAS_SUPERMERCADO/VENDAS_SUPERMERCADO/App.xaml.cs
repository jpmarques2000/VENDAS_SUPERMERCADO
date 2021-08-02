using System;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.ViewModels;
using VENDAS_SUPERMERCADO.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VENDAS_SUPERMERCADO
{
    public partial class App : Application
    {
        private UserService userService;
        public static User CurrentUser { get; internal set; }
        public App()
        {
            InitializeComponent();
            userService = new UserService();

           // var user = userService.GetUser()

            var mainViewModel = MainViewModel.GetInstance();
          //  mainViewModel.LoadUser(user);

            MainPage = new NavigationPage(new LoginView());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
