using Application.Features.Categories.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;
using Zero.Core.Specification;

namespace Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ErrorOr<IEnumerable<CategoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IEnumerable<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Category, int>();
            
            // Load ALL categories so EF Core builds the full tree in memory via navigation fix-up
            var spec = new BaseSpecifications<Category, int>();
            spec.IsAsNoTracking = false;

            var allCategories = await repo.GetAllWithSpecficationAsync(spec);

            // Extract roots; EF Core will have populated their Children recursively
            var roots = allCategories.Where(c => c.ParentCategoryId == null).ToList();

            return _mapper.Map<IEnumerable<CategoryDto>>(roots).ToList();
        }
    }
}
