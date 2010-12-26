using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Methods that exist in flickr.commons namespace
    /// </summary>
    public class Commons
    {
        private readonly string apiKey;

        /// <summary>
        /// Create Commons Object
        /// </summary>
        /// <param name="apiKey">the API Key of your Application</param>
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetInstitutionsAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
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
        /// the ID of the Institution
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Date of Lunching the Institution
        /// </summary>
        public DateTime DateLaunched { get; private set; }

        /// <summary>
        /// the Name of the Institution
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Urls of Institution
        /// </summary>
        public IEnumerable<URL> Urls { get; private set; }
    }
}
