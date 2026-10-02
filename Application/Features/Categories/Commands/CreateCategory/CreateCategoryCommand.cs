using Application.Features.Categories.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<ErrorOr<CategoryDto>>
    {
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
    }
}
