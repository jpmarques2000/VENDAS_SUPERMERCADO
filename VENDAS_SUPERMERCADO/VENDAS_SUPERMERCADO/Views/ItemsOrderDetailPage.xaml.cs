using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VENDAS_SUPERMERCADO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsOrderDetailPage : ContentPage
    {
        public List<ItemsOrder> ListaItens { get; set; }

        public ObservableCollection<ItemsOrder> ListaObservable { get; set; }

        public ItemsOrderDetailPage()
        {
            InitializeComponent();

            BindingContext = this;

            ListaObservable = new ObservableCollection<ItemsOrder>(MeuCarrinho.ListaItens);
            listView.ItemsSource = ListaObservable;
        }

    }
}