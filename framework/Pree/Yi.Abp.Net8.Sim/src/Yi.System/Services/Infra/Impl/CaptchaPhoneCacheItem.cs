namespace Yi.Sys.Services.Infra.Impl;

public class CaptchaPhoneCacheItem
{
    public CaptchaPhoneCacheItem(string code)
    {
        Code = code;
    }

    public string Code { get; set; }
}

public class CaptchaPhoneCacheKey
{
    public CaptchaPhoneCacheKey(string phone)
    {
        Phone = phone;
    }

    public string Phone { get; set; }

    public override string ToString()
    {
        return $"Phone:{Phone}";
    }
}