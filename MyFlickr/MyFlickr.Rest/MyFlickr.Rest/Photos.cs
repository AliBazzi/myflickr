using System.Collections.Generic;
using System.Xml.Linq;
using System;
using MyFlickr.Core;
using System.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of photos
    /// </summary>
    public class PhotosCollection : IEnumerable<Photo>
    {
        private AuthenticationTokens authTkns;

        private XElement data;

        internal PhotosCollection(AuthenticationTokens authTkns, XElement data)
        {
            this.authTkns = authTkns;
            this.data = data;
            this.Total = int.Parse(data.Attribute("total").Value);
            this.PerPage = int.Parse(data.Attribute("perpage").Value);
            this.Page = int.Parse(data.Attribute("page").Value);
            this.Pages = int.Parse(data.Attribute("pages").Value);
        }

        /// <summary>
        /// the Current page
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the total number of pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of photos per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of photos
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the photos objects
        /// </summary>
        public IEnumerable<Photo> Photos
        {
            get
            {
                return data.Elements("photo").Select(elm => new Photo(this.authTkns, elm));
            }
        }

        public IEnumerator<Photo> GetEnumerator()
        {
            foreach (var photo in this.Photos)
                yield return photo;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a Photo information
    /// </summary>
    public class Photo
    {
        private AuthenticationTokens authTkns;

        internal Photo(AuthenticationTokens authTkns, XElement element)
        {
            this.authTkns = authTkns;
            this.IsFriend = element.Attribute("isfriend").Value.ToBoolean();
            this.IsFamily = element.Attribute("isfamily").Value.ToBoolean();
            this.IsPublic = element.Attribute("ispublic").Value.ToBoolean();
            this.ID = Int64.Parse(element.Attribute("id").Value);
            this.Title = element.Attribute("title").Value;
            this.OwnerID = element.Attribute("owner").Value;
            this.Secret = element.Attribute("secret").Value;
            this.Server = int.Parse(element.Attribute("server").Value);
            this.Farm = int.Parse(element.Attribute("farm").Value);
        }

        /// <summary>
        /// determine if the Photo could be seen only by friends 
        /// </summary>
        public bool IsFriend { get; private set; }

        /// <summary>
        /// determine if the Photo could be seen only by family 
        /// </summary>
        public bool IsFamily { get; private set; }

        /// <summary>
        /// determine if the Photo is Public
        /// </summary>
        public bool IsPublic { get; private set; }

        /// <summary>
        /// The ID of the photo
        /// </summary>
        public Int64 ID { get; private set; }

        /// <summary>
        /// the title of the photo
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the owner ID
        /// </summary>
        public string OwnerID { get; private set; }

        /// <summary>
        /// this string is used to in the building of photo URL
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Server number which the photo is on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// The server Farm number which the photo is on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// Get the URL that leads to the Photo Web page on Flickr
        /// </summary>
        /// <returns></returns>
        public Uri GetPhotoWebPageURL()
        {
            return new Uri(string.Format("http://www.flickr.com/photos/{0}/{1}", this.OwnerID, this.ID));
        }

        /// <summary>
        /// Adds a photo to a user's favorites list.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token AddToFavoriteAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = MyFlickr.Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddTofavoriteCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeAddTofavoriteCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.favorites.add"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));

            return token;
        }

        /// <summary>
        /// Remove a photo from a user's favorites list.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token RemoveFromFavoriteAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemoveFromfavoriteCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeRemoveFromfavoriteCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.favorites.remove"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));

            return token;
        }

        #region Events
        private void InvokeRemoveFromfavoriteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemoveFromFavoriteCompleted != null)
            {
                this.RemoveFromFavoriteCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> RemoveFromFavoriteCompleted;
        private void InvokeAddTofavoriteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddToFavoriteCompleted != null)
            {
                this.AddToFavoriteCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> AddToFavoriteCompleted;
        #endregion
    }

    /// <summary>
    /// represents a Collection of photos that a user appears in
    /// </summary>
    public class PhotosOfUserCollection : IEnumerable<Photo>
    {
        private AuthenticationTokens authTkns;

        private XElement data;

        internal PhotosOfUserCollection(AuthenticationTokens authTkns, XElement element)
        {
            this.authTkns = authTkns;
            this.data = element;
            this.PerPage = int.Parse(element.Attribute("perpage").Value);
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = element.Attribute("pages") != null ? new Nullable<int>(int.Parse(element.Attribute("pages").Value)) : null;
            this.HasNextPage = element.Attribute("has_next_page") != null ? new Nullable<bool>(element.Attribute("has_next_page").Value.ToBoolean()) : null;
            this.Total = element.Attribute("total") != null ? new Nullable<int>(int.Parse(element.Attribute("total").Value)) : null;
        }

        /// <summary>
        /// the number of total pages , could be null when called without signing
        /// </summary>
        public Nullable<int> Pages { get; private set; }

        /// <summary>
        /// the number of current page
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the number of photos per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// determine whether there exists a next page , Could be Null when called with signing
        /// </summary>
        public Nullable<bool> HasNextPage { get; private set; }

        /// <summary>
        /// the Total Number of photos , Could be null when called with no Signing
        /// </summary>
        public Nullable<int> Total { get; private set; }

        /// <summary>
        /// Photos Objects
        /// </summary>
        public IEnumerable<Photo> Photos
        {
            get
            {
                return this.data.Elements("photos").Select(elm => new Photo(this.authTkns, elm));
            }
        }

        public IEnumerator<Photo> GetEnumerator()
        {
            foreach (var photo in this.Photos)
                yield return photo;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}