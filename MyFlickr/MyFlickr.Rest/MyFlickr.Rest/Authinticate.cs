using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    public static class Authinticate
    {
        public static Token GenerateAuthinticationUrlAsync(string apiKey , string sharedSecret , AccessPermission accessPermission) 
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentException("sharedSecret");
            accessPermission.ValidateRange();            

            Token token = Token.GenerateToken();

            FlickrCore.IntiateRequest((element) =>
            {
                var frob = element.Element("frob").Value;
                var url = UriHelper.CalculateRedirectionUrl(sharedSecret , new Parameter("api_key", apiKey) ,
                    new Parameter("perms", accessPermission.ToString().ToLower()) , new Parameter("frob", element.Element("frob").Value));
                InvokeGenerateAuthinticationUrlCompleted(new GenerateAuthinticationUrlArgs(frob, url,token));
            }
            , e => InvokeGenerateAuthinticationUrlCompleted(new GenerateAuthinticationUrlArgs(e,token))
            , sharedSecret , new Parameter("method","flickr.auth.getFrob"),new Parameter("api_key", apiKey));

            return token;
        }

        public static Token GetAuthinticationTokenAsync(string frob, string apiKey, string sharedSecret)
        {
            if (string.IsNullOrEmpty(frob))
                throw new ArgumentException("frob");
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (string.IsNullOrEmpty(sharedSecret))
                throw new ArgumentException("sharedSecret");

            Token token = Token.GenerateToken();

            FlickrCore.IntiateRequest(element =>
                {
                    InovkeGetAuthinticationTokenCompleted(new GetAuthinticationTokenArgs(
                        new AuthinticationTokens(element.Element("auth").Element("token").Value
                            , element.Element("auth").Element("perms").Value.GetValue()
                            , element.Element("auth").Element("user").Attribute("nsid").Value
                            , element.Element("auth").Element("user").Attribute("username").Value
                            , element.Element("auth").Element("user").Attribute("fullname").Value),token));
                }
                , e => InovkeGetAuthinticationTokenCompleted(new GetAuthinticationTokenArgs(e,token))
                , sharedSecret,new Parameter("method", "flickr.auth.getToken"), new Parameter("api_key", apiKey), new Parameter("frob", frob));

            return token;
        }

        private static void InovkeGetAuthinticationTokenCompleted(GetAuthinticationTokenArgs args)
        {
            if (GetAuthinticationTokenCompleted !=null)
            {
                GetAuthinticationTokenCompleted.Invoke(null, args);
            }
        }
        public static event EventHandler<GetAuthinticationTokenArgs> GetAuthinticationTokenCompleted;

        private static void InvokeGenerateAuthinticationUrlCompleted(GenerateAuthinticationUrlArgs args)
        {
            if (GenerateAuthinticationUrlCompleted !=null)
            {
                GenerateAuthinticationUrlCompleted.Invoke(null,args);
            }
        }
        public static event EventHandler<GenerateAuthinticationUrlArgs> GenerateAuthinticationUrlCompleted;
    }

    public class GetAuthinticationTokenArgs:EventArgsBase
    {
        public AuthinticationTokens AuthinticationTokens { get; private set; }

        internal GetAuthinticationTokenArgs(Exception excpetion, Token token)
            : base(token, excpetion)
        { }

        internal GetAuthinticationTokenArgs(AuthinticationTokens tokens, Token token)
            :base(token)
        {
            this.AuthinticationTokens = tokens;
        }
    }

    public class GenerateAuthinticationUrlArgs : EventArgsBase
    {
        public Uri Url { get; private set; }

        public string Frob { get; private set; }

        internal GenerateAuthinticationUrlArgs(string frob, Uri url,Token token)
            :base(token)
        {
            this.Frob = frob;
            this.Url = url;
        }

        internal GenerateAuthinticationUrlArgs(Exception exception,Token token)
            :base(token,exception)
        { }
    }
}
