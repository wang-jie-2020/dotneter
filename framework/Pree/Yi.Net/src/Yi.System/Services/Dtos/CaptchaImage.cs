namespace Yi.System.Services.Dtos;

public class CaptchaImage
{
    public Guid Uuid { get; set; } = Guid.Empty;
    
    public byte[] Img { get; set; }
}