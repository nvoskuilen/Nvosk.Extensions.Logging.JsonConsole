# Nvosk.Extensions.Logging.JsonConsole
JsonConsole logger provider implementation for Microsoft.Extensions.Logging.

Outputs Json formatted logs to stdout/console, ideal for centralised structured logging in Kubernetes with EFK stack (Elasticsearch/Fluent-Bit/Kibana).

### Install nuget package
```
PM> Install-Package Nvosk.Extensions.Logging.JsonConsole -Version 3.0.0
```

### appsettings.{environment}.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "System": "Information",
      "Microsoft": "Information"
    },
    "Console": {
      "Format": "Json", // Default|Json|Systemd
      "TimestampFormat": "yyyy-MM-ddTHH:mm:ss",
      "LogToStandardErrorThreshold": "Warning",
      "LogLevel": {
        "Default": "Information"
      },
      "JsonMessageTemplate": {
        "TimeStamp": "time",
        "LogLevel": "level",
        "Source": "source",
        "EventId": "event_id",
        "Message": "message",
        "Exception": "exception"
      }
    }
  }
}
```

### Program.cs
```c#
    .ConfigureLogging((hostContext, configLogging) =>
    {
        // clear all previously registered providers
        configLogging.ClearProviders();
        configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
    
        configLogging.AddJsonConsole();
    })
```


### 3.0.0 Release notes
- This version is based on Microsoft.Extensions.Logging.Console v3.0.0 and must be removed from your project PackageReference (if any)
  as all original functionality is also included in Nvosk.Extensions.Logging.JsonConsole v3.0.0. 
- Via the new "Format" property one can switch between Default, Json or Systemd formats.
- Replaced Newtonsoft.Json with Microsoft's new high performance System.Text.Json parser.
- Breaking changes: please review your appsettings.json files as there are some renames and new additions.


### 1.0.0 Release notes
- Initial JsonConsole based on Microsoft.Extensions.Logging.Console v2.2.0
