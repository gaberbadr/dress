using Application.Features.Categories.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<ErrorOr<IEnumerable<CategoryDto>>>
    {
    }
}
