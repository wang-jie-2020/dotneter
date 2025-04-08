namespace AESC.Shared.Entities
{
    public class Book : FullAuditedAggregateRoot<long>
    {
        public string Name { get; set; }

        public Guid UserId { get; set; }
    }
}
