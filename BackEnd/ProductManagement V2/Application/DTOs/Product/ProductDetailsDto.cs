namespace ProductManagement_V2.Application.DTOs.Product
{
    public class ProductDetailsDto: ProductResponseDto
    {
        public List<ProductStatusHistoryDto> History { get; set; }

    }
}
