using ProductManagement_V2.Domain.Enums;

namespace ProductManagement_V2.Application.DTOs.Product
{
    public class ProductResponseDto : AuditableDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public ProductStatus Status { get; set; }

    }
}
