using Founders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreEx
{
    public class ConsoleLogger : Logger
    {
        public ConsoleLogger GetInstance()
        {
            ConsoleLogger cl = new ConsoleLogger();
            return cl;
        }
        public override void DisplayMessage(string message,LogLevel logLevel=LogLevel.INFO, bool writeToLog = false)
        {

        }

        public void DisplayMessage(string message, ConsoleColor background,ConsoleColor foreground, bool writeToLog = false)
        {

        }


    }
}
