using Microsoft.Extensions.Options;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;

namespace NetServer.Server
{
    public class SocketServerInitializer
    {
        private readonly ILogger<SocketServerInitializer> _logger;
        private readonly SocketServerOptions _socketServerOptions;

        public SocketServerInitializer(
            IOptions<SocketServerOptions> socketServerOptions,
            ILogger<SocketServerInitializer> logger)
        {
            _logger = logger;
            _socketServerOptions = socketServerOptions.Value;
        }

        //static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public void Initialize()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, _socketServerOptions.Port));
            socket.Listen(100);

            socket.BeginAccept(OnAccept, socket);

            //int receivedDataSize = 10;
            //listener.BeginAccept(null, receivedDataSize, AcceptReceiveDataCallback, listener);
        }



        //public static void AcceptReceiveDataCallback(IAsyncResult ar)
        //{
        //    // Get the socket that handles the client request.
        //    Socket listener = (Socket)ar.AsyncState;

        //    // End the operation and display the received data on the console.
        //    byte[] Buffer;
        //    int bytesTransferred;
        //    Socket handler = listener.EndAccept(out Buffer, out bytesTransferred, ar);
        //    string stringTransferred = Encoding.ASCII.GetString(Buffer, 0, bytesTransferred);

        //    Console.WriteLine(stringTransferred);
        //    Console.WriteLine("Size of data transferred is {0}", bytesTransferred);

        //    // Create the state object for the asynchronous receive.
        //    StateObject state = new StateObject();
        //    state.workSocket = handler;
        //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
        //        new AsyncCallback(ReadCallback), state);
        //}


        //private void OnAccept(IAsyncResult ar)
        //{
        //    Socket listener = ar.AsyncState as Socket ?? throw new ArgumentNullException(nameof(listener));
        //    Socket handler = listener.EndAccept(ar);
        //    listener.BeginAccept(OnAccept, listener);

        //    byte[] buffer = new byte[100];
        //    var bufferLength = handler.Receive(buffer);

        //    string transferred = Encoding.ASCII.GetString(buffer, 0, bufferLength);
        //    handler.Send(Encoding.UTF8.GetBytes(transferred));
        //    handler.Close();
        //}

        public static void OnAccept(IAsyncResult ar)
        {
            var serverSocket = ar.AsyncState as Socket;

            //客户端socket
            var clientSocket = serverSocket.EndAccept(ar);

            //服务端进行下一步监听
            serverSocket.BeginAccept(OnAccept, serverSocket);

            var bytes = new byte[1000];
            //获取客户端socket内容
            var len = clientSocket.Receive(bytes);
            //转化正字符串
            var request = Encoding.UTF8.GetString(bytes, 0, len);

            //返回给客户端了
            clientSocket.Send(Encoding.UTF8.GetBytes(request));

            clientSocket.Close();

        }
    }
}
