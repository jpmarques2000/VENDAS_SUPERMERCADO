using System;
using System.Collections.Generic;
using System.Text;

namespace VENDAS_SUPERMERCADO.Models
{
    public class Products
    {
        public int pro_codigo { get; set; }
        public string pro_nome { get; set; }
        public string descricaoTecnica { get; set; }
        public string tipoEmbalagem { get; set; }
        public string secao { get; set; }
        public string categoria { get; set; }
        public string departamento { get; set; }
        public string ncm { get; set; }
        public string ean { get; set; }
        public double custo { get; set; }
        public double Preco { get; set; }
        public bool Promocao { get; set; }
        public double Precopromocao { get; set; }
        public decimal Estoque { get; set; }
        public double quantidade { get; set; }

        ////public Produto(int Produtoid, string Nome, double Preco, int Codigo, bool Promocao, double Precopromocao, decimal Estoque)
        ////{
        ////    this.Produtoid = Produtoid;
        ////    this.Nome = Nome;
        ////    this.Preco = Preco;
        ////    this.Codigo = Codigo;
        ////    this.Promocao = Promocao;
        ////    this.Precopromocao = Precopromocao;
        ////    this.Estoque = Estoque;
        ////}



        public string PrecoFormatado
        {
            get
            {
                return string.Format("Valor: R$ {0:F2}", Preco);
            }
        }

        public string departamentoFormatado
        {
            get
            {
                return string.Format("Departamento: {0}", departamento);
            }
        }
    }
}
