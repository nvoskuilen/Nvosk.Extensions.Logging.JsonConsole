// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    [Obsolete("This type is obsolete and will be removed in a future version. The recommended alternative is ConsoleLoggerOptions.")]
    public class JsonConsoleLoggerSettings : IJsonConsoleLoggerSettings
    {
        public IChangeToken ChangeToken { get; set; }

        public bool IncludeScopes { get; set; }

        public IDictionary<string, LogLevel> Switches { get; set; } = new Dictionary<string, LogLevel>();

        public IJsonConsoleLoggerSettings Reload()
        {
            return this;
        }

        public bool TryGetSwitch(string name, out LogLevel level)
        {
            return Switches.TryGetValue(name, out level);
        }
    }
}
