using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.MultiStadium
{
    public class CurrentStadium : ICurrentStadium, ITransientDependency
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual Guid? Id => _currentStadiumAccessor.Current?.StadiumId;

        public string Name => _currentStadiumAccessor.Current?.Name;

        private readonly ICurrentStadiumAccessor _currentStadiumAccessor;

        public CurrentStadium(ICurrentStadiumAccessor currentStadiumAccessor)
        {
            _currentStadiumAccessor = currentStadiumAccessor;
        }

        public IDisposable Change(Guid? id, string name = null)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(Guid? stadiumId, string name = null)
        {
            var parentScope = _currentStadiumAccessor.Current;
            _currentStadiumAccessor.Current = new BasicStadiumInfo(stadiumId, name);
            return new DisposeAction(() =>
            {
                _currentStadiumAccessor.Current = parentScope;
            });
        }
    }
}
