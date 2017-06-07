namespace W.Net.RPC
{
    /// <summary>
    /// Contains serializable Exception information
    /// </summary>
    /// <remarks>Taken from <see href="http://stackoverflow.com/questions/486460/how-to-serialize-an-exception-object-in-c"/></remarks>
    public class ExceptionInformation
    {
        //public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The string exception message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// The stacktrace of the exception
        /// </summary>
        public string StackTrace { get; set; }
        /// <summary>
        /// Constructs a new ExceptionInformation object
        /// </summary>
        public ExceptionInformation()
        {
            //this.TimeStamp = DateTime.Now;
        }
        /// <summary>
        /// Constructs a new ExceptionInformation object
        /// </summary>
        /// <param name="Message">An exception message used to initialize a new ExceptionInformation object</param>
        public ExceptionInformation(string Message) : this()
        {
            this.Message = Message ?? "";
        }

        /// <summary>
        /// Constructs a new ExceptionInformation object
        /// </summary>
        /// <param name="ex">An exception object used to initialize a new ExceptionInformation object</param>
        public ExceptionInformation(System.Exception ex) : this(ex?.Message)
        {
            this.StackTrace = ex?.StackTrace ?? "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}