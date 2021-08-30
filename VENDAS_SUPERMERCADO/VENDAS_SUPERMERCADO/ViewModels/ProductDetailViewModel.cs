using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using VENDAS_SUPERMERCADO.Models;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class ProductDetailViewModel  : INotifyPropertyChanged
    {
        private Products selectedProduct;

        public Products SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
