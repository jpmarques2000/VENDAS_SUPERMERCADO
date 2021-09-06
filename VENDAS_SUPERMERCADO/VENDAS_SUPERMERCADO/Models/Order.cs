using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class Order
    {
        public int numeroPedido { get; set; }
        public DateTime data { get; set; }
        public string telefone { get; set; }
        public string telefone2 { get; set; }
        public string pagamento { get; set; }
        public string email { get; set; }
        public string observacao { get; set; }
        public string cep { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string dataEntrega { get; set; }
        public string horarioEntrega { get; set; }
        public string status { get; set; }
        public int codigo { get; set; }
        public string nome { get; set; }
        public double valorTotal
        {
            get; set;
        }
        public double valorTotalDB { get; set; }


        public string valorTotalFormatado
        {
            get
            {
                return string.Format("Valor Total do pedido: R$ {0:F2}", valorTotal);
            }
        }
    }
}
