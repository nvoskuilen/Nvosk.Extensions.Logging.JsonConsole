// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Nvosk.Extensions.Logging.JsonConsole.Internal
{
    public interface IConsole
    {
        void Write(string message);
        void WriteLine(string message);
    }
}