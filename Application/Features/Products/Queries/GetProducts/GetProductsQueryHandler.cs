using Application.Common.Pagination;
using Application.Features.Products.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Specifications.Products;
using ErrorOr;
using MediatR;

namespace Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, ErrorOr<PaginationResponse<ProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<PaginationResponse<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product, int>();

            List<int>? categoryIdList = null;
            if (!string.IsNullOrEmpty(request.CategoryIds))
            {
                categoryIdList = request.CategoryIds.Split(',')
                    .Where(id => int.TryParse(id, out _))
                    .Select(int.Parse)
                    .ToList();
            }

            var spec = new ProductFilterSpecification(
                request.SearchTerm,
                request.CategoryId,
                categoryIdList,
                request.MinPrice,
                request.MaxPrice,
                request.IsAvailable,
                request.SortBy,
                request.SortDirection);

            var countSpec = new ProductCountSpecification(
                request.SearchTerm,
                request.CategoryId,
                categoryIdList,
                request.MinPrice,
                request.MaxPrice,
                request.IsAvailable);

            var totalCount = await repo.GetCountAsync(countSpec);

            spec.applyPagnation(request.PageSize * (request.PageIndex - 1), request.PageSize);

            var products = await repo.GetAllWithSpecficationAsync(spec);

            var data = _mapper.Map<IEnumerable<ProductDto>>(products);

            return new PaginationResponse<ProductDto>(request.PageSize, request.PageIndex, totalCount, data);
        }
    }
}
