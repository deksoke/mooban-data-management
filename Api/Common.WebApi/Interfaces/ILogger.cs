using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.WebApi.Interfaces
{
    public interface ILogger
    {
        void LogError(string message, int userId, Exception ex = null);
        void LogInfo(string message, int userId);
    }
}
