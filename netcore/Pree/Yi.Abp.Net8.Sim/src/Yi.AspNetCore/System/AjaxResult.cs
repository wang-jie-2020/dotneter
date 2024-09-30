namespace Yi.AspNetCore.System;

public class AjaxResult
{
    /// <summary>
    /// 状态码
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// 类型success、warning、error
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
    /// 附加数据
    /// </summary>
    public object? Extras { get; set; }

    /// <summary>
    /// 时间
    /// </summary>
    public DateTime Time { get; set; }

    public static AjaxResult Success()
    {
        return new AjaxResult()
        {
            Code = "0",
            Type = "success",
            Message = "",
            Details = "",
            Extras = null,
            Time = DateTime.Now
        };
    }

    public static AjaxResult Error(string message, string details = "")
    {
        return new AjaxResult()
        {
            Code = "1",
            Type = "error",
            Message = message,
            Details = details,
            Extras = null,
            Time = DateTime.Now
        };
    }

    public static AjaxResult Error(string code, string message, string details = "")
    {
        return new AjaxResult()
        {
            Code = code,
            Type = "error",
            Message = message,
            Details = details,
            Extras = null,
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
            Code = "0",
            Type = "success",
            Message = "",
            Details = "",
            Extras = null,
            Time = DateTime.Now,
            Data = result
        };
    }
}