using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Method that exist in flickr.Licenses namespace
    /// </summary>
    public class Licenses
    {
        private readonly string apiKey;

        /// <summary>
        /// Create Licenses Object
        /// </summary>
        /// <param name="apiKey">API Key of your Application</param>
        public Licenses(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Fetches a list of available photo licenses for Flickr.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetInfoAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetInfoCompletedEvent(new EventArgs<IEnumerable<License>>(token,
                    elm.Element("licenses").Elements("license").Select(lic => new License(lic)))), 
                e => this.InvokeGetInfoCompletedEvent(new EventArgs<IEnumerable<License>>(token,e)), null, 
                new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.photos.licenses.getInfo"));

            return token;
        }

        private void InvokeGetInfoCompletedEvent(EventArgs<IEnumerable<License>> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<License>>> GetInfoCompleted;
    }

    /// <summary>
    /// represents a License Info
    /// </summary>
    public class License
    {
        internal License(XElement element)
        {
            this.ID = int.Parse(element.Attribute("id").Value);
            this.Name = element.Attribute("name").Value;
            this.Url = element.Attribute("url").Value == string.Empty ? null : new Uri(element.Attribute("url").Value);
        }

        /// <summary>
        /// the ID of the License
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// the Name of the License
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// the Url of the License descriptor , Could Be Empty
        /// </summary>
        public Uri Url { get; private set; }
    }
}
