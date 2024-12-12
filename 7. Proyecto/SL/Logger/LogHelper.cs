using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SL.Logger
{
    public class LogHelper
    {
        static LogHelper()
        {
            // Configurar Serilog para escribir en la consola
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // Este es el sink para la consola
                .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day) // Este es el sink para archivo
                .CreateLogger();
        }

        public static void LogInformation(string message)
        {
            Log.Information(message);
        }

        public static void LogWarning(string message)
        {
            Log.Warning(message);
        }

        public static void LogError(string message, Exception ex)
        {
            Log.Error(ex, message);
        }
    }

}
