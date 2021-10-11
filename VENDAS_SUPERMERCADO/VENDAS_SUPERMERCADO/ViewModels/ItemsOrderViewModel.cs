using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        //public Command DetailsCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SelectCommand { get; set; }

        public static OrderFirebase OrderChoosed { get; set; }

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

        private ObservableCollection<ItemsOrder> _itemsOrderDetailsList;
        public ObservableCollection<ItemsOrder> ItemsOrderDetailsList
        {
            get
            {
                return _itemsOrderDetailsList;
            }
            set
            {
                _itemsOrderDetailsList = value;
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

            AlterarDadosCommand = new Command(() => AlterarDadosAsync());

            ProductCommand = new Command(() => ProductPageAsync());

            MyOrderCommand = new Command(() => OrderPageAsync());

            //DetailsCommand = new Command((async) => LoadDetailsPage());

            ListSchedule = new ObservableCollection<string>();

            ListPayments = new ObservableCollection<string>();

            SelectCommand = new Command(SelectCmd);

            apiService = new APIService();

            if (MeuCarrinho.Lista != null)
                MyCartList = new ObservableCollection<ItemsOrder>(MeuCarrinho.Lista);

            AllOrdersList = new ObservableCollection<Order>();

            ItemsOrderDetailsList = new ObservableCollection<ItemsOrder>();

            netService = new NetService();

            order = new Order();

            orderService = new OrderService();
          

            somaPedido = GetOrderTotal();

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
                var ordersList = await orderService.GetAllOrders();
                filterOrders(ordersList, UserLoggedIn.UserName);

            }).Wait();


        }

        public async Task LoadDetailsPage(OrderFirebase pedido)
        {
            MeuCarrinho.ListaItens = pedido.ItemsOrder;
            if (netService.IsConnected())
            {
                 await this._navigationService.NavigateToDetailsPage();
            }
            else
            {
                 Application.Current.MainPage.DisplayAlert("Erro", "Necessário conexão com a internet", "Ok");
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
                    var ok = true;
                    ok = VerificaDadosEntrega();
                    if(ok == true)
                    {
                        MontarPedido(ScheduleSelected, PaymentSelected);
                    }
                    else
                    {
                        Application.Current.MainPage.DisplayAlert("Aviso", "Necessário cadastrar todos os dados de entrega antes de finalizar o pedido", "Ok");
                    }
                    
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

        private bool VerificaDadosEntrega()
        {
            if( UserLoged.cep == null || UserLoged.username == null || 
                UserLogedDelivery.bairro == null || UserLoged.rua == null || UserLoged.telefone == null
                || UserLoged.nome == null || UserLoged.dataNascimento == null)
            {
                return false;
            }
            else
            {
                return true;
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
            order.dateFB = DateTime.Now;
            order.email = UserLoggedIn.UserName;
            order.cliente = UserLogedDelivery.nome;
            order.observacao = Observacao;
            order.rua = UserLogedDelivery.rua;
            order.valor_total_pedido = GetOrderTotal();
            order.telefone = UserLogedDelivery.telefone;
            order.complemento = ValorDinheiro;
            if (UsaCPF == true)
            { 
                order.cpf = UserLogedDelivery.cpf;
            }
            else
            {
                order.cpf = "";
            }
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
                Application.Current.MainPage.DisplayAlert("Erro", "Falha ao agendar pedido, tente novamente mais tarde", "Ok");
                return;
            }

            ok = false;
            var orderFb = orderService.CreateOrder(order, MeuCarrinho.Lista);
            Task.Run(async () =>
            {
                ok = await orderService.InserteNewOrder(orderFb);

            }).Wait();

           

            SendEmail();

            Application.Current.MainPage.DisplayAlert("Sucesso", "Seu pedido foi Realizado com Sucesso! Agradecemos a preferencia! ", "Ok");

            ClearPage();

        }

        private void SendEmail()
        {
            try
            {

                var qtdItens = 0;
                var body = new StringBuilder();

                body.Append("<html>");
                body.Append("<body>");
                body.Append("<h4>");
                body.Append("Você tem um novo pedido");
                body.Append("</h4>");

                body.Append("<table>");

                body.Append("<tr>");
                body.Append("<th>");
                body.Append("Descrição do produto");
                body.Append("</th>");
                body.Append("<th>");
                body.Append("Quantidade");
                body.Append("</th>");
                body.Append("<th>");
                body.Append("Valor Unitário");
                body.Append("</th>");
                body.Append("<th>");
                body.Append("Total do item");
                body.Append("</th>");
                body.Append("</tr>");

                foreach (var item in MeuCarrinho.Lista)
                {
                    body.Append("<tr>");

                    body.Append("<td>");
                    body.Append(item.pro_nome);
                    body.Append("</td>");

                    body.Append("<td style=\"text-align:right;\">");
                    body.Append(item.qtde);
                    body.Append("</td>");

                    body.Append("<td style=\"text-align:right;\">");
                    var unitario = string.Format(new CultureInfo("pt-BR", false), "{0:0.00}", item.unitario);
                    
                    body.Append(unitario);
                    body.Append("</td>");

                    var totalDoItem = string.Format(new CultureInfo("pt-BR", false), "{0:0.00}", item.qtde * item.unitario);

                    body.Append("<td style=\"text-align:right;\">");
                    body.Append(totalDoItem);
                    body.Append("</td>");

                    body.Append("</tr>");

                    qtdItens++;
                    
                }
                body.Append("</table>");

                var totalDoPedido = string.Format(new CultureInfo("pt-BR", false), "{0:0.00}", somaPedido);

                body.Append("<hr>");
                body.Append("<strong>");
                body.Append($"Quantidade de itens: {qtdItens}");
                body.Append("<br>");
                body.Append($"Total do pedido....: {totalDoPedido}");
                body.Append("</strong>");

                body.Append("</hr>");

               
                body.Append("</body>");
                body.Append("</html>");


                MailMessage mail = new MailMessage("tccvendassupermercado@gmail.com", "tccRecebedorEmailSups@gmail.com");

                mail.Subject = ("Novo Pedido");
                mail.IsBodyHtml = true;
                mail.Body = body.ToString();
                    // "Você recebeu um novo pedido em seu sistema, verifique!";
                mail.SubjectEncoding = Encoding.GetEncoding("UTF-8");
                mail.BodyEncoding = Encoding.GetEncoding("UTF-8");

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("tccvendassupermercado@gmail.com", "123456tcc");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Erro", "Falha ao enviar pedido", "Ok");
                return;
            }
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
            var lista = await apiService.GetSchedules();
            foreach (var item in lista)
            {
                ListSchedule.Add(item.Hora);
            }

        }

        public void LoadPayments()
        {
            ListPayments.Add("DINHEIRO");
            ListPayments.Add("CARTAO DEBITO");
            ListPayments.Add("CARTAO CREDITO");
            ListPayments.Add("PIX");
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

        string _valorDinheiro;
        public string ValorDinheiro
        {
            get
            {
                return _valorDinheiro;
            }
            set
            {
                _valorDinheiro = value;
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

        bool _usaCPF;
        public bool UsaCPF
        {
            get
            {
                return _usaCPF;
            }
            set
            {
                _usaCPF = value;
                OnPropertyChanged();
            }
        }

        bool _usaTroco;
        public bool UsaTroco
        {
            get
            {
                return _usaTroco;
            }
            set
            {
                _usaTroco = value;
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

        private static DateTime ToDataApp(string valor)
        {
            var chr32 = (char)32;
            var dados = valor.Split(chr32);
            var barra = (char)47;

            var aData = dados[0].Split(barra);

            var dia = int.Parse(aData[1]);
            var mes = int.Parse(aData[0]);
            var ano = int.Parse(aData[2]);

            var pontos = (char)58;

            var horas = dados[1].Split(pontos);

            var hora = int.Parse(horas[0]);
            var minuto = int.Parse(horas[1]);
            var segundo = int.Parse(horas[2]);

            var novaData = new DateTime(ano, mes, dia, hora, minuto, segundo);

            if (dados[2].Trim().Equals("PM"))
            {
                novaData = novaData.AddHours(12);
            }
            return novaData;
        }
        public void filterOrders(List<OrderFirebase> orders, string username)
        {
            AllOrdersList.Clear();

            try
            {

                var filtradas = orders
                                    .Where(o => o.email.ToUpper()
                                    .Contains(username.ToUpper()))
                                    .OrderByDescending(o => ToDataApp(o.data)).ToList();

                foreach (var order in filtradas)
                {




                    AllOrdersList.Add(new OrderFirebase
                    {
                        data_entrega = order.data_entrega,
                        cliente = order.cliente,
                        numeroPedido = order.numeroPedido,
                        observacao = order.observacao,
                        pagamento = order.pagamento,
                        valor_total_pedido = order.valor_total_pedido,
                        data = order.data,
                        email = order.email,
                        ItemsOrder = order.ItemsOrder

                    });
                }
            }
            catch (Exception e)
            {

                var lixao = e.Message;
            }
        }

        
    }
}
