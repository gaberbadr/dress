using Application.Features.Settings.DTOs;
using AutoMapper;
using Domain.Repositories;
using ErrorOr;
using MediatR;

namespace Application.Features.Settings.Queries.GetSettings
{
    public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, ErrorOr<SettingsDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetSettingsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ErrorOr<SettingsDto>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Domain.Entities.Settings, int>();
            var settingsList = await repo.GetAllAsNoTrackingAsync();
            var settings = settingsList.FirstOrDefault();

            if (settings == null)
            {
                return new SettingsDto(); // Return default empty settings instead of 404
            }

            return _mapper.Map<SettingsDto>(settings);
        }
    }
}
