//using System;
//using Volo.Abp.Domain.Entities.Auditing;
//using Volo.Abp.Identity;
//using Volo.Abp.Users;

//namespace LG.NetCore.Identity.Users
//{
//    /*
//     *  可以实现需求，但这种模式较为臃肿。不继承似乎不太合适，但继承了又有点破坏原模块功能
//     */
//    public class AppUser : FullAuditedAggregateRoot<Guid>, IUser
//    {
//        public virtual string CardNo { get; set; }

//        public virtual Guid? TenantId { get; set; }

//        public virtual string UserName { get; set; }

//        public virtual string Name { get; set; }

//        public virtual string Surname { get; set; }

//        public virtual string Email { get; set; }

//        public virtual bool EmailConfirmed { get; set; }

//        public virtual string PhoneNumber { get; set; }

//        public virtual bool PhoneNumberConfirmed { get; set; }

//        private AppUser()
//        {
//        }
//    }
//}
