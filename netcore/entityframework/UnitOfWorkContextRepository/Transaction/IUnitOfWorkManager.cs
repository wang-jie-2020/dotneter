namespace UnitOfWorkContextRepository.Transaction
{
    /// <summary>
    ///  manager主要做的其实就是控制uow的作用域和初始化
    /// </summary>
    public interface IUnitOfWorkManager
    {
        IUnitOfWork? Current { get; }

        IUnitOfWork Begin(UnitOfWorkOptions unitOfWorkOptions, bool requiresNew = false);
    }
}
