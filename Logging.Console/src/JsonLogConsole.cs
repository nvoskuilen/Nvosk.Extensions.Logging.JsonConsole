// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace Nvosk.Extensions.Logging.JsonConsole
{
    internal class JsonLogConsole : IConsole
    {
        private readonly TextWriter _textWriter;

        public JsonLogConsole(bool stdErr = false)
        {
            _textWriter = stdErr ? System.Console.Error : System.Console.Out;
        }

        public void Write(string message, ConsoleColor? background, ConsoleColor? foreground)
        {
            _textWriter.WriteLine(message);
        }

        public void WriteLine(string message, ConsoleColor? background, ConsoleColor? foreground) { }

        public void Flush() { }
    }
}
