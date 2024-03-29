﻿using System;
using System.Collections.Generic;
using System.Text;
using VENDAS_SUPERMERCADO.ViewModels;

namespace VENDAS_SUPERMERCADO.Models
{
    public class User : BaseViewModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string dataNascimento { get; set; }
        public string telefone { get; set; }
        public string email { get; set; }
        //Verificar possibilidade de criar uma classe para enderecos
        public string cep { get; set; }
        public string rua { get; set; }
        public string bairro { get; set; }
        public string numero { get; set; }
        public string cpf { get; set; }
        public Uri imagem { get; set; }
     //   public string username { get; set; }

     //   public string password { get; set; }

        private string _username;
        public string username
        {
            set
            {
                this._username = value;
                OnPropertyChanged();
            }
            get
            {
                return this._username;
            }
        }
        //private string _password;
        //public string password
        //{
        //    set
        //    {
        //        this._password = value;
        //        OnPropertyChanged();
        //    }
        //    get
        //    {
        //        return this._password;
        //    }
        //}

        public interface IGoogleManager
        {
            void Login(Action<User, string> OnLoginComplete);

            void Logout();
        }

    }
}
