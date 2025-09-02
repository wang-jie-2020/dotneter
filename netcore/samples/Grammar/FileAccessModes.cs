namespace Grammar;

public class FileAccessModes
{
    static FileStream stream;
    
    void Method1()
    {
        var path = @"C:\Users\jie.wang21\Desktop\新建文件夹\Rack11_Bat Vol_2025-04-08_16-31-19.csv";
        stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 32768, FileOptions.RandomAccess);

        Console.WriteLine(stream.Length);
    }

    void Method2()
    {
        stream.Dispose();
    }
}