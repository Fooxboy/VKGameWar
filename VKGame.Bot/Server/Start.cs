using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace VKGame.Bot.Server
{
    public class Start
    {
        public static void Listen()
        {
            int portIn = 8003;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portIn);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);

                    var result = builder.ToString();
                    Logger.WriteDebug($"НА СЕРВЕР ПОСТУПИЛ ЗАПРОС:  {result}");

                    var response = Core.Processing(result);
                    Logger.WriteDebug($"СЕРВЕР ОТВЕТИЛ:  {response}");

                    // отправляем ответ
                    data = Encoding.Unicode.GetBytes(response);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception e)
            {
                Bot.Statistics.NewError();
                Logger.WriteError($"{e.Message} \n {e.StackTrace}");
            }
        }
    }
}
