using Application.Features.Products.DTOs;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<ErrorOr<ProductDto>>
    {
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public IFormFile MainImage { get; set; } = null!;
        public List<IFormFile> AdditionalImages { get; set; } = new();
        public bool IsAvailable { get; set; } = true;
        public int CategoryId { get; set; }
        public List<string> Sizes { get; set; } = new();
        public List<ProductFeatureDto> Features { get; set; } = new();
    }
}
