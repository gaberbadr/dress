using Domain.ValueObjects;

namespace Application.Features.Products.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }

        public string MainImage { get; set; } = string.Empty;
        public List<string> AdditionalImages { get; set; } = new();

        public bool IsAvailable { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public List<string> Sizes { get; set; } = new();
        public List<ProductFeatureDto> Features { get; set; } = new();
    }

    public class ProductFeatureDto
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
