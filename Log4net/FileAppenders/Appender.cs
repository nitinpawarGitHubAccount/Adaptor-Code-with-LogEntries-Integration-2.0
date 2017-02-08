using System;
using System.Collections.Generic;
using System.IO;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net;
using System.Security;
using Avalara.AvaTax.Adapter.LogService;

namespace Avalara.AvaTax.Adapter.Log4net.FileAppenders
{
    internal class Appender
    {
        private static bool logFileCleanUpDone = false;
        private static bool isLogFileConfigured = false;
        private static bool islogEntriesConfigured = false;

        private string _TokenValue;
        public string TokenValue
        {
            get
            {
                return _TokenValue;
            }
            set
            {
                _TokenValue = value;
            }
        }

        public static string LogEntriesURL;

        private static IAppender CreateFileAppender(string folderPath)
        {
            RollingFileAppender appender = new RollingFileAppender();

            appender.Name = "FileAppender";
            string fileLocation = string.Empty;
            if (folderPath == string.Empty)
            {
                fileLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\logs\AvaLE.log";
            }
            else if (folderPath != string.Empty)
            {
                if (!Directory.Exists(folderPath))
                {
                    try
                    {
                        Directory.CreateDirectory(folderPath);
                        fileLocation = folderPath + @"\logs\AvaLE.log";
                    }
                    catch (Exception)
                    {
                        fileLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\logs\AvaLE.log";
                    }
                }
                else
                {
                    fileLocation = folderPath + @"\logs\AvaLE.log";
                }
            }

            appender.File = fileLocation;
            appender.LockingModel = new RollingFileAppender.MinimalLock();

            appender.AppendToFile = true;
            appender.RollingStyle = RollingFileAppender.RollingMode.Date;
            appender.DatePattern = "yyyyMMdd";
            appender.MaxSizeRollBackups = 7;
            PatternLayout layout = new PatternLayout();
            layout.ConversionPattern = "%newline%date %-5level %logger – %message – %property";
            layout.ActivateOptions();
            appender.Layout = layout;
            appender.ActivateOptions();
            return appender;
        }

        internal void ConfigureWithFile(string folderPath)
        {
            try
            {
                Hierarchy h = (Hierarchy)LogManager.GetRepository();
                h.Root.Level = Level.All;
                h.Root.AddAppender(CreateFileAppender(folderPath));
                h.Configured = true;
            }
            catch (Exception)
            {
            }
        }

        internal Hierarchy ConfigureLogEntries(string tokenValue)
        {

            //Hierarchy h = (Hierarchy)LogManager.GetRepository();
            //h.Root.Level = Level.All;
            //h.Root.AddAppender(Appender.LogEntriesAppender(tokenValue));
            //h.Configured = true;
            //return h;
            return null;
        }

        internal void InitializeAppender(Boolean writeToLocalFile, Boolean writeToLogEntries, string callerAcctNum, string licenseKey, string connectorName,
       ref Boolean AppendersInitialized, string folderPath = "")
        {
            try
            {
                #region Local File
                if (writeToLocalFile)
                {
                    if (isLogFileConfigured == false)
                    {
                        ConfigureWithFile(folderPath);
                        #region Delete old files
                        if (logFileCleanUpDone == false)
                        {
                            try
                            {
                                //var date = DateTime.Now.AddDays(-7);
                                //var task = new LogFileCleanupTask();
                                //task.CleanUp(date);
                                logFileCleanUpDone = true;
                            }
                            catch (Exception)
                            {
                                logFileCleanUpDone = false;
                            }
                        }
                        #endregion
                        isLogFileConfigured = true;
                    }
                }
                else
                {
                    Hierarchy h = (Hierarchy)LogManager.GetRepository();
                    h.Root.Level = Level.All;
                    h.Root.RemoveAppender("FileAppender");
                    isLogFileConfigured = false;
                }
                #endregion
                #region Log Entries
                if (writeToLogEntries)
                {
                    if (islogEntriesConfigured == false)
                    {
                        if (callerAcctNum != string.Empty && licenseKey != string.Empty && connectorName != string.Empty)
                        {
                            Token generateToken = new Token();
                            if (generateToken.GenerateToken(callerAcctNum, licenseKey, connectorName))
                            {
                                ConfigureLogEntries(TokenValue);
                                //    AppendersInitialized = true;
                                islogEntriesConfigured = true;
                            }
                        }
                    }
                }
                else
                {
                    Hierarchy h = (Hierarchy)LogManager.GetRepository();
                    h.Root.Level = Level.All;
                    h.Root.RemoveAppender("LogEntries");
                    islogEntriesConfigured = false;
                }
                #endregion
            }
            catch (Exception)
            {
            }
        }


    }

    public class Token
    {
        [SecurityCritical]
        public Boolean GenerateToken(string callerAcctNum, string licenseKey, string connectorName)
        {
            using (ILogTokenApi logTokenApi = new LogTokenApi())
            {
                try
                {
                    LogTokenRequest logTokenRequest = new LogTokenRequest() { Account = callerAcctNum, Authorization = Avalara.AvaTax.Adapter.LogService.AuthorizationType.AvaTax15Sdk, Client = "connector", LogName = string.Format("{0}:{1}:{2}", callerAcctNum, Environment.MachineName, Utilities.GetMacAddress()), LogSet = connectorName, Password = licenseKey, Source = Avalara.AvaTax.Adapter.LogService.LogTokenSourceType.Logentries };
                    LogTokenResult logTokenResult = logTokenApi.GetLogToken(logTokenRequest);
                    if (logTokenResult.Success == true)
                    {
                     //   TokenValue = logTokenResult.LogToken;
                        Appender.LogEntriesURL = @"https://webhook.logentries.com/noformat/logs/" + logTokenResult.LogToken + "/";
                        return true;
                    }
                    else
                    {
                        LogSvc.LogMessages("Token generation Un-Successful.");
                        LogSvc.LogMessages(logTokenResult.Error.ToString());
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
