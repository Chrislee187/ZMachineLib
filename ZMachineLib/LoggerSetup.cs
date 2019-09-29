using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ZMachineLib.Managers;

namespace ZMachineLib
{
    public static class LoggerSetup
    {

        public static ILoggerFactory Factory { get; }

        static LoggerSetup()
        {
            var config = Config.Get();
            Factory = LoggerFactory
                .Create(c => c
                    .AddConsole()
                    .AddConfiguration(
                        config
                            .GetSection("Logging")
                            .GetSection("Console")
                    ));
        }

        public static ILogger<T> Create<T>() => Factory.CreateLogger<T>();

    }

    public static class LoggerExtensions
    {
        public static void ZMachineInitialising(this ILogger logger)
        {
            LoggerMessage.Define<DateTimeOffset>(
                    logLevel: LogLevel.Information,
                    eventId: 1,
                    formatString: "{StartTime:T} ZMachine Initialising")
                (logger, DateTimeOffset.Now, null);
        }

        public static void InfoMessage(this ILogger logger, string text)
        {
            var format = $"{{StartTime:T}} {text}";
            LoggerMessage.Define<DateTimeOffset>(
                    logLevel: LogLevel.Information,
                    eventId: 1,
                    formatString: format)
                (logger, DateTimeOffset.Now, null);
        }
        public static void DebugMessage(this ILogger logger, string text)
        {
            var format = $"{{StartTime:T}} {text}";
            LoggerMessage.Define<DateTimeOffset>(
                    logLevel: LogLevel.Debug,
                    eventId: 1,
                    formatString: format)
                (logger, DateTimeOffset.Now, null);
        }
        public static void OpDebugMessage(this ILogger logger, string text)
        {
            var format = $"{text}";
            LoggerMessage.Define(
                    logLevel: LogLevel.Debug,
                    eventId: 1,
                    formatString: format)
                (logger, null);
        }
    }

    public static class LogTextStringFormattingExtensions
    {
        public static string ToLogText(this IEnumerable<ushort> args)
            => string.Join(" ", args);

        public static string ToLogText(this byte resultDestination)
        {
            var logDest = VariableManager.VariableDestination(resultDestination);
            var logVarId = VariableManager.VariableId(resultDestination).ToString();
            var logVariable = $"{logDest}:{logVarId}";
            return logVariable;
        }
    }

    public static class OpLogging
    {
        private static readonly ILogger<Operations.Operations> Logger = LoggerSetup.Create<Operations.Operations>();

        public static void Op2WithStore(string opName, ushort arg1, ushort arg2, int result, byte variable)
        {
            var msg = $"$ {opName} {arg1} {arg2} -> {result} in {variable.ToLogText()}";
            Logger.OpDebugMessage(msg);
        }
    }
    public static class Config
    {
        public static IConfiguration Get()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }
    }
}