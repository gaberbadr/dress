using Application.Features.Settings.DTOs;
using AutoMapper;
using Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Features.Settings.Commands.UpdateSettings
{
    public class UpdateSettingsCommandHandler : IRequestHandler<UpdateSettingsCommand, ErrorOr<SettingsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSettingsCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<SettingsDto>> Handle(UpdateSettingsCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Domain.Entities.Settings, int>();
            var settingsList = await repo.GetAllAsync();
            var settings = settingsList.FirstOrDefault();

            if (settings == null)
            {
                settings = new Domain.Entities.Settings
                {
                    StoreName = request.StoreName,
                    TikTokUrl = request.TikTokUrl,
                    FacebookUrl = request.FacebookUrl,
                    InstagramUrl = request.InstagramUrl,
                    PhoneNumber = request.PhoneNumber,
                    Location = request.Location
                };
                await repo.AddAsync(settings);
            }
            else
            {
                settings.StoreName = request.StoreName;
                settings.TikTokUrl = request.TikTokUrl;
                settings.FacebookUrl = request.FacebookUrl;
                settings.InstagramUrl = request.InstagramUrl;
                settings.PhoneNumber = request.PhoneNumber;
                settings.Location = request.Location;
                repo.Update(settings);
            }

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<SettingsDto>(settings);
        }
    }
}
