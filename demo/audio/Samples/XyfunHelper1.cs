using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Samples
{
    //这是一个过期的语音转写示例
    public class XyfunHelper1
    {
        static string appId = "6c37af35";
        static string appKey = "040eaa19cf13e2400ae95098816a7ba2";//SecretKey

        public static void Test()
        {
            var filePath = @"D:\Code\NAudioDemo.wav";
            var taskid = Prepare(filePath);
            UploadFile(taskid, filePath);
            MergeFile(taskid);
            Thread.Sleep(1000 * 60 * 2);//等待时间
            GetProgress(taskid);
            GetResult(taskid);
            Console.ReadKey();
        }

        static string Prepare(string filePath)
        {

            FileInfo t = new FileInfo(filePath);
            var fileSize = t.Length;
            var fileName = Path.GetFileName(filePath);
            var url = @"http://raasr.xfyun.cn/api/prepare";
            TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            long time = (long)mTimeSpan.TotalSeconds;
            var baseString = appId + time.ToString();
            //MD5加密
            var md5Str = Md5Encrypt(baseString);
            var signa = HMACSHA1Text2(md5Str, appKey);//HMACSHA1加密兼base64
            var dic = new Dictionary<string, string>();
            dic.Add("app_id", $"{appId}");
            dic.Add("signa", $"{signa}");
            dic.Add("ts", time.ToString());
            dic.Add("file_len", $"{fileSize}");
            dic.Add("file_name", $"{fileName}");
            dic.Add("slice_num", "1");
            dic.Add("language", "cn_cantonese");//广东话参数
            var result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpContent httpContent = new FormUrlEncodedContent(dic);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpResponseMessage responseMessage = client.PostAsync(url, httpContent).Result;
                result = responseMessage.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(result);
            var taskId = Regex.Match(result, @"(?<=""data"":"").+?(?="")").ToString();
            Console.WriteLine(taskId);
            return taskId;

        }

        public static void UploadFile(string taskId, string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var uploadUrl = @"https://raasr.xfyun.cn/api/upload";
            var signa2 = GetSigna();
            var ts2 = GetTs();
            var sliceId = GetSliceId();
            using (HttpClient client = new HttpClient())
            {
                var content = new MultipartFormDataContent();
                //content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");//这条不能加
                content.Add(new StringContent($"{appId}"), "app_id");//参数
                content.Add(new StringContent($"{signa2}"), "signa");
                content.Add(new StringContent($"{ts2}"), "ts");
                content.Add(new StringContent($"{taskId}"), "task_id");
                content.Add(new StringContent($"{sliceId}"), $"slice_id");
                FileStream fStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                content.Add(new StreamContent(fStream, (int)fStream.Length), "content", fileName);//文件,content为文件参数名称
                var res = client.PostAsync(uploadUrl, content).GetAwaiter().GetResult().Content.ReadAsStringAsync().Result;
                Console.WriteLine(res);
            }
        }

        public static void MergeFile(string taskId)
        {
            var url = @"https://raasr.xfyun.cn/api/merge";
            var signa = GetSigna();
            var ts = GetTs();
            var dic = new Dictionary<string, string>();
            dic.Add("app_id", $"{appId}");
            dic.Add("signa", $"{signa}");
            dic.Add("ts", ts.ToString());
            dic.Add("task_id", $"{taskId}");
            var result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpContent httpContent = new FormUrlEncodedContent(dic);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpResponseMessage responseMessage = client.PostAsync(url, httpContent).Result;
                result = responseMessage.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(result);

        }

        public static void GetProgress(string taskId)
        {
            var url = "https://raasr.xfyun.cn/api/getProgress";
            var signa = GetSigna();
            var ts = GetTs();
            var dic = new Dictionary<string, string>();
            dic.Add("app_id", $"{appId}");
            dic.Add("signa", $"{signa}");
            dic.Add("ts", ts.ToString());
            dic.Add("task_id", $"{taskId}");
            var result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpContent httpContent = new FormUrlEncodedContent(dic);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpResponseMessage responseMessage = client.PostAsync(url, httpContent).Result;
                result = responseMessage.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(result);
        }

        public static void GetResult(string taskId)
        {
            var url = "https://raasr.xfyun.cn/api/getResult";
            var signa = GetSigna();
            var ts = GetTs();
            var dic = new Dictionary<string, string>();
            dic.Add("app_id", $"{appId}");
            dic.Add("signa", $"{signa}");
            dic.Add("ts", ts.ToString());
            dic.Add("task_id", $"{taskId}");
            var result = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                HttpContent httpContent = new FormUrlEncodedContent(dic);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                httpContent.Headers.ContentType.CharSet = "utf-8";
                HttpResponseMessage responseMessage = client.PostAsync(url, httpContent).Result;
                result = responseMessage.Content.ReadAsStringAsync().Result;
            }
            Console.WriteLine(result);
        }

        public static string Md5Encrypt(string str)
        {
            MD5 md5 = MD5.Create();
            byte[] buffer = Encoding.UTF8.GetBytes(str);//将字符串转成字节数组
            byte[] byteArray = md5.ComputeHash(buffer);//调用加密方法
            StringBuilder sb = new StringBuilder();
            foreach (byte b in byteArray)//遍历字节数组
            {
                sb.Append(b.ToString("x2"));//将字节数组转成16进制的字符串。X表示16进制，2表示每个16字符占2位
            }
            return sb.ToString();
        }

        //加密后再次Base64编码
        public static string HMACSHA1Text2(string text, string key)
        {
            //HMACSHA1加密
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = Encoding.UTF8.GetBytes(key);

            byte[] dataBuffer = Encoding.UTF8.GetBytes(text);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);

            return Convert.ToBase64String(hashBytes);

        }

        public static string GetSigna()
        {
            TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            long time = (long)mTimeSpan.TotalSeconds;
            var baseString = appId + time.ToString();
            //MD5加密
            var md5Str = Md5Encrypt(baseString);
            var signa = HMACSHA1Text2(md5Str, appKey);//HMACSHA1加密兼base64
            return signa;
        }

        public static string GetTs()
        {
            TimeSpan mTimeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0);
            long time = (long)mTimeSpan.TotalSeconds;
            return time.ToString();
        }

        public static string GetSliceId()
        {
            string sliceId = "aaaaaaaaa`";
            char[] ch = sliceId.ToCharArray();
            for (int i = 0, j = sliceId.Length - 1; i < sliceId.Length && j >= 0; i++)
            {
                if (ch[j] != 'z')
                {
                    ch[j]++;
                    break;
                }
                else
                {
                    ch[j] = 'a';
                    j--;
                    continue;
                }
            }
            return new string(ch);
        }
    }
}