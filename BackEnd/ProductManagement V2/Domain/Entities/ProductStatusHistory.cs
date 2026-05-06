using ProductManagement_V2.Domain.Enums;

namespace ProductManagement_V2.Domain.Entities
{
    public class ProductStatusHistory : AuditableEntity
    {
        public int ProductId { get; private set; }
        public ProductStatus OldStatus { get; private set; }
        public ProductStatus NewStatus { get; private set; }


        public Product? Product { get; private set; }

        private ProductStatusHistory()
        {
        }

        public ProductStatusHistory(
            int productId,
            ProductStatus oldStatus,
            ProductStatus newStatus,
            string? createdByUserId,
            string createdByName)
        {
            ProductId = productId;
            OldStatus = oldStatus;
            NewStatus = newStatus;

            SetCreated(DateTime.UtcNow, createdByName, createdByUserId);
        }

    }
}
