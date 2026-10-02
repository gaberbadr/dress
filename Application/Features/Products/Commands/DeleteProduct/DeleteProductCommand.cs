using ErrorOr;
using MediatR;

namespace Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<ErrorOr<Success>>
    {
        public int Id { get; set; }
    }
}
