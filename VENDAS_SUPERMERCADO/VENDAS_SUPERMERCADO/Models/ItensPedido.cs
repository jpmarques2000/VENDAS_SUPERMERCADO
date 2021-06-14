using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class ItensPedido
    {
        public int id { get; set; }
        public string numeroPedido { get; set; }
        public int codigoProduto { get; set; }
        public double qtde { get; set; }
        public double unitario { get; set; }
        public double valorTotal { get; set; }
        public DateTime data { get; set; }
        public double custo { get; set; }
        public double desconto { get; set; }
    }
}
