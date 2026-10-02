using Application.Features.Categories.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, ErrorOr<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Category, int>();

            var category = await repo.GetAsync(request.Id);
            if (category == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found.");
            }

            if (request.ParentCategoryId.HasValue)
            {
                if (request.ParentCategoryId.Value == request.Id)
                {
                    return Error.Validation("Category.CircularDependency", "Category cannot be its own parent.");
                }

                var parent = await repo.GetAsync(request.ParentCategoryId.Value);
                if (parent == null)
                {
                    return Error.NotFound("Category.ParentNotFound", "Parent category not found.");
                }
            }

            category.Name = request.Name;
            category.ParentCategoryId = request.ParentCategoryId;

            repo.Update(category);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
