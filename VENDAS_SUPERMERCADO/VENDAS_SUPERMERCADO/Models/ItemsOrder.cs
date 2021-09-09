using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class ItemsOrder
    {
        public int id { get; set; }
        public string numeroPedido { get; set; }
        public int codigoProduto { get; set; }
        public int qtde { get; set; }
        public double unitario { get; set; }
        public double valorTotal { get; set; }
        public string data { get; set; }
        public double custo { get; set; }
        public double desconto { get; set; }
        public string pro_nome { get; set; }





        ///public string unitarioFormatado
        ///{
            ///get
            ///{
               /// return string.Format("Valor unidade: R$ {0:F2}", unitario);
            ///}
        ///}

        ///public string totalFormatado
        ///{
            ///get
           /// {
               /// return string.Format("Valor Total: R$ {0:F2}", valorTotal);
            ///}
        ///}

    }


}
