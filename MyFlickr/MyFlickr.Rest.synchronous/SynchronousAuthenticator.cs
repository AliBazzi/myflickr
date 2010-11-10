using System;
using System.Threading;
using MyFlickr.Core;

namespace MyFlickr.Rest.Synchronous
{
    public static class SynchronousAuthenticator
    {
        /// <summary>
        /// Returns a frob and an Authentication Url to be used during authentication. This method call must be signed.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="accessPermission">the permission your want to acquires</param>
        /// <returns></returns>
        public static GetFrobResult GetFrob(this Authenticator authenticator,AccessPermission accessPermission)
        {
            FlickrSynchronousPrmitive<GetFrobResult> FSP = new FlickrSynchronousPrmitive<GetFrobResult>();

            Action<object,EventArgs<GetFrobResult>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP,e);
            authenticator.GetFrobCompleted += new EventHandler<EventArgs<GetFrobResult>>(handler);
            FSP.Token = authenticator.GetFrobAsync(accessPermission);
            FSP.WaitForAsynchronousCall();
            authenticator.GetFrobCompleted -= new EventHandler<EventArgs<GetFrobResult>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the credentials attached to an authentication token. This call must be signed
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="authenticationToken">The authentication token to check</param>
        /// <returns></returns>
        public static CheckTokenResult CheckToken(this Authenticator authenticator, string authenticationToken)
        {
            FlickrSynchronousPrmitive<CheckTokenResult> FSP = new FlickrSynchronousPrmitive<CheckTokenResult>();

            Action<object, EventArgs<CheckTokenResult>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            authenticator.CheckTokenCompleted += new EventHandler<EventArgs<CheckTokenResult>>(handler);
            FSP.Token = authenticator.CheckTokenAsync(authenticationToken);
            FSP.WaitForAsynchronousCall();
            authenticator.CheckTokenCompleted -= new EventHandler<EventArgs<CheckTokenResult>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the full authentication token for a mini-token. This method call must be signed
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="miniToken">The mini-token typed in by a user. It should be 9 digits long. It may optionally contain dashes</param>
        /// <returns></returns>
        public static GetFullTokenResult GetFullToken(this Authenticator authenticator,string miniToken)
        {
            FlickrSynchronousPrmitive<GetFullTokenResult> FSP = new FlickrSynchronousPrmitive<GetFullTokenResult>();

            Action<object, EventArgs<GetFullTokenResult>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            authenticator.GetFullTokenCompleted += new EventHandler<EventArgs<GetFullTokenResult>>(handler);
            FSP.Token = authenticator.GetFullTokenAsync(miniToken);
            FSP.WaitForAsynchronousCall();
            authenticator.GetFullTokenCompleted -= new EventHandler<EventArgs<GetFullTokenResult>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the auth token for the given frob, if one has been attached. This method call must be signed.
        /// This method does not require authentication. 
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="frob">The frob to check</param>
        /// <returns></returns>
        public static GetTokenResult GetToken(this Authenticator authenticator,string frob)
        {
            FlickrSynchronousPrmitive<GetTokenResult> FSP = new FlickrSynchronousPrmitive<GetTokenResult>();

            Action<object, EventArgs<GetTokenResult>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            authenticator.GetTokenCompleted += new EventHandler<EventArgs<GetTokenResult>>(handler);
            FSP.Token = authenticator.GetTokenAsync(frob);
            FSP.WaitForAsynchronousCall();
            authenticator.GetTokenCompleted -= new EventHandler<EventArgs<GetTokenResult>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
