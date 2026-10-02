using Application.Features.Categories.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;
using Zero.Core.Specification;

namespace Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ErrorOr<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Category, int>();
            
            var spec = new BaseSpecifications<Category, int>(c => c.Id == request.Id)
            {
                IsAsNoTracking = true
            };
            spec.Includes.Add(c => c.Children);

            var category = await repo.GetWithSpecficationAsync(spec);

            if (category == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found.");
            }

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
