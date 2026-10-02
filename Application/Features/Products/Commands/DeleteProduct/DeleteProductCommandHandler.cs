using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Success>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var productRepo = _unitOfWork.Repository<Product, int>();

            var product = await productRepo.GetAsync(request.Id);
            if (product == null)
            {
                return Error.NotFound("Product.NotFound", "Product not found.");
            }

            productRepo.Delete(product);
            await _unitOfWork.CompleteAsync();

            return Result.Success;
        }
    }
}
