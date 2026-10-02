using Application.Features.Categories.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Domain.Specifications.Categories;
using ErrorOr;
using MediatR;

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
            var spec = new CategoryWithChildrenSpecification(null); // root categories
            var categories = await repo.GetAllWithSpecficationAsync(spec);

            return _mapper.Map<IEnumerable<CategoryDto>>(categories).ToList();
        }
    }
}
