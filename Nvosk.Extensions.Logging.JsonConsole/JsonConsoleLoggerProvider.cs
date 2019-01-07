// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nvosk.Extensions.Logging.JsonConsole.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    // IConsoleLoggerSettings is obsolete
#pragma warning disable CS0618 // Type or member is obsolete

    [ProviderAlias("JsonConsole")]
    public class JsonConsoleLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ConcurrentDictionary<string, JsonConsoleLogger> _loggers = new ConcurrentDictionary<string, JsonConsoleLogger>();

        private readonly Func<string, LogLevel, bool> _filter;
        private IJsonConsoleLoggerSettings _settings;
        private readonly JsonConsoleLoggerProcessor _messageQueue = new JsonConsoleLoggerProcessor();

        private static readonly Func<string, LogLevel, bool> trueFilter = (cat, level) => true;
        private static readonly Func<string, LogLevel, bool> falseFilter = (cat, level) => false;
        private IDisposable _optionsReloadToken;
        private bool _includeScopes;
        private IExternalScopeProvider _scopeProvider;
        private JsonConsoleLoggerMessageTemplate _messageTemplate;

        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is using LoggerFactory to configure filtering and ConsoleLoggerOptions to configure logging options.")]
        public JsonConsoleLoggerProvider(Func<string, LogLevel, bool> filter, bool includeScopes)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            _filter = filter;
            _includeScopes = includeScopes;
        }

        public JsonConsoleLoggerProvider(IOptionsMonitor<JsonConsoleLoggerOptions> options)
        {
            // Filter would be applied on LoggerFactory level
            _filter = trueFilter;
            _optionsReloadToken = options.OnChange(ReloadLoggerOptions);
            ReloadLoggerOptions(options.CurrentValue);
        }

        private void ReloadLoggerOptions(JsonConsoleLoggerOptions options)
        {
            _includeScopes = options.IncludeScopes;
            _messageTemplate = options.MessageTemplate;
            var scopeProvider = GetScopeProvider();
            foreach (var logger in _loggers.Values)
            {
                logger.ScopeProvider = scopeProvider;
            }
        }

        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is using LoggerFactory to configure filtering and ConsoleLoggerOptions to configure logging options")]
        public JsonConsoleLoggerProvider(IJsonConsoleLoggerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _settings = settings;

            if (_settings.ChangeToken != null)
            {
                _settings.ChangeToken.RegisterChangeCallback(OnConfigurationReload, null);
            }
        }

        private void OnConfigurationReload(object state)
        {
            try
            {
                // The settings object needs to change here, because the old one is probably holding on
                // to an old change token.
                _settings = _settings.Reload();

                _includeScopes = _settings?.IncludeScopes ?? false;

                var scopeProvider = GetScopeProvider();
                foreach (var logger in _loggers.Values)
                {
                    logger.Filter = GetFilter(logger.Name, _settings);
                    logger.ScopeProvider = scopeProvider;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error while loading configuration changes.{Environment.NewLine}{ex}");
            }
            finally
            {
                // The token will change each time it reloads, so we need to register again.
                if (_settings?.ChangeToken != null)
                {
                    _settings.ChangeToken.RegisterChangeCallback(OnConfigurationReload, null);
                }
            }
        }

        public ILogger CreateLogger(string name)
        {
            return _loggers.GetOrAdd(name, CreateLoggerImplementation);
        }

        private JsonConsoleLogger CreateLoggerImplementation(string name)
        {
            var includeScopes = _settings?.IncludeScopes ?? _includeScopes;

            return new JsonConsoleLogger(name, GetFilter(name, _settings), includeScopes ? _scopeProvider : null, _messageQueue, _messageTemplate ?? new JsonConsoleLoggerMessageTemplate
            {
                EventId = "EventId",
                Exception = "Exception",
                LogLevel = "LogLevel",
                Message = "Message",
                Name = "Name"
            });
        }

        private Func<string, LogLevel, bool> GetFilter(string name, IJsonConsoleLoggerSettings settings)
        {
            if (_filter != null)
            {
                return _filter;
            }

            if (settings != null)
            {
                foreach (var prefix in GetKeyPrefixes(name))
                {
                    LogLevel level;
                    if (settings.TryGetSwitch(prefix, out level))
                    {
                        return (n, l) => l >= level;
                    }
                }
            }

            return falseFilter;
        }

        private IEnumerable<string> GetKeyPrefixes(string name)
        {
            while (!string.IsNullOrEmpty(name))
            {
                yield return name;
                var lastIndexOfDot = name.LastIndexOf('.');
                if (lastIndexOfDot == -1)
                {
                    yield return "Default";
                    break;
                }
                name = name.Substring(0, lastIndexOfDot);
            }
        }

        private IExternalScopeProvider GetScopeProvider()
        {
            if (_includeScopes && _scopeProvider == null)
            {
                _scopeProvider = new LoggerExternalScopeProvider();
            }
            return _includeScopes ? _scopeProvider : null;
        }

        public void Dispose()
        {
            _optionsReloadToken?.Dispose();
            _messageQueue.Dispose();
        }

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }
    }
#pragma warning restore CS0618
}
