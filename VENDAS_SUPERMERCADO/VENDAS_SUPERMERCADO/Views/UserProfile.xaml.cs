using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VENDAS_SUPERMERCADO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserProfile : ContentPage
    {
        UserService userService = new UserService();
        public UserProfile()
        {
            InitializeComponent();

        }

    }
}