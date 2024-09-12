namespace Yi.Infra.Account.Options;

public class AliyunOptions
{
    public string AccessKeyId { get; set; }
    public string AccessKeySecret { get; set; }
    public AliyunSms Sms { get; set; }
}

public class AliyunSms
{
    public string SignName { get; set; }
    public string TemplateCode { get; set; }
}