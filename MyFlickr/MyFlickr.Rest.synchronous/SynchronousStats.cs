using System;
using MyFlickr.Core;
using System.Collections.Generic;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extensions Methods for Stats.
    /// </summary>
    public static class SynchronousStats
    {
        /// <summary>
        /// Get a list of referring domains for a photo.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photoID">The id of the photo to get stats for. If not provided, stats for all photos will be returned.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>DomainsCollection Object.</returns>
        public static DomainsCollection GetPhotoDomains(this Stats stats, string date, string photoID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<DomainsCollection> FSP = new FlickrSynchronousPrmitive<DomainsCollection>();

            Action<object, EventArgs<DomainsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotoDomainsCompleted += new EventHandler<EventArgs<DomainsCollection>>(handler);
            FSP.Token = stats.GetPhotoDomainsAsync(date,photoID, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotoDomainsCompleted -= new EventHandler<EventArgs<DomainsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Get a list of referring domains for a collection.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="collectionID">The id of the collection to get stats for. If not provided, stats for all collections will be returned.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>DomainsCollection Object.</returns>
        public static DomainsCollection GetCollectionDomains(this Stats stats, string date, string collectionID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<DomainsCollection> FSP = new FlickrSynchronousPrmitive<DomainsCollection>();

            Action<object, EventArgs<DomainsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetCollectionDomainsCompleted += new EventHandler<EventArgs<DomainsCollection>>(handler);
            FSP.Token = stats.GetCollectionDomainsAsync(date, collectionID, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetCollectionDomainsCompleted -= new EventHandler<EventArgs<DomainsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Get a list of referring domains for a photostream.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>DomainsCollection Object.</returns>
        public static DomainsCollection GetPhotostreamsDomains(this Stats stats, string date, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<DomainsCollection> FSP = new FlickrSynchronousPrmitive<DomainsCollection>();

            Action<object, EventArgs<DomainsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotostreamsDomainsCompleted += new EventHandler<EventArgs<DomainsCollection>>(handler);
            FSP.Token = stats.GetPhotostreamsDomainsAsync(date, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotostreamsDomainsCompleted -= new EventHandler<EventArgs<DomainsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the number of views on a user's photostream for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <returns>Number of Views.</returns>
        public static int GetPhotostreamStats(this Stats stats, string date)
        {
            FlickrSynchronousPrmitive<int> FSP = new FlickrSynchronousPrmitive<int>();

            Action<object, EventArgs<int>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotostreamStatsCompleted += new EventHandler<EventArgs<int>>(handler);
            FSP.Token = stats.GetPhotostreamStatsAsync(date);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotostreamStatsCompleted -= new EventHandler<EventArgs<int>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the overall view counts for an account.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <returns>Stats Object.</returns>
        public static StatsInfo GetTotalViews(this Stats stats, string date = null)
        {
            FlickrSynchronousPrmitive<StatsInfo> FSP = new FlickrSynchronousPrmitive<StatsInfo>();

            Action<object, EventArgs<StatsInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetTotalViewsCompleted += new EventHandler<EventArgs<StatsInfo>>(handler);
            FSP.Token = stats.GetTotalViewsAsync(date);
            FSP.WaitForAsynchronousCall();
            stats.GetTotalViewsCompleted -= new EventHandler<EventArgs<StatsInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of URLs for text files containing all your stats data (from November 26th 2007 onwards) for the currently auth'd user. Please note, these files will only be available until June 1, 2010 Noon PDT.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <returns>Enumerable of CSV objects.</returns>
        public static IEnumerable<CSV> GetCSVFiles(this Stats stats)
        {
            FlickrSynchronousPrmitive<IEnumerable<CSV>> FSP = new FlickrSynchronousPrmitive<IEnumerable<CSV>>();

            Action<object, EventArgs<IEnumerable<CSV>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetCSVFilesCompleted += new EventHandler<EventArgs<IEnumerable<CSV>>>(handler);
            FSP.Token = stats.GetCSVFilesAsync();
            FSP.WaitForAsynchronousCall();
            stats.GetCSVFilesCompleted -= new EventHandler<EventArgs<IEnumerable<CSV>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the number of views on a collection for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="collectionID">The id of the collection to get stats for.</param>
        /// <returns>Number of Views.</returns>
        public static int GetCollectionStats(this Stats stats , string date, string collectionID)
        {
            FlickrSynchronousPrmitive<int> FSP = new FlickrSynchronousPrmitive<int>();

            Action<object, EventArgs<int>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetCollectionStatsCompleted += new EventHandler<EventArgs<int>>(handler);
            FSP.Token = stats.GetCollectionStatsAsync(date,collectionID);
            FSP.WaitForAsynchronousCall();
            stats.GetCollectionStatsCompleted -= new EventHandler<EventArgs<int>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the number of views on a photoset for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photosetID">The id of the photoset to get stats for.</param>
        /// <returns>Tuple that holds the Number of Views , the Number of Comments.</returns>
        public static Tuple<int, int> GetPhotosetStats(this Stats stats, string date, string photosetID)
        {
            FlickrSynchronousPrmitive<Tuple<int, int>> FSP = new FlickrSynchronousPrmitive<Tuple<int, int>>();

            Action<object, EventArgs<Tuple<int, int>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotosetStatsCompleted += new EventHandler<EventArgs<Tuple<int, int>>>(handler);
            FSP.Token = stats.GetPhotosetStatsAsync(date, photosetID);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotosetStatsCompleted -= new EventHandler<EventArgs<Tuple<int, int>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a user's photostream.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>ReferrersCollection Object.</returns>
        public static ReferrersCollection GetPhotostreamReferrers(this Stats stats, string date, string domain, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<ReferrersCollection> FSP = new FlickrSynchronousPrmitive<ReferrersCollection>();

            Action<object, EventArgs<ReferrersCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotostreamReferrersCompleted += new EventHandler<EventArgs<ReferrersCollection>>(handler);
            FSP.Token = stats.GetPhotostreamReferrersAsync(date, domain,perPage,page);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotostreamReferrersCompleted -= new EventHandler<EventArgs<ReferrersCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photo.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photoID">The id of the photo to get stats for. If not provided, stats for all photos will be returned.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>ReferrersCollection Object.</returns>
        public static ReferrersCollection GetPhotoReferrers(this Stats stats, string date, string domain, string photoID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<ReferrersCollection> FSP = new FlickrSynchronousPrmitive<ReferrersCollection>();

            Action<object, EventArgs<ReferrersCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotoReferrersCompleted += new EventHandler<EventArgs<ReferrersCollection>>(handler);
            FSP.Token = stats.GetPhotoReferrersAsync(date, domain,photoID, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotoReferrersCompleted -= new EventHandler<EventArgs<ReferrersCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a photoset.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="photosetID">The id of the photoset to get stats for. If not provided, stats for all sets will be returned.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>ReferrersCollection Object.</returns>
        public static ReferrersCollection GetPhotosetReferrers(this Stats stats, string date, string domain, string photosetID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<ReferrersCollection> FSP = new FlickrSynchronousPrmitive<ReferrersCollection>();

            Action<object, EventArgs<ReferrersCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotosetReferrersCompleted += new EventHandler<EventArgs<ReferrersCollection>>(handler);
            FSP.Token = stats.GetPhotosetReferrersAsync(date, domain, photosetID, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotosetReferrersCompleted -= new EventHandler<EventArgs<ReferrersCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of referrers from a given domain to a collection.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="domain">The domain to return referrers for. This should be a hostname (eg: "flickr.com") with no protocol or pathname.</param>
        /// <param name="collectionID">The id of the collection to get stats for. If not provided, stats for all collections will be returned.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>ReferrersCollection Object.</returns>
        public static ReferrersCollection GetCollectionReferrers(this Stats stats, string date, string domain, string collectionID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<ReferrersCollection> FSP = new FlickrSynchronousPrmitive<ReferrersCollection>();

            Action<object, EventArgs<ReferrersCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetCollectionReferrersCompleted += new EventHandler<EventArgs<ReferrersCollection>>(handler);
            FSP.Token = stats.GetCollectionReferrersAsync(date, domain, collectionID, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetCollectionReferrersCompleted -= new EventHandler<EventArgs<ReferrersCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of referring domains for a photoset.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photosetID">The id of the photoset to get stats for. If not provided, stats for all sets will be returned.</param>
        /// <param name="perPage">Number of domains to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>DomainsCollection Object.</returns>
        public static DomainsCollection GetPhotosetDomains(this Stats stats, string date,string photosetID = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<DomainsCollection> FSP = new FlickrSynchronousPrmitive<DomainsCollection>();

            Action<object, EventArgs<DomainsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotosetDomainsCompleted += new EventHandler<EventArgs<DomainsCollection>>(handler);
            FSP.Token = stats.GetPhotosetDomainsAsync(date,photosetID, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotosetDomainsCompleted -= new EventHandler<EventArgs<DomainsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the number of views, comments and favorites on a photo for a given date.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day.</param>
        /// <param name="photoID">The id of the photo to get stats for.</param>
        /// <returns>Tuple that holds the Number of Views, the number of  Comments, the Number of Favorites.</returns>
        public static Tuple<int, int, int> GetPhotoStats(this Stats stats, string date, string photoID)
        {
            FlickrSynchronousPrmitive<Tuple<int, int, int>> FSP = new FlickrSynchronousPrmitive<Tuple<int, int, int>>();

            Action<object, EventArgs<Tuple<int, int, int>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPhotoStatsCompleted += new EventHandler<EventArgs<Tuple<int, int, int>>>(handler);
            FSP.Token = stats.GetPhotoStatsAsync(date, photoID);
            FSP.WaitForAsynchronousCall();
            stats.GetPhotoStatsCompleted -= new EventHandler<EventArgs<Tuple<int, int, int>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// List the photos with the most views, comments or favorites.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="stats">Instance.</param>
        /// <param name="date">Stats will be returned for this date. This should be in either be in YYYY-MM-DD or unix timestamp format. A day according to Flickr Stats starts at midnight GMT for all users, and timestamps will automatically be rounded down to the start of the day. If no date is provided, all time view counts will be returned.</param>
        /// <param name="sort">The order in which to sort returned photos. Defaults to views. The possible values are views, comments and favorites. Other sort options are available through flickr.photos.search.</param>
        /// <param name="perPage">Number of referrers to return per page. If this argument is omitted, it defaults to 25. The maximum allowed value is 100.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetPopularPhotos(this Stats stats, string date = null, string sort = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            stats.GetPopularPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = stats.GetPopularPhotosAsync(date, sort, perPage, page);
            FSP.WaitForAsynchronousCall();
            stats.GetPopularPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
