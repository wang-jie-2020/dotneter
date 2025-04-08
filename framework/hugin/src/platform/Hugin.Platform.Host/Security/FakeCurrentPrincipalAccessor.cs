using System.Collections.Generic;
using System.Security.Claims;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Security.Claims;

namespace Hugin.Platform.Security
{
    [ExposeServices(IncludeSelf = true, IncludeDefaults = false)]
    public class FakeCurrentPrincipalAccessor : ThreadCurrentPrincipalAccessor
    {
        protected override ClaimsPrincipal GetClaimsPrincipal()
        {
            return GetPrincipal();
        }

        private ClaimsPrincipal _principal;

        private ClaimsPrincipal GetPrincipal()
        {
            if (_principal == null)
            {
                lock (this)
                {
                    if (_principal == null)
                    {
                        _principal = new ClaimsPrincipal(
                            new ClaimsIdentity(
                                new List<Claim>
                                {
                                    new Claim(AbpClaimTypes.UserId,"2e701e62-0953-4dd3-910b-dc6cc93ccb0d"),
                                    new Claim(AbpClaimTypes.UserName,"admin"),
                                    new Claim(AbpClaimTypes.Email,"admin@abp.io"),
                                    new Claim("stadiums","0a301ffa-a83d-4c5e-9521-64eabaa5c329;a99863a7-3f2c-491b-b4d8-b549cd2fd0f5")
                                }
                            )
                        );
                    }
                }
            }

            return _principal;
        }
    }
}
