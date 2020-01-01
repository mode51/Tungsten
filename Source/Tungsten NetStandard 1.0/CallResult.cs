﻿using System;

namespace W
{
    /// <summary>
    /// <para>
    /// Generic class to be used as a return value.  CallResult encapsulates a success/failure, an exception and a return value.
    /// </para>
    /// </summary>
    /// <typeparam name="TResult">The type to be used for the Result member (the return value of the function)</typeparam>
    public class CallResult<TResult> : CallResult
    {
        /// <summary>
        /// The return value
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public CallResult() : this(false, default(TResult), null) { }
        /// <summary>
        /// Constructor accepting an initial Success value
        /// </summary>
        /// <param name="success">The initial Success value</param>
        public CallResult(bool success) : this(success, default(TResult), null) { }
        /// <summary>
        /// Constructor accepting an initial Success value and an initial Result value
        /// </summary>
        /// <param name="success">The initial Success value</param>
        /// <param name="result">The initial Result value</param>
        public CallResult(bool success, TResult result) : this(success, result, null) { }
        /// <summary>
        /// Constructor accepting an initial Success value, an initial Result value and an initial Exception value
        /// </summary>
        /// <param name="success">The initial value for Success</param>
        /// <param name="result">The initial Result value</param>
        /// <param name="e">An exception object, if an exception occurred</param>
        public CallResult(bool success, TResult result, Exception e)
        {
            Success = success;
            Result = result;
            Exception = e;
        }
#if NET461
        /// <summary>
        /// Creates a Tuple from the properties
        /// </summary>
        /// <returns>A Tuple contining the property values</returns>
        /// <remarks>This is essentially a hard-coded Tuple; reflection is not used to obtain the properties.  As a result, properties of child classes will not be in the returned Tuple.</remarks>
        public new Tuple<bool, TResult, Exception> AsTuple()
        {
            return new Tuple<bool, TResult, Exception>(Success, Result, Exception);
        }
#endif
        /// <summary>
        /// Provides a new instance of an uninitialized CallResult&lt;TResult&gt;
        /// </summary>
        public new static CallResult<TResult> Empty => new CallResult<TResult>();
    }

    /// <summary>
    /// A non-generic return value for a function.  CallResult encapsulates a success/failure and an exception.
    /// </summary>
    public class CallResult
    {
        /// <summary>
        /// Set to True if the function succeeds, otherwise False
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Provide exception data to the caller if desired
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Default constructor, initializes Success to false
        /// </summary>
        public CallResult() : this(false) { }
        /// <summary>
        /// Constructor which accepts an initial value for Success
        /// </summary>
        /// <param name="success">The initial value for Success</param>
        public CallResult(bool success)
        {
            Success = success;
        }
        /// <summary>
        /// Constructor which accepts an initial value for Success and an initial value for Exception
        /// </summary>
        /// <param name="success">The initial value for Success</param>
        /// <param name="e">The initial value for Exception</param>
        public CallResult(bool success, Exception e)
        {
            Success = success;
            Exception = e;
        }
#if NET45 || NET461
        /// <summary>
        /// Creates a Tuple from the properties
        /// </summary>
        /// <returns>A Tuple contining the property values</returns>
        /// <remarks>This is essentially a hard-coded Tuple; reflection is not used to obtain the properties.  As a result, properties of child classes will not be in the returned Tuple.</remarks>
        public Tuple<bool, Exception> AsTuple()
        {
            return new Tuple<bool, Exception>(Success, Exception);
        }
#endif
        /// <summary>
        /// Provides a new instance of an uninitialized CallResult
        /// </summary>
        public static CallResult Empty => new CallResult();
    }
}
