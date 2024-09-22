namespace Yi.System.Services.System.Dtos;

public class CaptchaImageDto
{
    public Guid Uuid { get; set; } = Guid.Empty;
    
    public byte[] Img { get; set; }
}