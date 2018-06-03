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
        Socket socket;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Send_Clik(object sender, RoutedEventArgs e)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), defaultPort);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {

                list.Items.Add("Oтправка сообщения...");
                socket.Connect(endPoint);

                // info = new ClientData { Sender = "First", SentDate = DateTime.Now, Text = "TextFirst" };
                //socket.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(info)));

                ClientData info = new ClientData { Sender = nameUser.Text, Text = text.Text, SentDate = DateTime.Now };
                socket.Send(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(info)));

            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }

            ReceiveMessage();
        }
        
    
        public void ReceiveMessage()
        {
            ClientData clientData = new ClientData { SocketConnect = socket };

            while (true)
            {
                int bytes;
                byte[] data = new byte[1024];
                bytes = socket.Receive(data);
                string newData = Encoding.UTF8.GetString(data, 0, bytes);
                list.Items.Add(newData);
            }


        }

    }
}



