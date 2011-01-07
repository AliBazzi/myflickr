using System;
using System.Collections.Generic;
using System.Linq;
using MyFlickr.Core;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Methods that Exist in Tags Namespace.
    /// </summary>
    public class Tags
    {
        private readonly AuthenticationTokens authTkns;

        /// <summary>
        /// Create an Instance of  Tags.
        /// </summary>
        /// <param name="apiKey">the Key of you Flickr Application.</param>
        public Tags(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.authTkns = new AuthenticationTokens(apiKey, null, null, AccessPermission.None, null, null, null);
        }

        /// <summary>
        /// Create an Instance of Tags.
        /// </summary>
        /// <param name="authenticationTokens">authentication Tokens Object.</param>
        public Tags(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authTkns = authenticationTokens;
        }

        /// <summary>
        /// Returns a list of hot tags for the given period.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="period">The period for which to fetch hot tags. Valid values are day and week (defaults to day).</param>
        /// <param name="count">The number of tags to return. Defaults to 20. Maximum allowed value is 200.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetHotListAsync(string period = null, Nullable<int> count = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetHotListCompletedEvent(new EventArgs<IEnumerable<Tuple<int,string>>>
                    (token,elm.Element("hottags").Elements("tag").Select(tag=>new Tuple<int,string>(int.Parse(tag.Attribute("score").Value),tag.Value)))), 
                e => this.InvokeGetHotListCompletedEvent(new EventArgs<IEnumerable<Tuple<int,string>>>(token,e)), null,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.tags.getHotList"), 
                new Parameter("period", period), new Parameter("count", count));

            return token;
        }

        /// <summary>
        /// Gives you a list of tag clusters for the given tag.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tag">The tag to fetch clusters for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetClustersAsync(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentException("tag");
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetClustersCompletedEvent(new EventArgs<IEnumerable<Cluster>>(token,elm.Element("clusters").Elements("cluster").Select(cls=>new Cluster(cls)))), 
                e => this.InvokeGetClustersCompletedEvent(new EventArgs<IEnumerable<Cluster>>(token,e)), null,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.tags.getClusters"), new Parameter("tag", tag));

            return token;
        }

        /// <summary>
        /// Returns the first 24 photos for a given tag cluster.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tag">The tag that this cluster belongs to.</param>
        /// <param name="clusterID">The top three tags for the cluster, separated by dashes (just like the url).</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetClusterPhotosAsync(string tag, string clusterID)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentException("tag");
            if (string.IsNullOrEmpty(clusterID))
                throw new ArgumentException("clusterID");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetClusterPhotosCompletedEvent(new EventArgs<IEnumerable<Photo>>
                    (token,elm.Element("photos").Elements("photo").Select(ph=>new Photo(this.authTkns,ph)))), 
                e => this.InvokeGetClusterPhotosCompletedEvent(new EventArgs<IEnumerable<Photo>>(token,e)), null,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.tags.getClusterPhotos"), 
                new Parameter("tag", tag), new Parameter("cluster_id", clusterID));

            return token;
        }

        /// <summary>
        /// Get the tag list for a given photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photoID">The id of the photo to return tags for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetTagsListofPhotoAsync(string photoID)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetTagsListofPhotoCompletedEvent(new EventArgs<IEnumerable<Tag>>(token,
                    elm.Element("photo").Element("tags").Elements("tag").Select(tg=>new Tag(tg)))),
                e => this.InvokeGetTagsListofPhotoCompletedEvent(new EventArgs<IEnumerable<Tag>>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.tags.getListPhoto"), new Parameter("photo_id", photoID));

            return token;
        }

        /// <summary>
        /// Get the tag list for a given user (or the currently logged in user).
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetTagsListofUserAsync(string userID = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetTagsListofUserCompletedEvent(new EventArgs<IEnumerable<string>>(token,
                    elm.Element("who").Element("tags").Elements("tag").Select(tag=> tag.Value))),
                e => this.InvokeGetTagsListofUserCompletedEvent(new EventArgs<IEnumerable<string>>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.tags.getListUser"), new Parameter("user_id", userID));

            return token;
        }

        /// <summary>
        /// Get the popular tags for a given user (or the currently logged in user).
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <param name="count">Number of popular tags to return. defaults to 10 when this argument is not present.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPopularTagsListofUserAsync(string userID = null, Nullable<int> count = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPopularTagsListofUserCompletedEvent(new EventArgs<IEnumerable<Tuple<int,string>>>(token,
                    elm.Element("who").Element("tags").Elements("tag").Select(tag=>new Tuple<int,string>(int.Parse(tag.Attribute("count").Value),tag.Value)))), 
                e => this.InvokeGetPopularTagsListofUserCompletedEvent(new EventArgs<IEnumerable<Tuple<int,string>>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.tags.getListUserPopular"), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("user_id", userID), new Parameter("count", count));

            return token;
        }

        /// <summary>
        /// Get the raw versions of a given tag (or all tags) for the currently logged-in user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="tag">The tag you want to retrieve all raw versions for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetRawTagsListofUserAsync(string tag = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetRawTagsListofUserCompletedEvent(new EventArgs<IEnumerable<Tuple<string,IEnumerable<string>>>>(token,
                    elm.Element("who").Element("tags").Elements("tag").Select(tg=>new Tuple<string,IEnumerable<string>>(tg.Attribute("clean").Value,tg.Elements("raw").Select(rw=>rw.Value))))), 
                e => this.InvokeGetRawTagsListofUserCompletedEvent(new EventArgs<IEnumerable<Tuple<string,IEnumerable<string>>>>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.tags.getListUserRaw"),new Parameter("tag", tag));

            return token;
        }

        /// <summary>
        /// Returns a list of tags 'related' to the given tag, based on clustered usage analysis.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tag">The tag to fetch related tags for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetRelatedTagsAsync(string tag)
        {
            if (string.IsNullOrEmpty(tag))
                throw new ArgumentException("tag");

            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(elm => this.InvokeGetRelatedTagsCompletedEvent(new EventArgs<Tuple<string, IEnumerable<string>>>(token,
                new Tuple<string, IEnumerable<string>>(elm.Element("tags").Attribute("source").Value, elm.Element("tags").Elements("tag").Select(tg => tg.Value))))
                , e => this.InvokeGetRelatedTagsCompletedEvent(new EventArgs<Tuple<string, IEnumerable<string>>>(token, e)), null,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.tags.getRelated"), new Parameter("tag", tag));

            return token;
        }

        #region Events
        private void InvokeGetRelatedTagsCompletedEvent(EventArgs<Tuple<string,IEnumerable<string>>> args)
        {
            if (this.GetRelatedTagsCompleted != null)
            {
                this.GetRelatedTagsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetRelatedTagsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Tuple<string,IEnumerable<string>>>> GetRelatedTagsCompleted;
        private void InvokeGetRawTagsListofUserCompletedEvent(EventArgs<IEnumerable<Tuple<string,IEnumerable<string>>>> args)
        {
            if (this.GetRawTagsListofUserCompleted != null)
            {
                this.GetRawTagsListofUserCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetRawTagsListofUserAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Tuple<string,IEnumerable<string>>>>> GetRawTagsListofUserCompleted;
        private void InvokeGetPopularTagsListofUserCompletedEvent(EventArgs<IEnumerable<Tuple<int,string>>> args)
        {
            if (this.GetPopularTagsListofUserCompleted != null)
            {
                this.GetPopularTagsListofUserCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPopularTagsListofUserAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Tuple<int,string>>>> GetPopularTagsListofUserCompleted;
        private void InvokeGetTagsListofUserCompletedEvent(EventArgs<IEnumerable<string>> args)
        {
            if (this.GetTagsListofUserCompleted != null)
            {
                this.GetTagsListofUserCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetTagsListofUserAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<string>>> GetTagsListofUserCompleted;
        private void InvokeGetTagsListofPhotoCompletedEvent(EventArgs<IEnumerable<Tag>> args)
        {
            if (this.GetTagsListofPhotoCompleted != null)
            {
                this.GetTagsListofPhotoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetTagsListofPhotoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Tag>>> GetTagsListofPhotoCompleted;
        private void InvokeGetClusterPhotosCompletedEvent(EventArgs<IEnumerable<Photo>> args)
        {
            if (this.GetClusterPhotosCompleted != null)
            {
                this.GetClusterPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetClusterPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Photo>>> GetClusterPhotosCompleted;
        private void InvokeGetClustersCompletedEvent(EventArgs<IEnumerable<Cluster>> args)
        {
            if (this.GetClustersCompleted != null)
            {
                this.GetClustersCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetClustersAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Cluster>>> GetClustersCompleted;
        private void InvokeGetHotListCompletedEvent(EventArgs<IEnumerable<Tuple<int,string>>> args)
        {
            if (this.GetHotListCompleted != null)
            {
                this.GetHotListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetHotListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Tuple<int, string>>>> GetHotListCompleted;
        #endregion
    }

    /// <summary>
    /// represents the Info of Tags Cluster.
    /// </summary>
    public class Cluster
    {
        internal Cluster(XElement element)
        {
            this.Total = int.Parse(element.Attribute("total").Value);
            this.Tags = element.Elements("tag").Select(tag => tag.Value);
        }

        /// <summary>
        /// the Total Number of Tags.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Enumerable of Tags.
        /// </summary>
        public IEnumerable<string> Tags { get; private set; }
    }
}
