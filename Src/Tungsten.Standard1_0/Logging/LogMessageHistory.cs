using System;
using System.Collections.ObjectModel;

namespace W.Logging
{
    public static partial class Log
    {
        public class LogMessageHistory : W.PropertyHost
        {
            public class LogMessage
            {
                public Property<Log.LogMessageCategory> Category { get; private set; }
                public Property<string> Message { get; private set; }

                public LogMessage()
                {
                    Category = new Property<Log.LogMessageCategory>(Log.LogMessageCategory.Verbose);
                    Message = new Property<string>("");
                }
            }

            private bool _newestFirst = true;
            private bool _enabled = true;

            public ObservableCollection<LogMessage> Messages { get; } = new ObservableCollection<LogMessage>();
            public Property<int> MaximumNumberOfMessages { get; } = new Property<int>(10000, (p, oldValue, newValue) => { if (newValue < 0) throw new ArgumentOutOfRangeException("MaximumNumberOfMessages"); });
            public bool Enabled { get { return _enabled; } set { _enabled = value; if (!_enabled) Truncate(_newestFirst, true); } }

            public LogMessageHistory(bool newestFirst = true)
            {
                _newestFirst = newestFirst;
                Log.LogTheMessage += (category, message) =>
                {
                    if (Enabled)
                    {
                        Truncate(_newestFirst);
                        var msg = new LogMessage();
                        msg.Category.Value = category;
                        msg.Message.Value = message;
                        if (_newestFirst)
                            Messages.Insert(0, msg);
                        else
                            Messages.Add(msg);
                    }
                };
            }
            private void Truncate(bool newestFirst = true, bool truncateAll = false)
            {
                if (truncateAll)
                {
                    Messages.Clear();
                }
                else
                {
                    //this should only occur once per addition, so the location of the "newestFirst" condition doesn't really matter (you can't speed it up)
                    while (Messages.Count > MaximumNumberOfMessages.Value)
                    {
                        if (newestFirst)
                            Messages.RemoveAt(Messages.Count - 1);
                        else
                            Messages.RemoveAt(0);
                    }
                }
            }
        }
    }
}
