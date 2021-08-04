using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VENDAS_SUPERMERCADO.Models;

namespace VENDAS_SUPERMERCADO.Services
{
    public class UserService
    {
        FirebaseClient client;

        public UserService()
        {
            client =
                new FirebaseClient("https://tcc-vendas-supermercado-default-rtdb.firebaseio.com/");
        }

        public async Task<bool> IsUserExists(string name)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>())
                .Where(u => u.Object.username == name)
                .FirstOrDefault();
            return (user != null);
        }

        public async Task<bool> RegisterUser(string name, string passwd)
        {
            if (await IsUserExists(name) == false)
            {
                await client.Child("Users")
                    .PostAsync(new User()
                    {
                        username = name,
                        password = passwd
                    });
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> LoginUser(string name, string passwd)
        {
            var user = (await client.Child("Users")
                .OnceAsync<User>())
                .Where(u => u.Object.username == name)
                .Where(u => u.Object.password == passwd)
                .FirstOrDefault();
            return (user != null);
        }

        public async Task UpdateUser(string username,string dataNasc, string userTel, string userCEP, string userRua, 
            string userBairro, string UserNumero)
        {
            try
            {
                var toUpdateUser = (await client
                    .Child("Users")
                    .OnceAsync<User>())
                    .Where(a => a.Object.username == username).FirstOrDefault();

                await client
                    .Child("Users")
                        .Child(toUpdateUser.Key)
                           .PutAsync(new User()
                           {dataNascimento = dataNasc, telefone = userTel, cep = userCEP, 
                               rua = userRua, bairro = userBairro, numero = UserNumero });
            }
            catch (System.Exception)
            {
                throw;
            }

        }

        //public async Task<User> GetUser(string username)
        //{
        //    //return (await client
        //    //    .Child("Users")
        //    //    .OnceAsync<User>()).Select(item => new User
        //    //    {
        //    //        dataNascimento = item.Object.dataNascimento,
        //    //        cep = item.Object.cep,
        //    //        rua = item.Object.rua,
        //    //        bairro = item.Object.bairro,
        //    //        numero = item.Object.numero
        //    //    });
        //}
    }
}
