using Application.Features.Categories.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ErrorOr<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Category, int>();

            if (request.ParentCategoryId.HasValue)
            {
                var parent = await repo.GetAsync(request.ParentCategoryId.Value);
                if (parent == null)
                {
                    return Error.NotFound("Category.NotFound", "Parent category not found.");
                }
            }

            var category = new Category
            {
                Name = request.Name,
                ParentCategoryId = request.ParentCategoryId
            };

            await repo.AddAsync(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
