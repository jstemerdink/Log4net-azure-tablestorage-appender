using System;
using Microsoft.WindowsAzure.Storage.Table;


namespace Log4NetTableStorageAppender.Models
{
    public class LogEntry : TableEntity
    {
        public LogEntry()
        {
        }

        public LogEntry(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public string Level { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ClassName { get; set; }
        public string FileName { get; set; }
        public string MethodName { get; set; }
        public string LineNumber { get; set; }
    }
}
