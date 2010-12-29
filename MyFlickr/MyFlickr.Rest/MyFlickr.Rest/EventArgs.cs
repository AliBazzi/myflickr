using System;

namespace MyFlickr.Core
{
    /// <summary>
    /// Represents Generic EventArgs.
    /// </summary>
    /// <typeparam name="T">Result Type.</typeparam>
    public class EventArgs<T>: EventArgs
    {
        /// <summary>
        /// determine whether the call was Successful or Not.
        /// </summary>
        public bool Successful { get; private set; }

        /// <summary>
        /// the Exception object if Any Thrown , Could be Null.
        /// </summary>
        public Exception Excpetion { get; private set; }

        /// <summary>
        /// the Token that was returned when Calling the OperantionNameAsync (useful when multiple operation were requested , this token Enables you to track your Asynchronous Call).
        /// </summary>
        public Token Token { get; private set; }

        /// <summary>
        /// the Result of the Asynchronous Call.
        /// </summary>
        public T Result { get; private set; }

        internal EventArgs(Token token,Exception excpetion)
        {
            this.Token = token;
            this.Excpetion = excpetion;
        }

        internal EventArgs(Token token,T result) 
        {
            this.Token = token;
            this.Result = result;
            this.Successful = true;
        }
    }
}
