using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.Interfaces;
using Error = ErrorOr.Error;
using Domain.Entities;
using Domain.Repositories;
using Application.Features.Auth.DTOs;

namespace Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ErrorOr<TokenResponseDTO>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public RefreshTokenCommandHandler(
            UserManager<ApplicationUser> userManager,
            IUnitOfWork unitOfWork,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<ErrorOr<TokenResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenHash = _jwtService.HashToken(request.RefreshToken);
            var refreshTokenRepo = _unitOfWork.Repository<RefreshTokenTable, int>();
            var tokens = await refreshTokenRepo.FindAsync(t => t.Token == tokenHash);
            var tokenEntry = tokens.FirstOrDefault();

            if (tokenEntry == null || !tokenEntry.IsActive)
            {
                return Error.Unauthorized("auth.refresh.token.invalid", "Invalid refresh token.");
            }

            // Find user
            var user = await _userManager.FindByIdAsync(tokenEntry.UserId);
            if (user == null)
            {
                return Error.NotFound("auth.user.not.found", "User not found.");
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return Error.Unauthorized("auth.user.blocked", "This account is blocked. Please contact support.");
            }

            // Generate new tokens
            var (accessToken, accessExp) = await _jwtService.GenerateAccessTokenAsync(user, _userManager);
            var (newRefreshToken, refreshExp) = _jwtService.GenerateRefreshToken();
            var newRefreshTokenHash = _jwtService.HashToken(newRefreshToken);

            // Update existing token record
            tokenEntry.Token = newRefreshTokenHash;
            tokenEntry.ExpiresAt = refreshExp;
            tokenEntry.CreatedByIp = request.IpAddress;
            tokenEntry.RevokedAt = null;

            refreshTokenRepo.Update(tokenEntry);
            await _unitOfWork.CompleteAsync();

            var tokenResponse = new TokenResponseDTO
            {
                AccessToken = accessToken,
                AccessTokenExpiresAt = accessExp,
                RefreshToken = newRefreshToken,
                RefreshTokenExpiresAt = refreshExp
            };

            return tokenResponse;
        }
    }
}