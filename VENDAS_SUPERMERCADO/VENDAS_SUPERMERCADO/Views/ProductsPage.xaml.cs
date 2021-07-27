using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using VENDAS_SUPERMERCADO.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VENDAS_SUPERMERCADO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsPage : ContentPage
    {
        public string departamentoFiltro { get; set; }

        public ProductsPage()
        {
            
            InitializeComponent();
          
        }

        private void pckDPT_SelectedIndexChanged(object sender, EventArgs e)
        {
            departamentoFiltro = pckDPT.Items[pckDPT.SelectedIndex];
            DisplayAlert(departamentoFiltro, "Departamento selecionado", "OK");
        }

    }
}