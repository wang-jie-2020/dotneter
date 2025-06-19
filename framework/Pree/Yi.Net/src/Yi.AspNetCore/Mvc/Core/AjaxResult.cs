namespace Yi.AspNetCore.Mvc.Core;

public class AjaxResult
{
    /// <summary>
    /// 错误类型码 
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string Details { get; set; }
    
    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public object? Data { get; set; }

    public static AjaxResult Success(string message = "")
    {
        return new AjaxResult()
        {
            Code = "",
            Message = message,
            Details = "",
            Time = DateTime.Now
        };
    }

    public static AjaxResult Success(object data, string message = "")
    {
        return new AjaxResult()
        {
            Code = "",
            Message = message,
            Details = "",
            Time = DateTime.Now,
            Data = data
        };
    }
}

public class AjaxResult<T> : AjaxResult
{
    /// <summary>
    /// 数据
    /// </summary>
    public new T Data { get; set; }

    public static AjaxResult<T> Success(T result)
    {
        return new AjaxResult<T>()
        {
            Code = "",
            Message = "",
            Details = "",
            Time = DateTime.Now,
            Data = result
        };
    }
}