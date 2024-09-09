using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Yi.Framework.Core.Helper;

public class JsonHelper
{
    public static string ObjToStr<T>(T obj, string dateTimeFormat)
    {
        var timeConverter = new IsoDateTimeConverter
        {
            DateTimeFormat = dateTimeFormat
        };
        return JsonConvert.SerializeObject(obj, Formatting.Indented, timeConverter);
    }

    public static string ObjToStr<T>(T obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public static T StrToObj<T>(string str)
    {
        return JsonConvert.DeserializeObject<T>(str)!;
    }

    /// <summary>
    ///     转换对象为JSON格式数据
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    /// <param name="obj">对象</param>
    /// <returns>字符格式的JSON数据</returns>
    public static string GetJSON<T>(object obj)
    {
        var result = string.Empty;
        var serializer =
            new DataContractJsonSerializer(typeof(T));
        using (var ms = new MemoryStream())
        {
            serializer.WriteObject(ms, obj);
            result = Encoding.UTF8.GetString(ms.ToArray());
        }

        return result;
    }

    /// <summary>
    ///     转换List<T>的数据为JSON格式
    /// </summary>
    /// <typeparam name="T">类</typeparam>
    /// <param name="vals">列表值</param>
    /// <returns>JSON格式数据</returns>
    public string JSON<T>(List<T> vals)
    {
        var st = new StringBuilder();
        try
        {
            var s = new DataContractJsonSerializer(typeof(T));

            foreach (var city in vals)
                using (var ms = new MemoryStream())
                {
                    s.WriteObject(ms, city);
                    st.Append(Encoding.UTF8.GetString(ms.ToArray()));
                }
        }
        catch (Exception)
        {
        }

        return st.ToString();
    }

    /// <summary>
    ///     JSON格式字符转换为T类型的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonStr"></param>
    /// <returns></returns>
    public static T ParseFormByJson<T>(string jsonStr)
    {
        var obj = Activator.CreateInstance<T>();
        using (var ms =
               new MemoryStream(Encoding.UTF8.GetBytes(jsonStr)))
        {
            var serializer =
                new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(ms)!;
        }
    }

    public string JSON1<SendData>(List<SendData> vals)
    {
        var st = new StringBuilder();
        try
        {
            var s = new DataContractJsonSerializer(typeof(SendData));

            foreach (var city in vals)
                using (var ms = new MemoryStream())
                {
                    s.WriteObject(ms, city);
                    st.Append(Encoding.UTF8.GetString(ms.ToArray()));
                }
        }
        catch (Exception)
        {
        }

        return st.ToString();
    }

    private static bool IsJsonStart(ref string json)
    {
        if (!string.IsNullOrEmpty(json))
        {
            json = json.Trim('\r', '\n', ' ');
            if (json.Length > 1)
            {
                var s = json[0];
                var e = json[json.Length - 1];
                return (s == '{' && e == '}') || (s == '[' && e == ']');
            }
        }

        return false;
    }

    public static bool IsJson(string json)
    {
        int errIndex;
        return IsJson(json, out errIndex);
    }

    public static bool IsJson(string json, out int errIndex)
    {
        errIndex = 0;
        if (IsJsonStart(ref json))
        {
            var cs = new CharState();
            char c;
            for (var i = 0; i < json.Length; i++)
            {
                c = json[i];
                if (SetCharState(c, ref cs) && cs.childrenStart) //设置关键符号状态。
                {
                    var item = json.Substring(i);
                    int err;
                    var length = GetValueLength(item, true, out err);
                    cs.childrenStart = false;
                    if (err > 0)
                    {
                        errIndex = i + err;
                        return false;
                    }

                    i = i + length - 1;
                }

                if (cs.isError)
                {
                    errIndex = i;
                    return false;
                }
            }

            return !cs.arrayStart && !cs.jsonStart;
        }

        return false;
    }

    /// <summary>
    ///     获取值的长度（当Json值嵌套以"{"或"["开头时）
    /// </summary>
    private static int GetValueLength(string json, bool breakOnErr, out int errIndex)
    {
        errIndex = 0;
        var len = 0;
        if (!string.IsNullOrEmpty(json))
        {
            var cs = new CharState();
            char c;
            for (var i = 0; i < json.Length; i++)
            {
                c = json[i];
                if (!SetCharState(c, ref cs)) //设置关键符号状态。
                {
                    if (!cs.jsonStart && !cs.arrayStart) //json结束，又不是数组，则退出。
                        break;
                }
                else if (cs.childrenStart) //正常字符，值状态下。
                {
                    var length = GetValueLength(json.Substring(i), breakOnErr, out errIndex); //递归子值，返回一个长度。。。
                    cs.childrenStart = false;
                    cs.valueStart = 0;
                    //cs.state = 0;
                    i = i + length - 1;
                }

                if (breakOnErr && cs.isError)
                {
                    errIndex = i;
                    return i;
                }

                if (!cs.jsonStart && !cs.arrayStart) //记录当前结束位置。
                {
                    len = i + 1; //长度比索引+1
                    break;
                }
            }
        }

        return len;
    }

    /// <summary>
    ///     设置字符状态(返回true则为关键词，返回false则当为普通字符处理）
    /// </summary>
    private static bool SetCharState(char c, ref CharState cs)
    {
        cs.CheckIsError(c);
        switch (c)
        {
            case '{': //[{ "[{A}]":[{"[{B}]":3,"m":"C"}]}]

                #region 大括号

                if (cs.keyStart <= 0 && cs.valueStart <= 0)
                {
                    cs.keyStart = 0;
                    cs.valueStart = 0;
                    if (cs.jsonStart && cs.state == 1)
                        cs.childrenStart = true;
                    else
                        cs.state = 0;
                    cs.jsonStart = true; //开始。
                    return true;
                }

                #endregion

                break;
            case '}':

                #region 大括号结束

                if (cs.keyStart <= 0 && cs.valueStart < 2 && cs.jsonStart)
                {
                    cs.jsonStart = false; //正常结束。
                    cs.state = 0;
                    cs.keyStart = 0;
                    cs.valueStart = 0;
                    cs.setDicValue = true;
                    return true;
                }

                // cs.isError = !cs.jsonStart && cs.state == 0;

                #endregion

                break;
            case '[':

                #region 中括号开始

                if (!cs.jsonStart)
                {
                    cs.arrayStart = true;
                    return true;
                }

                if (cs.jsonStart && cs.state == 1)
                {
                    cs.childrenStart = true;
                    return true;
                }

                #endregion

                break;
            case ']':

                #region 中括号结束

                if (cs.arrayStart && !cs.jsonStart && cs.keyStart <= 2 && cs.valueStart <= 0) //[{},333]//这样结束。
                {
                    cs.keyStart = 0;
                    cs.valueStart = 0;
                    cs.arrayStart = false;
                    return true;
                }

                #endregion

                break;
            case '"':
            case '\'':

                #region 引号

                if (cs.jsonStart || cs.arrayStart)
                {
                    if (cs.state == 0) //key阶段,有可能是数组["aa",{}]
                    {
                        if (cs.keyStart <= 0)
                        {
                            cs.keyStart = c == '"' ? 3 : 2;
                            return true;
                        }

                        if ((cs.keyStart == 2 && c == '\'') || (cs.keyStart == 3 && c == '"'))
                        {
                            if (!cs.escapeChar)
                            {
                                cs.keyStart = -1;
                                return true;
                            }

                            cs.escapeChar = false;
                        }
                    }
                    else if (cs.state == 1 && cs.jsonStart) //值阶段必须是Json开始了。
                    {
                        if (cs.valueStart <= 0)
                        {
                            cs.valueStart = c == '"' ? 3 : 2;
                            return true;
                        }

                        if ((cs.valueStart == 2 && c == '\'') || (cs.valueStart == 3 && c == '"'))
                        {
                            if (!cs.escapeChar)
                            {
                                cs.valueStart = -1;
                                return true;
                            }

                            cs.escapeChar = false;
                        }
                    }
                }

                #endregion

                break;
            case ':':

                #region 冒号

                if (cs.jsonStart && cs.keyStart < 2 && cs.valueStart < 2 && cs.state == 0)
                {
                    if (cs.keyStart == 1) cs.keyStart = -1;
                    cs.state = 1;
                    return true;
                }

                // cs.isError = !cs.jsonStart || (cs.keyStart < 2 && cs.valueStart < 2 && cs.state == 1);

                #endregion

                break;
            case ',':

                #region 逗号 //["aa",{aa:12,}]

                if (cs.jsonStart)
                {
                    if (cs.keyStart < 2 && cs.valueStart < 2 && cs.state == 1)
                    {
                        cs.state = 0;
                        cs.keyStart = 0;
                        cs.valueStart = 0;
                        //if (cs.valueStart == 1)
                        //{
                        //    cs.valueStart = 0;
                        //}
                        cs.setDicValue = true;
                        return true;
                    }
                }
                else if (cs.arrayStart && cs.keyStart <= 2)
                {
                    cs.keyStart = 0;
                    //if (cs.keyStart == 1)
                    //{
                    //    cs.keyStart = -1;
                    //}
                    return true;
                }

                #endregion

                break;
            case ' ':
            case '\r':
            case '\n': //[ "a",\r\n{} ]
            case '\0':
            case '\t':
                if (cs.keyStart <= 0 && cs.valueStart <= 0) //cs.jsonStart && 
                    return true; //跳过空格。
                break;
            default: //值开头。。
                if (c == '\\') //转义符号
                {
                    if (cs.escapeChar)
                    {
                        cs.escapeChar = false;
                    }
                    else
                    {
                        cs.escapeChar = true;
                        return true;
                    }
                }
                else
                {
                    cs.escapeChar = false;
                }

                if (cs.jsonStart || cs.arrayStart) // Json 或数组开始了。
                {
                    if (cs.keyStart <= 0 && cs.state == 0)
                        cs.keyStart = 1; //无引号的
                    else if (cs.valueStart <= 0 && cs.state == 1 && cs.jsonStart) //只有Json开始才有值。
                        cs.valueStart = 1; //无引号的
                }

                break;
        }

        return false;
    }
}

/// <summary>
///     字符状态
/// </summary>
public class CharState
{
    /// <summary>
    ///     数组开始【仅第一开头才算】，值嵌套的以【childrenStart】来标识。
    /// </summary>
    internal bool arrayStart; //以"[" 符号开始了

    internal bool childrenStart; //子级嵌套开始了。
    internal bool escapeChar; //以"\"转义符号开始了
    internal bool isError; //是否语法错误。
    internal bool jsonStart; //以 "{"开始了...

    /// <summary>
    ///     【-1 取值结束】【0 未开始】【1 无引号开始】【2 单引号开始】【3 双引号开始】
    /// </summary>
    internal int keyStart;

    internal bool setDicValue; // 可以设置字典值了。

    /// <summary>
    ///     【0 初始状态，或 遇到“,”逗号】；【1 遇到“：”冒号】
    /// </summary>
    internal int state;

    /// <summary>
    ///     【-1 取值结束】【0 未开始】【1 无引号开始】【2 单引号开始】【3 双引号开始】
    /// </summary>
    internal int valueStart;

    internal void CheckIsError(char c) //只当成一级处理（因为GetLength会递归到每一个子项处理）
    {
        if (keyStart > 1 || valueStart > 1) return;
        //示例 ["aa",{"bbbb":123,"fff","Ddd"}] 
        switch (c)
        {
            case '{': //[{ "[{A}]":[{"[{B}]":3,"m":"C"}]}]
                isError = jsonStart && state == 0; //重复开始错误 同时不是值处理。
                break;
            case '}':
                isError = !jsonStart || (keyStart != 0 && state == 0); //重复结束错误 或者 提前结束{"aa"}。正常的有{}
                break;
            case '[':
                isError = arrayStart && state == 0; //重复开始错误
                break;
            case ']':
                isError = !arrayStart || jsonStart; //重复开始错误 或者 Json 未结束
                break;
            case '"':
            case '\'':
                isError = !(jsonStart || arrayStart); //json 或数组开始。
                if (!isError)
                    //重复开始 [""",{"" "}]
                    isError = (state == 0 && keyStart == -1) || (state == 1 && valueStart == -1);
                if (!isError && arrayStart && !jsonStart && c == '\'') //['aa',{}]
                    isError = true;
                break;
            case ':':
                isError = !jsonStart || state == 1; //重复出现。
                break;
            case ',':
                isError = !(jsonStart || arrayStart); //json 或数组开始。
                if (!isError)
                {
                    if (jsonStart)
                        isError = state == 0 || (state == 1 && valueStart > 1); //重复出现。
                    else if (arrayStart) //["aa,] [,]  [{},{}]
                        isError = keyStart == 0 && !setDicValue;
                }

                break;
            case ' ':
            case '\r':
            case '\n': //[ "a",\r\n{} ]
            case '\0':
            case '\t':
                break;
            default: //值开头。。
                isError = (!jsonStart && !arrayStart) || (state == 0 && keyStart == -1) ||
                          (valueStart == -1 && state == 1); //
                break;
        }
        //if (isError)
        //{

        //}
    }
}