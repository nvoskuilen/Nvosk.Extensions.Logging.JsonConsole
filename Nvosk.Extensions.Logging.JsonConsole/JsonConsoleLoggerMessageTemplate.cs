// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Nvosk.Extensions.Logging.JsonConsole
{
    public class JsonConsoleLoggerMessageTemplate
    {
        public string LogLevel { get; set; }
        public string Name { get; set; }
        public string EventId { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
