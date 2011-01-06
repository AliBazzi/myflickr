using System;
using MyFlickr.Core;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Represents the Methods that Exist in Stats Namespace.
    /// </summary>
    public class Stats
    {
        private readonly AuthenticationTokens authTkns;

        /// <summary>
        /// Creates an Instance of Stats.
        /// </summary>
        /// <param name="authenticationTokens">authentication Tokens Object.</param>
        public Stats(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authTkns = authenticationTokens;
        }

        /// <summary>
        /// Get a list of referring domains for a photo.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photoID">The id of the photo to get stats for. If not provided, stats for all photos will be returned.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotoDomainsAsync(string date, string photoID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotoDomainsCompletedEvent(new EventArgs<DomainsCollection>(token,new DomainsCollection(elm.Element("domains")))), 
                e => this.InvokeGetPhotoDomainsCompletedEvent(new EventArgs<DomainsCollection>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotoDomains"), new Parameter("date", date), new Parameter("photo_id", photoID), 
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of referring domains for a collection.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="collectionID">The id of the collection to get stats for. If not provided, stats for all collections will be returned.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCollectionDomainsAsync(string date, string collectionID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetCollectionDomainsCompletedEvent(new EventArgs<DomainsCollection>(token,new DomainsCollection(elm.Element("domains")))), 
                e => this.InvokeGetCollectionDomainsCompletedEvent(new EventArgs<DomainsCollection>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getCollectionDomains"), new Parameter("date", date), new Parameter("collection_id", collectionID), 
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of referring domains for a photostream.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotostreamsDomainsAsync(string date, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotostreamsDomainsCompletedEvent(new EventArgs<DomainsCollection>(token, new DomainsCollection(elm.Element("domains")))),
                e => this.InvokeGetPhotostreamsDomainsCompletedEvent(new EventArgs<DomainsCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotostreamDomains"), new Parameter("date", date),
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get the number of views on a user's photostream for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotostreamStatsAsync(string date)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotostreamStatsCompletedEvent(new EventArgs<int>(token, int.Parse(elm.Element("stats").Attribute("views").Value))),
                e => this.InvokeGetPhotostreamStatsCompletedEvent(new EventArgs<int>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotostreamStats"), new Parameter("date", date));

            return token;
        }

        /// <summary>
        /// Get the overall view counts for an account.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetTotalViewsAsync(string date = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetTotalViewsCompletedEvent(new EventArgs<StatsInfo>(token, new StatsInfo(elm.Element("stats")))),
                e => this.InvokeGetTotalViewsCompletedEvent(new EventArgs<StatsInfo>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getTotalViews"), new Parameter("date", date));

            return token;
        }

        /// <summary>
        /// Returns a list of URLs for text files containing all your stats data (from November 26th 2007 onwards) for the currently auth'd user. Please note, these files will only be available until June 1, 2010 Noon PDT.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCSVFilesAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetCSVFilesCompletedEvent(new EventArgs<IEnumerable<CSV>>(token,
                    elm.Element("stats").Element("csvfiles").Elements("csv").Select(csv => new CSV(csv)))),
                e => this.InvokeGetCSVFilesCompletedEvent(new EventArgs<IEnumerable<CSV>>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), new Parameter("method", "flickr.stats.getCSVFiles"));

            return token;
        }

        /// <summary>
        /// Get the number of views on a collection for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="collectionID">The id of the collection to get stats for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCollectionStatsAsync(string date, string collectionID)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(collectionID))
                throw new ArgumentException("collectionID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetCollectionStatsCompletedEvent(new EventArgs<int>(token,int.Parse(elm.Element("stats").Attribute("views").Value))), 
                e => this.InvokeGetCollectionStatsCompletedEvent(new EventArgs<int>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getCollectionStats"), new Parameter("date", date), new Parameter("collection_id", collectionID));

            return token;
        }

        /// <summary>
        /// Get the number of views on a photoset for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photosetID">The id of the photoset to get stats for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosetStatsAsync(string date, string photosetID)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(photosetID))
                throw new ArgumentException("photosetID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(elm => this.InvokeGetPhotosetStatsCompletedEvent(new EventArgs<Tuple<int, int>>(token, new Tuple<int, int>
                (int.Parse(elm.Element("stats").Attribute("views").Value), int.Parse(elm.Element("stats").Attribute("comments").Value)))),
                e => this.InvokeGetPhotosetStatsCompletedEvent(new EventArgs<Tuple<int, int>>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotosetStats"),new Parameter("date", date), new Parameter("photoset_id", photosetID));

            return token;
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a user's photostream.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotostreamReferrersAsync(string date, string domain , Nullable<int> perPage= null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("domain");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotostreamReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token,new ReferrersCollection(elm.Element("domain")))), 
                e => this.InvokeGetPhotostreamReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotostreamReferrers"), new Parameter("date", date), new Parameter("domain", domain), 
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photo.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photoID">The id of the photo to get stats for. If not provided, stats for all photos will be returned.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotoReferrersAsync(string date, string domain,string photoID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("domain");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotoReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token, new ReferrersCollection(elm.Element("domain")))),
                e => this.InvokeGetPhotoReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotoReferrers"), new Parameter("date", date), new Parameter("domain", domain),
                new Parameter("per_page", perPage), new Parameter("page", page),new Parameter("photo_id",photoID));

            return token;
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photoset.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photosetID">The id of the photoset to get stats for. If not provided, stats for all sets will be returned.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosetReferrersAsync(string date, string domain, string photosetID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("domain");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotosetReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token, new ReferrersCollection(elm.Element("domain")))),
                e => this.InvokeGetPhotosetReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotosetReferrers"), new Parameter("date", date), new Parameter("domain", domain),
                new Parameter("per_page", perPage), new Parameter("page", page), new Parameter("photoset_id", photosetID));

            return token;
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a collection.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="collectionID">The id of the collection to get stats for. If not provided, stats for all collections will be returned.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCollectionReferrersAsync(string date, string domain, string collectionID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(domain))
                throw new ArgumentException("domain");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetCollectionReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token, new ReferrersCollection(elm.Element("domain")))),
                e => this.InvokeGetCollectionReferrersCompletedEvent(new EventArgs<ReferrersCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPhotosetReferrers"), new Parameter("date", date), new Parameter("domain", domain),
                new Parameter("per_page", perPage), new Parameter("page", page), new Parameter("collection_id", collectionID));

            return token;
        }

        /// <summary>
        /// Get a list of referring domains for a photoset.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photosetID">The id of the photoset to get stats for. If not provided, stats for all sets will be returned.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosetDomainsAsync(string date,string photosetID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
               elm => this.InvokeGetPhotosetDomainsCompletedEvent(new EventArgs<DomainsCollection>(token, new DomainsCollection(elm.Element("domains")))),
               e => this.InvokeGetPhotosetDomainsCompletedEvent(new EventArgs<DomainsCollection>(token, e)), this.authTkns.SharedSecret,
               new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
               new Parameter("method", "flickr.stats.getPhotosetDomains"), new Parameter("date", date),
               new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get the number of views, comments and favorites on a photo for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photoID">The id of the photo to get stats for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotoStatsAsync(string date, string photoID)
        {
            if (string.IsNullOrEmpty(date))
                throw new ArgumentException("date");
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(elm => this.InvokeGetPhotoStatsCompletedEvent(new EventArgs<Tuple<int, int, int>>(token,
                new Tuple<int, int, int>(int.Parse(elm.Element("stats").Attribute("views").Value), int.Parse(elm.Element("stats").Attribute("comments").Value),
                    int.Parse(elm.Element("stats").Attribute("favorites").Value)))),
                    e => this.InvokeGetPhotoStatsCompletedEvent(new EventArgs<Tuple<int, int, int>>(token, e)), this.authTkns.SharedSecret,
                    new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                    new Parameter("method", "flickr.stats.getPhotoStats"), new Parameter("date", date), new Parameter("photo_id", photoID));

            return token;
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <param name="sort">The order in which to sort returned photos. Defaults to views. The possible values are views, comments and favorites. Other sort options are available through flickr.photos.search.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPopularPhotosAsync(string date = null , string sort = null , Nullable<int> perPage= null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPopularPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos")))), 
                e => this.InvokeGetPopularPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.stats.getPopularPhotos"), new Parameter("sort", sort), new Parameter("date", date), 
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        #region Events
        private void InvokeGetPopularPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPopularPhotosCompleted != null)
            {
                this.GetPopularPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPopularPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetPopularPhotosCompleted;
        private void InvokeGetPhotoStatsCompletedEvent(EventArgs<Tuple<int,int,int>> args)
        {
            if (this.GetPhotoStatsCompleted != null)
            {
                this.GetPhotoStatsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotoStatsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Tuple<int,int,int>>> GetPhotoStatsCompleted;
        private void InvokeGetPhotosetDomainsCompletedEvent(EventArgs<DomainsCollection> args)
        {
            if (this.GetPhotosetDomainsCompleted != null)
            {
                this.GetPhotosetDomainsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosetDomainsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<DomainsCollection>> GetPhotosetDomainsCompleted;
        private void InvokeGetCollectionReferrersCompletedEvent(EventArgs<ReferrersCollection> args)
        {
            if (this.GetCollectionReferrersCompleted != null)
            {
                this.GetCollectionReferrersCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCollectionReferrersAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ReferrersCollection>> GetCollectionReferrersCompleted;
        private void InvokeGetPhotosetReferrersCompletedEvent(EventArgs<ReferrersCollection> args)
        {
            if (this.GetPhotosetReferrersCompleted != null)
            {
                this.GetPhotosetReferrersCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosetReferrersAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ReferrersCollection>> GetPhotosetReferrersCompleted;
        private void InvokeGetPhotoReferrersCompletedEvent(EventArgs<ReferrersCollection> args)
        {
            if (this.GetPhotoReferrersCompleted != null)
            {
                this.GetPhotoReferrersCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotoReferrersAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ReferrersCollection>> GetPhotoReferrersCompleted;
        private void InvokeGetPhotostreamReferrersCompletedEvent(EventArgs<ReferrersCollection> args)
        {
            if (this.GetPhotostreamReferrersCompleted != null)
            {
                this.GetPhotostreamReferrersCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotostreamReferrersAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ReferrersCollection>> GetPhotostreamReferrersCompleted;
        private void InvokeGetPhotosetStatsCompletedEvent(EventArgs<Tuple<int,int>> args)
        {
            if (this.GetPhotosetStatsCompleted != null)
            {
                this.GetPhotosetStatsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosetStatsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Tuple<int,int>>> GetPhotosetStatsCompleted;
        private void InvokeGetCollectionStatsCompletedEvent(EventArgs<int> args)
        {
            if (this.GetCollectionStatsCompleted != null)
            {
                this.GetCollectionStatsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCollectionStatsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<int>> GetCollectionStatsCompleted;
        private void InvokeGetCSVFilesCompletedEvent(EventArgs<IEnumerable<CSV>> args)
        {
            if (this.GetCSVFilesCompleted != null)
            {
                this.GetCSVFilesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCSVFilesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<CSV>>> GetCSVFilesCompleted;
        private void InvokeGetTotalViewsCompletedEvent(EventArgs<StatsInfo> args)
        {
            if (this.GetTotalViewsCompleted != null)
            {
                this.GetTotalViewsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetTotalViewsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<StatsInfo>> GetTotalViewsCompleted;
        private void InvokeGetPhotostreamStatsCompletedEvent(EventArgs<int> args)
        {
            if (this.GetPhotostreamStatsCompleted != null)
            {
                this.GetPhotostreamStatsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotostreamStatsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<int>> GetPhotostreamStatsCompleted;
        private void InvokeGetPhotostreamsDomainsCompletedEvent(EventArgs<DomainsCollection> args)
        {
            if (this.GetPhotostreamsDomainsCompleted != null)
            {
                this.GetPhotostreamsDomainsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotostreamsDomainsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<DomainsCollection>> GetPhotostreamsDomainsCompleted;
        private void InvokeGetCollectionDomainsCompletedEvent(EventArgs<DomainsCollection> args)
        {
            if (this.GetCollectionDomainsCompleted != null)
            {
                this.GetCollectionDomainsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCollectionDomainsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<DomainsCollection>> GetCollectionDomainsCompleted;
        private void InvokeGetPhotoDomainsCompletedEvent(EventArgs<DomainsCollection> args)
        {
            if (this.GetPhotoDomainsCompleted != null)
            {
                this.GetPhotoDomainsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotoDomainsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<DomainsCollection>> GetPhotoDomainsCompleted;
        #endregion
    } 

    /// <summary>
    /// represents a collection of Domains.
    /// </summary>
    public class DomainsCollection : IEnumerable<Domain>
    {
        private XElement data;

        internal DomainsCollection(XElement element)
        {
            this.data = element;
            this.Total = int.Parse(data.Attribute("total").Value);
            this.PerPage = int.Parse(data.Attribute("perpage").Value);
            this.Page = int.Parse(data.Attribute("page").Value);
            this.Pages = int.Parse(data.Attribute("pages").Value);
        }

        /// <summary>
        /// the number of total pages.
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of current page.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the number of domains per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the Total Number of domains .
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Enumerable of Domain Objects.
        /// </summary>
        public IEnumerable<Domain> Domains { get { return this.data.Elements("domain").Select(dom => new Domain(dom)); } }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Domain> GetEnumerator()
        {
            foreach (var domain in this.Domains)
                yield return domain;
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

    /// <summary>
    /// represents domain info.
    /// </summary>
    public class Domain
    {
        internal Domain(XElement element)
        {
            this.Name = element.Attribute("name").Value;
            this.Views = int.Parse(element.Attribute("views").Value);
        }

        /// <summary>
        /// the name of the Domain.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// the number of Views.
        /// </summary>
        public int Views { get; private set; }
    }

    /// <summary>
    /// represents the Info of your Account stats.
    /// </summary>
    public class StatsInfo
    {
        internal StatsInfo(XElement element)
        {
            this.Total = int.Parse(element.Element("total").Attribute("views").Value);
            this.PhotoSets = int.Parse(element.Element("sets").Attribute("views").Value);
            this.Photos = int.Parse(element.Element("photos").Attribute("views").Value);
            this.Photostream = int.Parse(element.Element("photostream").Attribute("views").Value);
            this.Collections = int.Parse(element.Element("collections").Attribute("views").Value);
            this.Galleries = int.Parse(element.Element("galleries").Attribute("views").Value);
        }

        /// <summary>
        /// the Total view of All .
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Total Views of Photos.
        /// </summary>
        public int Photos { get; private set; }

        /// <summary>
        /// the Total Views of Photosets.
        /// </summary>
        public int PhotoSets { get; private set; }

        /// <summary>
        /// the total Views of Photostream.
        /// </summary>
        public int Photostream { get; private set; }

        /// <summary>
        /// the Total Views of Collections.
        /// </summary>
        public int Collections { get; private set; }

        /// <summary>
        /// the Total Views of Galleries.
        /// </summary>
        public int Galleries { get; private set; }
    }

    /// <summary>
    /// represents CSV File Info.
    /// </summary>
    public class CSV
    {
        internal CSV(XElement element)
        {
            this.Url = new Uri(element.Attribute("href").Value);
            this.Type = element.Attribute("type").Value;
        }

        /// <summary>
        /// the Url of the File.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// the Type of the File.
        /// </summary>
        public string Type { get; private set; }
    }

    /// <summary>
    /// represents a Collection of Referrers.
    /// </summary>
    public class ReferrersCollection : IEnumerable<Referrer>
    {
        private XElement data;

        internal ReferrersCollection(XElement element)
        {
            this.data = element;
            this.Total = int.Parse(data.Attribute("total").Value);
            this.PerPage = int.Parse(data.Attribute("perpage").Value);
            this.Page = int.Parse(data.Attribute("page").Value);
            this.Pages = int.Parse(data.Attribute("pages").Value);
        }

        /// <summary>
        /// the number of total pages.
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of current page.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the number of referrers per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the Total Number of referrers .
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Enumerable of Referrer Object.
        /// </summary>
        public IEnumerable<Referrer> Referrers { get { return this.data.Elements("referrer").Select(referrer => new Referrer(referrer)); } }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Referrer> GetEnumerator()
        {
            foreach (var referrer in this.Referrers)
                yield return referrer;
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

    /// <summary>
    /// represents referrer info.
    /// </summary>
    public class Referrer
    {
        internal Referrer(XElement element)
        {
            this.Url = new Uri(element.Attribute("url").Value);
            this.Views = int.Parse(element.Attribute("views").Value);
        }

        /// <summary>
        /// the Url of the Referrer.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// the Number of Views by the Referrer.
        /// </summary>
        public int Views { get; private set; }
    }
}
