using ProductManagement_V2.Application.DTOs;


namespace ProductManagement_V2.Application.Contract
{
    public class ProductQueryContract : PaginationBaseDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Quantity { get; set; }
    }
}
