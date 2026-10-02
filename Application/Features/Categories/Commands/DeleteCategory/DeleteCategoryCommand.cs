using ErrorOr;
using MediatR;

namespace Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<ErrorOr<Success>>
    {
        public int Id { get; set; }
    }
}
