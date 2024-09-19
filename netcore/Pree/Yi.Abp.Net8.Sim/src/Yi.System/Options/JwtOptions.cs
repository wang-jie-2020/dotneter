namespace Yi.System.Options;

public class JwtOptions
{
    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string SecurityKey { get; set; }

    public long ExpiresMinuteTime { get; set; }
}