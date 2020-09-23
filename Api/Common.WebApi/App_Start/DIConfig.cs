using Common.DataAccess;
using Common.DataAccess.Repositories;
using Common.Entities;
using Common.IdentityManagement;
using Common.IdentityManagement.Interfaces;
using Common.IdentityManagement.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.WebApi.App_Start
{
    public static class DIConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IAuthenticationService, AuthenticationService<User>>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserPhotoRepository, UserPhotoRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            //services.AddTransient<ISettingsRepository, SettingsRepository>();
            //services.AddTransient<ISettingsService, SettingsService>();

            services.AddSingleton<IJwtManager>();
            services.AddTransient<IUserManager, UserManager>();
        }
    }
}
