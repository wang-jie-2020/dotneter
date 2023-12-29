using Microsoft.AspNetCore.Authorization;

namespace Apis
{
    public class ScopeRequirement : IAuthorizationRequirement
    {
        public string ScopeName { get; }

        public ScopeRequirement(string scopeName)
        {
            ScopeName = scopeName;
        }
    }
}
