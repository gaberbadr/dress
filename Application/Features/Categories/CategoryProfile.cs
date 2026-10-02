using AutoMapper;
using Domain.Entities;
using Application.Features.Categories.DTOs;

namespace Application.Features.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}
