namespace Yi.Abp.Infra.Rbac.Dtos.Account
{
    public class CaptchaImageDto
    {
        public Guid Uuid { get; set; } = Guid.Empty;
        public byte[] Img { get; set; }
    }
}
