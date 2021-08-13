using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using VENDAS_SUPERMERCADO.Interfaces;
using Xamarin.Forms;

namespace VENDAS_SUPERMERCADO.Services
{
    public class NetService
    {
        public bool IsConnected()
        {
            var response = CrossConnectivity.Current.IsConnected ? true : false;
            return response;
        }
    }
}
