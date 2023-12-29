using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.MultiStadium
{
    public class ActionStadiumResolveContributor : StadiumResolveContributorBase
    {
        public const string ContributorName = "Action";

        public override string Name => ContributorName;

        private readonly Action<IStadiumResolveContext> _resolveAction;

        public ActionStadiumResolveContributor([NotNull] Action<IStadiumResolveContext> resolveAction)
        {
            Check.NotNull(resolveAction, nameof(resolveAction));

            _resolveAction = resolveAction;
        }

        public override Task ResolveAsync(IStadiumResolveContext context)
        {
            _resolveAction(context);
            return Task.CompletedTask;
        }
    }
}
