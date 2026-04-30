using AutoMapper;
using ProductManagement_V2.Application.Contract;
using ProductManagement_V2.Application.DTOs.Product;
using ProductManagement_V2.Domain.Entities;


namespace ProductManagement_V2.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductCreateContract, Product>();
        }
    }
}
