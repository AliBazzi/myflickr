using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;
using MyFlickr.Rest;

namespace MyFlickr.Rest
{
    public static class SynchronousTest
    {
        /// <summary>
        /// A testing method which echo's all parameters back in the response.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="parameters">set of parameters </param>
        /// <returns>Enumerable of Parameters</returns>
        public static IEnumerable<Parameter> Echo(this Test test,params Parameter[] parameters)
        {
            FlickrSynchronousPrmitive<IEnumerable<Parameter>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Parameter>>();

            Action<object, EventArgs<IEnumerable<Parameter>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            test.EchoCompleted += new EventHandler<EventArgs<IEnumerable<Parameter>>>(handler);
            FSP.Token = test.EchoAsync(parameters);
            FSP.WaitForAsynchronousCall();
            test.EchoCompleted -= new EventHandler<EventArgs<IEnumerable<Parameter>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// /// Null test.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        public static NoReply Null(this Test test)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            test.NullCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = test.NullAsync();
            FSP.WaitForAsynchronousCall();
            test.NullCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// A testing method which checks if the caller is logged in then returns their username.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="test"></param>
        /// <returns>Tuple that holds the ID and the Username</returns>
        public static Tuple<string, string> Login(this Test test)
        {
            FlickrSynchronousPrmitive<Tuple<string, string>> FSP = new FlickrSynchronousPrmitive<Tuple<string, string>>();

            Action<object, EventArgs<Tuple<string, string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            test.LoginCompleted += new EventHandler<EventArgs<Tuple<string, string>>>(handler);
            FSP.Token = test.LoginAsync();
            FSP.WaitForAsynchronousCall();
            test.LoginCompleted -= new EventHandler<EventArgs<Tuple<string, string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
