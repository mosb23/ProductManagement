namespace ProductManagement_V2.Domain.Entities
{
    public class SoftDeleteEntity : AuditableEntity
    {
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }

        public void MarkAsDeleted(DateTime now)
        {
            IsDeleted = true;
            DeletedAt = now;
        }
    }
}
