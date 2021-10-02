using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class OrderFirebase: Order
    {
        [JsonProperty(PropertyName = "ItemsOrder")]
        public List<ItemsOrder> ItemsOrder { get; set; }
    }
}
