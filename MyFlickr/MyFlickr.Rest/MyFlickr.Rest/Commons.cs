using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Methods that exist in flickr.commons namespace.
    /// </summary>
    public class Commons
    {
        private readonly string apiKey;

        /// <summary>
        /// Create Commons Object.
        /// </summary>
        /// <param name="apiKey">the API Key of your Application.</param>
        public Commons(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Retrieves a list of the current Commons institutions.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetInstitutionsAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetInstitutionsCompletedEvent(new EventArgs<IEnumerable<Institution>>(token, 
                    elm.Element("institutions").Elements("institution").Select(inst => new Institution(inst)))),
                    e => this.InvokeGetInstitutionsCompletedEvent(new EventArgs<IEnumerable<Institution>>(token, e)), null,
                    new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.commons.getInstitutions"));

            return token;
        }

        private void InvokeGetInstitutionsCompletedEvent(EventArgs<IEnumerable<Institution>> args)
        {
            if (this.GetInstitutionsCompleted != null)
            {
                this.GetInstitutionsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetInstitutionsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Institution>>> GetInstitutionsCompleted;
    }

    /// <summary>
    /// represents Institution Info
    /// </summary>
    public class Institution
    {
        internal Institution(XElement element)
        {
            this.ID = element.Attribute("nsid").Value;
            this.DateLaunched = double.Parse(element.Attribute("date_launch").Value).ToDateTimeFromUnix();
            this.Name = element.Element("name").Value;
            this.Urls = element.Element("urls").Elements("url").Select(url => new URL(url));
        }

        /// <summary>
        /// the ID of the Institution.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Date of Lunching the Institution.
        /// </summary>
        public DateTime DateLaunched { get; private set; }

        /// <summary>
        /// the Name of the Institution.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Urls of Institution.
        /// </summary>
        public IEnumerable<URL> Urls { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Institution Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Institution left, Institution right)
        {
            if (left is Institution)
                return left.Equals(right);
            else if (right is Institution)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Institution Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Institution left, Institution right)
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
            return obj is Institution && this.ID == ((Institution)obj).ID;
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
