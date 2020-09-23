using AutoMapper;
using AutoMapper.Configuration;
using Common.IdentityManagement.MappingProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.WebApi.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure(IMapperConfigurationExpression config)
        {
            config.AllowNullCollections = false;

            //config.AddProfile<IdentityUserProfile>();
            config.AddProfile<UserProfile>();
            config.AddProfile<SettingsProfile>();
        }
    }
}
