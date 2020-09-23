using Serilog;
using System;
using ILogger = Common.WebApi.Interfaces.ILogger;

namespace Common.WebApi.Logger
{
    public class Logger : ILogger
    {
        public void LogError(string message, int userId, Exception ex = null)
        {
            Log.Logger.ForContext("UserId", userId).Error(ex, message);
        }

        public void LogInfo(string message, int userId)
        {
            Log.Logger.ForContext("UserId", userId).Information(message);
        }
    }
}
