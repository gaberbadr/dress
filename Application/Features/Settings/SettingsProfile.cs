using AutoMapper;
using Domain.Entities;
using Application.Features.Settings.DTOs;

namespace Application.Features.Settings
{
    public class SettingsProfile : Profile
    {
        public SettingsProfile()
        {
            CreateMap<Domain.Entities.Settings, SettingsDto>().ReverseMap();
        }
    }
}
