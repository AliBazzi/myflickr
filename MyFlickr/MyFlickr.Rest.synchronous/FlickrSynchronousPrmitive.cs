using MyFlickr.Core;
using System.Threading;
using System;

namespace MyFlickr.Rest
{
    internal class FlickrSynchronousPrmitive<Result>
    {
        internal EventArgs<Result> ResultHolder { get; set; }

        internal Token Token { get; set; }

        private EventWaitHandle WaitHandle { get; set; }

        internal void SetWaitHandle() { this.WaitHandle.Set(); }

        internal void WaitForAsynchronousCall() { this.WaitHandle.WaitOne(); }

        internal FlickrSynchronousPrmitive() { this.WaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset); }
    }

    internal static class EventArgsExtensions
    {
        public static T ReturnOrThrow<T>(this EventArgs<T> eventArgs)
        {
            if (eventArgs.Successful)
                return eventArgs.Result;
            throw eventArgs.Excpetion;
        }
    }
    internal static class TokenExtensions
    {
        public static void IfEqualSetValueandResume<Result>(this Token left, FlickrSynchronousPrmitive<Result> FSP,EventArgs<Result> args)
        {
            if (left == FSP.Token)
            {
                FSP.ResultHolder = args;
                FSP.SetWaitHandle();
            }
        }
    }
}
