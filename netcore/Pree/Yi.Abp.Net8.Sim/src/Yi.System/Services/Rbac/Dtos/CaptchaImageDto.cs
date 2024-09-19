namespace Yi.System.Services.Rbac.Dtos;

public class CaptchaImageDto
{
    public Guid Uuid { get; set; } = Guid.Empty;
    
    public byte[] Img { get; set; }
}