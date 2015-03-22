using System;
using System.Collections.Generic;
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
            
            _tableClient = tableClient.GetTableReference("Log4net");
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
