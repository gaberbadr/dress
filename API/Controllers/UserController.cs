
using API.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IMediator _mediator;
        private readonly CurrentUser _currentUser;

        public UserController(IMediator mediator, CurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }


        // Handles ErrorOr errors and returns appropriate HTTP responses.
        private IActionResult HandleErrorResult(IReadOnlyList<ErrorOr.Error> errors)
        {
            var firstError = errors.FirstOrDefault();

            return firstError.Type switch
            {
                ErrorOr.ErrorType.NotFound => NotFound(new { message = firstError.Description }),
                ErrorOr.ErrorType.Validation => BadRequest(new { message = firstError.Description }),
                ErrorOr.ErrorType.Conflict => Conflict(new { message = firstError.Description }),
                ErrorOr.ErrorType.Unauthorized => Unauthorized(new { message = firstError.Description }),
                ErrorOr.ErrorType.Failure => BadRequest(new { message = firstError.Description }),
                _ => StatusCode(500, new { message = firstError.Description })
            };
        }
    }
}
