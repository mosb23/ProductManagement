namespace ProductManagement_V2.Domain.Entities
{
    public class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public string? CreatedByUserId { get; private set; }

        public DateTime? UpdatedAt { get; private set; }
        public string? UpdatedBy { get; private set; }
        public string? UpdatedByUserId { get; private set; }

        protected AuditableEntity()
        {
            CreatedAt = DateTime.UtcNow;
            CreatedBy = "System";
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = "System";
        }

        public void SetCreated(DateTime at, string by, string? userId = null)
        {
            CreatedAt = at;
            CreatedBy = by;
            CreatedByUserId = userId;
        }

        public void SetUpdated(DateTime at, string by, string? userId = null)
        {
            UpdatedAt = at;
            UpdatedBy = by;
            UpdatedByUserId = userId;
        }

    }
}
