using Application.Features.Settings.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsCommand : IRequest<ErrorOr<SettingsDto>>
    {
        public string StoreName { get; set; } = string.Empty;
        public string? TikTokUrl { get; set; }
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Location { get; set; }
    }
}
