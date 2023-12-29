using System;
using System.IO;
using System.Text;

namespace Generator.Extensions
{
    public static class FileExtensions
    {
        public static void CreateFile(this string dir, string content)
        {
            var directory = Path.GetDirectoryName(dir);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory ?? throw new InvalidOperationException());
            }

            using (var fileStream = new FileStream(dir, FileMode.Create, FileAccess.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(content);
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}