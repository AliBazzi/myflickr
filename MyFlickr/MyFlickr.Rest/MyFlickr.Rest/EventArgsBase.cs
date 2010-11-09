using System;

namespace MyFlickr.Core
{
    public abstract class EventArgsBase : EventArgs
    {
        public bool Successful { get; private set; }

        public Exception Excpetion { get; private set; }

        public Token Token { get; private set; }

        internal EventArgsBase(Token token,Exception excpetion)
        {
            this.Token = token;
            this.Excpetion = excpetion;
        }

        internal EventArgsBase(Token token) 
        {
            this.Token = token;
            this.Successful = true;
        }
    }
}
