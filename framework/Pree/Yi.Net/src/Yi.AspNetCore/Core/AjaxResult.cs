namespace Yi.AspNetCore.Core;

public class AjaxResult
{
    /// <summary>
    /// 状态码 0=成功 1=失败
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 错误类型码 
    /// </summary>
    public string Type { get; set; }

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
            Code = 0,
            Type = "",
            Message = message,
            Details = "",
            Time = DateTime.Now
        };
    }

    public static AjaxResult Success(object data, string message = "")
    {
        return new AjaxResult()
        {
            Code = 0,
            Type = "",
            Message = message,
            Details = "",
            Time = DateTime.Now,
            Data = data
        };
    }

    public static AjaxResult Error(string message, string details = "")
    {
        return new AjaxResult()
        {
            Code = 1,
            Type = "",
            Message = message,
            Details = details,
            Time = DateTime.Now
        };
    }

    public static AjaxResult Error(string type, string message, string details = "")
    {
        return new AjaxResult()
        {
            Code = 1,
            Type = type,
            Message = message,
            Details = details,
            Time = DateTime.Now
        };
    }
}

public class AjaxResult<T> : AjaxResult
{
    /// <summary>
    /// 数据
    /// </summary>
    public T Data { get; set; }

    public static AjaxResult<T> Success(T result)
    {
        return new AjaxResult<T>()
        {
            Code = 0,
            Type = "",
            Message = "",
            Details = "",
            Time = DateTime.Now,
            Data = result
        };
    }
}