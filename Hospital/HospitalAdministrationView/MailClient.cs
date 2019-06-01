using System;
using System.Configuration;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace HospitalAdministrationView
{
    public static class MailClient
    {
        private static TcpClient mailClient;

        private static SslStream stream;

        private static StreamReader reader;

        private static StreamWriter writer;

        public static void Connect()
        {
            string response = null;

            mailClient = new TcpClient();
            mailClient.Connect("pop.gmail.com", 995);

            stream = new SslStream(mailClient.GetStream());
            stream.AuthenticateAsClient("pop.gmail.com");

            reader = new StreamReader(stream, Encoding.ASCII);
            writer = new StreamWriter(stream);

            response = reader.ReadLine();
            response = SendRequest(reader, writer,
                string.Format("USER {0}", ConfigurationManager.AppSettings["MailLogin"]),
                "Ошибка авторизации, неверный логин");

            response = SendRequest(reader, writer,
                string.Format("PASS {0}",
                ConfigurationManager.AppSettings["MailPassword"]),
                "Ошибка авторизации, неверный пароль");
        }

        /// <summary>
        /// Отправка запроса и полчение ответа от почтового сервера
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private static string SendRequest(StreamReader reader, StreamWriter writer,
        string message, string errorMessage)
        {
            writer.WriteLine(message);
            writer.Flush();
            var response = reader.ReadLine();
            if (response.StartsWith("-ERR"))
            {
                throw new Exception(errorMessage);
            }
            return response;
        }
    }
}
