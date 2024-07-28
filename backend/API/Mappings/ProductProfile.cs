using API.DTOs.ProductDTOs;
using API.Models;
using AutoMapper;

namespace API.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>(); 
            CreateMap<Product, ProductListDto>(); 
            CreateMap<ProductSaveDto, Product>(); 
        }
    }
}