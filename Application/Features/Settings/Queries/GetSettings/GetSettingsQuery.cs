using Application.Features.Settings.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Settings.Queries.GetSettings
{
    public class GetSettingsQuery : IRequest<ErrorOr<SettingsDto>>
    {
    }
}
