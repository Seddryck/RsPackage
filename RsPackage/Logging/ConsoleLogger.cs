using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RsPackage.Logging
{
    class ConsoleLogger : ILogger
    {
        public void Dispose()
        {
            return;
        }

        public void WriteMessage(object sender, MessageEventArgs eventArgs)
        {
            switch (eventArgs.Level)
            {
                case MessageEventArgs.LevelOption.Information:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case MessageEventArgs.LevelOption.Warning:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case MessageEventArgs.LevelOption.Error:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                default:
                    break;
            }
            Console.WriteLine(eventArgs.Message);
        }
    }
}
