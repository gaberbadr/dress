using Application.Features.Auth.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Features.Auth.Commands.AdminLogin
{
    public class AdminLoginCommandHandler : IRequestHandler<AdminLoginCommand, ErrorOr<AuthResponseDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminLoginCommandHandler(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<AuthResponseDTO>> Handle(AdminLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // To avoid email enumeration, we return generic message, 
                // but let's use standard Unauthorized.
                return Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                return Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials.");
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (!isAdmin)
            {
                return Error.Forbidden("Auth.Forbidden", "User does not have administrative privileges.");
            }

            var (accessToken, accessTokenExpires) = await _jwtService.GenerateAccessTokenAsync(user, _userManager);
            var (refreshToken, refreshTokenExpires) = _jwtService.GenerateRefreshToken();

            var tokenTable = new RefreshTokenTable
            {
                Token = _jwtService.HashToken(refreshToken),
                ExpiresAt = refreshTokenExpires,
                UserId = user.Id,
                CreatedByIp = request.IpAddress,
                CreatedAt = DateTime.UtcNow
            };

            var repo = _unitOfWork.Repository<RefreshTokenTable, int>();
            await repo.AddAsync(tokenTable);
            
            var loginAttempt = new LoginAttempt
            {
                Email = user.Email ?? request.Email,
                IsSuccessful = true,
                IpAddress = request.IpAddress ?? string.Empty,
                AttemptedAt = DateTime.UtcNow
            };
            var attemptRepo = _unitOfWork.Repository<LoginAttempt, int>();
            await attemptRepo.AddAsync(loginAttempt);

            await _unitOfWork.CompleteAsync();

            var response = new AuthResponseDTO
            {
                Success = true,
                Message = "Login successful.",
                Token = new TokenResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiresAt = accessTokenExpires,
                    RefreshTokenExpiresAt = refreshTokenExpires
                }
            };

            return response;
        }
    }
}
