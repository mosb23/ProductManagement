namespace ProductManagement_V2.Application.DTOs.Product
{
    public class AuditableDto : BaseDto
    {
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
