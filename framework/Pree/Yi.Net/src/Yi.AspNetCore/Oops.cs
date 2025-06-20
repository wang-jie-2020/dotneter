namespace Yi.AspNetCore;

public static class Oops
{
    public static BusinessException Oh(
        string? code = null,
        string? message = null,
        string? details = null,
        Exception? innerException = null)
    {
        return new BusinessException(code, message, details, innerException);
    }

    public static UserFriendlyException Friendly(
        string message,
        string? code = null,
        string? details = null,
        Exception? innerException = null)
    {
        return new UserFriendlyException(message, code, details, innerException);
    }
}