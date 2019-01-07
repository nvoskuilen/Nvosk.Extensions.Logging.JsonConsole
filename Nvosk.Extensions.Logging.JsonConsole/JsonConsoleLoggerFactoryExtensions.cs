// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Options;
using Nvosk.Extensions.Logging.JsonConsole;
using System;

namespace Microsoft.Extensions.Logging
{
    public static class JsonConsoleLoggerExtensions
    {
        /// <summary>
        /// Adds a console logger named 'JsonConsole' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        public static ILoggingBuilder AddJsonConsole(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, JsonConsoleLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<JsonConsoleLoggerOptions>, JsonConsoleLoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<JsonConsoleLoggerOptions>, LoggerProviderOptionsChangeTokenSource<JsonConsoleLoggerOptions, JsonConsoleLoggerProvider>>());
            return builder;
        }

        /// <summary>
        /// Adds a console logger named 'JsonConsole' to the factory.
        /// </summary>
        /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
        /// <param name="configure"></param>
        public static ILoggingBuilder AddJsonConsole(this ILoggingBuilder builder, Action<JsonConsoleLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddJsonConsole();
            builder.Services.Configure(configure);

            return builder;
        }

        /// <summary>
        /// Adds a console logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(this ILoggerFactory factory)
        {
            return factory.AddJsonConsole(includeScopes: false);
        }

        /// <summary>
        /// Adds a console logger that is enabled for <see cref="LogLevel"/>.Information or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(this ILoggerFactory factory, bool includeScopes)
        {
            factory.AddJsonConsole((n, l) => l >= LogLevel.Information, includeScopes);
            return factory;
        }

        /// <summary>
        /// Adds a console logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(this ILoggerFactory factory, LogLevel minLevel)
        {
            factory.AddJsonConsole(minLevel, includeScopes: false);
            return factory;
        }

        /// <summary>
        /// Adds a console logger that is enabled for <see cref="LogLevel"/>s of minLevel or higher.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(
            this ILoggerFactory factory,
            LogLevel minLevel,
            bool includeScopes)
        {
            factory.AddJsonConsole((category, logLevel) => logLevel >= minLevel, includeScopes);
            return factory;
        }

        /// <summary>
        /// Adds a console logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="filter">The category filter to apply to logs.</param>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(
            this ILoggerFactory factory,
            Func<string, LogLevel, bool> filter)
        {
            factory.AddJsonConsole(filter, includeScopes: false);
            return factory;
        }

        /// <summary>
        /// Adds a console logger that is enabled as defined by the filter function.
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="filter">The category filter to apply to logs.</param>
        /// <param name="includeScopes">A value which indicates whether log scope information should be displayed
        /// in the output.</param>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(
            this ILoggerFactory factory,
            Func<string, LogLevel, bool> filter,
            bool includeScopes)
        {
            factory.AddProvider(new JsonConsoleLoggerProvider(filter, includeScopes));
            return factory;
        }


        /// <summary>
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="settings">The settings to apply to created <see cref="JsonConsoleLogger"/>'s.</param>
        /// <returns></returns>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(
            this ILoggerFactory factory,
            IJsonConsoleLoggerSettings settings)
        {
            factory.AddProvider(new JsonConsoleLoggerProvider(settings));
            return factory;
        }

        /// <summary>
        /// </summary>
        /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/> to use for <see cref="IJsonConsoleLoggerSettings"/>.</param>
        /// <returns></returns>
        [Obsolete("This method is obsolete and will be removed in a future version. The recommended alternative is AddJsonConsole(this ILoggingBuilder builder).")]
        public static ILoggerFactory AddJsonConsole(this ILoggerFactory factory, IConfiguration configuration)
        {
            var settings = new ConfigurationJsonConsoleLoggerSettings(configuration);
            return factory.AddJsonConsole(settings);
        }
    }
}