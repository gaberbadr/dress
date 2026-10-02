using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;
using Application.Features.Products.DTOs;

namespace Application.Features.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : string.Empty));
            
            CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
        }
    }
}
