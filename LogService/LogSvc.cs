using System.Runtime.InteropServices;
using System;
using Avalara.AvaTax.Adapter.MSMQ;
using System.IO;
using System.Xml;
using log4net;
using System.Diagnostics;
using System.Security;
using System.Net.NetworkInformation;
using System.Management;
using System.Collections.Generic;
using Avalara.AvaTax.Adapter.Log4net.FileAppenders;
using log4net.Core;
using log4net.Repository.Hierarchy;
using System.Threading;
using System.Text;
using System.Net;
using System.Collections.Specialized;


namespace Avalara.AvaTax.Adapter.LogService
{
    /// <summary>
    /// LogLevel enum
    /// </summary>
    [Guid("85C9C335-E376-4be4-A830-D86CE8EA5FC0")]
    [ComVisible(true)]
    public enum LogLevel
    {
        /// <summary>
        /// None
        /// </summary>
        None = -1,

        /// <summary>
        /// Debug
        /// </summary>
        Debug,

        /// <summary>
        /// Info
        /// </summary>
        Info,

        /// <summary>
        /// Warning
        /// </summary>
        Warning,

        /// <summary>
        /// Error
        /// </summary>
        Error,

        /// <summary>
        /// Fatal
        /// </summary>
        Fatal
    };

    [ComVisible(true)]
    public enum LogWriting
    {
        File,
        LogEntries
    }

    /// <summary>
    /// LogSvc
    /// </summary>
    [Guid("8E8B9308-7E9F-4036-A567-747F804AA207")]
    [ComVisible(true)]
    public class LogSvc
    {
        /// <summary>
        /// Default log file path. If folder path is not passed by user then below path is used.
        /// </summary>
        private static readonly ILog logEntriesLog = LogManager.GetLogger(typeof(LogSvc).FullName);
        private readonly string logEntiresConfigFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\Avalara.AvaTax.Adapter.dll.config";
        private static Boolean appendersInitialized = false;

        [DispId(2)]
        public void UsageLog(LogType logType, ServiceLogLevel logLevel, string connectorName, string connectorVersion,
            string docCode, string operation, string serviceUrl, string source, int lineCount, EventBlock eventBlock, string functionName, TimeType timeType, string callerAcctNum, string licenseKey, String message, Boolean writeToLocalFile, Boolean writeToLogEntries, string folderPath = "")
        {

            try
            {
                InitializeAppender(writeToLocalFile, writeToLogEntries, callerAcctNum, licenseKey, connectorName, folderPath);
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state)
                        {
                            WriteLogs(callerAcctNum, logType, logLevel, connectorName, connectorVersion,
                                docCode, operation, serviceUrl, source, lineCount, eventBlock, functionName, timeType, licenseKey, message,
                                writeToLocalFile, writeToLogEntries, folderPath);
                        }), null);

            }
            catch (Exception ex)
            {
                try
                {
                    LogMessages("Error in Logging.");
                    LogMessages(ex.Message.ToString());
                    if (ex.InnerException != null)
                    {
                        LogMessages(ex.InnerException.ToString());
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        [DispId(3)]
        public void ConfigurationLog(string callerAcctNum, LogType logType, ServiceLogLevel logLevel, string connectorName, string connectorVersion, string operation, string serviceUrl, string functionName, string source, String message, string licenseKey, Boolean writeToLocalFile, Boolean writeToLogEntries, string folderPath = "")
        {
            try
            {
                InitializeAppender(writeToLocalFile, writeToLogEntries, callerAcctNum, licenseKey, connectorName, folderPath);
                ThreadPool.QueueUserWorkItem(new WaitCallback(delegate(object state)
                {
                    WriteLogs(callerAcctNum, logType, logLevel, connectorName, connectorVersion, operation, serviceUrl, functionName, source, message, licenseKey, writeToLocalFile, writeToLogEntries, folderPath);
                }), null);
            }
            catch (Exception ex)
            {
                try
                {
                    LogMessages("Error in Logging.");
                    LogMessages(ex.Message);
                    if (ex.InnerException != null)
                    {
                        LogMessages(ex.InnerException.ToString());
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private void InitializeAppender(Boolean writeToLocalFile, Boolean writeToLogEntries, string callerAcctNum, string licenseKey, string connectorName,
            string folderPath = "")
        {
            try
            {
                Appender appender = new Appender();
                appender.InitializeAppender(writeToLocalFile, writeToLogEntries, callerAcctNum, licenseKey, connectorName, ref appendersInitialized, folderPath);
            }
            catch (Exception ex)
            {
                LogMessages("-----------------------------------------");
                LogMessages("Error while Initializing appenders");
                LogMessages(ex.Message);
                LogMessages(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogMessages(ex.InnerException.ToString());
                }
            }
        }

        private void WriteLogs(string callerAcctNum, LogType logType, ServiceLogLevel logLevel, string connectorName, string connectorVersion, string operation, string serviceUrl, string functionName, string source, string message, string licenseKey, Boolean writeToLocalFile, Boolean writeToLogEntries, string folderPath = "")
        {
            try
            {

                string logMessage = string.Empty;
                logMessage = "[";
                logMessage = logMessage + "{\"" + "CallerAcctNum" + "\"" + ":" + "\"" + callerAcctNum + "\"}";
                logMessage = logMessage + "{\"" + "LogType" + "\"" + ":" + "\"" + GetLogTypeAsString(logType) + "\"}";
                logMessage = logMessage + "{\"" + "LogLevel" + "\"" + ":" + "\"" + GetLogLevelAsString(logLevel) + "\"}";
                logMessage = logMessage + "{\"" + "ConnectorName" + "\"" + ":" + "\"" + connectorName + "\"}";
                logMessage = logMessage + "{\"" + "ConnectorVersion" + "\"" + ":" + "\"" + connectorVersion + "\"}";
                logMessage = logMessage + "{\"" + "Operation" + "\"" + ":" + "\"" + operation + "\"}";
                logMessage = logMessage + "{\"" + "ServiceURL" + "\"" + ":" + "\"" + serviceUrl + "\"}";
                logMessage = logMessage + "{\"" + "Source" + "\"" + ":" + "\"" + source + "\"}";
                logMessage = logMessage + "{\"" + "FunctionName" + "\"" + ":" + "\"" + functionName + "\"}";
                logMessage = logMessage + "{\"" + "Message" + "\"" + ":" + "\"" + message + "\"}";
                logMessage = logMessage + "]";


                LogToLogEntries(logMessage);
            }
            catch (Exception ex)
            {
                try
                {
                    LogMessages("-----------------------------------------");
                    LogMessages("Error while writing logs ");
                    LogMessages(ex.Message);
                    LogMessages(ex.StackTrace);
                    if (ex.InnerException != null)
                    {
                        LogMessages(ex.InnerException.ToString());
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                }

            }
        }

        private void LogToLogEntries(string logMessage)
        {
            Uri uri = new Uri(Appender.LogEntriesURL);
            {
                byte[] data = Encoding.UTF8.GetBytes(logMessage);
                string result_post = SendRequest(uri, data, "application/json", "POST");
            }
        }

        private string SendRequest(Uri uri, byte[] jsonDataBytes, string contentType, string method)
        {
            WebRequest req = WebRequest.Create(uri);
            req.ContentType = contentType;
            req.Method = method;
            req.ContentLength = jsonDataBytes.Length;

            Stream stream = req.GetRequestStream();
            stream.Write(jsonDataBytes, 0, jsonDataBytes.Length);
            stream.Close();

            Stream response = req.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(response);
            String res = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return res;
        }

         private void WriteLogs(string callerAcctNum, LogType logType, ServiceLogLevel logLevel, string connectorName,
                                string connectorVersion, string docCode,
                                string operation, string serviceUrl, string source, int lineCount, EventBlock eventBlock,
                                string functionName, TimeType timeType, string licenseKey, string message, Boolean writeToLocalFile, Boolean writeToLogEntries, string folderPath)
        {
            try
            {
                string msg = string.Empty;
                if (message == string.Empty)
                {
                    msg = string.Format("LINECOUNT - {0} :: {1} : {2} - {3}",
                                  lineCount,
                                  (eventBlock == EventBlock.InternalFunction) ? functionName : eventBlock.ToString(),
                                  timeType.ToString(),
                                  DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz")
                                  );
                }
                else
                {
                    msg = message;
                }

                string logMessage = string.Empty;
                logMessage = "[";
                logMessage = logMessage + "{\"" + "CallerAcctNum" + "\"" + ":" + "\"" + callerAcctNum + "\"}";
                logMessage = logMessage + "{\"" + "LogType" + "\"" + ":" + "\"" + GetLogTypeAsString(logType) + "\"}";
                logMessage = logMessage + "{\"" + "LogLevel" + "\"" + ":" + "\"" + GetLogLevelAsString(logLevel) + "\"}";
                logMessage = logMessage + "{\"" + "ConnectorName" + "\"" + ":" + "\"" + connectorName + "\"}";
                logMessage = logMessage + "{\"" + "ConnectorVersion" + "\"" + ":" + "\"" + connectorVersion + "\"}";
                logMessage = logMessage + "{\"" + "Operation" + "\"" + ":" + "\"" + operation + "\"}";
                logMessage = logMessage + "{\"" + "ServiceURL" + "\"" + ":" + "\"" + serviceUrl + "\"}";
                logMessage = logMessage + "{\"" + "Source" + "\"" + ":" + "\"" + source + "\"}";
                logMessage = logMessage + "{\"" + "DocCode" + "\"" + ":" + "\"" + docCode + "\"}";
                logMessage = logMessage + "{\"" + "Message" + "\"" + ":" + "\"" + msg + "\"}";
                logMessage = logMessage + "]";

                LogToLogEntries(logMessage);
            }
            catch (Exception ex)
            {
                LogMessages("-----------------------------------------");
                LogMessages("Error while writing logs ");
                LogMessages(ex.Message);
                LogMessages(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    LogMessages(ex.InnerException.ToString());
                }
            }
        }

        public static void LogMessages(string strMessage)
        {
            string directoryName = Environment.CurrentDirectory + "\\AdaptorLog";
            try
            {
                if (!System.IO.Directory.Exists(directoryName))
                {
                    System.IO.Directory.CreateDirectory(directoryName);
                }
                System.IO.File.AppendAllText(string.Format(directoryName + "\\" + "{0:ddMMMyyyy}.log", System.DateTime.Now), string.Format("{0:dd.MM.yyyy HH:mm:ss} - {1}", DateTime.Now, strMessage + Environment.NewLine));
            }
            catch (Exception ex)
            {

            }
        }


        [DispId(1)]
        public void LogMessage(string clientName, LogLevel messageType, string message)
        {
            switch (messageType)
            {
                case LogLevel.Debug:
                    _avaLogger.Debug(clientName + "," + message);
                    break;
                case LogLevel.Info:
                    _avaLogger.Information(clientName + "," + message);
                    break;
                case LogLevel.Warning:
                    _avaLogger.Warning(clientName + "," + message);
                    break;
                case LogLevel.Error:
                    _avaLogger.Error(clientName + "," + message);
                    break;
                case LogLevel.Fatal:
                    _avaLogger.Fail(clientName + "," + message);
                    break;
            }
        }

        private string GetLogTypeAsString(LogType logType)
        {
            String returnValue;
            switch (logType)
            {
                case LogType.ConfigAudit:
                    returnValue = "ConfigAudit";
                    break;
                case LogType.Debug:
                    returnValue = "Debug";
                    break;
                case LogType.Performance:
                    returnValue = "Performance";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        private string GetLogLevelAsString(ServiceLogLevel logLevel)
        {
            String returnValue;
            switch (logLevel)
            {
                case ServiceLogLevel.Error:
                    returnValue = "Error";
                    break;
                case ServiceLogLevel.Exception:
                    returnValue = "Exception";
                    break;
                case ServiceLogLevel.Informational:
                    returnValue = "Informational";
                    break;
                default:
                    returnValue = "";
                    break;
            }
            return returnValue;
        }

        private readonly AvaLogger _avaLogger = AvaLogger.GetLogger();
    }

    /// <summary>
    /// Represents log type    /// 
    /// </summary>
    [Guid("9624AF1B-A66A-4E03-B9FA-7B29D4862DCE")]
    [ComVisible(true)]
    public enum LogType
    {
        Performance = 0,
        Debug = 1,
        ConfigAudit = 2
    }

    [ComVisible(true)]
    public enum FormStatus
    {
        Open,
        Close
    }

    [Guid("C5FB48A2-DA04-4AA2-98B8-485D01C66598")]
    [ComVisible(true)]
    public enum ServiceLogLevel
    {
        Error = 0,
        Exception = 1,
        Informational = 2
    }

    [Guid("788BBDED-3E05-4A67-9237-74C186DC27A8")]
    [ComVisible(true)]
    public enum EventBlock
    {
        InternalFunction = 0,
        PreGetTax = 1,
        PostGetTax = 2,
        PrePostTax = 3,
        PostPostTax = 4,
        PreCommitTax = 5,
        PostCommitTax = 6,
        PreAdjustTax = 7,
        PostAdjustTax = 8,
        PreCancelTax = 9,
        PostCancelTax = 10,
        PreGetTaxHistory = 11,
        PostGetTaxHistory = 12,
        PreReconcileTaxHistory = 13,
        PostReconcileTaxHistory = 14,
        PreBatchTax = 15,
        PostBatchTax = 16,
        FormOpen = 17,
        FormClose = 18
    }

    [Guid("D3C9A049-37D6-4966-A5B4-B7FCE01116DD")]
    [ComVisible(true)]
    public enum TimeType
    {
        StartTime = 0,
        EndTime = 1
    }
}
