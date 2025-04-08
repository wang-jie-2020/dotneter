namespace UnitOfWorkContextRepository.Transaction
{
    public class UnitOfWorkEventArgs
    {
        public IUnitOfWork UnitOfWork { get; }

        public UnitOfWorkEventArgs(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
    }
}
