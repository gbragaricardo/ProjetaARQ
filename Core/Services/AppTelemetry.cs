using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetaARQ.Core.Services
{
    public class AppTelemetry : IAppTelemetry
    {
        public void LogInfo(string message)
        {
            // Imprime na janela de Output do Visual Studio
            Debug.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
        }

        public void LogError(Exception ex, string contextMessage)
        {
            Debug.WriteLine($"[ERROR] {DateTime.Now:HH:mm:ss} - {contextMessage}");
            Debug.WriteLine($"[Detalhes da Exceção] {ex}");
        }

        public IDisposable TrackActivity(string activityName)
        {
            // Retorna a nossa classe aninhada que liga o cronômetro
            return new TelemetryScope(activityName, this);
        }

        // Classe auxiliar privada que gerencia o bloco 'using' e o cronômetro
        private class TelemetryScope : IDisposable
        {
            private readonly string _activityName;
            private readonly AppTelemetry _telemetry;
            private readonly Stopwatch _stopwatch;

            public TelemetryScope(string activityName, AppTelemetry telemetry)
            {
                _activityName = activityName;
                _telemetry = telemetry;
                _stopwatch = Stopwatch.StartNew(); // Inicia o cronômetro

                _telemetry.LogInfo($"Iniciando atividade: {_activityName}");
            }

            public void Dispose()
            {
                _stopwatch.Stop(); // Para o cronômetro quando o 'using' termina
                _telemetry.LogInfo($"Finalizado: {_activityName} (Tempo de execução: {_stopwatch.ElapsedMilliseconds}ms)");
            }
        }
    }
}
