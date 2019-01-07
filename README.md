# Nvosk.Extensions.Logging.JsonConsole
JsonConsole logger provider implementation for Microsoft.Extensions.Logging.

Outputs json formatted logs to console, ideal for centralised logging with EFK stack.

### Install nuget package
```
PM> Install-Package Nvosk.Extensions.Logging.JsonConsole -Version 1.0.0
```

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "System": "Information",
      "Microsoft": "Information"
    },
    "JsonConsole": {
      "IncludeScopes": false,
      "LogLevel": {
        "Default": "Error"
      },
      "MessageTemplate": {
        "LogLevel": "@level",
        "Name": "@source",
        "EventId": "@event_id",
        "Message": "@message",
        "Exception": "@exception"
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