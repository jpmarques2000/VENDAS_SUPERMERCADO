using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class Order
    {
        public int numeroPedido { get; set; }
        public string data { get; set; }
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
        public string data_entrega { get; set; }
        public string horario_entrega { get; set; }
        public string status { get; set; }
        public int codigo { get; set; }
        public string cliente { get; set; }
        public double valor_total_pedido
        {
            get; set;
        }
        public string cpf { get; set; }

        [JsonIgnore]
        public string naomandaresse { get; set; }

        [JsonProperty(PropertyName="product_ids")]
        public List<int> Products { get; set; }

        [JsonIgnore]
        public DateTime dateFB { get; set; }
        //public double valorTotalDB { get; set; }


        //public string valorTotalFormatado
        //{
        //    get
        //    {
        //        return string.Format("Valor Total do pedido: R$ {0:F2}", valorTotal);
        //    }
        //}
    }
}
