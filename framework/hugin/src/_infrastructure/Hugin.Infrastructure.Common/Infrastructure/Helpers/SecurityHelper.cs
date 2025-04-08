using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hugin.Infrastructure.Helpers
{
    public static class SecurityHelper
    {
        #region md5

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Md5(string str)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    builder.Append(result[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        #endregion

        #region sha256

        /// <summary>
        /// sha256加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Sha256(string str)
        {
            using (var sha256 = SHA256.Create())
            {
                var result = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    builder.Append(result[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        #endregion

        #region AES

        const string AES_IV = "1234567890000000";

        /// AES加密算法  
        /// <param name="input">明文字符串</param>  
        /// <param name="key">密钥（32位）</param>  
        /// <returns>字符串</returns>  
        public static string EncryptByAes(string input, string key)
        {
            key = key.PadRight(32, '0');
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }

                        byte[] bytes = msEncrypt.ToArray();
                        return ByteArrayToHexString(bytes);
                    }
                }
            }
        }

        /// <summary>  
        /// AES解密  
        /// </summary>  
        /// <param name="input">密文字节数组</param>  
        /// <param name="key">密钥（32位）</param>  
        /// <returns>返回解密后的字符串</returns>  
        public static string DecryptByAes(string input, string key)
        {
            key = key.PadRight(32, '0');

            byte[] inputBytes = HexStringToByteArray(input);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 32));

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = keyBytes;
                aesAlg.IV = Encoding.UTF8.GetBytes(AES_IV.Substring(0, 16));

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srEncrypt = new StreamReader(csEncrypt))
                        {
                            return srEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将指定的16进制字符串转换为byte数组
        /// </summary>
        /// <param name="s">16进制字符串(如：“7F 2C 4A”或“7F2C4A”都可以)</param>
        /// <returns>16进制字符串对应的byte数组</returns>
        private static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        /// <summary>
        /// 将一个byte数组转换成一个格式化的16进制字符串
        /// </summary>
        /// <param name="data">byte数组</param>
        /// <returns>格式化的16进制字符串</returns>
        private static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
            {
                //16进制数字
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
                //16进制数字之间以空格隔开
                //sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return sb.ToString().ToUpper();
        }


        #endregion
    }
}
