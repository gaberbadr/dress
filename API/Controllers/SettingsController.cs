using API.Controllers;
using Application.Features.Settings.Commands.UpdateSettings;
using Application.Features.Settings.Queries.GetSettings;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class SettingsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetSettings()
        {
            var result = await _mediator.Send(new GetSettingsQuery());

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(result.Value);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateSettings([FromBody] UpdateSettingsCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsError)
            {
                return HandleErrorResult(result.Errors);
            }

            return Ok(result.Value);
        }
    }
}
