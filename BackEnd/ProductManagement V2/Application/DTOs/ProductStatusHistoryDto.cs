using ProductManagement_V2.Application.DTOs.Product;
using ProductManagement_V2.Domain.Enums;

namespace ProductManagement_V2.Application.DTOs
{
    public class ProductStatusHistoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

        public ProductStatus OldStatus { get; set; }
        public ProductStatus NewStatus { get; set; }

        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string? CreatedByRole { get; set; }
    }
}

