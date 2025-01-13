using System.Reflection;
using SevenZip;

namespace ConsoleApp1
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Environment.Is64BitProcess ? "x64" : "x86", "7z.dll");
			SevenZip.SevenZipBase.SetLibraryPath(path);


			var tmp = new SevenZipCompressor(); //7z压缩
			tmp.ScanOnlyWritable = true; //只可写
			//tmp.CompressFiles()这个有三个重载，这里只讲其中一个比较常用的。
			//public void CompressFiles(string archiveName, params string[] fileFullNames)
			//archiveName:这个是代表生成的7z文件存在哪里
			//fileFullNames:这个参数是要压缩的文件是一个params数组，特别注意必须是完整的路径名才有效
			//tmp.CompressFiles(@"D:\max\arch.7z", @"D:\max\SourceCode\DataExch\SevenZipSharpDemo\bin\Debug\test.txt", @"D:\max\SourceCode\DataExch\SevenZipSharpDemo\bin\Debug\test1.txt");

			//tmp.CompressDirectory 压缩指定路径下面的所有文件,这个有12个重载，也只讲其中一个简单的。
			// public void CompressDirectory( string directory, string archiveName) 
			tmp.CompressDirectory(@"C:\Users\jie.wang21\Desktop\tool", @"C:\Users\jie.wang21\Desktop\arch.7z");




			Console.WriteLine("Hello, World!");
		}
	}
}
