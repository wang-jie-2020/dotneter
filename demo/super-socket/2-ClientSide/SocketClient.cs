using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientSide
{
    internal class SocketClient
    {
        public static void Init()
        {

            TcpClient client = new TcpClient();

            client.Client.Connect(IPAddress.Parse("127.0.0.1"), 4040);
            Task.Run(() => ReadDataLoop(client));

            var tmp1 = Encoding.ASCII.GetBytes("{\"TID\":1111,\"blabla\":{}}");
            var tmp2 = Encoding.ASCII.GetBytes("{\"TID\":1111,\"blabla\":{}}" + "\r\n");


            client.Client.Send(Encoding.ASCII.GetBytes("{\"TID\":1111,\"blabla\":{}}" + "\r\n"));

            //client.Client.Connect(IPAddress.Parse("127.0.0.1"), 18200);

            ////Task.Run(() => ReadData(client)); 

            //Task.Run(() => ReadDataLoop(client));

            //client.Client.Send(Encoding.ASCII.GetBytes("{\"TID\":1111,\"blabla\":{}}" + "\r\n"));




            Console.Read();
        }

        private static void ReadDataLoop(TcpClient client)
        {
            while (true)
            {
                if (!client.Connected)
                    break;
                string xxx = "";
                xxx = ReadData(client);
                Console.WriteLine(xxx);
            }
        }

        private static string ReadData(TcpClient client)
        {
            string retVal;
            byte[] data = new byte[1024];
            NetworkStream stream = client.GetStream();
            byte[] myReadBuffer = new byte[1024];
            StringBuilder myCompleteMessage = new StringBuilder();
            int numberOfBytesRead = 0;
            do
            {
                numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

            }
            while (stream.DataAvailable);
            retVal = myCompleteMessage.ToString();
            return retVal;
        }
    }
}