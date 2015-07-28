Seleniumtest template
==============

## What is Log4net azure table storage appender?

Log4net azure table storage appender is a log4net appender to write the logs to azure table storage

## How to get started?

* Install log4net in your project.
* Add the TableStorageLogAppender configuration to your log4net section 
* Add a application setting named StorageConnectionString to your configuration or add a connectionstring to your configuration
* Set the connectionstring name in the configuration

## What is in the appender
* A logger that uses a buffer that sends a batch update to azure table storage.
* A azure runbook used by azure automation to cleanup the table storage table.

## Examples
<appender name="TableStorageLogAppender" type="Log4NetTableStorageAppender.AzureTableStorageAppender, Log4NetTableStorageAppender">      
  <bufferSize value="100" />
  <ConnectionStringName value="StorageConnectionString" />
  <layout type="log4net.Layout.PatternLayout">
    <conversionPattern value="%-5p %d %5rms %-22.22c{1} %-18.18M - %m%n" />
  </layout>
</appender>