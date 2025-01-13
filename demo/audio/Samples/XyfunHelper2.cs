﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebSocket4Net;
using System.IO;
using System.Formats.Asn1;
using System.Threading;

namespace Samples
{
    /// <summary>
    /// https://www.xfyun.cn/doc/asr/voicedictation/API.html#%E6%8E%A5%E5%8F%A3%E8%AF%B4%E6%98%8E
    /// </summary>
    public class XyfunHelper2
    {
        private const string hostUrl = "wss://ws-api.xfyun.cn/v2/iat"; //http url 不支持解析 ws/wss schema    
        private const string appid = "6c37af35";//到控制台-语音合成页面获取
        private const string APIKey = "eba8cc8b1c51406c9b39a1fe8467e11c";//到控制台-语音合成页面获取
        private const string APISecret = "ODhiMThhMWI4MzgwNjZkYmIzYzI0YTNk";//到控制台-语音合成页面获取
        private const string testFile = @"D:\Code\16k_10.pcm";  //测试文件
        private WebSocket webSocket;
        MemoryStream pcmStream = new MemoryStream();

        //public XyfunHelper2()
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

        enum Status
        {
            FirstFrame = 0,
            ContinueFrame = 1,
            LastFrame = 2
        }
        private volatile Status status = Status.FirstFrame;
        private void OnError(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }
        public StringBuilder retsb = new StringBuilder();
        public string resultStr = "";
        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("OnMessageReceived");
            Console.WriteLine(e.Message);
            dynamic msg = JsonConvert.DeserializeObject(e.Message);
            if (msg.code != 0)
            {
                Console.WriteLine($"error => {msg.message},sid => {msg.sid}");
                return;
            }
            var ws = msg.data.result.ws;
            if (ws == null)
            {
                return;
            }
            foreach (var item in ws)
            {
                resultStr = resultStr + item.cw[0].w;
                Console.Write(item.cw[0].w);
            }
            Console.WriteLine();
            if (msg.data.status == 2)
            {
                Console.Write("识别结果:" + resultStr);
                Console.WriteLine("识别结束");
                webSocket.Close();
            }
        }
        private void OnClosed(object sender, EventArgs e)
        {
            Console.WriteLine("OnClosed");

            string adsf = retsb.ToString();
            Console.WriteLine("OnClosed+结果" + adsf);
        }
        private void OnOpened(object sender, EventArgs e)
        {
            Console.WriteLine("OnOpened");
            var filestream = File.Open(testFile, FileMode.Open);
            while (true)
            {
                byte[] buffer = new byte[1280];//一次读取5M内容
                int r = filestream.Read(buffer, 0, buffer.Length);//实际读取的有效字节数
                if (r < 1280)//读到最后内容
                {
                    status = Status.LastFrame;

                }
                switch (status)
                {
                    case Status.FirstFrame:
                        {
                            dynamic frame = new JObject();
                            frame.common = new JObject
                        {
                            {"app_id" ,appid }
                        };
                            frame.business = new JObject
                        {
                            { "language","zh_cn" },
                            { "domain","iat" },
                            { "accent","mandarin"},
                            { "dwa","wpgs"}
                        };
                            frame.data = new JObject
                        {
                            { "status",(int)Status.FirstFrame },
                            { "format","audio/L16;rate=16000"},
                            { "encoding","raw" },
                            { "audio",Convert.ToBase64String(buffer)}
                        };
                            webSocket.Send(frame.ToString());
                            status = Status.ContinueFrame;
                        }
                        break;
                    case Status.ContinueFrame:
                        {
                            dynamic frame = new JObject();
                            frame.data = new JObject
                        {
                            { "status",(int)Status.ContinueFrame },
                            { "format","audio/L16;rate=16000"},
                            { "encoding","raw" },
                            { "audio",Convert.ToBase64String(buffer)}
                        };
                            webSocket.Send(frame.ToString());
                        }
                        break;
                    case Status.LastFrame:
                        {
                            dynamic frame = new JObject();
                            frame.data = new JObject
                        {
                            { "status",(int)Status.LastFrame },
                            { "format","audio/L16;rate=16000"},
                            { "encoding","raw" },
                            { "audio",Convert.ToBase64String(buffer)}
                        };
                            webSocket.Send(frame.ToString());
                            break;
                        }
                        break;
                    default:
                        break;
                }
                if (r < 1280)//读到最后内容
                {
                    break;
                }
            }
        }
        private string GetAuthUrl(string hostUrl, string apiKey)
        {
            Uri url = new Uri(hostUrl);

            var date = DateTime.Now.ToString("R");
            string signature_origin = "host: ws-api.xfyun.cn\ndate: " + date + "\nGET /v2/iat HTTP/1.1";
            HMAC hmac = HMAC.Create("System.Security.Cryptography.HMACSHA256");
            hmac.Key = Encoding.UTF8.GetBytes(APISecret);
            var signature_sha = hmac.ComputeHash(Encoding.UTF8.GetBytes(signature_origin));
            var signature_origin1 = Convert.ToBase64String(signature_sha);
            var authorization_origin = string.Format("api_key=\"{0}\", algorithm=\"hmac-sha256\", headers=\"host date request-line\", signature=\"{1}\"", APIKey, signature_origin1);
            string authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorization_origin));
            UriBuilder builder = new UriBuilder()
            {
                Scheme = "wss",
                Host = url.Host,
                Path = url.AbsolutePath,
                Query = $"?authorization={authorization}&date={System.Net.WebUtility.UrlEncode(date)}&host=ws-api.xfyun.cn",
            };
            return builder.ToString();
        }
    }
}

