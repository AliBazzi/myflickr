using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Methods that exist in flickr.panda namespace.
    /// </summary>
    public class Panda
    {
        private readonly AuthenticationTokens authtkns;

        /// <summary>
        /// Create Panda Object.
        /// </summary>
        /// <param name="authenticationTokens">Authentication Tokens object.</param>
        public Panda(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authtkns = authenticationTokens;
        }

        /// <summary>
        /// Return a list of Flickr pandas, from whom you can request photos using the flickr.panda.getPhotos API method. 
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetListAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetListCompletedEvent(new EventArgs<IEnumerable<string>>(token, elm.Element("pandas").Elements("panda").Select(pnda => pnda.Value))),
                e => this.InvokeGetListCompletedEvent(new EventArgs<IEnumerable<string>>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token), new Parameter("method", "flickr.panda.getList"));

            return token;
        }

        /// <summary>
        /// Ask the Flickr Pandas for a list of recent public (and "safe") photos. 
        /// This method does not require authentication.
        /// </summary>
        /// <param name="pandaName">The name of the panda to ask for photos from.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosAsync(string pandaName, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(pandaName))
                throw new ArgumentException("pandaName");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PandaPhotosCollection>(token,new PandaPhotosCollection(this.authtkns,elm.Element("photos")))),
                e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PandaPhotosCollection>(token,e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token), new Parameter("method", "flickr.panda.getPhotos"),
                new Parameter("panda_name",pandaName),new Parameter("extras",extras),new Parameter("per_page",perPage),new Parameter("page",page));

            return token;
        }

        private void InvokeGetPhotosCompletedEvent(EventArgs<PandaPhotosCollection> args)
        {
            if (this.GetPhotosCompleted != null)
            {
                this.GetPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PandaPhotosCollection>> GetPhotosCompleted;
        private void InvokeGetListCompletedEvent(EventArgs<IEnumerable<string>> args)
        {
            if (this.GetListCompleted != null)
            {
                this.GetListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<string>>> GetListCompleted;
    }

    /// <summary>
    /// represents a photo collection.
    /// </summary>
    public class PandaPhotosCollection : IEnumerable<Photo>
    {
        private XElement data;

        private readonly AuthenticationTokens authtkns;

        internal PandaPhotosCollection(AuthenticationTokens authtkns, XElement element)
        {
            this.authtkns = authtkns;
            this.data = element;
            this.Total = int.Parse(element.Attribute("total").Value);
            this.Panda = element.Attribute("panda").Value;
            this.Interval = int.Parse(element.Attribute("interval").Value);
            this.LastUpdate = double.Parse(element.Attribute("lastupdate").Value).ToDateTimeFromUnix();
        }

        /// <summary>
        /// the Total Number of photos.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Panda Name .
        /// </summary>
        public string Panda { get; private set; }

        /// <summary>
        /// indicating when the list of photos was generated.
        /// </summary>
        public DateTime LastUpdate { get; private set; }

        /// <summary>
        /// the number of seconds to wait before polling the Flickr API again.
        /// </summary>
        public int Interval { get; private set; }

        /// <summary>
        /// Enumerable of Photo Objects.
        /// </summary>
        public IEnumerable<Photo> Photos { get { return this.data.Elements("photo").Select(ph => new Photo(this.authtkns, ph)); } }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Photo> GetEnumerator()
        {
            foreach (var photo in this.Photos)
                yield return photo;
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
