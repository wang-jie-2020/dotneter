using System.Security.Claims;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;

namespace NanoService.IdentityServer.Extend
{
    /*
     *  IProfileService 是为IClaimService中的claims集合赋值的
     *  在Default上简单实现一下，ProfileService<TUser>是基于Identity实现
     */
    public class ExtendProfileService : DefaultProfileService, IProfileService
    {
        public ExtendProfileService(ILogger<DefaultProfileService> logger) : base(logger)
        {
        }

        public override Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //from base
            context.LogProfileRequest(this.Logger);
            context.AddRequestedClaims(context.Subject.Claims);
            context.LogIssuedClaims(this.Logger);
            return Task.CompletedTask;

            ////copy&simplify from AspNetCore.Identity
            //ClaimsPrincipal subject = context.Subject;
            //string? subjectId = subject?.GetSubjectId();
            //if (subjectId == null)
            //    throw new Exception("No sub claim present");

            //TestUser userAsync = Config.Users().FirstOrDefault(p => p.SubjectId == subjectId);
            //if ((object)userAsync == null)
            //    return Task.CompletedTask;

            ////real: await this.ClaimsFactory.CreateAsync(user) ?? throw new Exception("ClaimsFactory failed to create a principal");
            //var claims = userAsync.Claims;

            ////same name claim will be filled
            //context.AddRequestedClaims(claims);
        }
    }
}
