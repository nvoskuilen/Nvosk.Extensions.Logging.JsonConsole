// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    [Obsolete("Use " + nameof(LoggerExternalScopeProvider) + "instead")]
    public class JsonConsoleLogScope
    {
        private readonly string _name;
        private readonly object _state;

        internal JsonConsoleLogScope(string name, object state)
        {
            _name = name;
            _state = state;
        }

        public JsonConsoleLogScope Parent { get; private set; }

        private static AsyncLocal<JsonConsoleLogScope> _value = new AsyncLocal<JsonConsoleLogScope>();
        public static JsonConsoleLogScope Current
        {
            set
            {
                _value.Value = value;
            }
            get
            {
                return _value.Value;
            }
        }

        public static IDisposable Push(string name, object state)
        {
            var temp = Current;
            Current = new JsonConsoleLogScope(name, state);
            Current.Parent = temp;

            return new DisposableScope();
        }

        public override string ToString()
        {
            return _state?.ToString();
        }

        private class DisposableScope : IDisposable
        {
            public void Dispose()
            {
                Current = Current.Parent;
            }
        }
    }
}
