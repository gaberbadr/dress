using Application.Common.Pagination;
using Application.Features.Products.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQuery : PaginationRequest, IRequest<ErrorOr<PaginationResponse<ProductDto>>>
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public string? CategoryIds { get; set; } // Comma separated list of category IDs
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
