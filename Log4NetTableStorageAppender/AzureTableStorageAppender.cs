using System;
using System.Collections.Generic;
using System.Configuration;
using log4net.Appender;
using log4net.Core;
using Log4NetTableStorageAppender.Models;

namespace Log4NetTableStorageAppender
{
    public class AzureTableStorageAppender : BufferingAppenderSkeleton
    {
        public string ConnectionStringName { get; set; }

        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (BufferSize > 99)
            {
                BufferSize = 99;
            }
        }

        protected override void SendBuffer(LoggingEvent[] logEvents)
        {
            if (ConnectionStringName == null)
            {
                throw new Exception("Connectionstring is null");
            }

            ConnectionStringSettings connectionstring = ConfigurationManager.ConnectionStrings[ConnectionStringName];
            string connectionstringValue;
            if (connectionstring == null)
            {
                connectionstringValue = ConfigurationManager.AppSettings["StorageConnectionString"];
                if (string.IsNullOrEmpty(connectionstringValue))
                {
                    throw new Exception("No connectionstring or appsetting is found with name: " + ConnectionStringName);
                }
            }
            else
            {
                connectionstringValue = connectionstring.ConnectionString;
                if (string.IsNullOrEmpty(connectionstringValue))
                {
                    throw new Exception("No Connectionstring attribute is found in connectionstring: " + ConnectionStringName);
                }
            }

            var logRepository = new LogRepository<LogEntry>(connectionstringValue);
            var list = new List<LogEntry>();

            foreach (var logEvent in logEvents)
            {
                var logEntry = new LogEntry(partitionKey: Environment.MachineName, rowKey: Guid.NewGuid().ToString())
                {
                    Message = logEvent.RenderedMessage,
                    UserName = logEvent.UserName
                };

                if (logEvent.LocationInformation != null)
                {
                    logEntry.Level = logEvent.Level.Name;
                    logEntry.ClassName = logEvent.LocationInformation.ClassName;
                    logEntry.FileName = logEvent.LocationInformation.FileName;
                    logEntry.MethodName = logEvent.LocationInformation.MethodName;
                    logEntry.LineNumber = logEvent.LocationInformation.LineNumber;
                }

                if (logEvent.ExceptionObject != null)
                {
                    logEntry.ExceptionMessage = logEvent.ExceptionObject.Message;
                    logEntry.ExceptionStackTrace = logEvent.ExceptionObject.StackTrace;
                }

                list.Add(logEntry);
            }

            logRepository.Add(list);
        }

        protected override void OnClose()
        {
            base.OnClose();
            Console.Write("Close");
        }
    }
}
