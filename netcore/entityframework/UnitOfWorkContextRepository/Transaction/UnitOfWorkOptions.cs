using System.Data;

namespace UnitOfWorkContextRepository.Transaction
{
    public class UnitOfWorkOptions
    {
        public IsolationLevel? IsolationLevel { get; set; }
    }

    public class UnitOfWorkDefaultOptions
    {
        public IsolationLevel IsolationLevel { get; set; } = System.Data.IsolationLevel.ReadUncommitted;

        internal UnitOfWorkOptions Normalize(UnitOfWorkOptions options)
        {
            if (options.IsolationLevel == null)
            {
                options.IsolationLevel = IsolationLevel;
            }

            return options;
        }
    }
}
