//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace NetClient.Client
//{
//    public class NetSocket
//    {
//        private Thread revdataThread;

//        private TcpClient tcpClient;

//        private NetworkStream networkStream;

//        public bool state = false;

//        public event ReceiveData OnRead;

//        public bool ConnectServer(string sHost, int iPort)
//        {
//            try
//            {
//                tcpClient = new TcpClient();
//                tcpClient.Connect(sHost, iPort);
//                networkStream = tcpClient.GetStream();
//                revdataThread = new Thread(ReceiveDataFromTcp);
//                revdataThread.Start();
//                LogHelper.WriteLog($"连接设备{sHost}:{iPort},连接成功", null, LogType.Info);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                LogHelper.WriteLog($"连接设备{sHost}:{iPort},连接失败，失败原因：{ex.ToString()}");
//                return false;
//            }
//        }

//        public void Disconnection()
//        {
//            networkStream.Close();
//            tcpClient.Close();
//        }

//        public void SendDataFromTcp(string sSendString)
//        {
//            try
//            {
//                sSendString += "end";
//                byte[] bytes = Encoding.UTF8.GetBytes(sSendString);
//                networkStream.Write(bytes, 0, bytes.Length);
//            }
//            catch (Exception ex)
//            {
//                LogHelper.WriteLog(string.Format("发送数据失败，失败原因：{0}\n\r发送数据：{0}", ex.Message, sSendString));
//            }
//        }

//        public void ReceiveDataFromTcp()
//        {
//            while (true)
//            {
//                bool flag = true;
//                try
//                {
//                    state = true;
//                    int num = 0;
//                    List<byte> list = new List<byte>();
//                    string text = "";
//                    while (num != -1)
//                    {
//                        num = networkStream.ReadByte();
//                        list.Add((byte)num);
//                        if (!networkStream.DataAvailable)
//                        {
//                            text = Encoding.UTF8.GetString(list.ToArray());
//                            string revString = text;
//                            text = "";
//                            OnRead(((IPEndPoint)tcpClient.Client.RemoteEndPoint).Address.ToString(), revString);
//                            list.Clear();
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    LogHelper.WriteLog($"接收数据失败，失败原因：{ex.Message}");
//                    state = false;
//                    return;
//                }
//            }
//        }
//    }
//}