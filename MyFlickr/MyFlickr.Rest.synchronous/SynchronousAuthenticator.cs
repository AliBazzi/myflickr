using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Authenticator
    /// </summary>
    public static class SynchronousAuthenticator
    {
        /// <summary>
        /// Returns a frob and an Authentication Url to be used during authentication. This method call must be signed.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="accessPermission">the permission your want to acquires</param>
        /// <returns>GetFrobResult object</returns>
        public static Frob GetFrob(this Authenticator authenticator,AccessPermission accessPermission)
        {
            FlickrSynchronousPrmitive<Frob> FSP = new FlickrSynchronousPrmitive<Frob>();

            Action<object,EventArgs<Frob>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP,e);
            authenticator.GetFrobCompleted += new EventHandler<EventArgs<Frob>>(handler);
            FSP.Token = authenticator.GetFrobAsync(accessPermission);
            FSP.WaitForAsynchronousCall();
            authenticator.GetFrobCompleted -= new EventHandler<EventArgs<Frob>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the credentials attached to an authentication token. This call must be signed
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="authenticationToken">The authentication token to check</param>
        /// <returns>AuthenticationTokens</returns>
        public static AuthenticationTokens CheckToken(this Authenticator authenticator, string authenticationToken)
        {
            FlickrSynchronousPrmitive<AuthenticationTokens> FSP = new FlickrSynchronousPrmitive<AuthenticationTokens>();

            Action<object, EventArgs<AuthenticationTokens>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            authenticator.CheckTokenCompleted += new EventHandler<EventArgs<AuthenticationTokens>>(handler);
            FSP.Token = authenticator.CheckTokenAsync(authenticationToken);
            FSP.WaitForAsynchronousCall();
            authenticator.CheckTokenCompleted -= new EventHandler<EventArgs<AuthenticationTokens>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the full authentication token for a mini-token. This method call must be signed
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="miniToken">The mini-token typed in by a user. It should be 9 digits long. It may optionally contain dashes</param>
        /// <returns>AuthenticationTokens</returns>
        public static AuthenticationTokens GetFullToken(this Authenticator authenticator,string miniToken)
        {
            FlickrSynchronousPrmitive<AuthenticationTokens> FSP = new FlickrSynchronousPrmitive<AuthenticationTokens>();

            Action<object, EventArgs<AuthenticationTokens>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            authenticator.GetFullTokenCompleted += new EventHandler<EventArgs<AuthenticationTokens>>(handler);
            FSP.Token = authenticator.GetFullTokenAsync(miniToken);
            FSP.WaitForAsynchronousCall();
            authenticator.GetFullTokenCompleted -= new EventHandler<EventArgs<AuthenticationTokens>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the auth token for the given frob, if one has been attached. This method call must be signed.
        /// This method does not require authentication. 
        /// </summary>
        /// <param name="authenticator">instance</param>
        /// <param name="frob">The frob to check</param>
        /// <returns>AuthenticationTokens object</returns>
        public static AuthenticationTokens GetToken(this Authenticator authenticator,string frob)
        {
            FlickrSynchronousPrmitive<AuthenticationTokens> FSP = new FlickrSynchronousPrmitive<AuthenticationTokens>();

            Action<object, EventArgs<AuthenticationTokens>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            authenticator.GetTokenCompleted += new EventHandler<EventArgs<AuthenticationTokens>>(handler);
            FSP.Token = authenticator.GetTokenAsync(frob);
            FSP.WaitForAsynchronousCall();
            authenticator.GetTokenCompleted -= new EventHandler<EventArgs<AuthenticationTokens>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
