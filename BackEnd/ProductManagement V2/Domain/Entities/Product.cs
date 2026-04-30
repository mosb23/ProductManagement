using Microsoft.EntityFrameworkCore;
using ProductManagement_V2.Domain.Enums;
using ProductManagement_V2.Domain;


namespace ProductManagement_V2.Domain.Entities
{
    public class Product : SoftDeleteEntity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public ProductStatus Status { get; private set; } = ProductStatus.Available;

        public List<ProductStatusHistory> StatusHistories { get; private set; } = new();


        public Product(string name, string? description, decimal price, int quantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
        }

        public void Update(string? name = null, string? description = null, decimal? price = null, int? quantity = null)
        {
            if (name != null) Name = name;
            if (description != null) Description = description;
            if (price.HasValue) Price = price.Value;
            if (quantity.HasValue) Quantity = quantity.Value;

        }

        public void UpdateStatus(ProductStatus newStatus)
        {
            if (Status == newStatus)
                throw new Exception("Status is the same");

            var oldStatus = Status;

            Status = newStatus;

            StatusHistories.Add(new ProductStatusHistory(Id, oldStatus, newStatus));


        }
    }
}
