using Application.Features.Auth.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Features.Auth.Commands.AdminLogin
{
    public class AdminLoginCommand : IRequest<ErrorOr<AuthResponseDTO>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
    }
}
