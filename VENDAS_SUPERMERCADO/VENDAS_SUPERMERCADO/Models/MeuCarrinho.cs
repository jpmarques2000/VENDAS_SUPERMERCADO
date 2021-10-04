using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public static class MeuCarrinho
    {
        public static List<ItemsOrder> Lista { get; set; }
        public static List<ItemsOrder> ListaItens { get; set; }
    }
}
