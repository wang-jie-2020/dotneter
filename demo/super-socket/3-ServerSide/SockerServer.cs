using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerSide
{
    internal class SocketServer
    {
        private static Socket tcpServer;

        public static void Init()
        {
            tcpServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建socket对象
            //tcpServer.Bind(new IPEndPoint(IPAddress.Parse("172.21.0.16"),18200));
            tcpServer.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 18200));
            tcpServer.Listen(100);
            //Thread serverThread = new Thread(serverRead);

            Console.WriteLine("服务器已启动，等待连接.......");

            while (true)
            {
                Socket clientSocket = tcpServer.Accept();
                var epoint = (clientSocket.RemoteEndPoint as IPEndPoint);
                Console.WriteLine(epoint.Address + ":" + epoint.Port + "已连接");
                //serverThread.Start();
                Task.Run(() => ServerRead(clientSocket));

                //_Client = new Client(clientSocket);
            }
        }

        private static void ServerRead(Socket clientSocket)
        {
            string recvStr = "";
            byte[] recvBytes = new byte[1024];
            int bytes;
            do
            {
                bytes = clientSocket.Receive(recvBytes, recvBytes.Length, 0); //从客户端接受消息
                recvStr += Encoding.ASCII.GetString(recvBytes, 0, bytes);
            } while (bytes == 1024);
            Console.WriteLine("{0} 收到：{1}", clientSocket.RemoteEndPoint.ToString(), recvStr);
        }
    }
}