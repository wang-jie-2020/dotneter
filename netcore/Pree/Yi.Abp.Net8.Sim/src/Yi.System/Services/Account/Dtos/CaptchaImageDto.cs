namespace Yi.System.Services.Account.Dtos;

public class CaptchaImageDto
{
    public Guid Uuid { get; set; } = Guid.Empty;
    
    public byte[] Img { get; set; }
}