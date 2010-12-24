using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Method that exist in flickr.test namespace
    /// </summary>
    public class Test
    {
        private readonly AuthenticationTokens authtkns;

        /// <summary>
        /// Create Test Object
        /// </summary>
        /// <param name="authenticationTokens">Authentication Tokens</param>
        public Test(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authtkns = authenticationTokens;
        }

        /// <summary>
        /// A testing method which echo's all parameters back in the response.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="parameters">set of parameters </param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token EchoAsync(params Parameter[] parameters)
        {
            Token token = Token.GenerateToken();

            List<Parameter> list = new List<Parameter>(){ new Parameter("method", "flickr.test.echo") , new Parameter("api_key",this.authtkns.ApiKey) };
            if (parameters != null)
                list.AddRange(parameters);

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeEchoCompletedEvent(new EventArgs<IEnumerable<Parameter>>(token,elm.Elements().Select(par=>new Parameter(par.Name.LocalName,par.Value)))), 
                e => this.InvokeEchoCompletedEvent(new EventArgs<IEnumerable<Parameter>>(token,e)), this.authtkns.SharedSecret, list.ToArray());

            return token;
        }

        /// <summary>
        /// Null test.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token NullAsync()
        {
            this.authtkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            Uri url = FlickrCore.IntiateGetRequest(
                elm => this.InvokeNullCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeNullCompletedEvent(new EventArgs<NoReply>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("method", "flickr.test.null"), new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token));

            return token;
        }

        /// <summary>
        /// A testing method which checks if the caller is logged in then returns their username.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token LoginAsync()
        {
            this.authtkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeLoginCompletedEvent(new EventArgs<Tuple<string,string>>
                    (token,new Tuple<string,string>(elm.Element("user").Attribute("id").Value,elm.Element("user").Element("username").Value))), 
                e => this.InvokeLoginCompletedEvent(new EventArgs<Tuple<string,string>>(token,e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("method", "flickr.test.login"), new Parameter("auth_token", this.authtkns.Token));

            return token;
        }

        #region Events
        private void InvokeLoginCompletedEvent(EventArgs<Tuple<string,string>> args)
        {
            if (this.LoginCompleted != null)
            {
                this.LoginCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<Tuple<string,string>>> LoginCompleted;
        private void InvokeNullCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.NullCompleted != null)
            {
                this.NullCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> NullCompleted;
        private void InvokeEchoCompletedEvent(EventArgs<IEnumerable<Parameter>> args)
        {
            if (this.EchoCompleted != null)
            {
                this.EchoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Parameter>>> EchoCompleted;
        #endregion
    }
}
