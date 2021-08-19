using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public interface IMessageService
    {
        Task ExibirMensagemAsync(string mensagem);
    }
}
