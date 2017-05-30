namespace W.Net.RPC
{
    /// <summary>
    /// Contains serializable Exception information
    /// </summary>
    /// <remarks>Taken from <see href="http://stackoverflow.com/questions/486460/how-to-serialize-an-exception-object-in-c"/></remarks>
    public class ExceptionInformation
    {
        //public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public ExceptionInformation()
        {
            //this.TimeStamp = DateTime.Now;
        }

        public ExceptionInformation(string Message) : this()
        {
            this.Message = Message ?? "";
        }

        public ExceptionInformation(System.Exception ex) : this(ex?.Message)
        {
            this.StackTrace = ex?.StackTrace ?? "";
        }

        public override string ToString()
        {
            return this.Message + this.StackTrace;
        }
    }
}