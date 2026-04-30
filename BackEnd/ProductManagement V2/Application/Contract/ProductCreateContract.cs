using System.ComponentModel.DataAnnotations;

namespace ProductManagement_V2.Application.Contract
{
    public class ProductCreateContract
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
