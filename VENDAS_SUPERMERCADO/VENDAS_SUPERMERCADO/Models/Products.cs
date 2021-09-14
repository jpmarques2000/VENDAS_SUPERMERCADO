using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace VENDAS_SUPERMERCADO.Models
{
    public class Products 
    {
        public int pro_codigo { get; set; }
        public string pro_nome { get; set; }
        public string descricao_tecnica { get; set; }
        public string tipo_embalagem { get; set; }
        public string secao { get; set; }
        public string categoria { get; set; }
        public string departamento { get; set; }
        public string ncm { get; set; }
        public string ean { get; set; }
        public double custo { get; set; }
        public double preco { get; set; }
        public bool promocao { get; set; }
        public double preco_desconto { get; set; }
        public decimal estoque { get; set; }

        public double preco_promocao { get; set; }

        public string precopromocao { get; set; }

        public double quantidade { get; set; }
        public int id { get; set; }

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



        ///public string PrecoFormatado
        ///{
            ///get
            ///{
               /// return string.Format("Valor: R$ {0:F2}", Preco);
            ///}
        ///}

       /// public string departamentoFormatado
        ///{
           /// get
            ///{
               /// return string.Format("Departamento: {0}", departamento);
            ///}
        ///}

    }
}
