using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Controller
{
    public class Connect
    {
        // адрес и порт сервера, к которому будем подключаться
        static int port = 8003; // порт сервера
        static string address = "85.143.223.221"; // адрес сервера

        public static Models.Response<object> Request(Models.InData json)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // подключаемся к удаленному хосту
                socket.Connect(ipPoint);
                var message = JsonConvert.SerializeObject(json);

                byte[] data = Encoding.Unicode.GetBytes(message);
                socket.Send(data);

                // получаем ответ
                data = new byte[256]; // буфер для ответа
                StringBuilder builder = new StringBuilder();
                int bytes = 0; // количество полученных байт

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                var otvet = builder.ToString();

                var answer = JsonConvert.DeserializeObject<Models.Response<object>>(otvet);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

                return answer;
            }
            catch (Exception e)
            {
                return new Models.Response<object>() { Ok = false, Data = null, Error = new Models.Error() { Code = 6, Message = $"Произошла ошибка при обращению к серверу. {e.Message}" } };
            }
        }
    }
}
