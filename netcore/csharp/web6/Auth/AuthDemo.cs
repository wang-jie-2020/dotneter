// using Microsoft.AspNetCore.Authorization;
//
// namespace web.Auth;
//
// public class AuthDemo
// {
//     
// }
//
//
// public class EmailRequirement : IAuthorizationRequirement
// {
//     public string RequiredEmail { get; set; }
//
//     public EmailRequirement(string requiredEmail)
//     {
//         RequiredEmail = requiredEmail;
//     }
// }
//
// public class EmailHandler : AuthorizationHandler<EmailRequirement>
// {
//     protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmailRequirement requirement)
//     {
//         var claim = context.User.Claims.FirstOrDefault(x => x.Type == "CanEditEmail");
//         if (claim != null)
//         {
//             if (claim.Value.EndsWith(requirement.RequiredEmail))
//             {
//                 context.Succeed(requirement);
//             }
//         }
//
//         return Task.CompletedTask;
//     }
// }