using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace W.Logging
{
    public static partial class Log
    {
        /// <summary>
        /// Maintains a history of Log information
        /// </summary>
        public class MessageHistory : PropertyHost
        {
            /// <summary>
            /// An individual log message
            /// </summary>
            public class LogMessage : PropertyHost
            {
                //private LogMessageCategory _category;
                private string _message;

                ///// <summary>
                ///// The category of the log message
                ///// </summary>
                //public Log.LogMessageCategory Category { get => _category; internal set { SetValue(this, () => _category = value); } }
                /// <summary>
                /// The logged message
                /// </summary>
                public string Message { get => _message; internal set { SetValue(this, () => _message = value); } }
                /// <summary>
                /// Constructs a new LogMessage
                /// </summary>
                public LogMessage()
                {
                    //Category = Log.LogMessageCategory.Verbose;
                    Message = "";
                }
            }

            private bool _newestFirst = true;
            private bool _enabled = true;
            private int _maximumNumberOfMessages = 10000;

            private void Truncate(bool newestFirst = true, bool truncateAll = false)
            {
                if (truncateAll)
                {
                    Messages.Clear();
                }
                else
                {
                    //This should only occur once per addition, so the while is just insurance
                    if (newestFirst)
                        while (Messages.Count > MaximumNumberOfMessages) Messages.RemoveAt(Messages.Count - 1);
                    else
                        while (Messages.Count > MaximumNumberOfMessages) Messages.RemoveAt(0);
                }
            }

            /// <summary>
            /// The history of log messages
            /// </summary>
            public ObservableCollection<LogMessage> Messages { get; } = new ObservableCollection<LogMessage>();
            /// <summary>
            /// The maximum number of historical messages to maintain.  When the maximum is reached, the oldest messages are removed as needed.
            /// </summary>
            public int MaximumNumberOfMessages { get => _maximumNumberOfMessages; private set { SetValue(this, () => _maximumNumberOfMessages = value); } }
            /// <summary>
            /// If True, log messages will be added to the history.  If False, no history is maintained.
            /// </summary>
            public bool Enabled { get { return _enabled; } set { _enabled = value; if (!_enabled) Truncate(_newestFirst, true); } }

            /// <summary>
            /// Constructs a new LogMessageHistory
            /// </summary>
            /// <param name="newestFirst">If True, log messages are inserted at the start of the collection rather than appended to the end</param>
            public MessageHistory(bool newestFirst = true)
            {
                _newestFirst = newestFirst;
                Log.LogTheMessage += (message) =>
                {
                    if (Enabled)
                    {
                        Truncate(_newestFirst);
                        var msg = new LogMessage();
                        msg.Message = message;
                        if (_newestFirst)
                            Messages.Insert(0, msg);
                        else
                            Messages.Add(msg);
                    }
                };
            }
        }
    }
}
