namespace Yi.AspNetCore.Mvc;

public class AjaxResult
{
    public string? Code { get; set; }
    
    public string Message { get; set; }
    
    public string Details { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;
    
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