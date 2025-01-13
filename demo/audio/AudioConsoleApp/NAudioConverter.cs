﻿using System.Security.Cryptography;
using System.Text;
using NAudio.Wave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;
using ErrorEventArgs = SuperSocket.ClientEngine.ErrorEventArgs;

namespace AudioConsoleApp
{
    internal class NAudioConverter
    {
        public int mode { get; }
        private IWaveIn capture;
        private WebSocket socket;

        public NAudioConverter(int _mode)
        {
            mode = _mode;
        }

        public void RunOnce()
        {
            Init();

            Console.WriteLine("按回车键开始。");
            Console.ReadLine();

            BeginSocket();
            capture.StartRecording();

            Console.WriteLine("正在录音，按回车键停止。");
            Console.ReadLine();

            capture.StopRecording();
            
            //在实时转写中,这个end是不是有必要的?
            //Socket.Send("{\"end\":true}");
            //Socket.Dispose();

            Console.WriteLine("按回车键退出");
            Console.ReadLine();
        }

        public void Init()
        {
            var record = Path.Combine(AppContext.BaseDirectory,
                "Audios",
                $"{DateTime.Now.ToString("yyyyMMddhhmmss")}.wav");

            //讯飞要求的音频要求是采样率16k 采样深度16bit 
            if (mode == 1)
            {
                //麦克风
                capture = new WaveInEvent();
            }
            else
            {
                //音频输出
                capture = new WasapiLoopbackCapture();
            }
            
            capture.WaveFormat = new WaveFormat(16000, 16, 1);

            var writer = new WaveFileWriter(record, capture.WaveFormat);
            capture.DataAvailable += (_s, _e) =>
            {
                writer.Write(_e.Buffer, 0, _e.BytesRecorded);
                Send(_e.Buffer);
            };

            capture.RecordingStopped += (_s, _e) =>
            {
                writer.Dispose();
                capture.Dispose();

                var waveOutEvent = new NAudio.Wave.WaveOutEvent();
                var waveReader = new NAudio.Wave.WaveFileReader(record);
                waveOutEvent.Init(waveReader);
                waveOutEvent.Play();
            };
        }

        public void Send(byte[] barr)
        {
            if (socket.State != WebSocketState.Open)
            {
                return;
            }

            MemoryStream ms = new MemoryStream(barr);
            while (true)
            {
                //建议音频流每40ms发送1280字节,see https://www.xfyun.cn/doc/asr/rtasr/API.html#%E6%8E%A5%E5%8F%A3%E8%A6%81%E6%B1%82
                var buffer = new byte[1280];
                int r = ms.Read(buffer, 0, buffer.Length);

                if (r == -1 || r == 0)
                {
                    break;
                }

                socket.Send(buffer, 0, buffer.Length);
                Thread.Sleep(40); 
            }
        }

        private const string HostUrl = "wss://rtasr.xfyun.cn/v1/ws?";
        private const string AppId = "6c37af35";
        private const string ApiKey = "281cad92ed99b869eff148ce8dc735f7";
        private string result = string.Empty;

        public void BeginSocket()
        {
            string uri = GetAuthUrl(HostUrl, ApiKey);
            socket = new WebSocket(uri);
            socket.Opened += OnOpened;
            socket.Closed += OnClosed;
            socket.Error += OnError;
            socket.MessageReceived += OnMessageReceived;
            socket.Open();
        }

        private string GetAuthUrl(string hostUrl, string apiKey)
        {
            Uri url = new Uri(hostUrl);
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var tt1 = Convert.ToInt64(ts.TotalSeconds).ToString();

            string tt = AppId + tt1;
            var tt5 = MD5Hash(tt);

            string signa = ToBase64hmac(tt5, apiKey);

            UriBuilder builder = new UriBuilder()
            {
                Scheme = "wss",
                Host = url.Host,
                Path = url.AbsolutePath,
                Query = $"?appid={AppId}&ts={System.Net.WebUtility.UrlEncode(tt1)}&signa={System.Net.WebUtility.UrlEncode(signa)}",
            };

            return builder.ToString();
        }

        private string MD5Hash(string source)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(source));

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }

        private string ToBase64hmac(string strText, string strKey)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(strKey));
            byte[] byteText = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(strText));
            return System.Convert.ToBase64String(byteText);
        }

        private void OnOpened(object? sender, EventArgs e)
        {
            Console.WriteLine("OnOpened");
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            Console.WriteLine("OnClosed");
        }

        private void OnError(object? sender, ErrorEventArgs e)
        {
            Console.WriteLine("Error: " + e.Exception.Message);
        }

        private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            dynamic msg = JsonConvert.DeserializeObject(e.Message) ?? throw new InvalidOperationException();
            if (msg.action == "started")
            {
                //todo 似乎DF不够,连接不稳定
                Console.WriteLine("握手成功");
            }
            else if (msg.action == "result")
            {
                var ret = new StringBuilder();

                dynamic data = JsonConvert.DeserializeObject(JObject.Parse(e.Message).SelectToken("data").ToString());
                foreach (var item in data.cn.st.rt)
                {
                    foreach (var item1 in item.ws)
                    {
                        foreach (var item3 in item1.cw)
                        {
                            ret.Append(item3.w.ToString());
                        }
                    }
                }

                //Console.WriteLine($"{DateTime.Now:yyyyMMddhhmmss}的中间结果 ---{ret.ToString()}");
                if (data.cn.st.type == 0)
                {
                    result += ret.ToString();
                    ret.Clear();
                    Console.WriteLine(result + ret.ToString());
                }

                //这个还是不要了,文字太多
                //Console.WriteLine(result + ret.ToString());
            }
            else if (msg.action == "error")
            {
                Console.WriteLine("出错了：" + msg);
            }
        }
    }
}
