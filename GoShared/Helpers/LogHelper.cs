using GoShared.Interface;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;

namespace GoShared.Helpers;

public static class LogHelper
{
    public static Log _log;
    public static readonly Logger Logger;
    static LogHelper()
    {
        string directory = string.Empty + Directory.GetParent(Environment.CurrentDirectory);
        LoggingConfiguration config = new();

        FileTarget target_Common = new()
        {
            FileName = Path.Combine(directory, "log", "comm", "${date:format=yyyyMM}/comm_${date:format=yyyyMMdd}.txt"),
            Layout = @"[${date:format=HH\:mm\:ss}] ${message} (${callsite:className=true:includeNamespace=false:fileName=false:includeSourcePath=false:methodName=true}[${callsite-linenumber}][${threadid}])",
            ArchiveEvery = FileArchivePeriod.Day
        };

        FileTarget target_Error = new()
        {
            FileName = Path.Combine(directory, "log", "error", "${date:format=yyyyMM}/error_${date:format=yyyyMMdd}.txt"),
            Layout = @"[${date:format=HH\:mm\:ss}] ${message} (${callsite:className=true:includeNamespace=false:fileName=false:includeSourcePath=false:methodName=true}[${callsite-linenumber}][${threadid}])",
            ArchiveEvery = FileArchivePeriod.Day
        };

        FileTarget target_Trace = new()
        {
            FileName = Path.Combine(directory, "log", "trace", "${date:format=yyyyMM}/trace_${date:format=yyyyMMdd}.txt"),
            Layout = @"[${date:format=HH\:mm\:ss}] ${message} (${callsite:className=true:includeNamespace=false:fileName=false:includeSourcePath=false:methodName=true}[${callsite-linenumber}][${threadid}])",
            ArchiveEvery = FileArchivePeriod.Day
        };

        FileTarget target_jrn  = new()
        {
            FileName = Path.Combine(directory, "log", "jrn", "${date:format=yyyyMM}/jrn_${date:format=yyyyMMdd}.txt"),
            Layout = @"[${date:format=HH\:mm\:ss}] ${message} (${callsite:className=true:includeNamespace=false:fileName=false:includeSourcePath=false:methodName=true}[${callsite-linenumber}][${threadid}])",
            ArchiveEvery = FileArchivePeriod.Day
        };

        AsyncTargetWrapper asyncWrapper_Common = new(target_Common, 1000, AsyncTargetWrapperOverflowAction.Grow);
        LoggingRule rule_Common = new("*", asyncWrapper_Common);
        rule_Common.SetLoggingLevels(LogLevel.Debug, LogLevel.Fatal);
        rule_Common.DisableLoggingForLevel(LogLevel.Fatal);
        rule_Common.DisableLoggingForLevel(LogLevel.Error);
        rule_Common.DisableLoggingForLevel(LogLevel.Trace);
        config.LoggingRules.Add(rule_Common);

        AsyncTargetWrapper asyncWrapper_Error = new(target_Error, 1000, AsyncTargetWrapperOverflowAction.Grow);
        LoggingRule rule_Error = new("*", LogLevel.Error, asyncWrapper_Error);
        rule_Error.DisableLoggingForLevel(LogLevel.Fatal);
        config.LoggingRules.Add(rule_Error);

        AsyncTargetWrapper asyncWrapper_Trace = new(target_Trace, 1000, AsyncTargetWrapperOverflowAction.Grow);
        LoggingRule rule_Trace = new("*", asyncWrapper_Trace);
        rule_Trace.EnableLoggingForLevel(LogLevel.Trace);
        config.LoggingRules.Add(rule_Trace);

        AsyncTargetWrapper asyncWrapper_jrn = new(target_jrn, 1000, AsyncTargetWrapperOverflowAction.Grow);
        LoggingRule rule_jrn = new("*", asyncWrapper_jrn);
        rule_jrn.EnableLoggingForLevel(LogLevel.Fatal);
        config.LoggingRules.Add(rule_jrn);

        //#if DEBUG
        //        OutputDebugStringTarget targetOutput = new()
        //        {
        //            Layout = target_Common.Layout
        //        };

        //        AsyncTargetWrapper asyncWrapperOutput = new(targetOutput, 1000, AsyncTargetWrapperOverflowAction.Grow);
        //        LoggingRule ruleOutput = new("*", LogLevel.Trace, asyncWrapperOutput);
        //        config.LoggingRules.Add(ruleOutput);
        //#endif
        LogManager.Configuration = config;
        Logger = LogManager.GetLogger("LogHelper");
    }

    public static void ShutdownLogManager()
    {
        LogManager.Flush();
        LogManager.Shutdown();
    }

    public static ILog GetLog => _log ?? (_log = new Log());
}
