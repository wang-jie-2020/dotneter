//using System;
//using System.Security.Cryptography;
//using System.Text;

//namespace Hugin.Infrastructure.Helpers
//{
//    public static class AppHelper
//    {
//        /// <summary>
//        /// appId appSecret
//        /// </summary>
//        /// <param name="data"></param>
//        /// <returns></returns>
//        public static (string appId, string appSecret) Get(string data)
//        {
//            string appid = CreateMd5(data + DateTime.Now.ToFileTimeUtc() + new Random(10000).Next());
//            string appsecret = CreateMd5(appid + DateTime.Now.ToFileTimeUtc() + new Random(10000).Next());
//            return (appid, appsecret);
//        }

//        /// <summary>
//        /// MD5加密
//        /// </summary>
//        /// <param name="str"></param>
//        /// <returns></returns>
//        private static string CreateMd5(string str)
//        {
//            using (var md5 = MD5.Create())
//            {
//                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
//                var strResult = BitConverter.ToString(result);

//                return strResult.Replace("-", "");
//            }
//        }
//    }
//}
