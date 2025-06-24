namespace Yi.AspNetCore.Mvc;

public class AjaxResult
{
    public string Code { get; set; }

    public string Message { get; set; }

    public string Details { get; set; }

    public object? Data { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;

    protected AjaxResult()
    {
    }

    public static AjaxResult Success()
    {
        return new AjaxResult()
        {
            Code = "0",
            Message = "",
            Details = ""
        };
    }

    public static AjaxResult Success(object data)
    {
        var ajaxResult = Success();
        ajaxResult.Data = data;

        return ajaxResult;
    }

    public static AjaxResult Error()
    {
        return new AjaxResult()
        {
            Code = "1",
            Message = "",
            Details = ""
        };
    }

    public static AjaxResult Error(string message, string details = "")
    {
        var ajaxResult = Success();
        ajaxResult.Message = message;
        ajaxResult.Details = details;

        return ajaxResult;
    }
}

public class AjaxResult<T> : AjaxResult
{
    public new T Data { get; set; }

    public new static AjaxResult<T> Success()
    {
        return new AjaxResult<T>()
        {
            Code = "0",
            Message = "",
            Details = ""
        };
    }

    public static AjaxResult<T> Success(T data)
    {
        var ajaxResult = Success();
        ajaxResult.Data = data;

        return ajaxResult;
    }
}