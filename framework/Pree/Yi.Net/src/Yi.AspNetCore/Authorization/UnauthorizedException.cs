namespace Yi.AspNetCore.Authorization;

public class UnauthorizedException : Exception
{
    public UnauthorizedException()
    {

    }
    
    public UnauthorizedException(string message)
        : base(message)
    {

    }
    
    public UnauthorizedException(string message, Exception innerException)
        : base(message, innerException)
    {

    }
    
    public UnauthorizedException(string? message = null, string? code = null, Exception? innerException = null)
        : base(message, innerException)
    {

    }

    public UnauthorizedException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
