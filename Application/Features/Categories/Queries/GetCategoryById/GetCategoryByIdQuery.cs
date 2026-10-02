using Application.Features.Categories.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<ErrorOr<CategoryDto>>
    {
        public int Id { get; set; }
    }
}
