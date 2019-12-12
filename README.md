# Nvosk.Extensions.Logging.JsonConsole
JsonConsole logger provider implementation for Microsoft.Extensions.Logging.

Outputs json formatted logs to stdout/console. 

Ideal for high performance centralised structured logging in environments such as Kubernetes with EFK stack (Elasticsearch/Fluent-Bit/Kibana).

### Install nuget package
```
PM> Install-Package Nvosk.Extensions.Logging.JsonConsole -Version 3.0.0
```

### Add the "Console" section in appsettings.{environment}.json
The "JsonMessageTemplate" section is optional.
```javascript
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "System": "Information",
      "Microsoft": "Information"
    },
    "Console": {
      "Format": "Json",
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

### Add the "ConfigureLogging" section in Program.cs
```c#
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((hostContext, configLogging) =>
            {
                // clear all previously registered providers
                configLogging.ClearProviders();

                configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                configLogging.AddJsonConsole();
            });
```

### Example json output
```javascript
{"time":"2019-12-31T23:59:54","level":"trce","source":"ConsoleApp.Program","event_id":0,"message":"LogTrace..."}
{"time":"2019-12-31T23:59:55","level":"dbug","source":"ConsoleApp.Program","event_id":0,"message":"LogDebug..."}
{"time":"2019-12-31T23:59:56","level":"info","source":"ConsoleApp.Program","event_id":0,"message":"LogInformation..."}
{"time":"2019-12-31T23:59:57","level":"warn","source":"ConsoleApp.Program","event_id":0,"message":"LogWarning..."}
{"time":"2019-12-31T23:59:58","level":"fail","source":"ConsoleApp.Program","event_id":0,"message":"LogError..."}
{"time":"2019-12-31T23:59:59","level":"crit","source":"ConsoleApp.Program","event_id":0,"message":"LogCritical..."}
```

### 3.1.0 Release notes
- Maintenance release
- Updated Microsoft.Extensions.* packages to v3.1.0
- Updated System.Text.Json to v4.7.0

### 3.0.0 Release notes
- Based on Microsoft.Extensions.Logging.Console v3.0.0.
- All original functionality from Microsoft.Extensions.Logging.Console is included.
- Via the new "Format" property one can switch between Default, Json or Systemd formats.
- Replaced Newtonsoft.Json with Microsoft's new high performance System.Text.Json.Utf8JsonWriter.
- Breaking changes: please review your appsettings.json files as there are some renames and new additions.

### 1.0.0 Release notes
- Based on Microsoft.Extensions.Logging.Console v2.2.0.
- Initial JsonConsole.
