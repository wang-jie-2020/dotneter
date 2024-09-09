using System.Security.Cryptography;

namespace Yi.Framework.Core.Helper;

public class RSAFileHelper
{
    public static RSA GetKey()
    {
        return GetRSA("key.pem");
    }

    public static RSA GetPublicKey()
    {
        return GetRSA("public.pem");
    }

    private static RSA GetRSA(string fileName)
    {
        var rootPath = Directory.GetCurrentDirectory();
        var filePath = Path.Combine(rootPath, fileName);
        if (!File.Exists(filePath))
            throw new Exception("文件不存在");
        var key = File.ReadAllText(filePath);
        var rsa = RSA.Create();
        rsa.ImportFromPem(key.AsSpan());
        return rsa;
    }
}