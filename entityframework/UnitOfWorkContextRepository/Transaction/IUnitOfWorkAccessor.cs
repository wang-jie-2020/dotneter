namespace UnitOfWorkContextRepository.Transaction
{
    /// <summary>
    ///  UnitOfWork的线程访问对象
    /// </summary>
    public interface IUnitOfWorkAccessor
    {
        IUnitOfWork? UnitOfWork { get; }

        void SetUnitOfWork(IUnitOfWork? unitOfWork);
    }
}
