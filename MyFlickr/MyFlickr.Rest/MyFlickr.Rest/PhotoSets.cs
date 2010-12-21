using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of photosets
    /// </summary>
    public class PhotoSetsCollection : IEnumerable<PhotoSet>
    {
        private XElement data;
        private readonly AuthenticationTokens authTkns;

        internal PhotoSetsCollection(AuthenticationTokens authTkns,XElement element)
        {
            this.authTkns = authTkns;
            this.data = element;
            this.PhotosSetsCount = element.Elements("photoset").Count();
        }

        /// <summary>
        /// the number of photosets in this collection
        /// </summary>
        public int PhotosSetsCount { get; private set; }

        /// <summary>
        /// the Photosets Objects
        /// </summary>
        public IEnumerable<PhotoSet> PhotoSets
        {
            get
            {
                return data.Elements("photoset").Select(elm => new PhotoSet(authTkns,elm));
            }
        }

        public IEnumerator<PhotoSet> GetEnumerator()
        {
            foreach (var photoset in this.PhotoSets)
                yield return photoset;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents photosSet Basic Information
    /// </summary>
    public class PhotoSetBasic
    {
        private readonly AuthenticationTokens authTkns;

        internal PhotoSetBasic(AuthenticationTokens authTkns,XElement elm)
        {
            this.authTkns = authTkns;
            this.ID = elm.Attribute("id").Value;
            this.Title = elm.Element("title") != null ? elm.Element("title").Value : elm.Attribute("title").Value ;
            this.Description = elm.Attribute("description") != null ?  elm.Attribute("description").Value : null;
        }

        /// <summary>
        /// the ID that identifies the photoset
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the title of the photoset
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the description of the photoset , could be Null
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Returns the comments for a photoset.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetCommentsListAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeGetCommentsListCompletedEvent(new EventArgs<IEnumerable<Comment>>(token,
                    elm.Element("comments").Elements("comment").Select(comment => new Comment(comment)))),
                e => this.InvokeGetCommentsListCompletedEvent(new EventArgs<IEnumerable<Comment>>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.comments.getList"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photoset_id", this.ID));

            return token;
        }

        /// <summary>
        /// Add a comment to a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="text">Text of the comment.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token AddCommentAsync(string text)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddCommentCompletedEvent(new EventArgs<string>(token,elm.Element("comment").Attribute("id").Value)),
                e => this.InvokeAddCommentCompletedEvent(new EventArgs<string>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("method", "flickr.photosets.comments.addComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photoset_id", this.ID), new Parameter("comment_text", text));

            return token;
        }

        /// <summary>
        /// Delete a photoset comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="commentID">The id of the comment to delete from a photoset.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token DeleteCommentAsync(string commentID)
        {
            if (string.IsNullOrEmpty(commentID))
                throw new ArgumentException("commentID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeDeleteCommentCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeDeleteCommentCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.comments.deleteComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("comment_id", commentID));

            return token;
        }

        /// <summary>
        /// Edit the text of a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="commentID">The id of the comment to edit.</param>
        /// <param name="text">Update the comment to this text.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token EditCommentAsync(string commentID, string text)
        {
            if (string.IsNullOrEmpty(commentID))
                throw new ArgumentException("commentID");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditCommentCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeEditCommentCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.comments.editComment"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("comment_id", commentID), new Parameter("comment_text", text));

            return token;
        }

        /// <summary>
        /// Delete a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token DeleteAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(elm => this.InvokeDeleteCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeDeleteCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), new Parameter("method", "flickr.photosets.delete"), new Parameter("photoset_id", this.ID));

            return token;
        }

        /// <summary>
        /// Add a photo to the end of an existing photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The id of the photo to add to the set.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token AddPhotoAsync(string photoID)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddPhotoCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeAddPhotoCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.addPhoto"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", photoID), new Parameter("photoset_id", this.ID));

            return token;
        }

        /// <summary>
        /// Remove a photo from a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The id of the photo to remove from the set.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token RemovePhotoAsync(string photoID)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemovePhotoCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeRemovePhotoCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.removePhoto"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", photoID), new Parameter("photoset_id", this.ID));

            return token;
        }

        /// <summary>
        /// Remove multiple photos from a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoIDs">list of photo ids to remove from the photoset.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token RemovePhotosAsync(params string[] photoIDs)
        {
            if (photoIDs == null)
                throw new ArgumentNullException("photoIDs");
            
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemovePhotosCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeRemovePhotosCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.removePhotos"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photoset_id", this.ID),
                new Parameter("photo_ids", photoIDs.Aggregate((left,right)=>string.Format("{0},{1}",left,right))));

            return token;
        }

        /// <summary>
        /// Get the list of photos in a set.
        /// This method does not require authentication.
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view a photoset you own.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 500. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="mediaType">Filter results by media type. Possible values are all (default), photos or videos</param>
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotosAsync(string extras = null , Nullable<PrivacyFilter> privacyFilter = null ,
            Nullable<int> perPage = null, Nullable<int> page = null ,Nullable<MediaType> mediaType = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photoset")))),
                e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.getPhotos"),new Parameter("api_key", this.authTkns.ApiKey),new Parameter("auth_token", this.authTkns.Token),
                new Parameter("photoset_id", this.ID), new Parameter("per_page", perPage), new Parameter("page", page), 
                new Parameter("privacy_filter", privacyFilter.HasValue ? (object)(int)privacyFilter : null), new Parameter("media",mediaType));

            return token;
        }

        /// <summary>
        /// Modify the photos in a photoset. Use this method to add, remove and re-order photos.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="primaryPhotoID">The id of the photo to use as the 'primary' photo for the set. This id must also be passed along in photo_ids list argument.</param>
        /// <param name="photosIDs">A comma-delimited list of photo ids to include in the set. They will appear in the set in the order sent. This list must contain the primary photo id. All photos must belong to the owner of the set. This list of photos replaces the existing list. Call flickr.photosets.addPhoto to append a photo to a set.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token EditPhotosAsync(string primaryPhotoID, string[] photosIDs)
        {
            if (string.IsNullOrEmpty(primaryPhotoID))
                throw new ArgumentException("primaryPhotoID");
            if (photosIDs == null)
                throw new ArgumentNullException("photosIDs");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            var uri = FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditPhotosCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeEditPhotosCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.photosets.editPhotos"),new Parameter("photoset_id",this.ID),
                new Parameter("primary_photo_id", primaryPhotoID), new Parameter("auth_token",this.authTkns.Token),
                new Parameter("photo_ids", photosIDs.Aggregate((left, right) => string.Format("{0},{1}", left, right))));

            return token;
        }

        /// <summary>
        /// Modify the meta-data for a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="title">The new title for the photoset.</param>
        /// <param name="description">A description of the photoset. May contain limited html.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token SetMetadataAsync(string title, string description = null)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("title");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm=>this.InvokeSetMetadataCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e=>this.InvokeSetMetadataCompletedEvent(new EventArgs<NoReply>(token,e)),this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), new Parameter("method", "flickr.photosets.editMeta"),
                new Parameter("photoset_id",this.ID),new Parameter("title",title),new Parameter("description",description));

            return token;
        }

        /// <summary>
        /// Set photoset primary photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The id of the photo to set as primary.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token SetPrimaryPhotoAsync(string photoID)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetPrimaryPhotoCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeSetPrimaryPhotoCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), 
                new Parameter("method", "flickr.photosets.setPrimaryPhoto"), new Parameter("photoset_id", this.ID), new Parameter("photo_id", photoID));

            return token;
        }

        /// <summary>
        /// Reorder photos in the photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photosIDs">Ordered, comma-delimited list of photo ids. Photos that are not in the list will keep their original order.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token ReorderPhotosAsync(params string[] photosIDs)
        {
            if (photosIDs == null)
                throw new ArgumentNullException("photosIDs");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeReorderPhotosCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeReorderPhotosCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.photosets.reorderPhotos"), new Parameter("photoset_id", this.ID), 
                new Parameter("photo_ids", photosIDs.Aggregate((left, right) => string.Format("{0},{1}", left, right))));

            return token;
        }

        #region Events
        private void InvokeReorderPhotosCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.ReorderPhotosCompleted != null)
            {
                this.ReorderPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> ReorderPhotosCompleted;
        private void InvokeSetPrimaryPhotoCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetPrimaryPhotoCompleted != null)
            {
                this.SetPrimaryPhotoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> SetPrimaryPhotoCompleted;
        private void InvokeSetMetadataCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetMetadataCompleted != null)
            {
                this.SetMetadataCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> SetMetadataCompleted;
        private void InvokeEditPhotosCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditPhotosCompleted != null)
            {
                this.EditPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> EditPhotosCompleted;
        private void InvokeGetPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPhotosCompleted != null)
            {
                this.GetPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetPhotosCompleted;
        private void InvokeRemovePhotosCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemovePhotosCompleted != null)
            {
                this.RemovePhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> RemovePhotosCompleted;
        private void InvokeRemovePhotoCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemovePhotoCompleted != null)
            {
                this.RemovePhotoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> RemovePhotoCompleted;
        private void InvokeAddPhotoCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddPhotoCompleted != null)
            {
                this.AddPhotoCompleted.Invoke(this,args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> AddPhotoCompleted;
        private void InvokeDeleteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.DeleteCompleted != null)
            {
                this.DeleteCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> DeleteCompleted;
        private void InvokeEditCommentCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditCommentCompleted != null)
            {
                this.EditCommentCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> EditCommentCompleted;
        private void InvokeDeleteCommentCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.DeleteCommentCompleted != null)
            {
                this.DeleteCommentCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<NoReply>> DeleteCommentCompleted;
        private void InvokeAddCommentCompletedEvent(EventArgs<string> args)
        {
            if (this.AddCommentCompleted != null)
            {
                this.AddCommentCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<string>> AddCommentCompleted;
        private void InvokeGetCommentsListCompletedEvent(EventArgs<IEnumerable<Comment>> args)
        {
            if (this.GetCommentsListCompleted != null)
            {
                this.GetCommentsListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Comment>>> GetCommentsListCompleted;
        #endregion

        #region Equality
        public static bool operator ==(PhotoSetBasic left, PhotoSetBasic right)
        {
            if (left is PhotoSetBasic)
                return left.Equals(right);
            else if (right is PhotoSetBasic)
                return right.Equals(left);
            return true;
        }

        public static bool operator !=(PhotoSetBasic left, PhotoSetBasic right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            return obj is PhotoSetBasic && this.ID == ((PhotoSetBasic)obj).ID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// represents a user PhotosSet
    /// </summary>
    public class PhotoSet : PhotoSetBasic
    {
        internal PhotoSet(AuthenticationTokens authTkns,XElement elm)
            :base(authTkns,elm)
        {
            this.Primary = Int64.Parse(elm.Attribute("primary").Value);
            this.Secret = elm.Attribute("secret").Value;
            this.Server = int.Parse(elm.Attribute("server").Value);
            this.Farm = int.Parse(elm.Attribute("farm").Value);
            this.PhotosCount = int.Parse(elm.Attribute("photos").Value);
            this.VideosCount = elm.Attribute("video")!= null ? int.Parse(elm.Attribute("videos").Value) : 0 ;
        }

        /// <summary>
        /// the ID of default photo in this photoset
        /// </summary>
        public Int64 Primary { get; private set; }

        /// <summary>
        /// the secret used to build the URL of the default photo in this photoset
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the number of server the default photo in this set resides on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// the number of servers farm the default photo in this set resides on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the number of photos contained in this photoset
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of videos contained in this photoset
        /// </summary>
        public int VideosCount { get; private set; }

    }

    /// <summary>
    /// represents basic photoset info that are returned when creating a new one
    /// </summary>
    public class PhotoSetToken
    {
        private readonly AuthenticationTokens authtkns;

        /// <summary>
        /// the ID of the photoset that created
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Url of the photoset that created
        /// </summary>
        public Uri Url { get; private set; }

        internal PhotoSetToken(AuthenticationTokens authTkns,XElement element)
        {
            this.authtkns = authTkns;
            this.ID = element.Attribute("id").Value;
            this.Url = new Uri(element.Attribute("url").Value);
        }

        /// <summary>
        /// Get a Photoset Instance
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token CreateInstanceAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeCreateInstanceCompletedEvent(new EventArgs<PhotoSet>(token,new PhotoSet(this.authtkns,elm.Element("photoset")))), 
                e => this.InvokeCreateInstanceCompletedEvent(new EventArgs<PhotoSet>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("method","flickr.photosets.getInfo"), new Parameter("api_key", this.authtkns.ApiKey),
                new Parameter("auth_token",this.authtkns.Token),new Parameter("photoset_id",this.ID));

            return token;
        }

        private void InvokeCreateInstanceCompletedEvent(EventArgs<PhotoSet> args)
        {
            if (this.CreateInstanceCompleted != null)
            {
                this.CreateInstanceCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotoSet>> CreateInstanceCompleted;
    }
}
