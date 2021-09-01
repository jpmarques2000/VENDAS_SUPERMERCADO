using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VENDAS_SUPERMERCADO.Models;
using VENDAS_SUPERMERCADO.Services;
using Xamarin.Forms;

namespace VENDAS_SUPERMERCADO.ViewModels
{
    public class ItemsOrderViewModel : INotifyPropertyChanged
    {
        public User UserLoged { get; set; }

        private readonly IMessageService _messageService;

        private readonly INavigationService _navigationService;

        private NetService netService;

        public Command AlterarDadosCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
     
        public ICommand SelectCommand { get; set; }

        public List<ItemsOrder> MyCartList { get; set; }

        //   public ObservableCollection<ItemsOrder> MyCartList { get; set; }

        public ItemsOrderViewModel()
        {
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();

            var userService = new UserService();

            AlterarDadosCommand = new Command(async () => await AlterarDadosAsync());

            ListSchedule = new ObservableCollection<string>();

            ListPayments = new ObservableCollection<string>();

            SelectCommand = new Command(SelectCmd);

            MyCartList = MeuCarrinho.Lista;

            netService = new NetService();

         //   ReloadItems();

            LoadPayments();

            Task.Run(async () =>
            {
                await LoadSchedules();

            }).Wait();

            Task.Run(async () =>
            {
                await Buscar();

            }).Wait();

            if (UserLoged != null)
            {
                LoadUser();
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SelectCmd()
        {
            if (netService.IsConnected())
            { 
                if (!string.IsNullOrEmpty(Schedule))
                {
                    ScheduleSelected = Schedule;
                }
                else
                {
                    ScheduleSelected = "Selecione uma previsao de entrega";
                }

                if (!string.IsNullOrEmpty(Payments))
                {
                    PaymentSelected = Payments;
                }
                else
                {
                    ScheduleSelected = "Selecione uma forma de pagamento";
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
            }
        }

        private async Task Buscar()
        {
            var userService = new UserService();
            UserLoged = await userService.GetUser(UserLoggedIn.UserName);

        }

        public void LoadUser()
        {
            UserLoged.nome = UserLoged.nome;
            UserLoged.telefone = UserLoged.telefone;
            UserLoged.cep = "Cep:  " + UserLoged.cep;
            UserLoged.rua = "Rua:  " + UserLoged.rua + "  Num." + UserLoged.numero;
            UserLoged.bairro = "Bairro:  " + UserLoged.bairro;
          //  UserLoged.numero = UserLoged.numero;
        }

        private async Task AlterarDadosAsync()
        {
             this._navigationService.NavigateToUserProfile();
        }

        public async Task LoadSchedules()
        {
            ListSchedule.Add("09:00 - 12:00");
            ListSchedule.Add("12:00 - 15:00");
            ListSchedule.Add("15:00 - 18:00");
            ListSchedule.Add("18:00 - 20:00");
        }

        public void LoadPayments()
        {
            ListPayments.Add("DINHEIRO");
            ListPayments.Add("CARTAO DEBITO");
            ListPayments.Add("CARTAO CREDITO");
            ListPayments.Add("PIX");
        }

        public void ReloadItems()
        {
            //ItemsOrderList.Add(new ItemsOrder
            //{
            //    numeroPedido = ("1"),
            //    codigoProduto = (00055555),
            //    qtde = (2.00),
            //    unitario = (6.99),
            //    valorTotal = (13.98),
            //    custo = (4.99),
            //    desconto = (0.00)
            //});

            //ItemsOrderList.Add(new ItemsOrder
            //{
            //    numeroPedido = ("1"),
            //    codigoProduto = (00054555),
            //    qtde = (3.00),
            //    unitario = (7.99),
            //    valorTotal = (13.98),
            //    custo = (4.99),
            //    desconto = (0.00)
            //});
        }

        ObservableCollection<string> _listSchedule;
        public ObservableCollection<string> ListSchedule
        {
            get
            {
                return _listSchedule;
            }
            set
            {
                _listSchedule = value;
                OnPropertyChanged();
            }
        }
        string _schedule;
        public string Schedule
        {
            get
            {
                return _schedule;
            }
            set
            {
                _schedule = value;
                OnPropertyChanged();
            }
        }
        string _scheduleSelected;
        public string ScheduleSelected
        {
            get
            {
                return _scheduleSelected;
            }
            set
            {
                _scheduleSelected = value;
                OnPropertyChanged();
            }
        }


        ObservableCollection<string> _listPayments;
        public ObservableCollection<string> ListPayments
        {
            get
            {
                return _listPayments;
            }
            set
            {
                _listPayments = value;
                OnPropertyChanged();
            }
        }
        string _payments;
        public string Payments
        {
            get
            {
                return _payments;
            }
            set
            {
                _payments = value;
                OnPropertyChanged();
            }
        }
        string _paymentSelected;
        public string PaymentSelected
        {
            get
            {
                return _paymentSelected;
            }
            set
            {
                _paymentSelected = value;
                OnPropertyChanged();
            }
        }

    }
}
