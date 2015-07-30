Log4netTableStorageAppender is a log 4 net appender to store your logs in azure table storage.

Append the following section to your log4net configuration:
```
<appender name="TableStorageLogAppender" type="Log4NetTableStorageAppender.AzureTableStorageAppender, Log4NetTableStorageAppender">      
  <bufferSize value="1" />
  <ConnectionStringName value="StorageConnectionString" />
  <layout type="log4net.Layout.PatternLayout">
    <conversionPattern value="%-5p %d %5rms %-22.22c{1} %-18.18M - %m%n" />
  </layout>
</appender>
```

You can customize the tablename the logger uses by setting the "AzureTableStorageAppender.TableName" setting in your AppSettings. If not set the logger will use the "log4Net" table. 
```
<appSettings>
  <add key="AzureTableStorageAppender.TableName" value="MyCustomTableName" />
</appSettings>
```