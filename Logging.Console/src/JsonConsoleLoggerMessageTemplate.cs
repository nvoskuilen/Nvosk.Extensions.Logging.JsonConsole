// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Nvosk.Extensions.Logging.JsonConsole
{
    public class JsonConsoleLoggerMessageTemplate
    {
        public string TimeStamp { get; set; } = "time";
        public string LogLevel { get; set; } = "level";
        public string Source { get; set; } = "source";
        public string EventId { get; set; } = "event_id";
        public string Message { get; set; } = "message";
        public string Exception { get; set; } = "exception";
    }
}
