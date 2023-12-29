using System.Threading;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium
{
    public class AsyncLocalCurrentStadiumAccessor : ICurrentStadiumAccessor, ISingletonDependency
    {
        public BasicStadiumInfo Current
        {
            get => _currentScope.Value;
            set => _currentScope.Value = value;
        }

        private readonly AsyncLocal<BasicStadiumInfo> _currentScope;

        public AsyncLocalCurrentStadiumAccessor()
        {
            _currentScope = new AsyncLocal<BasicStadiumInfo>();
        }
    }
}