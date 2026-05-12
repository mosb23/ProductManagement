namespace ProductManagement_V2.Domain.Entities
{
    public class SoftDeleteEntity : AuditableEntity
    {
        public bool IsDeleted { get; private set; } = false;
        public DateTime? DeletedAt { get; private set; }
        public string? DeletedBy { get; private set; }
        public string? DeletedByUserId { get; private set; }

        public void MarkAsDeleted(DateTime now, string deletedBy, string? deletedByUserId = null)
        {
            IsDeleted = true;
            DeletedAt = now;
            DeletedBy = deletedBy;
            DeletedByUserId = deletedByUserId;
        }
    }
}
