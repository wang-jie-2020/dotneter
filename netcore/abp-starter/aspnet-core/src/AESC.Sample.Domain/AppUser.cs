using Volo.Abp.Auditing;
using Volo.Abp.Users;

namespace AESC.Sample
{
    public class AppUser : FullAuditedAggregateRoot<Guid>, IUser
    {
        public virtual Guid? TenantId { get; }

        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        public virtual string UserName { get; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedUserName { get; }

        /// <summary>
        /// Gets or sets the Name for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Name { get; }

        /// <summary>
        /// Gets or sets the Surname for the user.
        /// </summary>
        [CanBeNull]
        public virtual string Surname { get; }

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        public virtual string Email { get; }

        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [DisableAuditing]
        public virtual string NormalizedEmail { get; }

        public bool EmailConfirmed { get; }

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [CanBeNull]
        public virtual string PhoneNumber { get; }

        public bool PhoneNumberConfirmed { get; }

        /// <summary>
        /// Gets or sets a flag indicating if the user is active.
        /// </summary>
        public virtual bool IsActive { get; }

        protected AppUser()
        {
        }

        public override string ToString()
        {
            return $"{base.ToString()}, UserName = {UserName}";
        }
    }
}
