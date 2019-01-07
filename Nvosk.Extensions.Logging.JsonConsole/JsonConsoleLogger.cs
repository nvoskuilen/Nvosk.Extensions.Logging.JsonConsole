// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Newtonsoft.Json;
using Nvosk.Extensions.Logging.JsonConsole.Internal;
using System;
using System.IO;
using System.Text;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    [Obsolete("This type is obsolete and will be removed in a future version. The recommended alternative is ConsoleLoggerProvider.")]
    public class JsonConsoleLogger : ILogger
    {
        private readonly JsonConsoleLoggerProcessor _queueProcessor;
        private readonly JsonConsoleLoggerMessageTemplate _messageTemplate;
        private Func<string, LogLevel, bool> _filter;

        public JsonConsoleLogger(string name, Func<string, LogLevel, bool> filter, bool includeScopes, JsonConsoleLoggerMessageTemplate messageTemplate)
            : this(name, filter, includeScopes ? new LoggerExternalScopeProvider() : null, new JsonConsoleLoggerProcessor(), messageTemplate)
        {
        }

        public JsonConsoleLogger(string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider, JsonConsoleLoggerMessageTemplate messageTemplate)
            : this(name, filter, scopeProvider, new JsonConsoleLoggerProcessor(), messageTemplate)
        {
        }

        internal JsonConsoleLogger(string name, Func<string, LogLevel, bool> filter, IExternalScopeProvider scopeProvider, JsonConsoleLoggerProcessor loggerProcessor, JsonConsoleLoggerMessageTemplate messageTemplate)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Filter = filter ?? ((category, logLevel) => true);
            ScopeProvider = scopeProvider;
            _queueProcessor = loggerProcessor;
            _messageTemplate = messageTemplate;

            Console = new LogJsonConsole();
        }

        public IConsole Console
        {
            get { return _queueProcessor.Console; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _queueProcessor.Console = value;
            }
        }

        public Func<string, LogLevel, bool> Filter
        {
            get { return _filter; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _filter = value;
            }
        }

        public string Name { get; }

        [Obsolete("Changing this property has no effect. Use " + nameof(JsonConsoleLoggerOptions) + "." + nameof(JsonConsoleLoggerOptions.IncludeScopes) + " instead")]
        public bool IncludeScopes { get; set; }

        internal IExternalScopeProvider ScopeProvider { get; set; }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                var logLevelString = GetLogLevelString(logLevel);
                var level = !string.IsNullOrEmpty(logLevelString) ? logLevelString : null;

                _queueProcessor.EnqueueMessage(ToJson(level, Name, eventId.Id, message, exception, _messageTemplate));
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }

            return Filter(Name, logLevel);
        }

        public IDisposable BeginScope<TState>(TState state) => ScopeProvider?.Push(state) ?? NullScope.Instance;

        private static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return "trce";
                case LogLevel.Debug:
                    return "dbug";
                case LogLevel.Information:
                    return "info";
                case LogLevel.Warning:
                    return "warn";
                case LogLevel.Error:
                    return "fail";
                case LogLevel.Critical:
                    return "crit";
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }

        private void GetScopeInformation(StringBuilder stringBuilder)
        {
            var scopeProvider = ScopeProvider;
            if (scopeProvider != null)
            {
                //var initialLength = stringBuilder.Length;

                //scopeProvider.ForEachScope((scope, state) =>
                //{
                //    var (builder, length) = state;
                //    var first = length == builder.Length;
                //    builder.Append(first ? "=> " : " => ").Append(scope);
                //}, (stringBuilder, initialLength));

                //if (stringBuilder.Length > initialLength)
                //{
                //    stringBuilder.Insert(initialLength, _messagePadding);
                //    stringBuilder.AppendLine();
                //}
            }
        }

        private static string ToJson(string logLevel, string logName, int eventId, string message, Exception exception, JsonConsoleLoggerMessageTemplate messageTemplate)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonWriter = new JsonTextWriter(stringWriter) { CloseOutput = false })
                {
                    jsonWriter.DateFormatString = "O"; // ISO 8601
                    jsonWriter.WriteStartObject();
                    jsonWriter.WritePropertyName(messageTemplate.LogLevel);
                    jsonWriter.WriteValue(logLevel);
                    jsonWriter.WritePropertyName(messageTemplate.Name);
                    jsonWriter.WriteValue(logName);
                    jsonWriter.WritePropertyName(messageTemplate.EventId);
                    jsonWriter.WriteValue(eventId);
                    // TODO: GetScopeInformation
                    if (!string.IsNullOrEmpty(message))
                    {
                        jsonWriter.WritePropertyName(messageTemplate.Message);
                        jsonWriter.WriteValue(message);
                    }
                    if (exception != null)
                    {
                        jsonWriter.WritePropertyName(messageTemplate.Exception);
                        jsonWriter.WriteValue(exception.ToString());
                    }
                    jsonWriter.WriteEndObject();
                }
                stringWriter.Write(Environment.NewLine);
                return stringWriter.ToString();
            }
        }
    }
}