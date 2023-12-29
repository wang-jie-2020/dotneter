using System.Collections.Generic;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace Hugin.IdentityServer.EntityExtensions
{
    public static class IdentityUserExtensions
    {
        public static string GetUserOpenId(this IdentityUser user)
        {
            return user.GetProperty<string>("OpenId");
        }

        public static void SetUserOpenId(this IdentityUser user, string openId)
        {
            user.SetProperty("OpenId", openId);
        }

        public static string GetUserStadiumId(this IdentityUser user)
        {
            return user.GetProperty<string>("StadiumId");
        }

        public static void SetUserStadiumId(this IdentityUser user, params string[] stadiumIds)
        {
            user.SetProperty("StadiumId", stadiumIds.JoinAsString(";"));
        }
    }
}
