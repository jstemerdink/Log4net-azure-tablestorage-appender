using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace Log4NetTableStorageAppender.Models
{
    public class LogRepository<T> where T : ITableEntity, new()
    {
        private readonly CloudTable _tableClient;

        public LogRepository(string connectionString)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = cloudStorageAccount.CreateCloudTableClient();

            var tableName = ConfigurationManager.AppSettings["AzureTableStorageAppender.TableName"];
            if (string.IsNullOrWhiteSpace(tableName)) tableName = "Log4net";
            
            _tableClient = tableClient.GetTableReference(tableName);
            _tableClient.CreateIfNotExists();
        }

        public void Add(IEnumerable<T> obj)
        {
            var batchOperation = new TableBatchOperation();
            foreach (var item in obj)
            {
                batchOperation.Insert(item);
            }

            if (batchOperation.Any())
            {
                _tableClient.ExecuteBatch(batchOperation);
            }
        }
    }
}
