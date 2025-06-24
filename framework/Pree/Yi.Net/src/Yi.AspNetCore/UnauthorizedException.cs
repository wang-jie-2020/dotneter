namespace Yi.AspNetCore;

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

    public UnauthorizedException WithData(string name, object value)
    {
        Data[name] = value;
        return this;
    }
}
