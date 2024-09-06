using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Infra.Rbac.Dtos.LoginLog
{
    public class LoginLogGetListInputVo : PagedAllResultRequestDto
    {
        public string? LoginUser { get; set; }

        public string? LoginIp { get; set; }
    }
}
