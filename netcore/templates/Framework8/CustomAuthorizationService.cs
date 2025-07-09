using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Framework8
{
    public class CustomAuthorizationService : DefaultAuthorizationService
    {
        private readonly AuthorizationOptions _options;
        private readonly IAuthorizationHandlerContextFactory _contextFactory;
        private readonly IAuthorizationHandlerProvider _handlers;
        private readonly IAuthorizationEvaluator _evaluator;
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly ILogger _logger;


        public CustomAuthorizationService(IAuthorizationPolicyProvider policyProvider, IAuthorizationHandlerProvider handlers, ILogger<DefaultAuthorizationService> logger, IAuthorizationHandlerContextFactory contextFactory, IAuthorizationEvaluator evaluator, IOptions<AuthorizationOptions> options, AuthorizationOptions options2, ILogger logger2) : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
        {
            _options = options2;
            _contextFactory = contextFactory;
            _handlers = handlers;
            _evaluator = evaluator;
            _policyProvider = policyProvider;
            _logger = logger2;
        }

        /// <summary>
        /// Checks if a user meets a specific set of requirements for the specified resource.
        /// </summary>
        /// <param name="user">The user to evaluate the requirements against.</param>
        /// <param name="resource">The resource to evaluate the requirements against.</param>
        /// <param name="requirements">The requirements to evaluate.</param>
        /// <returns>
        /// A flag indicating whether authorization has succeeded.
        /// This value is <value>true</value> when the user fulfills the policy otherwise <value>false</value>.
        /// </returns>
        public virtual async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            //ArgumentNullThrowHelper.ThrowIfNull(requirements);

            var authContext = _contextFactory.CreateContext(requirements, user, resource);
            var handlers = await _handlers.GetHandlersAsync(authContext).ConfigureAwait(false);
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(authContext).ConfigureAwait(false);
                if (!_options.InvokeHandlersAfterFailure && authContext.HasFailed)
                {
                    break;
                }
            }

            var result = _evaluator.Evaluate(authContext);
            if (result.Succeeded)
            {
                //_logger.UserAuthorizationSucceeded();
            }
            else
            {
                //_logger.UserAuthorizationFailed(result.Failure);
            }

            return result;
        }

        /// <summary>
        /// Checks if a user meets a specific authorization policy.
        /// </summary>
        /// <param name="user">The user to check the policy against.</param>
        /// <param name="resource">The resource the policy should be checked with.</param>
        /// <param name="policyName">The name of the policy to check against a specific context.</param>
        /// <returns>
        /// A flag indicating whether authorization has succeeded.
        /// This value is <value>true</value> when the user fulfills the policy otherwise <value>false</value>.
        /// </returns>
        public virtual async Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            //ArgumentNullThrowHelper.ThrowIfNull(policyName);

            var policy = await _policyProvider.GetPolicyAsync(policyName).ConfigureAwait(false);
            if (policy == null)
            {
                throw new InvalidOperationException($"No policy found: {policyName}.");
            }

            return await this.AuthorizeAsync(user, resource, policy).ConfigureAwait(false);
        }
    }
}