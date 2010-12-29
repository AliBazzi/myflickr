using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of galleries.
    /// </summary>
    public class GalleriesCollection : IEnumerable<Gallery>
    {
        private XElement data;

        private readonly AuthenticationTokens authtkns;

        internal GalleriesCollection(AuthenticationTokens authTkns,XElement element)
        {
            this.authtkns = authTkns;
            this.data = element;
            this.Total = int.Parse(element.Attribute("total").Value);
            this.PerPage = int.Parse(element.Attribute("per_page").Value);
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
        }

        /// <summary>
        /// the Total Number of Galleries.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Number of Galleries per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the number of pages.
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the current page number.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the Galleries Objects.
        /// </summary>
        public IEnumerable<Gallery> Galleries
        {
            get
            {
                return data.Elements("gallery").Select(elm => new Gallery(this.authtkns,elm));
            }
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Gallery> GetEnumerator()
        {
            foreach (var gallery in this.Galleries)
                yield return gallery;
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
    /// represents a gallery.
    /// </summary>
    public class Gallery
    {
        private readonly AuthenticationTokens authtkns;

        internal Gallery(AuthenticationTokens authTkns,XElement elm)
        {
            this.authtkns = authTkns;
            this.ID = elm.Attribute("id").Value;
            this.Url = new Uri(elm.Attribute("url").Value);
            this.OwnerID = elm.Attribute("owner").Value;
            this.Primary = elm.Attribute("primary_photo_id").Value;
            this.PhotosCount = int.Parse(elm.Attribute("count_photos").Value);
            this.VideosCount = int.Parse(elm.Attribute("count_videos").Value);
            this.Farm = elm.Attribute("primary_photo_farm")!=null ? new Nullable<int>(int.Parse(elm.Attribute("primary_photo_farm").Value)) : null;
            this.Server =elm.Attribute("primary_photo_server")!=null ? new Nullable<int>(int.Parse(elm.Attribute("primary_photo_server").Value)) :  null;
            this.Secret =elm.Attribute("primary_photo_secret")!= null ? elm.Attribute("primary_photo_secret").Value : null;
            this.DateCreated = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(elm.Attribute("date_create").Value));
            this.DateUpdated =new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(elm.Attribute("date_update").Value));
            this.Title = elm.Element("title").Value;
            this.Description = elm.Element("description").Value;
        }

        /// <summary>
        /// the ID of the Gallery.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the URL that leads to the page of the gallery on Flickr.
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// The Owner ID of the Gallery .
        /// </summary>
        public string OwnerID { get; private set; }

        /// <summary>
        /// the ID of the primary photo of the gallery.
        /// </summary>
        public string Primary { get; private set; }

        /// <summary>
        /// the number of photos in this gallery.
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of videos in this gallery.
        /// </summary>
        public int VideosCount { get; private set; }

        /// <summary>
        /// the number of server farm that the primary photo of this gallery resides on.
        /// </summary>
        public Nullable<int> Farm { get; private set; }

        /// <summary>
        /// the number of server that the primary photo of this gallery resides on.
        /// </summary>
        public Nullable<int> Server { get; private set; }

        /// <summary>
        /// the secret used to build the URL of primary photo of this gallery.
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Date and time of the creation of this gallery.
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// the Date and time of the last modification of this gallery.
        /// </summary>
        public DateTime DateUpdated { get; private set; }

        /// <summary>
        /// the Title of this gallery.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Description of this gallery , could Be Null.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Add a photo to a gallery.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The photo ID to add to the gallery.</param>
        /// <param name="comment">A short comment or story to accompany the photo.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token AddPhotoAsync(string photoID, string comment = null)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authtkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();
            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddPhotoCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeAddPhotoCompletedEvent(new EventArgs<NoReply>(token,e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.galleries.addPhoto"),
                new Parameter("photo_id", photoID), new Parameter("comment", comment), new Parameter("gallery_id", this.ID));

            return token;
        }

        /// <summary>
        /// Modify the meta-data for a gallery.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="title">The new title for the gallery.</param>
        /// <param name="description">The new description for the gallery.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token EditMetadataAsync(string title, string description=null)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("title");

            this.authtkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditMetadataCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeEditMetadataCompletedEvent(new EventArgs<NoReply>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.galleries.editMeta"), new Parameter("gallery_id", this.ID), 
                new Parameter("title", title), new Parameter("description", description));

            return token;
        }

        /// <summary>
        /// Edit the comment for a gallery photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The photo ID to edit in the gallery.</param>
        /// <param name="comment">The comment .</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token EditPhotoAsync(string photoID, string comment)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");
            if (comment == null)
                throw new ArgumentNullException("comment");

            this.authtkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditPhotoCompeltedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeEditPhotoCompeltedEvent(new EventArgs<NoReply>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.galleries.editPhoto"), new Parameter("photo_id", photoID), 
                new Parameter("gallery_id", this.ID), new Parameter("comment", comment));

            return token;
        }

        /// <summary>
        /// Return the list of photos for a gallery.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosAsync(string extras = null,Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authtkns, elm.Element("photos")))),
                e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authtkns.SharedSecret,
                new Parameter("method", "flickr.galleries.getPhotos"), new Parameter("api_key", this.authtkns.ApiKey), 
                new Parameter("auth_token", this.authtkns.Token),new Parameter("gallery_id",this.ID),
                new Parameter("photoset_id", this.ID), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Modify the photos in a gallery. Use this method to add, remove and re-order photos.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="primaryPhotoID">The id of the photo to use as the 'primary' photo for the gallery.</param>
        /// <param name="photosIDs">A comma-delimited list of photo ids to include in the gallery. They will appear in the set in the order sent. This list must contain the primary photo id. This list of photos replaces the existing list.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token EditPhotosAsync(string primaryPhotoID, params string[] photosIDs)
        {
            if (string.IsNullOrEmpty(primaryPhotoID))
                throw new ArgumentException("primaryPhotoID");
            if (photosIDs == null)
                throw new ArgumentNullException("photosIDs");

            this.authtkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeEditPhotosCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeEditPhotosCompletedEvent(new EventArgs<NoReply>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),new Parameter("gallery_id",this.ID),
                new Parameter("method", "flickr.galleries.editPhotos"), new Parameter("primary_photo_id", primaryPhotoID), 
                new Parameter("photo_ids", photosIDs.Aggregate((left, right) => string.Format("{0},{1}", left, right))));

            return token;
        }

        #region Events
        private void InvokeEditPhotosCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditPhotosCompleted != null)
            {
                this.EditPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when EditPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> EditPhotosCompleted;
        private void InvokeGetPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPhotosCompleted != null)
            {
                this.GetPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetPhotosCompleted;
        private void InvokeEditPhotoCompeltedEvent(EventArgs<NoReply> args)
        {
            if (this.EditPhotoCompleted != null)
            {
                this.EditPhotoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when EditPhotoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> EditPhotoCompleted;
        private void InvokeEditMetadataCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditMetadataCompleted != null)
            {
                this.EditMetadataCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when EditMetadataAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> EditMetadataCompleted;
        private void InvokeAddPhotoCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddPhotoCompleted != null)
            {
                this.AddPhotoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when AddPhotoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> AddPhotoCompleted;
        #endregion

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Gallery Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Gallery left, Gallery right)
        {
            if (left is Gallery)
                return left.Equals(right);
            else if (right is Gallery)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Gallery Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Gallery left, Gallery right)
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
            return obj is Gallery && this.ID == ((Gallery)obj).ID;
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

    /// <summary>
    /// represents information of a newly created Gallery.
    /// </summary>
    public class GalleryToken
    {
        private readonly AuthenticationTokens authTkns;

        internal GalleryToken(AuthenticationTokens authTkns, XElement element)
        {
            this.authTkns = authTkns;
            this.ID = element.Attribute("id").Value;
            this.Url = element.Attribute("url").Value;
        }

        /// <summary>
        /// the ID of the newly created Gallery.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Partial of the Url of the newly created gallery.
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Get an Instance of the newly created Gallery
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token CreateInstanceAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeCreateInstanceCompletedEvent(new EventArgs<Gallery>(token,new Gallery(this.authTkns,elm.Element("gallery")))), 
                e => this.InvokeCreateInstanceCompletedEvent(new EventArgs<Gallery>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), 
                new Parameter("method", "flickr.galleries.getInfo"), new Parameter("gallery_id",this.ID));

            return token;
        }

        private void InvokeCreateInstanceCompletedEvent(EventArgs<Gallery> args)
        {
            if (this.CreateInstanceCompleted != null)
            {
                this.CreateInstanceCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when CreateInstanceAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Gallery>> CreateInstanceCompleted;

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of GalleryToken Are Equal or Not.
        /// </summary>
        /// <param name="left">instance.</param>
        /// <param name="right">instance.</param>
        /// <returns>True or False.</returns>
        public static bool operator ==(GalleryToken left, GalleryToken right)
        {
            if (left is GalleryToken)
                return left.Equals(right);
            else if (right is GalleryToken)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of GalleryToken Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance.</param>
        /// <param name="right">instance.</param>
        /// <returns>True or False</returns>
        public static bool operator !=(GalleryToken left, GalleryToken right)
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
            return obj is GalleryToken && this.ID == ((GalleryToken)obj).ID;
        }

        /// <summary>
        /// Serve as Hash Function for a Particular Type.
        /// </summary>
        /// <returns>Hashed Value.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}