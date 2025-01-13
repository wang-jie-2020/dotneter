using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Translate.Services
{
    public class FrontFragmentCollector : IFragmentCollector
    {
        private readonly string[] extensions = { ".js", ".vue" };

        private readonly Regex[] regexes =
        {
           new Regex("(\"\\s*)[\\u4e00-\\u9fa5][^\"]*(\")"),
           new Regex( "('\\s*)[\\u4e00-\\u9fa5][^']*(')"),
           new Regex( "(>\\s*)[\\u4e00-\\u9fa5][^<]*(<)")
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
                context.ViewTexts = await CollectTranslateAsync(stream);

                stream.Seek(0, SeekOrigin.Begin);
                context.CoderTexts = await CollectCoderTextAsync(stream);
            }

            return context;
        }

        /// <summary>
        ///     前端翻译语句,理论上的api写法是this.$t("")或this.$t('')
        ///     实际考虑的是准备阶段的表达匹配,不太追求一致,尽可能取即可
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected async Task<List<ViewText>> CollectTranslateAsync(Stream stream)
        {
            var texts = new List<ViewText>();

            var buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            var context = Encoding.UTF8.GetString(buffer);
            foreach (var regex in regexes)
            {
                var match = regex.Match(context);
                while (match != Match.Empty)
                {
                    var text = match.Value;

                    texts.Add(new ViewText()
                    {
                        OriginalText = text,
                        Text = text.Trim('\"').Trim('\'').Trim('<').Trim('>').Trim()
                    });

                    match = match.NextMatch();
                }
            }

            return texts;
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

                if (!anyChineseRegex.IsMatch(line))
                    continue;

                var text = line.Trim();

                //html多行注释 <!-- --> 
                if (text.StartsWith("<!--") || text.EndsWith("-->"))
                    continue;

                //js多行注释 /* */
                if (text.StartsWith("/*") || text.EndsWith("*/"))
                    continue;

                //js单行注释 整体替换
                if (annotationRegex.IsMatch(text))
                {
                    text = annotationRegex.Replace(text, "");
                }

                if (!anyChineseRegex.IsMatch(text))
                    continue;

                texts.Add(new CoderText()
                {
                    OriginalText = line,
                    Line = lineNo
                });

            }

            return texts;
        }
    }
}