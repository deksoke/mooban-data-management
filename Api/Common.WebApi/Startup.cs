using AutoMapper;
using Common.WebApi.App_Start;
using Common.WebApi.ConnectionsDb;
using Common.WebApi.Filters;
using Common.WebApi.HealthChecks;
using Common.WebApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.DataProtection;
using MySql.Data.MySqlClient;
using Serilog;
using System.Data;
using System.Text;

namespace Common.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public static IDataProtectionProvider DataProtectionProvider { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.Configure<ConnectionStringList>(Configuration.GetSection("ConnectionStrings"));
            services.AddMemoryCache();
            services.AddMvc()
                .AddMvcOptions(options =>
                {
                    options.Filters.Add<LoggingActionFilter>();
                    //options.Filters.Add<CustomExceptionFilter>();
                });

            // Add health check services
            services.AddHealthChecks()
                .AddCheck(
                    "MoobanDB-check",
                    new MySqlConnectionHealthCheck(Configuration.GetConnectionString("MySqlDatabase")),
                    HealthStatus.Unhealthy,
                    new string[] { "MoobanDB" }
                    );
            

            // Auto Mapper Configurations
            ConfigureMapping(services);
            //services.AddAutoMapper(typeof(Startup));

            // Cors
            ConfigCors(services);

            // Configure JWT Authentication
            ConfigureJwtAuth(services);

            // Register the Swagger generator, defining 1 or more Swagger documents
            //SwaggerConfig.Configure(services);

            // Create the Serilog logger, and configure the sinks
            SerilogConfig.Configure(Configuration);

            RegisterDependencies(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<SerilogRequestLoggerMiddleware>();
            app.UseSerilogRequestLogging(opts =>
            {
                // EnrichFromRequest helper function is shown in the previous post
                opts.EnrichDiagnosticContext = LogHelper.EnrichFromRequest;

                // Use custom level function
                opts.GetLevel = LogHelper.ExcludeHealthChecks; 
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/hc"); //Add health check endpoint
                endpoints.MapControllers();
            });
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            var dbConnection = new MySqlConnection(Configuration.GetConnectionString("MySqlDatabase"));
            services.AddSingleton<IDbConnection>(dbConnection);

            DIConfig.Configure(services);
        }

        private void ConfigureMapping(IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                AutoMapperConfig.Configure(mc);
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
        }

        private void ConfigCors(IServiceCollection services)
        {
            // config cors
            services.AddCors(options => {
                //options.AddPolicy("AllowOrigin", builder =>
                //{
                //    builder.SetIsOriginAllowed(isOriginAllowed: _ => true);
                //    builder.AllowAnyOrigin();
                //    builder.AllowAnyHeader();
                //    builder.AllowAnyMethod();
                //    builder.AllowCredentials();
                //});

                var origins = Configuration["AllowedHosts"].Split(',');

                //options.AddDefaultPolicy(
                //    builder => builder
                //        .WithOrigins(origins)
                //        .AllowAnyMethod()
                //        .AllowAnyHeader()
                //        .AllowCredentials()
                //    );

                options.AddPolicy("AllowOrigin",
                    builder => builder
                        .WithOrigins(origins)
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                    );

                //options.AddPolicy("signalr",
                //    builder => builder
                //    .SetIsOriginAllowed(_ => true)
                //    .AllowAnyMethod()
                //    .AllowAnyHeader()
                //    .AllowCredentials()
                //    );

            });
        }

        private void ConfigureJwtAuth(IServiceCollection services)
        {
            services
                .AddAuthentication(x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Application:Host"],
                        ValidAudience = Configuration["Application:Host"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AppSecurity:Secret"]))
                    };
                });
        }


    }
}
