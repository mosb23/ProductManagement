using ProductManagement_V2.Application.DTOs;
using ProductManagement_V2.Domain.Enums;

namespace ProductManagement_V2.Application.Contract
{
    public class ProductStatusHistoryQueryContract : PaginationBaseDto
    {
        public int? ProductId { get; set; }
        public ProductStatus? OldStatus { get; set; }
        public ProductStatus? NewStatus { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

    }
}
