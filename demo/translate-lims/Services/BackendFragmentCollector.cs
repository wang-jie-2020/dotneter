using System.Text.RegularExpressions;
using System.Text;

namespace Translate.Services
{
    public class BackendFragmentCollector : IFragmentCollector
    {
        private readonly string[] extensions = { ".cs" };

        private readonly Regex[] regexes =
        {
           new Regex("(\"\\s*)[\\u4e00-\\u9fa5][^\"]*(\")"),
           new Regex( "('\\s*)[\\u4e00-\\u9fa5][^']*(')"),
        };

        private readonly Regex anyChineseRegex = new Regex("[\u4e00-\u9fa5]+");

        private readonly Regex annotationRegex = new Regex("[/]{2}.*");

        public bool Satisfied(FileInfo file)
        {
            var ext = file.Extension;
            return extensions.Contains(ext);
        }

        public async Task<CollectContext> CollectAsync(FileInfo file)
        {
            var context = new CollectContext()
            {
                Path = file.FullName
            };

            using (var stream = file.OpenRead())
            {
                stream.Seek(0, SeekOrigin.Begin);
                context.CoderTexts = await CollectCoderTextAsync(stream);
            }

            return context;
        }

        protected async Task<List<CoderText>> CollectCoderTextAsync(Stream stream)
        {
            var texts = new List<CoderText>();

            var lineNo = 0;
            var streamReader = new StreamReader(stream, Encoding.UTF8);
            while (!streamReader.EndOfStream)
            {
                lineNo++;
                var line = await streamReader.ReadLineAsync();

                if (line == null)
                    continue;

                bool anyMatch = false;
                foreach (var regex in regexes)
                {
                    if (regex.IsMatch(line))
                    {
                        anyMatch = true;
                        break;
                    }
                }

                if (anyMatch)
                {
                    texts.Add(new CoderText()
                    {
                        OriginalText = line,
                        Line = lineNo
                    });
                }
            }

            return texts;
        }
    }
}