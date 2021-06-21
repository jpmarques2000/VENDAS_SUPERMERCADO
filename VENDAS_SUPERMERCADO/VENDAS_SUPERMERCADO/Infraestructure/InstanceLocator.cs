using System;
using System.Collections.Generic;
using System.Text;
using VENDAS_SUPERMERCADO.ViewModels;

namespace VENDAS_SUPERMERCADO.Infraestructure
{
    public class InstanceLocator
    {
        public MainViewModel Main { get; set; }

        public InstanceLocator()
        {
            Main = new MainViewModel();
        }
    }
}
