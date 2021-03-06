﻿using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Allows you to Authenticate your Application , or Check if the authentication is still valid.
    /// </summary>
    public class Authenticator
    {
        /// <summary>
        /// Flickr API application key.
        /// </summary>
        public string ApiKey { get; private set; }

        /// <summary>
        /// A shared secret for the api key that is issued by flickr.
        /// </summary>
        public string SharedSecret { get; private set; }

        /// <summary>
        /// Create an Authenticator Object.
        /// </summary>
        /// <param name="apiKey">Your API application key.</param>
        /// <param name="sharedSecret">A shared secret for the api key that is issued by flickr.</param>
        public Authenticator(string apiKey, string sharedSecret)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentException("sharedSecret");
            this.ApiKey = apiKey;
            this.SharedSecret = sharedSecret;
        }

        /// <summary>
        /// Returns a frob and an Authentication Url to be used during authentication. This method call must be signed.
        /// Result are Returned via GetFrobCompleted Event
        /// this Method is an Asynchronous Call
        /// This method does not require authentication.
        /// </summary>
        /// <param name="accessPermission">the permission your want to acquires.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetFrobAsync(AccessPermission accessPermission) 
        {
            accessPermission.ValidateRange();            

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest((element) =>
            {
                var frob = element.Element("frob").Value;
                var url = UriHelper.CalculateRedirectionUrl(this.SharedSecret , new Parameter("api_key", this.ApiKey) ,
                    new Parameter("perms", accessPermission.ToString().ToLower()) , new Parameter("frob", element.Element("frob").Value));
                this.InvokeGetFrobCompleted(new EventArgs<Frob>(token,new Frob(frob,url)));
            }
            , e => this.InvokeGetFrobCompleted(new EventArgs<Frob>(token,e))
            , this.SharedSecret , new Parameter("method","flickr.auth.getFrob"),new Parameter("api_key",this.ApiKey));

            return token;
        }

        /// <summary>
        /// Returns the auth token for the given frob, if one has been attached. This method call must be signed.
        ///  Result are Returned via GetTokenCompleted Event
        /// this Method is an Asynchronous Call
        /// This method does not require authentication.
        /// </summary>
        /// <param name="frob">The frob to check.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetTokenAsync(string frob)
        {
            if (string.IsNullOrEmpty(frob))
                throw new ArgumentException("frob");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(element =>
                {
                    this.InvokeGetTokenCompleted(new EventArgs<AuthenticationTokens>(token ,new AuthenticationTokens
                             (this.ApiKey,this.SharedSecret,element.Element("auth").Element("token").Value
                            , AccessPermissionExtensions.GetValue(element.Element("auth").Element("perms").Value)
                            , element.Element("auth").Element("user").Attribute("nsid").Value
                            , element.Element("auth").Element("user").Attribute("username").Value
                            , element.Element("auth").Element("user").Attribute("fullname").Value)));
                }
            , e => this.InvokeGetTokenCompleted(new EventArgs<AuthenticationTokens>(token,e))
            , this.SharedSecret,new Parameter("method", "flickr.auth.getToken"), new Parameter("api_key", this.ApiKey), new Parameter("frob", frob));

            return token;
        }

        /// <summary>
        /// Returns the credentials attached to an authentication token. This call must be signed
        ///  Result are Returned via CheckTokenCompleted Event
        /// this Method is an Asynchronous Call
        /// This method does not require authentication.
        /// </summary>
        /// <param name="authenticationToken">The authentication token to check.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token CheckTokenAsync(string authenticationToken)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(element =>
            {
                this.InvokeCheckTokenCompleted(new EventArgs<AuthenticationTokens>(token, new AuthenticationTokens
                         (this.ApiKey,this.SharedSecret,element.Element("auth").Element("token").Value
                        , AccessPermissionExtensions.GetValue(element.Element("auth").Element("perms").Value)
                        , element.Element("auth").Element("user").Attribute("nsid").Value
                        , element.Element("auth").Element("user").Attribute("username").Value
                        , element.Element("auth").Element("user").Attribute("fullname").Value)));
            }
            , e => this.InvokeCheckTokenCompleted(new EventArgs<AuthenticationTokens>(token, e))
            , this.SharedSecret, new Parameter("method", "flickr.auth.checkToken"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", authenticationToken));

            return token;
        }

        /// <summary>
        /// Get the full authentication token for a mini-token. This method call must be signed
        ///  Result are Returned via CheckFullTokenCompleted Event
        /// this Method is an Asynchronous Call
        /// This method does not require authentication.
        /// </summary>
        /// <param name="miniToken">The mini-token typed in by a user. It should be 9 digits long. It may optionally contain dashes.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetFullTokenAsync(string miniToken)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(element =>
            {
                this.InvokeGetFullTokenCompleted(new EventArgs<AuthenticationTokens>(token, new AuthenticationTokens
                         (this.ApiKey,this.SharedSecret,element.Element("auth").Element("token").Value
                        , AccessPermissionExtensions.GetValue(element.Element("auth").Element("perms").Value)
                        , element.Element("auth").Element("user").Attribute("nsid").Value
                        , element.Element("auth").Element("user").Attribute("username").Value
                        , element.Element("auth").Element("user").Attribute("fullname").Value)));
            }
            , e => this.InvokeGetFullTokenCompleted(new EventArgs<AuthenticationTokens>(token, e))
            , this.SharedSecret, new Parameter("method", "flickr.auth.getFullToken"), new Parameter("api_key", this.ApiKey), new Parameter("mini_token", miniToken));


            return token;
        }

        #region Events
        private void InvokeGetFullTokenCompleted(EventArgs<AuthenticationTokens> args)
        {
            if (GetFullTokenCompleted !=null)
            {
                GetFullTokenCompleted.Invoke(null, args);
            }
        }
        /// <summary>
        /// Raised when GetFullTokenAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<AuthenticationTokens>> GetFullTokenCompleted;
        private void InvokeCheckTokenCompleted(EventArgs<AuthenticationTokens> args)
        {
            if (CheckTokenCompleted != null)
            {
                CheckTokenCompleted.Invoke(null,args);
            }
        }
        /// <summary>
        /// Raised when CheckTokenAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<AuthenticationTokens>> CheckTokenCompleted;
        private void InvokeGetTokenCompleted(EventArgs<AuthenticationTokens> args)
        {
            if (GetTokenCompleted !=null)
            {
                GetTokenCompleted.Invoke(null, args);
            }
        }
        /// <summary>
        /// Raised when GetTokenAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<AuthenticationTokens>> GetTokenCompleted;
        private void InvokeGetFrobCompleted(EventArgs<Frob> args)
        {
            if (GetFrobCompleted !=null)
            {
                GetFrobCompleted.Invoke(null,args);
            }
        }
        /// <summary>
        /// Raised when GetFrobAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Frob>> GetFrobCompleted;
        #endregion
    }

    /// <summary>
    /// The frob that will be Used to retrieve the UserToken if Authentication from user was successful
    /// </summary>
    public class Frob
    {
        /// <summary>
        /// the URL that you can redirects the user to , To Authenticate the you Application
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// the Value of the frob
        /// </summary>
        public string Value { get; private set; }

        internal Frob(string frob, Uri url)
        {
            this.Value = frob;
            this.Url = url;
        }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Frob Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Frob left, Frob right)
        {
            if (left is Frob)
                return left.Equals(right);
            else if (right is Frob)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Frob Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Frob left, Frob right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determine whether a Given Object Equals this Object.
        /// </summary>
        /// <param name="obj">Instance</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            return obj is Frob && this.Value == ((Frob)obj).Value;
        }

        /// <summary>
        /// Serve as Hash Function for a Particular Type.
        /// </summary>
        /// <returns>Hashed Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
