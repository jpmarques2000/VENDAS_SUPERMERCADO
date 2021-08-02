using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Services;
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

        private async void Button_Salvar_Clicked(object sender, EventArgs e)
        {
            await userService
                .UpdateUser(Convert.ToString(txtEmail), Convert.ToString(txtNascimento), Convert.ToString(txtTelefone)
                , Convert.ToString(txtCep), Convert.ToString(txtRua), Convert.ToString(txtBairro), Convert.ToString(txtNumero));
        }

    }
}