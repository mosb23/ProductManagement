namespace ProductManagement_V2.Domain.Entities
{
    public class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }

        public DateTime? UpdatedAt { get; private set; }
        public string? UpdatedBy { get; private set; }

        protected AuditableEntity()
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = "Admin";
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = "Admin";
        }

        public void SetCreated(DateTime at, string by)
        {
            CreatedAt = at;
            CreatedBy = by;
        }

        public void SetUpdated(DateTime at, string by)
        {
            UpdatedAt = at;
            UpdatedBy = by;
        }

    }
}
