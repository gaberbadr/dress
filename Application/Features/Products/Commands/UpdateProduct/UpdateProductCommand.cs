using Application.Features.Products.DTOs;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<ErrorOr<ProductDto>>
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile> NewAdditionalImages { get; set; } = new();
        public List<string> ExistingAdditionalImages { get; set; } = new();
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public List<string> Sizes { get; set; } = new();
        public List<ProductFeatureDto> Features { get; set; } = new();
    }
}
