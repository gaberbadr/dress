using Application.Features.Products.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;
using Zero.Core.Specification;

namespace Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ErrorOr<ProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Product, int>();
            
            var spec = new BaseSpecifications<Product, int>(p => p.Id == request.Id)
            {
                IsAsNoTracking = true
            };
            spec.Includes.Add(p => p.Category);
            
            var product = await repo.GetWithSpecficationAsync(spec);

            if (product == null)
            {
                return Error.NotFound("Product.NotFound", "Product not found.");
            }

            return _mapper.Map<ProductDto>(product);
        }
    }
}
