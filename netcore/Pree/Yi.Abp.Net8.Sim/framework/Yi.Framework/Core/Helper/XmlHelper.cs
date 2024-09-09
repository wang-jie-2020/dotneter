﻿using System.Xml.Serialization;

namespace Yi.Framework.Core.Helper;

public class XmlHelper
{
    /// <summary>
    ///     转换对象为JSON格式数据
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    /// <param name="obj">对象</param>
    /// <returns>字符格式的JSON数据</returns>
    public static string? GetXML<T>(object obj)
    {
        try
        {
            var xs = new XmlSerializer(typeof(T));

            using (TextWriter tw = new StringWriter())
            {
                xs.Serialize(tw, obj);
                return tw.ToString();
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    /// <summary>
    ///     Xml格式字符转换为T类型的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="xml"></param>
    /// <returns></returns>
    public static T ParseFormByXml<T>(string xml, string rootName = "root")
    {
        var serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(rootName));
        var reader = new StringReader(xml);

        var res = (T)serializer.Deserialize(reader)!;
        reader.Close();
        reader.Dispose();
        return res;
    }
}