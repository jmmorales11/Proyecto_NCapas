using Serilog;
using System;
using System.IO;

namespace SL.Logger
{
    public class LogHelper
    {
        static LogHelper()
        {
            Serilog.Debugging.SelfLog.Enable(msg => System.Diagnostics.Debug.WriteLine(msg));

            // Ruta específica para los logs
            var logDirectory = @"C:\Logs";

            // Crear el directorio si no existe
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            var logFilePath = Path.Combine(logDirectory, "app.log");

            // Configurar Serilog para escribir en la consola y en un archivo en la ruta especificada
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console() // Este es el sink para la consola
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day) // Este es el sink para archivo
                .CreateLogger();

            // Log de la ruta del archivo de log
            Log.Information($"El archivo de log se creará en: {logFilePath}");
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