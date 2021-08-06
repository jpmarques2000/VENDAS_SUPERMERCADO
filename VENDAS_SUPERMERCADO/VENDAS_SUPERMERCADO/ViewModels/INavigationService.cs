using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public interface INavigationService
    {
        Task NavigateToLogin();
        Task NavigateToMenu();
        Task NavigateToRegister();
        Task NavigateToProducts();
        Task NavigateToMyCart();
        Task NavigateToMyOrder();
        Task NavigateToUserProfile();
    }
}
