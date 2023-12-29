using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using JetBrains.Annotations;
using UnitOfWorkContextRepository.Transaction;

namespace UnitOfWorkContextRepository.Extensions
{
    public static class UnitOfWorkManagerExtensions
    {
        public static IUnitOfWork Begin(
            this IUnitOfWorkManager unitOfWorkManager,
            bool requiresNew = false,
            IsolationLevel? isolationLevel = null)
        {
            return unitOfWorkManager.Begin(new UnitOfWorkOptions
            {
                IsolationLevel = isolationLevel,
            }, requiresNew);
        }

    }
}
