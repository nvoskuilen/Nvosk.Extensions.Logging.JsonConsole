// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    public class JsonConsoleLoggerOptions
    {
        public bool IncludeScopes { get; set; }
        public JsonConsoleLoggerMessageTemplate MessageTemplate { get; set; }
    }
}