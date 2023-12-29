using Volo.Abp.Caching;

namespace Hugin.BookStore.FileObjects
{
    [CacheName("FileCache")]
    public class FileCto
    {
        public FileCto(string name, string type, byte[] content)
        {
            this.Name = name;
            this.Type = type;
            this.Content = content;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public byte[] Content { get; set; }
    }
}
