using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
        bool isRefreshing;
        const int RefreshDuration = 2;
        public User UserLoged { get; set; }

        private readonly IMessageService _messageService;

        private readonly INavigationService _navigationService;

        private NetService netService;

        public Command AlterarDadosCommand { get; set; }

        public Command ProductCommand { get; set; }

        public Command MyOrderCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
     
        public ICommand SelectCommand { get; set; }

        private ObservableCollection<ItemsOrder> _myCartList;
        public ObservableCollection<ItemsOrder> MyCartList
        {
            get
            {
                somaPedido = GetOrderTotal();
                return _myCartList;
            }
            set
            {
                _myCartList = value;
                OnPropertyChanged();
            }
        }
        

        public User UserLogedDelivery { get; set; }

        public Order order;

        private static ItemsOrderViewModel instance;
        public static ItemsOrderViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ItemsOrderViewModel();
            }

            return instance;
        }


        private OrderService orderService;
        public ICommand RefreshCommand => new Command(async () => await RefreshItemsAsync());

        //   public ObservableCollection<ItemsOrder> MyCartList { get; set; }

        public ObservableCollection<Order> AllOrdersList { get; set; }

        private APIService apiService;

        public ItemsOrderViewModel()
        {
            instance = this;
            _messageService = DependencyService.Get<IMessageService>();
            _navigationService = DependencyService.Get<INavigationService>();

            var userService = new UserService();

            AlterarDadosCommand = new Command( () =>  AlterarDadosAsync());

            ProductCommand = new Command(() => ProductPageAsync());

            MyOrderCommand = new Command(() => OrderPageAsync());

            ListSchedule = new ObservableCollection<string>();

            ListPayments = new ObservableCollection<string>();

            SelectCommand = new Command(SelectCmd);

            apiService = new APIService();

            if (MeuCarrinho.Lista != null)
                MyCartList = new ObservableCollection<ItemsOrder>(MeuCarrinho.Lista);

            AllOrdersList = new ObservableCollection<Order>();

            netService = new NetService();

            order = new Order();

            orderService = new OrderService();

            somaPedido = GetOrderTotal();

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

            Task.Run(async () =>
            {

                var ordersList = new List<Order>();
                ordersList = await orderService.GetAllOrders();
                filterOrders(ordersList, UserLoggedIn.UserName);

            }).Wait();

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
                    Application.Current.MainPage.DisplayAlert("Erro", "Selecione uma previsao de entrega", "Ok");
                    return;
                }

                if (!string.IsNullOrEmpty(Payments))
                {
                    PaymentSelected = Payments;
                }
                else
                {
                    PaymentSelected = "Selecione uma forma de pagamento";
                    Application.Current.MainPage.DisplayAlert("Erro", "Selecione uma forma de pagamento", "Ok");
                    return;
                }
                if (MeuCarrinho.Lista!= null)
                {
                    MontarPedido(ScheduleSelected, PaymentSelected);
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Erro", "Adicione algum item ao pedido antes de finalizar o agendamento", "Ok");
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
            UserLogedDelivery = await userService.GetUser(UserLoggedIn.UserName);
        }

        private void ClearPage()
        {
            MeuCarrinho.Lista.Clear();
            MyCartList.Clear();

            ScheduleSelected = "Selecione uma previsao de entrega";
            PaymentSelected = "Selecione uma forma de pagamento";
            Payments = "Selecione uma forma de pagamento";
            Schedule = "Selecione uma previsao de entrega";
            Observacao = "";
            

        }

        private void MontarPedido(string schedule, string payment)
        {
            order.bairro = UserLogedDelivery.bairro;
            order.cep = UserLogedDelivery.cep;
            //   order.numeroPedido = Verificar futuramente seqpedido
            order.data = DateTime.Now.ToString();
            order.email = UserLoggedIn.UserName;
            order.cliente = UserLogedDelivery.nome;
            order.observacao = Observacao;
            order.rua = UserLogedDelivery.rua;
            order.valor_total_pedido = GetOrderTotal();
            order.telefone = UserLogedDelivery.telefone;
            order.cpf = UserLogedDelivery.cpf;
            order.pagamento = payment;
            order.data_entrega = schedule;
            order.numero = UserLogedDelivery.numero;

            order.Products = new List<int>();
            foreach (var produto in MeuCarrinho.Lista)
            {
                order.Products.Add(produto.id);
            }

            var ok = true;
            Task.Run(async () =>
            {
                ok = await APIService.Post(order); 
            }).Wait();

            if (!ok)
            { 
            }


            Task.Run(async () =>
            {
                await orderService.CreateNewOrder(order.email, order.bairro, order.cep, order.data, order.cliente,
                    order.observacao, order.rua, order.valor_total_pedido, order.telefone, order.pagamento, order.data_entrega, order.cpf); 

            }).Wait();

            Task.Run(async () =>
            {
                await orderService.SaveItemsOrder(MeuCarrinho.Lista);

            }).Wait();

            Application.Current.MainPage.DisplayAlert("Sucesso", "Seu pedido foi agendado", "Ok");
            ClearPage();

        }


        private double GetOrderTotal()
        {
            somaPedido = 0.00;
            if (MeuCarrinho.Lista == null)
                return somaPedido = 0.00;
            foreach (var s in MeuCarrinho.Lista)
            {
                var somaTotal = (s.qtde * s.unitario);
                somaPedido = somaPedido + somaTotal;
            }
            return somaPedido;

        }

        public void LoadUser()
        {
            UserLoged.cep = "Cep:  " + UserLoged.cep;
            UserLoged.rua = "Rua:  " + UserLoged.rua + "  Num." + UserLoged.numero;
            UserLoged.bairro = "Bairro:  " + UserLoged.bairro;

        }

        private void AlterarDadosAsync()
        {
              this._navigationService.NavigateToUserProfile();
        }

        private void ProductPageAsync()
        {
            this._navigationService.NavigateToProducts();
        }

        private void OrderPageAsync()
        {
            this._navigationService.NavigateToMyOrder();
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

        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                isRefreshing = value;
                OnPropertyChanged();
            }
        }

        string _observacao;
        public string Observacao
        {
            get
            {
                return _observacao;
            }
            set
            {
                _observacao = value;
                OnPropertyChanged();
            }
        }

        double _somaPedido;
        public double somaPedido
        {
            get
            {
                return _somaPedido;
            }
            set
            {
                _somaPedido = value;
                OnPropertyChanged();
            }
        }

        private async Task RefreshItemsAsync()
        {
            IsRefreshing = true;
            await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
            UpdateList();
            IsRefreshing = false;
        }

        private void UpdateList()
        {
            MyCartList.Clear();
        }

        public void filterOrders(List<Order> orders, string username)
        {
            AllOrdersList.Clear();

            foreach (var order in orders.Where(o => o.email.ToUpper().Contains(username.ToUpper())))
            {
                AllOrdersList.Add(new Order
                {
                    data_entrega = order.data_entrega,
                    cliente = order.cliente,
                    numeroPedido = order.numeroPedido,
                    observacao = order.observacao,
                    pagamento = order.pagamento,
                    valor_total_pedido = order.valor_total_pedido,
                    data = order.data,
                    email = order.email
                    
                });
            }
        }
    }
}
