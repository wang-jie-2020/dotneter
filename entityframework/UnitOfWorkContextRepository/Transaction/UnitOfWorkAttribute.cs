using System;
using System.Data;

namespace UnitOfWorkContextRepository.Transaction
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute
    {
        public IsolationLevel? IsolationLevel { get; set; }

        public UnitOfWorkAttribute()
        {

        }

        public UnitOfWorkAttribute(IsolationLevel isolationLevel)
        {
            IsolationLevel = isolationLevel;
        }

        public virtual void SetOptions(UnitOfWorkOptions options)
        {
            if (IsolationLevel.HasValue)
            {
                options.IsolationLevel = IsolationLevel;
            }
        }
    }
}
