using System;

namespace Hugin.Domain.Entities
{
    public interface IMultiUser
    {
        Guid? UserId { get; }
    }
}
