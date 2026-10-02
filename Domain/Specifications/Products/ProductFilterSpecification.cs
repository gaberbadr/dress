using Domain.Entities;
using Zero.Core.Specification;

namespace Domain.Specifications.Products
{
    public class ProductFilterSpecification : BaseSpecifications<Product, int>
    {
        public ProductFilterSpecification(
            string? searchTerm,
            int? categoryId,
            List<int>? categoryIds,
            decimal? minPrice,
            decimal? maxPrice,
            bool? isAvailable,
            string? sortBy,
            string? sortDirection)
        {
            IsAsNoTracking = true;

            // Criteria for filtering
            Criteria = p =>
                (string.IsNullOrEmpty(searchTerm) || p.Description.Contains(searchTerm)) &&
                (!categoryId.HasValue || p.CategoryId == categoryId.Value) &&
                (categoryIds == null || !categoryIds.Any() || categoryIds.Contains(p.CategoryId)) &&
                (!minPrice.HasValue || p.Price >= minPrice.Value) &&
                (!maxPrice.HasValue || p.Price <= maxPrice.Value) &&
                (!isAvailable.HasValue || p.IsAvailable == isAvailable.Value);

            AddInclude(p => p.Category);

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isAsc = sortDirection?.ToLower() == "asc";

                switch (sortBy.ToLower())
                {
                    case "price":
                        if (isAsc) AddOrderBy(p => p.Price);
                        else AddOrderByDescending(p => p.Price);
                        break;

                    default:
                        AddOrderByDescending(p => p.CreatedAt);
                        break;
                }
            }
            else
            {
                // Default sort: Newest first
                AddOrderByDescending(p => p.CreatedAt);
            }
        }
    }
}
