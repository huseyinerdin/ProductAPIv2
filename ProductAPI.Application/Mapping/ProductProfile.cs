using AutoMapper;
using ProductAPI.Application.DTOs.ProductDtos;
using ProductAPI.Core.Entities;

namespace ProductAPI.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponseDto>();
            CreateMap<ProductCreateRequestDto, Product>();
            CreateMap<ProductUpdateRequestDto, Product>();
        }
    }
}
