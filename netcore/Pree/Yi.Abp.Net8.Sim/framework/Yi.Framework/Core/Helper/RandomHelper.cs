using System.Text;
using System.Text.RegularExpressions;

namespace Yi.Framework.Core.Helper;

public class RandomHelper
{
    public static string replaceBianLiang(string content)
    {
        content = content.Replace("{当前时间}", DateTime.Now.TimeOfDay.ToString());
        string[] bianliang = { "{随机字母}", "{随机数字}", "{随机汉字}" };
        Regex r;
        int count;
        var readstr = "";
        foreach (var str in bianliang)
        {
            count = (content.Length - content.Replace(str, "").Length) / str.Length;
            if (str == "{随机汉字}") readstr = RandChina(count);
            if (str == "{随机数字}") readstr = GenerateCheckCodeNum(count);
            if (str == "{随机字母}") readstr = GenerateRandomLetter(count);
            if (count > readstr.Length) count = readstr.Length;
            r = new Regex(str.Replace("{", "\\{").Replace("}", "\\}"));
            for (var i = 0; i < count; i++) content = r.Replace(content, readstr.Substring(i, 1), 1);
        }

        return content;
    }


    /// <summary>
    ///     随机生成字母
    /// </summary>
    /// <param name="Length"></param>
    /// <returns></returns>
    public static string GenerateRandomLetter(int Length)
    {
        char[] Pattern =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p',
            'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
        };
        var result = "";
        var n = Pattern.Length;
        var random = new Random(~unchecked((int)DateTime.Now.Ticks));
        for (var i = 0; i < Length; i++)
        {
            var rnd = random.Next(0, n);
            result += Pattern[rnd];
        }

        return result;
    }

    /// <summary>
    ///     随机生成数字
    /// </summary>
    /// <param name="codeCount"></param>
    /// <returns></returns>
    public static string GenerateCheckCodeNum(int codeCount)
    {
        var rep = 0;
        var str = string.Empty;
        var num2 = DateTime.Now.Ticks + rep;
        rep++;
        var random = new Random((int)((ulong)num2 & 0xffffffffL) | (int)(num2 >> rep));
        for (var i = 0; i < codeCount; i++)
        {
            var num = random.Next();
            str = str + (char)(0x30 + (ushort)(num % 10));
        }

        return str;
    }

    /// <summary>
    ///     此函数为生成指定数目的汉字
    /// </summary>
    /// <param name="charLen">汉字数目</param>
    /// <returns>所有汉字</returns>
    public static string RandChina(int charLen)
    {
        int area, code; //汉字由区位和码位组成(都为0-94,其中区位16-55为一级汉字区,56-87为二级汉字区,1-9为特殊字符区)
        var strtem = new StringBuilder();
        var rand = new Random();
        for (var i = 0; i < charLen; i++)
        {
            area = rand.Next(16, 88);
            if (area == 55) //第55区只有89个字符
                code = rand.Next(1, 90);
            else
                code = rand.Next(1, 94);
            strtem.Append(Encoding.GetEncoding("GB2312")
                .GetString(new[] { Convert.ToByte(area + 160), Convert.ToByte(code + 160) }));
        }

        return strtem.ToString();
    }
}