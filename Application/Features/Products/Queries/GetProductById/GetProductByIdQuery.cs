using Application.Features.Products.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQuery : IRequest<ErrorOr<ProductDto>>
    {
        public int Id { get; set; }
    }
}
