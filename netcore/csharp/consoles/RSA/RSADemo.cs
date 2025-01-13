using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace consoles.RSA
{
    internal class RASDemo
    {
        void Method1()
        {
            X509Certificate2 cer = CertificateHelper.GetCertificateFromPfxFile(
                Path.Combine(Path.GetDirectoryName(typeof(RASDemo).Assembly.Location) ?? string.Empty, "RSA", "ids4.pfx"), "lg");

            string keyPublic = cer.PublicKey.Key.ToXmlString(false); // 公钥  
            string keyPrivate = cer.PrivateKey.ToXmlString(true); // 私钥  

            string cypher = RSAEncrypt(keyPublic, "程序员3"); // 加密  
            string plain = RSADecrypt(keyPrivate, cypher); // 解密  

            System.Diagnostics.Debug.Assert(plain == "程序员3");
        }

        /// <summary>
        /// RSA解密 
        /// </summary>
        /// <param name="xmlPrivateKey"></param>
        /// <param name="m_strDecryptString"></param>
        /// <returns></returns>
        static string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPrivateKey);
            byte[] rgb = Convert.FromBase64String(m_strDecryptString);
            byte[] bytes = provider.Decrypt(rgb, false);
            return new UnicodeEncoding().GetString(bytes);
        }

        /// <summary>     
        /// RSA加密     
        /// </summary>     
        /// <param name="xmlPublicKey"></param>     
        /// <param name="m_strEncryptString"></param>     
        /// <returns></returns>     
        static string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.FromXmlString(xmlPublicKey);
            byte[] bytes = new UnicodeEncoding().GetBytes(m_strEncryptString);
            return Convert.ToBase64String(provider.Encrypt(bytes, false));
        }
    }
}
