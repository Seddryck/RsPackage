using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsrsDeploy
{
    public class MessageEventArgs : EventArgs
    {
        private DateTime time;
        private string message;
        private LevelOption level;

        protected MessageEventArgs(string message, LevelOption level)
        {
            this.message = message;
            this.time = DateTime.Now;
            this.level = level;
        }

        public DateTime Time
        {
            get { return this.time; }
        }

        public string Message
        {
            get { return this.message; }
        }

        public LevelOption Level
        {
            get { return this.level; }
        }

        public enum LevelOption
        {
            Information,
            Warning,
            Error
        }

        public static MessageEventArgs Information(string message)
        {
            return new MessageEventArgs(message, LevelOption.Information);
        }

        public static MessageEventArgs Warning(string message)
        {
            return new MessageEventArgs(message, LevelOption.Warning);
        }

        public static MessageEventArgs Error(string message)
        {
            return new MessageEventArgs(message, LevelOption.Error);
        }
    }
}
