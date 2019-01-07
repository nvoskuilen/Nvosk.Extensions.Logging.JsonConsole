// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Nvosk.Extensions.Logging.JsonConsole.Internal
{
    public class LogJsonConsole : IConsole
    {
        public void Write(string message)
        {
            Console.Out.Write(message);
        }

        public void WriteLine(string message)
        {
            Console.Out.WriteLine(message);
        }
    }
}