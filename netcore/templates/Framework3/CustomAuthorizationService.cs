using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework3
{
    public class CustomAuthorizationService : DefaultAuthorizationService
    {
        public CustomAuthorizationService(IAuthorizationPolicyProvider policyProvider, IAuthorizationHandlerProvider handlers, ILogger<DefaultAuthorizationService> logger, IAuthorizationHandlerContextFactory contextFactory, IAuthorizationEvaluator evaluator, IOptions<AuthorizationOptions> options) : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
        {
        }

        private readonly AuthorizationOptions _options;
        private readonly IAuthorizationHandlerContextFactory _contextFactory;
        private readonly IAuthorizationHandlerProvider _handlers;
        private readonly IAuthorizationEvaluator _evaluator;
        private readonly IAuthorizationPolicyProvider _policyProvider;
        private readonly ILogger _logger;

        // /// <summary>
        // /// Creates a new instance of <see cref="T:Microsoft.AspNetCore.Authorization.DefaultAuthorizationService" />.
        // /// </summary>
        // /// <param name="policyProvider">The <see cref="T:Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider" /> used to provide policies.</param>
        // /// <param name="handlers">The handlers used to fulfill <see cref="T:Microsoft.AspNetCore.Authorization.IAuthorizationRequirement" />s.</param>
        // /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        // /// <param name="contextFactory">The <see cref="T:Microsoft.AspNetCore.Authorization.IAuthorizationHandlerContextFactory" /> used to create the context to handle the authorization.</param>
        // /// <param name="evaluator">The <see cref="T:Microsoft.AspNetCore.Authorization.IAuthorizationEvaluator" /> used to determine if authorization was successful.</param>
        // /// <param name="options">The <see cref="T:Microsoft.AspNetCore.Authorization.AuthorizationOptions" /> used.</param>
        // public DefaultAuthorizationService(
        //     IAuthorizationPolicyProvider policyProvider,
        //     IAuthorizationHandlerProvider handlers,
        //     ILogger<DefaultAuthorizationService> logger,
        //     IAuthorizationHandlerContextFactory contextFactory,
        //     IAuthorizationEvaluator evaluator,
        //     IOptions<AuthorizationOptions> options)
        // {
        //     if (options == null)
        //         throw new ArgumentNullException(nameof(options));
        //     if (policyProvider == null)
        //         throw new ArgumentNullException(nameof(policyProvider));
        //     if (handlers == null)
        //         throw new ArgumentNullException(nameof(handlers));
        //     if (logger == null)
        //         throw new ArgumentNullException(nameof(logger));
        //     if (contextFactory == null)
        //         throw new ArgumentNullException(nameof(contextFactory));
        //     if (evaluator == null)
        //         throw new ArgumentNullException(nameof(evaluator));
        //     this._options = options.Value;
        //     this._handlers = handlers;
        //     this._policyProvider = policyProvider;
        //     this._logger = (ILogger)logger;
        //     this._evaluator = evaluator;
        //     this._contextFactory = contextFactory;
        // }

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
        public async Task<AuthorizationResult> AuthorizeAsync(
            ClaimsPrincipal user,
            object resource,
            IEnumerable<IAuthorizationRequirement> requirements)
        {
            if (requirements == null)
                throw new ArgumentNullException(nameof(requirements));
            AuthorizationHandlerContext authContext = this._contextFactory.CreateContext(requirements, user, resource);
            foreach (IAuthorizationHandler authorizationHandler in await this._handlers.GetHandlersAsync(authContext))
            {
                await authorizationHandler.HandleAsync(authContext);
                if (!this._options.InvokeHandlersAfterFailure)
                {
                    if (authContext.HasFailed)
                        break;
                }
            }

            AuthorizationResult authorizationResult = this._evaluator.Evaluate(authContext);
            // if (authorizationResult.Succeeded)
            //     this._logger.UserAuthorizationSucceeded();
            // else
            //     this._logger.UserAuthorizationFailed();
            return authorizationResult;
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
        public async Task<AuthorizationResult> AuthorizeAsync(
            ClaimsPrincipal user,
            object resource,
            string policyName)
        {
            var service = this;
            if (policyName == null)
                throw new ArgumentNullException(nameof(policyName));
            AuthorizationPolicy policyAsync = await service._policyProvider.GetPolicyAsync(policyName);
            if (policyAsync == null)
                throw new InvalidOperationException("No policy found: " + policyName + ".");
            return await service.AuthorizeAsync(user, resource, policyAsync);
        }
    }
}