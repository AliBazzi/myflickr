using System;

namespace MyFlickr.Core
{
    public class EventArgs<T>: EventArgs
    {
        public bool Successful { get; private set; }

        public Exception Excpetion { get; private set; }

        public Token Token { get; private set; }

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
