using Domain.Entities;
using Zero.Core.Specification;

namespace Domain.Specifications.Products
{
    public class ProductCountSpecification : BaseSpecifications<Product, int>
    {
        public ProductCountSpecification(
            string? searchTerm,
            int? categoryId,
            List<int>? categoryIds,
            decimal? minPrice,
            decimal? maxPrice,
            bool? isAvailable)
        {
            Criteria = p =>
                (string.IsNullOrEmpty(searchTerm) || p.Description.Contains(searchTerm)) &&
                (!categoryId.HasValue || p.CategoryId == categoryId.Value) &&
                (categoryIds == null || !categoryIds.Any() || categoryIds.Contains(p.CategoryId)) &&
                (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
                (!isAvailable.HasValue || p.IsAvailable == isAvailable.Value);
        }
    }
}
