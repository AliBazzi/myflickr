using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents reflection methods
    /// </summary>
    public class Reflection
    {
        private readonly string apiKey;

        public Reflection(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Returns a list of available flickr API methods.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetMethodsAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetMethodsCompletedEvent(new EventArgs<IEnumerable<Method>>(token,elm.Element("methods").Elements("method").Select(mthd=>new Method(mthd.Value)))), 
                e => this.InvokeGetMethodsCompletedEvent(new EventArgs<IEnumerable<Method>>(token,e)),
                null, new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.reflection.getMethods"));

            return token;
        }

        /// <summary>
        /// Returns information for a given flickr API method.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="methodName">The name of the method to fetch information for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetMethodInfoAsync(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentException("methodName");

            Token token = Token.GenerateToken();

            Uri url = FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetMethodInfoCompletedEvent(new EventArgs<MethodInfo>(token,new MethodInfo(elm))), 
                e => this.InvokeGetMethodInfoCompletedEvent(new EventArgs<MethodInfo>(token,e)), null, 
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.reflection.getMethodInfo"), new Parameter("method_name", methodName));

            return token;
        }

        #region Events
        private void InvokeGetMethodInfoCompletedEvent(EventArgs<MethodInfo> args)
        {
            if (this.GetMethodInfoCompleted != null)
            {
                this.GetMethodInfoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<MethodInfo>> GetMethodInfoCompleted;
        private void InvokeGetMethodsCompletedEvent(EventArgs<IEnumerable<Method>> args)
        {
            if (this.GetMethodsCompleted != null)
            {
                this.GetMethodsCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Method>>> GetMethodsCompleted;
        #endregion
    }

    /// <summary>
    /// represents a Method
    /// </summary>
    public class Method
    {
        /// <summary>
        /// the name of the Method
        /// </summary>
        public string Name { get; private set; }

        internal Method(string name)
        {
            this.Name = name;
        }
    }

    /// <summary>
    /// represents a method info
    /// </summary>
    public class MethodInfo
    {
        internal MethodInfo(XElement element)
        {
            var method = element.Element("method");
            this.Description = method.Element("description").Value;
            this.Name = method.Attribute("name").Value;
            this.NeedsLogin = method.Attribute("needslogin").Value.ToBoolean();
            this.NeedsSigning = method.Attribute("needssigning").Value.ToBoolean();
            this.Description = method.Element("description")!= null ? method.Element("description").Value : null ;
            this.Explanation = method.Element("explanation") != null ? method.Element("explanation").Value : null;
            this.RequiredPermission = AccessPermissionExtensions.GetValue(int.Parse(method.Attribute("requiredperms").Value));
            this.Arguments = element.Element("arguments").Elements("argument").Select(arg => new Argument(arg));
            this.Errors = element.Element("errors").Elements("error").Select(err => new Error(err));
        }

        /// <summary>
        /// the Name of the Method
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Description of the method , could be Null
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// determine whether the Call should be signed or Not
        /// </summary>
        public bool NeedsSigning { get; private set; }

        /// <summary>
        ///  determine whether the User should be logged in before calling of the method or Not
        /// </summary>
        public bool NeedsLogin { get; private set; }

        /// <summary>
        /// the Required Permission that should be granted when calling this Method
        /// </summary>
        public AccessPermission RequiredPermission { get; private set; }

        /// <summary>
        /// the format of the response ,could be Null
        /// </summary>
        public string Response { get; private set; }

        /// <summary>
        /// explanation of the Method , Could Be Null
        /// </summary>
        public string Explanation { get; private set; }

        /// <summary>
        /// Enumerable of parameters of the method
        /// </summary>
        public IEnumerable<Argument> Arguments { get; private set; }

        /// <summary>
        /// Enumerable of possible Errors that could Occur while Calling the Method
        /// </summary>
        public IEnumerable<Error> Errors { get; private set; }
    }

    /// <summary>
    /// represents the argument Info
    /// </summary>
    public class Argument
    {
        internal Argument(XElement element)
        {
            this.Name = element.Attribute("name").Value;
            this.IsOptional = element.Attribute("optional").Value.ToBoolean();
            this.Description = element.Value;
        }

        /// <summary>
        /// the Name of the Argument
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// determine whether the Argument is optional or Not
        /// </summary>
        public bool IsOptional { get; private set; }

        /// <summary>
        /// a Description for the Argument
        /// </summary>
        public string Description { get; private set; }
    }

    /// <summary>
    /// represents an Error info
    /// </summary>
    public class Error
    {
        internal Error(XElement element)
        {
            this.Message = element.Attribute("message").Value;
            this.Code = int.Parse(element.Attribute("code").Value);
            this.Description = element.Value;
        }

        /// <summary>
        /// the Message of the Error
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// the Code of the Error
        /// </summary>
        public int Code { get; private set; }

        /// <summary>
        /// the Description of the Error
        /// </summary>
        public string Description { get; private set; }
    }
}
