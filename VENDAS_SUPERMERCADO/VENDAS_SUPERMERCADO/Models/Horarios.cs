using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class Horarios
    {
        [JsonProperty(PropertyName = "hora")]
        public string Hora { get; set; }
    }
}
