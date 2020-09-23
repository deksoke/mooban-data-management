using AutoMapper;
using Common.DTO;
using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.IdentityManagement.MappingProfiles
{
    public class SettingsProfile : Profile
    {
        public SettingsProfile()
        {
            CreateMap<Settings, SettingsDTO>().ReverseMap();
        }
    }
}
