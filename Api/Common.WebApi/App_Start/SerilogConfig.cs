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
    public static class SerilogConfig
    {
        public static void Configure(IConfiguration configuration)
        {
            //LogHelper.NEVER_EAT_POISON_Disable_CertificateValidation();
            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Debug()
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .WriteTo.MySQL(
                                connectionString: configuration.GetConnectionString("MySqlDatabase"),
                                tableName: "Logs"
                            )
                            .WriteTo.Seq(
                                serverUrl: configuration.GetSection("SeriLogConfig:SeqHostUrl").Value,
                                apiKey: configuration.GetSection("SeriLogConfig:ApiKey").Value
                            )
                            .Enrich.WithMachineName()
                            .Enrich.WithEnvironmentUserName()
                            .CreateLogger();
        }
    }
}
