using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;
using Zero.Core.Specification;

namespace Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var categoryRepo = _unitOfWork.Repository<Category, int>();
            var productRepo = _unitOfWork.Repository<Product, int>();

            var category = await categoryRepo.GetAsync(request.Id);
            if (category == null)
            {
                return Error.NotFound("Category.NotFound", "Category not found.");
            }

            // Check if there are children
            var spec = new BaseSpecifications<Category, int>(c => c.ParentCategoryId == request.Id);
            var children = await categoryRepo.GetAllWithSpecficationAsync(spec);
            if (children.Any())
            {
                return Error.Validation("Category.HasChildren", "Cannot delete category with child categories.");
            }

            // Check if there are products
            var prodSpec = new BaseSpecifications<Product, int>(p => p.CategoryId == request.Id);
            var products = await productRepo.GetAllWithSpecficationAsync(prodSpec);
            if (products.Any())
            {
                return Error.Validation("Category.HasProducts", "Cannot delete category with associated products.");
            }

            categoryRepo.Delete(category);
            await _unitOfWork.CompleteAsync();

            return Result.Success;
        }
    }
}
