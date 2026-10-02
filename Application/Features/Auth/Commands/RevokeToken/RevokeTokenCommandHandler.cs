using Domain.Entities;
using Domain.Repositories;
using ErrorOr;
using MediatR;

using Error = ErrorOr.Error;

using Application.Interfaces;

namespace Application.Features.Auth.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, ErrorOr<Success>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;

        public RevokeTokenCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<ErrorOr<Success>> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var tokenHash = _jwtService.HashToken(request.RefreshToken);
            var refreshTokenRepo = _unitOfWork.Repository<RefreshTokenTable, int>();
            var tokens = await refreshTokenRepo.FindAsync(t => t.Token == tokenHash);
            var tokenEntry = tokens.FirstOrDefault();

            if (tokenEntry == null)
            {
                return Error.NotFound("auth.token.not.found", "Refresh token not found.");
            }

            tokenEntry.RevokedAt = DateTime.UtcNow;
            refreshTokenRepo.Update(tokenEntry);
            await _unitOfWork.CompleteAsync();

            return Result.Success;
        }
    }
}