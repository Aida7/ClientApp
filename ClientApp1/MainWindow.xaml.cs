using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//Создать клиент-серверное приложение чат.Чат будет работать ТОЛЬКО ДЛЯ ТРЁХ клиентов.
//Суть проста, клиент построен на постоянном соединении с сервером.
//Когда клиент подключается к серверу, все уже подключенные клиенты об этом узнают (Пользователь N заходит в чат). 
//Отправленное сообщение любым клиентом видно другим клиентам.
//Сообщения передавать в формате json(от кого, сообщение).

//Клиент отправляет данные серверу, сервер рассылает их всем клиентам.


namespace ClientApp1
{
    public partial class MainWindow : Window
    {
        private static int defaultPort = 3535;
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), defaultPort);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Clik(object sender, RoutedEventArgs e)
        {
            try
            {
                list.Items.Add("Oтправка сообщения...");
                socket.Connect(endPoint);
                ClientData clientData = new ClientData { Sender = nameUser.Text };
                var jsonConvert = JsonConvert.DeserializeObject<ClientData>(clientData.Sender);
                socket.Send(Encoding.Default.GetBytes(jsonConvert.Sender));

                clientData = new ClientData { Sender = text.Text };
                var jsonConvert1 = JsonConvert.DeserializeObject<ClientData>(clientData.Text);
                socket.Send(Encoding.Default.GetBytes(jsonConvert1.Text));

                while (true)
                {
                    Socket incomeConnection = socket.Accept();
                    int bytes;
                    //Размер буферра 
                    byte[] data = new byte[1024];
                    StringBuilder builder = new StringBuilder();

                    do
                    {
                        bytes = incomeConnection.Receive(data);
                        builder.Append(Encoding.Default.GetString(data));
                    }
                    while (incomeConnection.Available > 0);

                    list.Items.Add(builder.ToString());

                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Close();
            }
        }
    }
}
