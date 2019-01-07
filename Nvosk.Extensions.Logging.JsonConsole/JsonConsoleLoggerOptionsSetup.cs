// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    internal class JsonConsoleLoggerOptionsSetup : ConfigureFromConfigurationOptions<JsonConsoleLoggerOptions>
    {
        public JsonConsoleLoggerOptionsSetup(ILoggerProviderConfiguration<JsonConsoleLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}