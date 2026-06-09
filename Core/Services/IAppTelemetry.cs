using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Core.Services
{
    public interface IAppTelemetry
    {
        void LogInfo(string message);
        void LogError(Exception ex, string contextMessage);
        IDisposable TrackActivity(string activityName);
    }
}
