using System;
using System.Collections.ObjectModel;

namespace W.Logging
{
    public static partial class Log
    {
        /// <summary>
        /// Maintains a history of Log information
        /// </summary>
        public class LogMessageHistory : W.PropertyHost
        {
            /// <summary>
            /// An individual log message
            /// </summary>
            public class LogMessage
            {
                /// <summary>
                /// The category of the log message
                /// </summary>
                public Property<Log.LogMessageCategory> Category { get; private set; }
                /// <summary>
                /// The logged message
                /// </summary>
                public Property<string> Message { get; private set; }
                /// <summary>
                /// Constructs a new LogMessage
                /// </summary>
                public LogMessage()
                {
                    Category = new Property<Log.LogMessageCategory>(Log.LogMessageCategory.Verbose);
                    Message = new Property<string>("");
                }
            }

            private bool _newestFirst = true;
            private bool _enabled = true;

            /// <summary>
            /// The history of log messages
            /// </summary>
            public ObservableCollection<LogMessage> Messages { get; } = new ObservableCollection<LogMessage>();
            /// <summary>
            /// The maximum number of historical messages to maintain.  When the maximum is reached, the oldest messages are removed as needed.
            /// </summary>
            public Property<int> MaximumNumberOfMessages { get; } = new Property<int>(10000, (p, oldValue, newValue) => { if (newValue < 0) throw new ArgumentOutOfRangeException("MaximumNumberOfMessages"); });
            /// <summary>
            /// If True, log messages will be added to the history.  If False, no history is maintained.
            /// </summary>
            public bool Enabled { get { return _enabled; } set { _enabled = value; if (!_enabled) Truncate(_newestFirst, true); } }

            /// <summary>
            /// Constructs a new LogMessageHistory
            /// </summary>
            /// <param name="newestFirst"></param>
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
