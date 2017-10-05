//using System;
//using System.Diagnostics;
//using System.Threading;
//using System.Threading.Tasks;

//namespace W.Threading
//{

//    /// <summary>
//    /// A thread wrapper which makes multi-threading easier
//    /// </summary>
//    /// <typeparam name="TCustomData">The type of custom data to pass to the thread Action</typeparam>
//    public class Thread<TCustomData> : Thread
//    {
//        /// <summary>
//        /// The custom data to pass into the Action
//        /// </summary>
//        public TCustomData CustomData { get; set; }
//        /// <summary>
//        /// The Action to be run on a new thread
//        /// </summary>
//        protected new Action<TCustomData, CancellationToken> Action { get; set; }

//        /// <summary>
//        /// Invokes the Action
//        /// </summary>
//        protected override void InvokeAction(CancellationToken token)
//        {
//            Action?.Invoke(CustomData, token);
//        }

//        /// <summary>
//        /// Constructs a new Thread object
//        /// </summary>
//        /// <param name="action">The Action to be called in the thread</param>
//        /// <param name="onExit">The Action to be called when the thread completes</param>
//        /// <param name="customData">The custom data to be passed into the thread</param>
//        /// <param name="cts">A CancellationTokenSource which can be used to cancel the thread</param>
//        public Thread(Action<TCustomData, CancellationToken> action,
//            Action<bool, Exception> onExit = null,
//            TCustomData customData = default(TCustomData),
//            CancellationTokenSource cts = null)
//            : base(null, onExit, cts)
//        {
//            Action = action;
//            CustomData = customData;
//        }

//        /// <summary>
//        /// Starts a new thread
//        /// </summary>
//        /// <param name="action">Action to call on a thread</param>
//        /// <param name="onExit">Action to call upon comletion.  Executes on the same thread as Action.</param>
//        /// <param name="customData">Custom data to be passed into the thread</param>
//        /// <param name="cts">A CancellationTokenSource which can be used to cancel the operation</param>
//        /// <returns>A new long-running Thread object</returns>
//        public static Thread<TCustomDataType> Create<TCustomDataType>(Action<TCustomDataType, CancellationToken> action, Action<bool, Exception> onExit = null, TCustomDataType customData = default(TCustomDataType), CancellationTokenSource cts = null)
//        {
//            var result = new W.Threading.Thread<TCustomDataType>(action, onExit, customData, cts);
//            return result;
//        }
//    }
//}
