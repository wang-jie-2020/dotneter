using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocket4Net;

namespace Samples
{
    public class XyfunHelper3
    {
        private const string hostUrl = "wss://rtasr.xfyun.cn/v1/ws?"; //http url 不支持解析 ws/wss schema    
        private const string appid = "6c37af35";//到控制台-语音合成页面获取
        private const string APIKey = "281cad92ed99b869eff148ce8dc735f7";//到控制台-语音合成页面获取

        //private const String fileurl = @"D:\Code\16k_10.pcm";

        //private const string fileurl = "D:\\Code\\java\\rtasr_java_demo_wss\\java\\rtasr-demo\\resource\\test_1.pcm";

        private const string fileurl = "D:\\Code\\Audios\\20231127104609.wav";

        private WebSocket webSocket;
        MemoryStream pcmStream = new MemoryStream();
        public string result = "";

        //public XyfunHelper3()
        //{
        //    string uri = GetAuthUrl(hostUrl, APIKey);
        //    webSocket = new WebSocket(uri);
        //    webSocket.Opened += OnOpened;
        //    webSocket.Closed += OnClosed;
        //    webSocket.Error += OnError;
        //    webSocket.MessageReceived += OnMessageReceived;
        //    webSocket.Open();

        //}

        public void Test()
        {
            string uri = GetAuthUrl(hostUrl, APIKey);
            webSocket = new WebSocket(uri);
            webSocket.Opened += OnOpened;
            webSocket.Closed += OnClosed;
            webSocket.Error += OnError;
            webSocket.MessageReceived += OnMessageReceived;
            webSocket.Open();

        }


        private void OnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }
        public StringBuilder retsb = new StringBuilder();
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("OnMessageReceived");
            dynamic msg = JsonConvert.DeserializeObject(e.Message);
            if (msg.action == "result")
            {

                //var adsfas = JsonConvert.DeserializeObject<Root>(JObject.Parse(e.Message).SelectToken("data").ToString());
                dynamic adsfas = JsonConvert.DeserializeObject(JObject.Parse(e.Message).SelectToken("data").ToString());
                foreach (var item in adsfas.cn.st.rt)
                {
                    foreach (var item1 in item.ws)
                    {

                        foreach (var item3 in item1.cw)
                        {
                            Console.WriteLine($"{DateTime.Now.ToString("F")} result: {item3.w}");
                            retsb.Append(item3.w.ToString());
                        }
                    }
                }
                if (adsfas.ls == "true")
                {
                    ///结果识别结果
                    result = retsb.ToString();
                }
            }
        }



        private void OnClosed(object sender, EventArgs e)
        {
            Console.WriteLine("OnClosed");
            //dynamic msg = JsonConvert.DeserializeObject(retsb.ToString());
            //retsb.Clear();
            //foreach (var item in msg.cn.st.rt.ws)
            //{
            //    retsb.Append(item.cw.w);
            //}

            string adsf = retsb.ToString();
            //Console.WriteLine("OnClosed+结果" + adsf);
        }

        private void OnOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OnOpened");


            var filestream = File.Open(fileurl, FileMode.Open);
            while (true)
            {
                byte[] buffer = new byte[1280];//一次读取5M内容
                int r = filestream.Read(buffer, 0, buffer.Length);//实际读取的有效字节数

                if (r == 0)//读到最后内容
                {
                    break;
                }

                //以ANSI默认格式读取文本内容
                //string tempStr = Encoding.Default.GetString(buffer, 0, r);
                //Console.WriteLine(tempStr);

                webSocket.Send(buffer, 0, buffer.Length);
                Thread.Sleep(40);
            }



            webSocket.Send("{\"end\": true}");


        }

        public string MD5Hash(string source)
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

        public string ToBase64hmac(string strText, string strKey)

        {

            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(strKey));

            byte[] byteText = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(strText));

            return Convert.ToBase64String(byteText);

        }
        private string GetAuthUrl(string hostUrl, string apiKey)
        {
            Uri url = new Uri(hostUrl);
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var tt1 = Convert.ToInt64(ts.TotalSeconds).ToString();

            string tt = appid + tt1;
            var tt5 = MD5Hash(tt);

            string signa = ToBase64hmac(tt5, apiKey);




            UriBuilder builder = new UriBuilder()
            {
                Scheme = "wss",
                Host = url.Host,
                Path = url.AbsolutePath,
                Query = $"?appid={appid}&ts={System.Net.WebUtility.UrlEncode(tt1)}&signa={signa}",
            };
            return builder.ToString();
        }

        public void Play()
        {

            string aftee23 = retsb.ToString();


            pcmStream.Seek(0, SeekOrigin.Begin);

            using (var fs = File.Create("test.pcm"))
            {
                pcmStream.WriteTo(fs);//保存为语音文件



                FileStream fileStream = new FileStream("ptest111.wav", FileMode.Create);
                pcmStream.WriteTo(fileStream);
                pcmStream.Close();
                fileStream.Close();
            }
            //WaveOutEvent waveOutEvent = new WaveOutEvent();
            //waveOutEvent.Init(new RawSourceWaveStream(pcmStream, new WaveFormat(16000, 1)));
            //waveOutEvent.Play();
            Console.WriteLine("正在播放，按回车键停止");
            Console.ReadLine();
            // waveOutEvent.Stop();
            pcmStream.Close();
        }
    }
}

