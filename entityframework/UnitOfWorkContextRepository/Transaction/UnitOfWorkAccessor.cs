using System.Threading;

namespace UnitOfWorkContextRepository.Transaction
{
    public class UnitOfWorkAccessor : IUnitOfWorkAccessor
    {
        public IUnitOfWork UnitOfWork => _current.Value;

        private readonly AsyncLocal<IUnitOfWork> _current;

        public UnitOfWorkAccessor()
        {
            _current = new AsyncLocal<IUnitOfWork>();
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _current.Value = unitOfWork;
        }
    }
}