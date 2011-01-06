using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of photos.
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
        /// the Current page.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the total number of pages.
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of photos per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of photos.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the photos objects.
        /// </summary>
        public IEnumerable<Photo> Photos
        {
            get
            {
                if (this.data.Attribute("owner") != null)
                    return data.Elements("photo").Select(elm => new Photo(this.authTkns, elm, this.data.Attribute("owner").Value));
                else
                    return data.Elements("photo").Select(elm => new Photo(this.authTkns, elm));
            }
        }

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

    /// <summary>
    /// represents a Photo information.
    /// </summary>
    public class Photo
    {
        private readonly AuthenticationTokens authTkns;

        internal Photo(AuthenticationTokens authTkns)
        {
            this.authTkns = authTkns; 
        }

        internal Photo(AuthenticationTokens authTkns, XElement element)
            :this(authTkns)
        {
            this.IsFriend = element.Attribute("isfriend")!= null ? new Nullable<bool>(element.Attribute("isfriend").Value.ToBoolean()) : null ;
            this.IsFamily = element.Attribute("isfamily") != null ? new Nullable<bool>(element.Attribute("isfamily").Value.ToBoolean()) : null;
            this.IsPublic = element.Attribute("ispublic") != null ? new Nullable<bool>(element.Attribute("ispublic").Value.ToBoolean()) : null;
            this.ID = element.Attribute("id").Value;
            this.Title = element.Attribute("title") != null ? element.Attribute("title").Value : element.Element("title").Value;
            this.OwnerID = element.Attribute("owner").Value;
            this.Secret = element.Attribute("secret").Value;
            this.Server = int.Parse(element.Attribute("server").Value);
            this.Farm = int.Parse(element.Attribute("farm").Value);
            this.HasComment = element.Attribute("has_comment") != null ? new Nullable<bool>(element.Attribute("has_comment").Value.ToBoolean()) : null;
            this.Comment = element.Element("comment") != null ? element.Element("comment").Value : null;
            this.DateAdded = element.Attribute("dateadded") != null ? new Nullable<DateTime>(double.Parse(element.Attribute("dateadded").Value).ToDateTimeFromUnix()) : null;
            this.OwnerName = element.Attribute("ownername") != null ? element.Attribute("ownername").Value : null;
            this.Stats = element.Element("stats") != null ? new Tuple<int, int, int>
                (int.Parse(element.Element("stats").Attribute("views").Value),
                 int.Parse(element.Element("stats").Attribute("comments").Value),
                 int.Parse(element.Element("stats").Attribute("favorites").Value)) : null;
        }

        internal Photo(AuthenticationTokens authTkns, XElement element, string ownerID)
            :this(authTkns)
        {
            this.OwnerID = ownerID;
            this.ID = element.Attribute("id").Value;
            this.Title = element.Attribute("title").Value;
            this.Secret = element.Attribute("secret").Value;
            this.Server = int.Parse(element.Attribute("server").Value);
            this.Farm = int.Parse(element.Attribute("farm").Value);
            this.IsPrimary = element.Attribute("isprimary").Value.ToBoolean();
        }

        /// <summary>
        /// Tuple that holds the Number of Views, the number of  Comments, the Number of Favorites, could Be Null.
        /// </summary>
        public Tuple<int, int, int> Stats { get; private set; }

        /// <summary>
        /// determine if the Photo is primary in the set or Not , Could be Null.
        /// </summary>
        public Nullable<bool> IsPrimary { get; private set; }

        /// <summary>
        /// determine if the Photo could be seen only by friends, Could be Null.
        /// </summary>
        public Nullable<bool> IsFriend { get; private set; }

        /// <summary>
        /// determine if the Photo could be seen only by family, Could be Null.
        /// </summary>
        public Nullable<bool> IsFamily { get; private set; }

        /// <summary>
        /// determine if the Photo is Public, Could be Null.
        /// </summary>
        public Nullable<bool> IsPublic { get; private set; }

        /// <summary>
        /// determine whether the photo has comment in a given gallery or Not , Could be Null.
        /// </summary>
        public Nullable<bool> HasComment { get; private set; }

        /// <summary>
        /// get the Comment of photo in a given gallery , Could Be Null.
        /// </summary>
        public string Comment { get; private set; }

        /// <summary>
        /// The ID of the photo.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the title of the photo.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the owner ID.
        /// </summary>
        public string OwnerID { get; private set; }

        /// <summary>
        /// this string is used in the building of photo URL.
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Server number which the photo is on.
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// The server Farm number which the photo is on.
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the date when this photo was added to the group , Could be Null.
        /// </summary>
        public Nullable<DateTime> DateAdded { get; private set; }

        /// <summary>
        /// the name of the Owner of the Photo , Could be Null.
        /// </summary>
        public string OwnerName { get; private set; }

        /// <summary>
        /// Get the URL that leads to the Photo Web page on Flickr.
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token AddToFavoriteAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Token.GenerateToken();

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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
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

        /// <summary>
        /// Retrieves a list of EXIF/TIFF/GPS tags for a given photo. The calling user must have permission to view the photo.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetExifAsync()
        {
            Token token = Core.Token.GenerateToken();
            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetExifCompletedEvent(new EventArgs<IEnumerable<Exif>>(token,elm.Element("photo").Elements("exif").Select(exif=>new Exif(exif)))),
                e => this.InvokeGetExifCompletedEvent(new EventArgs<IEnumerable<Exif>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getExif"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));
            return token;
        }

        /// <summary>
        /// Returns the list of people who have favorited a given photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="page">Number of users to return per page. If this argument is omitted, it defaults to 10. The maximum allowed value is 50.</param>
        /// <param name="perPage">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetFavoritesAsync(Nullable<int> page = null, Nullable<int> perPage = null)
        {
            Token token = Core.Token.GenerateToken();
            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetFavoritesCompletedEvent(new EventArgs<IEnumerable<Person>>(token, elm.Element("photo").Elements("person").Select(person => new Person(person)))),
                e => this.InvokeGetFavoritesCompletedEvent(new EventArgs<IEnumerable<Person>>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getFavorites"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID),
                new Parameter("page",page),new Parameter("per_page",perPage));
            
            return token;
        }

        /// <summary>
        /// Post the Photo to a Blog.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="title">The blog post title</param>
        /// <param name="description">The blog post body</param>
        /// <param name="blogPassword">The password for the blog (used when the blog does not have a stored password).</param>
        /// <param name="blogID">The id of the blog to post to.</param>
        /// <param name="service">A Flickr supported blogging service. Instead of passing a blog id you can pass a service id and we'll post to the first blog of that service we find.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token PostPhotoToBlogAsync(string title, string description, string blogPassword = null, Nullable<Int64> blogID = null, string service = null)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("title");
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("description");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokePostPhotoToBlogCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokePostPhotoToBlogCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.blogs.postPhoto"), new Parameter("api_key",this.authTkns.ApiKey), new Parameter("auth_token",this.authTkns.Token),
                new Parameter("title",title), new Parameter("description",description), new Parameter("photo_id",this.ID), new Parameter("blog_id",blogID),
                new Parameter("blog_password",blogPassword), new Parameter("service",service));
            
            return token;
        }

        /// <summary>
        /// Get information about a photo. The calling user must have permission to view the photo.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetInfoAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetInfoCompletedEvent(new EventArgs<PhotoInfo>(token,new PhotoInfo(elm.Element("photo")))),
                e => this.InvokeGetInfoCompletedEvent(new EventArgs<PhotoInfo>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getInfo"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));

            return token;
        }

        /// <summary>
        /// Add tags to a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="tags">The tags to add to the photo.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token AddTagAsync(string tags)
        {
            if (string.IsNullOrEmpty(tags))
                throw new ArgumentException("tag");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            Uri url = FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddTagCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeAddTagCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.addTags"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("tags", tags));

            return token;
        }

        /// <summary>
        /// Remove a tag from a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="tagID">The tag to remove from the photo. This parameter should contain a tag id, as returned by flickr.photos.getInfo.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token RemoveTagAsync(string tagID)
        {
            if (string.IsNullOrEmpty(tagID))
                throw new ArgumentException("tagID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemoveTagCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeRemoveTagCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.removeTag"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("tag_id", tagID));

            return token;
        }

        /// <summary>
        /// Get permissions for a photo.
        /// This method requires authentication with 'read' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPermissionsAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPermissionsCompletedEvent(new EventArgs<Permissions>(token,new Permissions(elm.Element("perms")))), 
                e => this.InvokeGetPermissionsCompletedEvent(new EventArgs<Permissions>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getPerms"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("photo_id", this.ID)
                ,new Parameter("auth_token",this.authTkns.Token));

            return token;
        }

        /// <summary>
        /// Set permissions for a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="isPublic">true to set the photo to public, false to set it to private.</param>
        /// <param name="isFriend">true to make the photo visible to friends when private, false to not.</param>
        /// <param name="isFamily">true to make the photo visible to family when private, false to not.</param>
        /// <param name="commentPermission">who can add comments to the photo and it's notes.</param>
        /// <param name="addMetadataPermission">who can add notes and tags to the photo.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SetPermissionsAsync(bool isPublic,bool isFriend , bool isFamily,CommentPermission commentPermission, AddMetadataPermission addMetadataPermission)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetPermissionsCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeSetPermissionsCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.setPerms"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("photo_id", this.ID), new Parameter("is_friend", isFriend ? 1 : 0), new Parameter("is_family", isFamily ? 1 : 0),
                new Parameter("is_public", isPublic ? 1 : 0), new Parameter("perm_comment", (int)commentPermission),
                new Parameter("perm_addmeta", (int)addMetadataPermission));

            return token;
        }

        /// <summary>
        /// Set the meta information for a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="title">The title for the photo.</param>
        /// <param name="description">The description for the photo.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SetMetaAsync(string title, string description)
        {
            if (title == null)
                throw new ArgumentNullException("title");
            if (description == null)
                throw new ArgumentNullException("description");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetMetaCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeSetMetaCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.setMeta"), new Parameter("auth_token", this.authTkns.Token), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("photo_id", this.ID), new Parameter("title", title), new Parameter("description", description));

            return token;
        }

        /// <summary>
        /// Set the safety level of a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="safetyLevel">The safety level of the photo.</param>
        /// <param name="isHidden">Whether or not to additionally hide the photo from public searches. Must be either True for Yes or false for No.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SetSafetyLevelAsync(SafetyLevel safetyLevel , Nullable<bool> isHidden = null) 
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetSafetyLevelCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeSetSafetyLevelCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method","flickr.photos.setSafetyLevel"), new Parameter("api_key",this.authTkns.ApiKey),
                new Parameter("auth_token",this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("safety_level", (int)safetyLevel),
                new Parameter("hidden", isHidden.HasValue ? (object)(isHidden.Value ? 1 : 0) : null));

            return token;
        }

        /// <summary>
        /// Set one or both of the dates for a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="datePosted">The date the photo was uploaded to flickr . more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="dateTaken">The date the photo was taken . more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="dateTakenGranularity">The granularity of the date the photo was taken. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SetDatesAsync(string datePosted = null, string dateTaken = null, string dateTakenGranularity = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetDatesCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeSetDatesCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.setDates"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("photo_id", this.ID), new Parameter("date_posted", datePosted), new Parameter("date_taken", dateTaken),
                new Parameter("date_taken_granularity", dateTakenGranularity));

            return token;
        }

        /// <summary>
        /// Set the content type of a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="contentType">The content type of the photo. Must be one of: Photo, Screenshot, andOther , Only .</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SetContentTypeAsync(ContentType contentType)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetContentTypeCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeSetContentTypeCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.setContentType"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("content_type", (int)contentType));

            return token;
        }

        /// <summary>
        /// Delete a photo from flickr.
        /// This method requires authentication with 'delete' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token DeleteAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Delete);
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeDeleteCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeDeleteCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.delete"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));

            return token;
        }

        /// <summary>
        /// Returns next and previous photos for a photo in a photostream.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetContextAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetContextCompletedEvent(new EventArgs<PhotoContext>(token,new PhotoContext(this.authTkns,elm))),
                e => this.InvokeGetContextCompletedEvent(new EventArgs<PhotoContext>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getContext"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));

            return token;
        }

        /// <summary>
        /// Returns next and previous photos for a photo in a set.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photosetID">The id of the photoset for which to fetch the photo's context.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetContextinSetAsync(string photosetID)
        {
            if (string.IsNullOrEmpty(photosetID))
                throw new ArgumentException("photosetID");

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetContextCompletedEvent(new EventArgs<PhotoContext>(token, new PhotoContext(this.authTkns, elm))),
                e => this.InvokeGetContextCompletedEvent(new EventArgs<PhotoContext>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photosets.getContext"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID),new Parameter("photoset_id",photosetID));

            return token;
        }

        /// <summary>
        /// Returns next and previous photos for a photo in a group pool.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="GroupID">The nsid of the group who's pool to fetch the photo's context for.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetContextAsync(string GroupID)
        {
            if (string.IsNullOrEmpty(GroupID))
                throw new ArgumentException("GroupID");

            Token token = Token.GenerateToken();
            FlickrCore.InitiatePostRequest(
                elm => this.InvokeGetContextCompletedEvent(new EventArgs<PhotoContext>(token,new PhotoContext(this.authTkns,elm))),
                e => this.InvokeGetContextCompletedEvent(new EventArgs<PhotoContext>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.groups.pools.getContext"), new Parameter("photo_id", this.ID), new Parameter("group_id", GroupID));

            return token;
        }
        
        /// <summary>
        /// Returns all visible sets and pools the photo belongs to.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetAllContextsAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetAllContextsCompletedEvent(new EventArgs<PhotoContexts>(token,new PhotoContexts(this.authTkns,elm))),
                e => this.InvokeGetAllContextsCompletedEvent(new EventArgs<PhotoContexts>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getAllContexts"), new Parameter("auth_token", this.authTkns.Token)
            , new Parameter("api_key", this.authTkns.ApiKey), new Parameter("photo_id", this.ID));

            return token;
        }

        /// <summary>
        /// Returns the available sizes for a photo. The calling user must have permission to view the photo.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetSizesAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetSizesCompletedEvent(new EventArgs<IEnumerable<Size>>(token,elm.Element("sizes").Elements("size").Select(size=> new Size(size)))),
                e => this.InvokeGetSizesCompletedEvent(new EventArgs<IEnumerable<Size>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getSizes"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID));

            return token;
        }

        /// <summary>
        /// Add a note to a photo. Coordinates and sizes are in pixels, based on the 500px image size shown on individual photo pages.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="x">The left coordinate of the note.</param>
        /// <param name="y">The top coordinate of the note.</param>
        /// <param name="height">The height of the note.</param>
        /// <param name="width">The width of the note.</param>
        /// <param name="text">The description of the note</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token AddNoteAync(int x, int y, int height, int width, string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddNoteCompletedEvent(new EventArgs<string>(token, elm.Element("note").Attribute("id").Value)),
                e => this.InvokeAddNoteCompletedEvent(new EventArgs<string>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.notes.add"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("photo_id", this.ID), new Parameter("note_x", x), new Parameter("note_y", y), new Parameter("note_h", height),
                new Parameter("note_w", width), new Parameter("note_text", text));

            return token;
        }

        /// <summary>
        /// Delete a note from a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="noteID">The id of the note to delete.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token DeleteNoteAsync(string noteID)
        {
            if (string.IsNullOrEmpty(noteID))
                throw new ArgumentException("noteID");

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeDeleteNoteCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeDeleteNoteCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.notes.delete"), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("photo_id", this.ID), new Parameter("note_id", noteID));

            return token;
        }

        /// <summary>
        /// Edit a note on a photo. Coordinates and sizes are in pixels, based on the 500px image size shown on individual photo pages. 
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="noteID">The id of the note to edit.</param>
        /// <param name="x">The left coordinate of the note.</param>
        /// <param name="y">The top coordinate of the note.</param>
        /// <param name="height">The height of the note</param>
        /// <param name="width">The width of the note.</param>
        /// <param name="text">The description of the note.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token EditNoteAsync(string noteID, int x, int y, int height, int width, string text)
        {
            if (string.IsNullOrEmpty(noteID))
                throw new ArgumentException("noteID");
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text");

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditNoteCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeEditNoteCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.notes.edit"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("photo_id", this.ID), new Parameter("note_x", x), new Parameter("note_y", y), new Parameter("note_h", height),
                new Parameter("note_w", width), new Parameter("note_text", text),new Parameter("note_id",noteID));

            return token;
        }

        /// <summary>
        /// Add a person to a photo. Coordinates and sizes of boxes are optional; they are measured in pixels, based on the 500px image size shown on individual photo pages.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="personID">The NSID of the user to add to the photo.</param>
        /// <param name="x">The left-most pixel co-ordinate of the box around the person.</param>
        /// <param name="y">The top-most pixel co-ordinate of the box around the person.</param>
        /// <param name="height">The height (in pixels) of the box around the person.</param>
        /// <param name="width">The width (in pixels) of the box around the person.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token AddPersonAync(string personID, Nullable<int> x = null, Nullable<int> y = null, Nullable<int> height = null, Nullable<int> width = null)
        {
            if (string.IsNullOrEmpty(personID))
                throw new ArgumentException("personID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddPersonCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeAddPersonCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.people.add"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("user_id", personID),
                new Parameter("person_x", x), new Parameter("person_y", y), new Parameter("person_w", width), new Parameter("person_h", height));

            return token;
        }

        /// <summary>
        /// Remove a person from a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="personID">The NSID of the person to remove from the photo.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token RemovePersonAsync(string personID)
        {
            if (string.IsNullOrEmpty(personID))
                throw new ArgumentException("personID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemovePersonCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeRemovePersonCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.people.delete"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("user_id", personID));

            return token;
        }

        /// <summary>
        /// Remove the bounding box from a person in a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="personID">The NSID of the person whose bounding box you want to remove.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token RemovePersonCoordsAsync(string personID)
        {
            if (string.IsNullOrEmpty(personID))
                throw new ArgumentException("personID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemovePersonCoordsCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeRemovePersonCoordsCompletedEvent(new EventArgs<NoReply>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.people.deleteCoords"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("user_id", personID));

            return token;
        }

        /// <summary>
        /// Edit the bounding box of an existing person on a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="personID">The NSID of the person to edit in a photo.</param>
        /// <param name="x">The left-most pixel co-ordinate of the box around the person.</param>
        /// <param name="y">The top-most pixel co-ordinate of the box around the person.</param>
        /// <param name="height">The width (in pixels) of the box around the person.</param>
        /// <param name="width">The width (in pixels) of the box around the person.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token EditPersonCoordsAsync(string personID, int x, int y, int height, int width)
        {
            if (string.IsNullOrEmpty(personID))
                throw new ArgumentException("personID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeEditPersonCoordsCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeEditPersonCoordsCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.people.editCoords"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("user_id", personID),
                new Parameter("person_x", x), new Parameter("person_y", y), new Parameter("person_h", height), new Parameter("person_w", width));

            return token;
        }

        /// <summary>
        /// Get a list of people in a given photo.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPersonsListAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPersonsListCompletedEvent(new EventArgs<IEnumerable<PersonInPhoto>>(token,
                    elm.Element("people").Elements("person").Select(person=>new PersonInPhoto(person)))),
                e => this.InvokeGetPersonsListCompletedEvent(new EventArgs<IEnumerable<PersonInPhoto>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.people.getList"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id",this.ID));

            return token;
        }

        /// <summary>
        /// Rotate a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="degrees">The amount of degrees by which to rotate the photo (clockwise) from it's current orientation. Valid values are 90, 180 and 270.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token RotateAsync(Degrees degrees)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRotateCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeRotateCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("method", "flickr.photos.transform.rotate") , new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("degrees", (int)degrees));

            return token;
        }

        /// <summary>
        /// Add comment to a photo as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="text">Text of the comment.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token AddCommentAsync(string text)
        {
            if (string.IsNullOrEmpty(text))
                throw new ArgumentException("text");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddCommentCompletedEvent(new EventArgs<string>(token,elm.Element("comment").Attribute("id").Value)),
                e => this.InvokeAddCommentCompletedEvent(new EventArgs<string>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.comments.addComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("comment_text", text));

            return token;
        }

        /// <summary>
        /// Delete a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="commentID">The id of the comment to delete.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token DeleteCommentAsync(string commentID)
        {
            if (string.IsNullOrEmpty(commentID))
                throw new ArgumentException("commentID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeDeleteCommentCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)),
                e => this.InvokeDeleteCommentCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.comments.deleteComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID), new Parameter("comment_id", commentID));

            return token;
        }

        /// <summary>
        /// Edit the text of a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="commentID">The id of the comment to edit.</param>
        /// <param name="text">Update the comment to this text.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
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
                new Parameter("method", "flickr.photos.comments.editComment"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("comment_id", commentID), new Parameter("comment_text", text));

            return token;
        }

        /// <summary>
        /// Returns the comments for a photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="minCommentDate">Minimum date that a a comment was added. The date should be in the form of a unix timestamp.</param>
        /// <param name="maxCommentDate">Maximum date that a comment was added. The date should be in the form of a unix timestamp.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCommentsListAsync(string minCommentDate = null, string maxCommentDate = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeGetCommentsListCompletedEvent(new EventArgs<IEnumerable<Comment>>(token,
                    elm.Element("comments").Elements("comment").Select(comment=>new Comment(comment)))),
                e => this.InvokeGetCommentsListCompletedEvent(new EventArgs<IEnumerable<Comment>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.comments.getList"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("photo_id", this.ID),
                new Parameter("min_comment_date", minCommentDate), new Parameter("max_comment_date", maxCommentDate));

            return token;
        }

        /// <summary>
        /// Return the list of galleries to which a photo has been added. Galleries are returned sorted by date which the photo was added to the gallery.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="perPage">Number of galleries to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetGalleriesListAsync(Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetGalleriesCompletedEvent(new EventArgs<GalleriesCollection>(token, new GalleriesCollection(this.authTkns, elm.Element("galleries")))), 
                e => this.InvokeGetGalleriesCompletedEvent(new EventArgs<GalleriesCollection>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.galleries.getListForPhoto"), new Parameter("photo_id", this.ID), 
                new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Sets the license for a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="licenseID">The license to apply, or 0 (zero) to remove the current license. Note : as of this writing the "no known copyright restrictions" license (7) is not a valid argument.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token SetLicenseAsync(int licenseID)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeSetLicenseCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeSetLicenseCompletedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.photos.licenses.setLicense"), new Parameter("photo_id", this.ID), new Parameter("license_id", licenseID));

            return token;
        }

        #region Events
        private void InvokeSetLicenseCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetLicenseCompleted != null)
            {
                this.SetLicenseCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SetLicenseAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> SetLicenseCompleted;
        private void InvokeGetGalleriesCompletedEvent(EventArgs<GalleriesCollection> args)
        {
            if (this.GetGalleriesCompleted != null)
            {
                this.GetGalleriesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetGalleriesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GalleriesCollection>> GetGalleriesCompleted;
        private void InvokeGetCommentsListCompletedEvent(EventArgs<IEnumerable<Comment>> args)
        {
            if (this.GetCommentsListCompleted != null)
            {
                this.GetCommentsListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCommentsListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Comment>>> GetCommentsListCompleted;
        private void InvokeEditCommentCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditCommentCompleted != null)
            {
                this.EditCommentCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when EditCommentAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> EditCommentCompleted;
        private void InvokeDeleteCommentCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.DeleteCommentCompleted != null)
            {
                this.DeleteCommentCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when DeleteAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> DeleteCommentCompleted;
        private void InvokeAddCommentCompletedEvent(EventArgs<string> args)
        {
            if (this.AddCommentCompleted != null)
            {
                this.AddCommentCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when AddCommentAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<string>> AddCommentCompleted;
        private void InvokeRotateCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RotateCompleted != null)
            {
                this.RotateCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when RotateAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> RotateCompleted;
        private void InvokeGetPersonsListCompletedEvent(EventArgs<IEnumerable<PersonInPhoto>> args)
        {
            if (this.GetPersonsListCompleted != null)
            {
                this.GetPersonsListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPersonsListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<PersonInPhoto>>> GetPersonsListCompleted;
        private void InvokeEditPersonCoordsCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditPersonCoordsCompleted != null)
            {
                this.EditPersonCoordsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when EditPersonCoordsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> EditPersonCoordsCompleted;
        private void InvokeRemovePersonCoordsCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemovePersonCoordsCompleted != null)
            {
                this.RemovePersonCoordsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when RemovePersonCoordsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> RemovePersonCoordsCompleted;
        private void InvokeRemovePersonCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemovePersonCompleted != null)
            {
                this.RemovePersonCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when RemovePersonAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> RemovePersonCompleted;
        private void InvokeAddPersonCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddPersonCompleted != null)
            {
                this.AddPersonCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when AddPersonAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> AddPersonCompleted;
        private void InvokeEditNoteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.EditNoteCompleted != null)
            {
                this.EditNoteCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when EditNoteAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> EditNoteCompleted;
        private void InvokeDeleteNoteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.DeleteNoteCompleted != null)
            {
                this.DeleteNoteCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when DeleteNoteAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> DeleteNoteCompleted;
        private void InvokeAddNoteCompletedEvent(EventArgs<string> args)
        {
            if (this.AddNoteCompleted != null)
            {
                this.AddNoteCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when AddNoteAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<string>> AddNoteCompleted;
        private void InvokeGetSizesCompletedEvent(EventArgs<IEnumerable<Size>> args)
        {
            if (this.GetSizesCompleted != null)
            {
                this.GetSizesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetSizesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Size>>> GetSizesCompleted;
        private void InvokeGetAllContextsCompletedEvent(EventArgs<PhotoContexts> args)
        {
            if (this.GetAllContextsCompleted != null)
            {
                this.GetAllContextsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetAllContextsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotoContexts>> GetAllContextsCompleted;
        private void InvokeGetContextCompletedEvent(EventArgs<PhotoContext> args)
        {
            if (this.GetContextCompleted != null)
            {
                this.GetContextCompleted.Invoke(this, args);
            }    
        }
        /// <summary>
        /// Raised when GetContextAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotoContext>> GetContextCompleted;
        private void InvokeDeleteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.DeleteCompleted != null)
            {
                this.DeleteCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when DeleteAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> DeleteCompleted;
        private void InvokeSetContentTypeCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetContentTypeCompleted != null)
            {
                this.SetContentTypeCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SetContentTypeAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> SetContentTypeCompleted;
        private void InvokeSetDatesCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetDatesCompleted != null)
            {
                this.SetDatesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SetDatesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> SetDatesCompleted;
        private void InvokeSetSafetyLevelCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetSafetyLevelCompleted != null)
            {
                this.SetSafetyLevelCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SetSafetyLevelAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> SetSafetyLevelCompleted;
        private void InvokeSetMetaCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetMetaCompleted != null)
            {
                this.SetMetaCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SetMetaAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> SetMetaCompleted;
        private void InvokeSetPermissionsCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.SetPermissionsCompleted != null)
            {
                this.SetPermissionsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SetPermissionsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> SetPermissionsCompleted;
        private void InvokeGetPermissionsCompletedEvent(EventArgs<Permissions> args)
        {
            if (this.GetPermissionsCompleted != null)
            {
                this.GetPermissionsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPermissionsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<Permissions>> GetPermissionsCompleted;
        private void InvokeRemoveTagCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemoveTagCompleted != null)
            {
                this.RemoveTagCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when RemoveTagAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> RemoveTagCompleted;
        private void InvokeAddTagCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddTagCompleted != null)
            {
                this.AddTagCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when AddTagAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> AddTagCompleted;
        private void InvokeGetInfoCompletedEvent(EventArgs<PhotoInfo> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetInfoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotoInfo>> GetInfoCompleted;
        private void InvokePostPhotoToBlogCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.PostPhotoToBlogCompleted != null)
            {
                this.PostPhotoToBlogCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when PostToBlogAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> PostPhotoToBlogCompleted;
        private void InvokeGetFavoritesCompletedEvent(EventArgs<IEnumerable<Person>> args)
        {
            if (this.GetFavoritesCompleted != null)
            {
                this.GetFavoritesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetFavoritesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Person>>> GetFavoritesCompleted;
        private void InvokeGetExifCompletedEvent(EventArgs<IEnumerable<Exif>> args)
        {
            if (this.GetExifCompleted != null)
            {
                this.GetExifCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetExifAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Exif>>> GetExifCompleted;
        private void InvokeRemoveFromfavoriteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemoveFromFavoriteCompleted != null)
            {
                this.RemoveFromFavoriteCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when RemoveFromFavoritesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> RemoveFromFavoriteCompleted;
        private void InvokeAddTofavoriteCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddToFavoriteCompleted != null)
            {
                this.AddToFavoriteCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when AddToFavoriteAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> AddToFavoriteCompleted;
        #endregion

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Photo Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Photo left, Photo right)
        {
            if (left is Photo)
                return left.Equals(right);
            else if (right is Photo)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Photo Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Photo left, Photo right)
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
            return obj is Photo && this.ID == ((Photo)obj).ID;
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
    /// represents a Collection of photos that a user appears in.
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
        /// the number of total pages , could be null when called without signing.
        /// </summary>
        public Nullable<int> Pages { get; private set; }

        /// <summary>
        /// the number of current page.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the number of photos per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// determine whether there exists a next page , Could be Null when called with signing.
        /// </summary>
        public Nullable<bool> HasNextPage { get; private set; }

        /// <summary>
        /// the Total Number of photos , Could be null when called with no Signing.
        /// </summary>
        public Nullable<int> Total { get; private set; }

        /// <summary>
        /// Photos Objects.
        /// </summary>
        public IEnumerable<Photo> Photos
        {
            get
            {
                return this.data.Elements("photos").Select(elm => new Photo(this.authTkns, elm));
            }
        }

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

    /// <summary>
    /// Represents an Exif piece of information , http://en.wikipedia.org/wiki/Exchangeable_image_file_format .
    /// </summary>
    public class Exif
    {
        internal Exif(XElement element)
        {
            this.TagSpace = element.Attribute("tagspace").Value;
            this.TagSpaceID = int.Parse(element.Attribute("tagspaceid").Value);
            this.Tag = element.Attribute("tag").Value;
            this.Raw = element.Element("raw").Value;
            this.Clean = element.Element("clean") != null ? element.Element("clean").Value : null;
            this.Label = element.Attribute("label").Value;
        }

        /// <summary>
        /// the tag space this Exif is belong to.
        /// </summary>
        public string TagSpace { get; private set; }

        /// <summary>
        /// the tag space ID this Exif is belong to.
        /// </summary>
        public int TagSpaceID { get; private set; }

        /// <summary>
        /// the Tag name.
        /// </summary>
        public string Tag { get; private set; }

        /// <summary>
        /// the name of label.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// the Raw value of the Tag.
        /// </summary>
        public string Raw { get; private set; }

        /// <summary>
        /// contains a pretty-formatted version of the tag where available, Could Be Null.
        /// </summary>
        public string Clean { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Exif Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Exif left, Exif right)
        {
            if (left is Exif)
                return left.Equals(right);
            else if (right is Exif)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Exif Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Exif left, Exif right)
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
            return obj is Exif && this.Raw == ((Exif)obj).Raw;
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
    /// represents person information that added a photo to his favorite list.
    /// </summary>
    public class Person
    {
        internal Person(XElement element)
        {
            this.ID = element.Attribute("nsid").Value;
            this.UserName = element.Attribute("username").Value;
            this.FaveDate = new DateTime(1970,1,1,0,0,0,0).AddSeconds(double.Parse(element.Attribute("favedate").Value));
        }

        /// <summary>
        /// the ID of the Person.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the user name of the person.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// the date where the user added the photo to his favorite list.
        /// </summary>
        public DateTime FaveDate { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Person Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Person left, Person right)
        {
            if (left is Person)
                return left.Equals(right);
            else if (right is Person)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Person Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Person left, Person right)
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
            return obj is Person && this.ID == ((Person)obj).ID;
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
    /// represents Information of a Photo.
    /// </summary>
    public class PhotoInfo
    {
        private XElement data;

        internal PhotoInfo(XElement element)
        {
            this.data = element;
        }

        /// <summary>
        /// the ID of the Photo.
        /// </summary>
        public string ID { get { return data.Attribute("id").Value; } }

        /// <summary>
        /// the Secret of the Photo.
        /// </summary>
        public string Secret { get { return this.data.Attribute("secret").Value; } }

        /// <summary>
        /// the Number of Server that photo resides on.
        /// </summary>
        public int Server { get { return int.Parse(this.data.Attribute("server").Value); } }
        
        /// <summary>
        /// the Number of Server Farm that photo resides on.
        /// </summary>
        public int Farm { get { return int.Parse(this.data.Attribute("farm").Value); } }

        /// <summary>
        /// the Date when the photo was Uploaded.
        /// </summary>
        public DateTime DateUploaded { get {return new DateTime(1970,1,1,0,0,0,0).AddSeconds(double.Parse(this.data.Attribute("dateuploaded").Value)); } }

        /// <summary>
        /// determine whether the Photo is Favorited by You or Not.
        /// </summary>
        public bool IsFavorite { get { return this.data.Attribute("isfavorite").Value.ToBoolean(); } }

        /// <summary>
        /// the License Number that this photo is Under.
        /// </summary>
        public int License { get { return int.Parse(this.data.Attribute("license").Value); } }

        /// <summary>
        /// the current clockwise rotation, in degrees, by which the smaller image sizes differ from the original image.
        /// </summary>
        public int Rotation { get { return int.Parse(this.data.Attribute("rotation").Value); } }

        /// <summary>
        /// The Original Secret of the photo , Could Be Null.
        /// </summary>
        public string OriginalSecret { get { return this.data.Attribute("originalsecret") != null ? this.data.Attribute("originalsecret").Value : null; } }

        /// <summary>
        /// the Original format of the photo file , could Be Null.
        /// </summary>
        public string OriginalFormat { get { return this.data.Attribute("originalformat") != null ? this.data.Attribute("originalformat").Value : null; } }

        /// <summary>
        /// the Number of Views of the photos.
        /// </summary>
        public int Views { get { return int.Parse(this.data.Attribute("views").Value); } }

        /// <summary>
        /// the Type of the Photo ( video , photo ).
        /// </summary>
        public string Media { get { return this.data.Attribute("media").Value; } }

        /// <summary>
        /// the Title of the Photo.
        /// </summary>
        public string Title { get { return this.data.Element("title").Value; } }

        /// <summary>
        /// the Description of the photo.
        /// </summary>
        public string Description { get { return this.data.Element("description").Value; } }

        /// <summary>
        /// determine whether the photo is Public or Not.
        /// </summary>
        public bool IsPublic { get { return this.data.Element("visibility").Attribute("ispublic").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether the photo is Friend or Not.
        /// </summary>
        public bool IsFriend { get { return this.data.Element("visibility").Attribute("isfriend").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether the photo is Family or Not.
        /// </summary>
        public bool IsFamily { get { return this.data.Element("visibility").Attribute("isfamily").Value.ToBoolean(); } }

        /// <summary>
        /// The date when the photo was posted.
        /// </summary>
        public DateTime DatePosted { get { return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(this.data.Element("dates").Attribute("posted").Value)); } }
        
        /// <summary>
        /// the date when the photo was taken.
        /// </summary>
        public DateTime DateTaken { get { return DateTime.Parse(this.data.Element("dates").Attribute("taken").Value); } }
        
        /// <summary>
        /// the date when the photo was updated.
        /// </summary>
        public DateTime LastUpdate { get { return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(this.data.Element("dates").Attribute("lastupdate").Value)); } }
        
        /// <summary>
        /// the Taken Granularity.
        /// </summary>
        public int TakenGranularity { get { return int.Parse(this.data.Element("dates").Attribute("takengranularity").Value); } }

        /// <summary>
        /// the permission to comment , could be Null.
        /// </summary>
        public Nullable<int> PermissionComment { get { return this.data.Element("permissions") != null ? new Nullable<int>(int.Parse(this.data.Element("permissions").Attribute("permcomment").Value)) : null; } }

        /// <summary>
        /// the permission to add metadata , could be Null.
        /// </summary>
        public Nullable<int> PermissionAddMetadata { get { return this.data.Element("permissions") != null ? new Nullable<int>(int.Parse(this.data.Element("permissions").Attribute("permaddmeta").Value)) : null; } }

        /// <summary>
        /// determine whether you can comment on the photo or Not.
        /// </summary>
        public bool CanComment { get { return this.data.Element("editability").Attribute("cancomment").Value.ToBoolean(); } }
        
        /// <summary>
        /// determine whether you can Add Metadata for the photo or Not.
        /// </summary>
        public bool CanAddMeta { get { return this.data.Element("editability").Attribute("canaddmeta").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether the Public can Comment or Not.
        /// </summary>
        public bool PublicCanComment { get { return this.data.Element("publiceditability").Attribute("cancomment").Value.ToBoolean(); } }
        
        /// <summary>
        /// determine whether the Public can add Metadata or Not.
        /// </summary>
        public bool PublicCanAddMeta { get { return this.data.Element("publiceditability").Attribute("canaddmeta").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether you can Download the original file of the photo or Not.
        /// </summary>
        public bool CanDownload { get { return this.data.Element("usage").Attribute("candownload").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether you can blog the photo or Not.
        /// </summary>
        public bool CanBlog { get { return this.data.Element("usage").Attribute("canblog").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether you can Print the photo or Not.
        /// </summary>
        public bool CanPrint { get { return this.data.Element("usage").Attribute("canprint").Value.ToBoolean(); } }

        /// <summary>
        /// determine whether you can Share the photo or Not.
        /// </summary>
        public bool CanShare { get { return this.data.Element("usage").Attribute("canshare").Value.ToBoolean(); } }

        /// <summary>
        /// The Number of Comments.
        /// </summary>
        public int CommentsCount { get { return int.Parse(this.data.Element("comments").Value); } }

        /// <summary>
        /// The Notes on the Photo.
        /// </summary>
        public IEnumerable<Note> Notes { get { return this.data.Element("notes").Elements("note").Select(note => new Note(note)); } }

        /// <summary>
        /// the Tags of the Photos.
        /// </summary>
        public IEnumerable<Tag> Tags { get { return this.data.Element("tags").Elements("tag").Select(tag => new Tag(tag)); } }

        /// <summary>
        /// The Location of the Photo , Could Be Null.
        /// </summary>
        public Location Location { get { return this.data.Element("location") != null ? new Location(this.data.Element("location")) : null ; } }

        /// <summary>
        /// Determine Whether the Geo Tags are visible to Public or Not.
        /// </summary>
        public Nullable<bool> GeoPermissionsIsPublic { get { return this.data.Element("geoperms") != null ? new Nullable<bool>(this.data.Element("geoperms").Attribute("ispublic").Value.ToBoolean()) : null; } }

        /// <summary>
        /// Determine Whether the Geo Tags are visible to Contacts or Not.
        /// </summary>
        public Nullable<bool> GeoPermissionsIsContact { get { return this.data.Element("geoperms") != null ? new Nullable<bool>(this.data.Element("geoperms").Attribute("iscontact").Value.ToBoolean()) : null; } }

        /// <summary>
        /// Determine Whether the Geo Tags are visible to Friends or Not.
        /// </summary>
        public Nullable<bool> GeoPermissionsIsFriend { get { return this.data.Element("geoperms") != null ? new Nullable<bool>(this.data.Element("geoperms").Attribute("isfriend").Value.ToBoolean()) : null; } }

        /// <summary>
        /// Determine Whether the Geo Tags are visible to Family or Not.
        /// </summary>
        public Nullable<bool> GeoPermissionsIsFamily { get { return this.data.Element("geoperms") != null ? new Nullable<bool>(this.data.Element("geoperms").Attribute("isfamily").Value.ToBoolean()) : null; } }

        /// <summary>
        /// the Urls of the Photo.
        /// </summary>
        public IEnumerable<URL> Urls { get { return this.data.Element("urls").Elements("url").Select(url => new URL(url)); } }

        /// <summary>
        /// Represents Video Info , If the Photo is a Video , So Could be Null.
        /// </summary>
        public Video Video { get { return this.data.Element("video") != null ? new Video(this.data.Element("video")) : null; } }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of PhotoInfo Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(PhotoInfo left, PhotoInfo right)
        {
            if (left is PhotoInfo)
                return left.Equals(right);
            else if (right is PhotoInfo)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of PhotoInfo Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(PhotoInfo left, PhotoInfo right)
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
            return obj is PhotoInfo && this.ID == ((PhotoInfo)obj).ID;
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
    /// Represents Video info.
    /// </summary>
    public class Video
    {
        internal Video(XElement element)
        {
            this.Duration = int.Parse(element.Attribute("duration").Value);
            this.Height = int.Parse(element.Attribute("height").Value);
            this.Width = int.Parse(element.Attribute("width").Value);
            this.IsReady = element.Attribute("ready").Value.ToBoolean();
            this.IsPending = element.Attribute("pending").Value.ToBoolean();
            this.IsFailed = element.Attribute("failed").Value.ToBoolean();
        }

        /// <summary>
        /// determine whether the Video is Ready or Not.
        /// </summary>
        public bool IsReady { get; private set; }

        /// <summary>
        /// determine whether the Video is Still in Processing or Not.
        /// </summary>
        public bool IsPending { get; private set; }

        /// <summary>
        /// determine whether the Video processing has Failed or Not.
        /// </summary>
        public bool IsFailed { get; private set; }

        /// <summary>
        /// the Duration of the Video , In Seconds.
        /// </summary>
        public int Duration { get; private set; }

        /// <summary>
        /// the Width of the Video.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// the Height of the Video.
        /// </summary>
        public int Height { get; private set; }
    }

    /// <summary>
    /// Represents the Photo Owner Info.
    /// </summary>
    public class Owner
    {
        private XElement data;

        internal Owner(XElement element)
        {
            this.data = element;
        }
        /// <summary>
        /// the ID of the Owner.
        /// </summary>
        public string ID { get { return this.data.Attribute("nsid").Value; } }

        /// <summary>
        /// the UserName of the Owner.
        /// </summary>
        public string UserName { get { return this.data.Attribute("username").Value; } }

        /// <summary>
        /// the Real Name of the Owner.
        /// </summary>
        public string RealName { get { return this.data.Attribute("realname").Value; } }

        /// <summary>
        /// the location of the owner.
        /// </summary>
        public string Location { get { return this.data.Attribute("location").Value; } }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Owner Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Owner left, Owner right)
        {
            if (left is Owner)
                return left.Equals(right);
            else if (right is Owner)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Owner Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Owner left, Owner right)
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
            return obj is Owner && this.ID == ((Owner)obj).ID;
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
    /// represents a Note on a Photo.
    /// </summary>
    public class Note
    {
        internal Note(XElement element)
        {
            this.AuthorID = element.Attribute("author").Value;
            this.ID = element.Attribute("id").Value;
            this.AuthorName = element.Attribute("authorname").Value;
            this.X = int.Parse(element.Attribute("x").Value);
            this.Y = int.Parse(element.Attribute("y").Value);
            this.Height = int.Parse(element.Attribute("h").Value);
            this.Width = int.Parse(element.Attribute("w").Value);
            this.Content = element.Value;
        }

        /// <summary>
        /// the ID of the Note.
        /// </summary>
        public string ID { get; private set;}

        /// <summary>
        /// the Author ID that Created the Note.
        /// </summary>
        public string AuthorID { get; private set; }

        /// <summary>
        /// the Author name that Created the Note.
        /// </summary>
        public string AuthorName { get; private set; }

        /// <summary>
        /// the X Value.
        /// </summary>
        public int X { get; private set; }

        /// <summary>
        /// the Y Value.
        /// </summary>
        public int Y { get; private set; }

        /// <summary>
        /// the height of the Note Box.
        /// </summary>
        public int Height { get; private set; }
        
        /// <summary>
        /// the Width of the Note Box.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// the Note Content.
        /// </summary>
        public string Content { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Note Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Note left, Note right)
        {
            if (left is Note)
                return left.Equals(right);
            else if (right is Note)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Note Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Note left, Note right)
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
            return obj is Note && this.ID == ((Note)obj).ID;
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
    /// Represents a Tag on a Photo.
    /// </summary>
    public class Tag
    {
        internal Tag(XElement element)
        {
            this.AuthorID = element.Attribute("author").Value;
            this.ID = element.Attribute("id").Value;
            this.Raw = element.Attribute("raw").Value;
            this.MachineTag = int.Parse(element.Attribute("machine_tag").Value);
            this.Content = element.Value;
        }

        /// <summary>
        /// The ID of the Tag.
        /// </summary>
        public string ID { get; private set;}

        /// <summary>
        /// the Author ID that Add the Tag to the Photo.
        /// </summary>
        public string AuthorID { get; private set; }

        /// <summary>
        /// the raw value that represents the Tag.
        /// </summary>
        public string Raw { get; private set; }

        /// <summary>
        /// the value of the Tag.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// the Machine Tag
        /// </summary>
        public int MachineTag { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Tag Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Tag left, Tag right)
        {
            if (left is Tag)
                return left.Equals(right);
            else if (right is Tag)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Tag Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Tag left, Tag right)
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
            return obj is Tag && this.ID == ((Tag)obj).ID;
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
    /// represents the location information of the photo.
    /// </summary>
    public class Location
    {
        private XElement data;

        internal Location(XElement element)
        {
            this.data = element;
        }

        /// <summary>
        /// The Latitude Value.
        /// </summary>
        public double Latitude { get { return double.Parse(this.data.Attribute("latitude").Value); } }

        /// <summary>
        /// the Longitude Value.
        /// </summary>
        public double Longitude { get { return double.Parse(this.data.Attribute("longitude").Value); } }

        /// <summary>
        /// Accuracy Value.
        /// </summary>
        public int Accuracy { get { return int.Parse(this.data.Attribute("accuracy").Value); } }

        /// <summary>
        /// Context Value.
        /// </summary>
        public int Context { get { return int.Parse(this.data.Attribute("context").Value); } }

        /// <summary>
        /// the Place ID.
        /// </summary>
        public string PlaceID { get { return this.data.Attribute("place_id").Value; } }
        
        /// <summary>
        /// the where on Earth ID.
        /// </summary>
        public string WoeID { get { return this.data.Attribute("woeid").Value; } }

        /// <summary>
        /// the neighborhood .
        /// </summary>
        public string Neighbourhood { get { return this.data.Element("neighbourhood ").Value; } }

        /// <summary>
        /// the Locality place ID.
        /// </summary>
        public string LocalityPlaceID { get { return this.data.Element("locality").Element("place_id").Value; } }

        /// <summary>
        /// the Locality Where on Earth ID.
        /// </summary>
        public string LocalityWoeID { get { return this.data.Element("locality").Element("woeid").Value; } }

        /// <summary>
        /// the Region Place ID.
        /// </summary>
        public string RegionPlaceID { get { return this.data.Element("region").Element("place_id").Value;} }

        /// <summary>
        /// the Region Where on Earth ID.
        /// </summary>
        public string RegionWoeID { get { return this.data.Element("region").Element("woeid").Value; } }

        /// <summary>
        /// the Country Place ID.
        /// </summary>
        public string CountryPlaceID { get { return this.data.Element("country").Element("place_id").Value;} }

        /// <summary>
        /// the Country Where on Earth ID.
        /// </summary>
        public string CountryWoeID { get { return this.data.Element("country").Element("woeid").Value;} }
    }
    
    /// <summary>
    /// Represents a Photo URL.
    /// </summary>
    public class URL
    {
        internal URL(XElement element)
        {
            this.Type = element.Attribute("type").Value;
            this.Value = element.Value;
        }

        /// <summary>
        /// The Url Type.
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// The Actual Url Value.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Return String Representation of the Instance.
        /// </summary>
        /// <returns>a String.</returns>
        public override string ToString()
        {
            return this.Value;
        }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of URL Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(URL left, URL right)
        {
            if (left is URL)
                return left.Equals(right);
            else if (right is URL)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of URL Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(URL left, URL right)
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
            return obj is URL && this.Value == ((URL)obj).Value;
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
    /// represents a set of permissions on a photo.
    /// </summary>
    public class Permissions
    {
        internal Permissions(XElement element)
        {
            this.IsFriend = element.Attribute("isfriend").Value.ToBoolean();
            this.IsPublic = element.Attribute("ispublic").Value.ToBoolean();
            this.IsFamily = element.Attribute("isfamily").Value.ToBoolean();
            this.CommentPermission = (CommentPermission)int.Parse(element.Attribute("permcomment").Value);
            this.AddMetadataPermission = (AddMetadataPermission)int.Parse(element.Attribute("permaddmeta").Value);
        }

        /// <summary>
        /// can be Seen by Friends.
        /// </summary>
        public bool IsFriend { get; private set; }

        /// <summary>
        /// can be seen by Public.
        /// </summary>
        public bool IsPublic { get; private set; }

        /// <summary>
        /// can be Seen by Family.
        /// </summary>
        public bool IsFamily { get; private set; }

        /// <summary>
        /// Determine whether you can Add Comment.
        /// </summary>
        public CommentPermission CommentPermission { get; private set; }

        /// <summary>
        /// Determine whether you can Add Metadata Info.
        /// </summary>
        public AddMetadataPermission AddMetadataPermission { get; private set; }
    }

    /// <summary>
    /// Setting of adding comments to the photo and it's notes.
    /// </summary>
    public enum CommentPermission
    {
        /// <summary>
        /// nobody / just the owner.
        /// </summary>
        NoBody = 0,
        /// <summary>
        /// friends and family.
        /// </summary>
        FriendsAndFamily = 1,
        /// <summary>
        /// contacts.
        /// </summary>
        Contacts = 2,
        /// <summary>
        /// every body on Flickr.
        /// </summary>
        Everybody =3
    }

    /// <summary>
    /// Setting of adding notes and tags to the photo.
    /// </summary>
    public enum AddMetadataPermission
    {
        /// <summary>
        /// nobody / just the owner.
        /// </summary>
        NoBody = 0,
        /// <summary>
        /// friends and family.
        /// </summary>
        FriendsAndFamily = 1,
        /// <summary>
        /// contacts.
        /// </summary>
        Contacts = 2,
        /// <summary>
        /// every body on Flickr.
        /// </summary>
        Everybody = 3
    }

    /// <summary>
    /// The safety level of the photo.
    /// </summary>
    public enum SafetyLevel
    {
        /// <summary>
        /// for safe.
        /// </summary>
        Safe = 1,
        /// <summary>
        /// for moderate.
        /// </summary>
        Moderate = 2,
        /// <summary>
        /// for restricted.
        /// </summary>
        Restricted = 3
    }

    /// <summary>
    /// represent a photo context.
    /// </summary>
    public class PhotoContext
    {
        internal PhotoContext(AuthenticationTokens authTkns, XElement element)
        {
            if (element.Element("nextphoto").Attribute("id").Value != "0")
            {
                this.NextPhoto = new NeighborPhoto(authTkns, element.Element("nextphoto"));
            }
            if (element.Element("prevphoto").Attribute("id").Value != "0")
            {
                this.PreviousPhoto = new NeighborPhoto(authTkns, element.Element("prevphoto"));
            }
            this.Count = int.Parse(element.Element("count").Value);
        }

        /// <summary>
        /// previous photo in the Current Context, Could be Null when there is no Previous Photo.
        /// </summary>
        public NeighborPhoto PreviousPhoto { get; private set; }

        /// <summary>
        /// next photo in the current context , Could be Null when there is no Next Photo.
        /// </summary>
        public NeighborPhoto NextPhoto { get; private set; }

        /// <summary>
        /// the total number of photos in the current Context.
        /// </summary>
        public int Count { get; private set; }

    }

    /// <summary>
    /// represents a neighbor photo in the current context.
    /// </summary>
    public class NeighborPhoto
    {
        internal NeighborPhoto(AuthenticationTokens authTkns,XElement element)
        {
            this.ThumbURL = new Uri(element.Attribute("thumb").Value);
            this.URL = element.Attribute("url").Value;
            this.License = int.Parse(element.Attribute("license").Value);
            this.IsFavorite = element.Attribute("is_faved")!= null ? new Nullable<bool>(element.Attribute("is_faved").Value.ToBoolean()) : null;
            this.Media = element.Attribute("media").Value;
            this.ID = element.Attribute("id").Value;
            this.Title = element.Attribute("title").Value;
            this.Server = int.Parse(element.Attribute("server").Value);
            this.Farm = int.Parse(element.Attribute("farm").Value);
        }

        /// <summary>
        /// the Thumb URL of this photo.
        /// </summary>
        public Uri ThumbURL { get; private set; }

        /// <summary>
        /// the type of the media (video , photo).
        /// </summary>
        public string Media { get; private set; }

        /// <summary>
        /// the license number.
        /// </summary>
        public int License { get; private set; }

        /// <summary>
        /// determine whether this photo is Favorited by you or Not , Could Be Null.
        /// </summary>
        public Nullable<bool> IsFavorite { get; private set; }
        
        /// <summary>
        /// the ID of the Photo.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// this string is used to in the building of photo URL.
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Title of the photo.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Server number which the photo is on.
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// The server Farm number which the photo is on.
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// relative path for the photo in the Photostream of the Owner.
        /// </summary>
        public string URL { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of NeighborPhoto Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(NeighborPhoto left, NeighborPhoto right)
        {
            if (left is NeighborPhoto)
                return left.Equals(right);
            else if (right is NeighborPhoto)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of NeighborPhoto Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(NeighborPhoto left, NeighborPhoto right)
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
            return obj is NeighborPhoto && this.ID == ((NeighborPhoto)obj).ID;
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
    /// represents a collection of sets and pools that a photo is contained in..
    /// </summary>
    public class PhotoContexts
    {
        internal PhotoContexts(AuthenticationTokens authTkns,XElement element)
        {
            this.Pools = element.Elements("pool").Select(pool => new Pool(authTkns,pool));
            this.Sets = element.Elements("set").Select(set => new PhotoSetBasic(authTkns,set));
        }

        /// <summary>
        /// Enumerable of Group Pools.
        /// </summary>
        public IEnumerable<Pool> Pools { get; private set; }

        /// <summary>
        /// Enumerable of Sets.
        /// </summary>
        public IEnumerable<PhotoSetBasic> Sets { get; private set; }
    }

    /// <summary>
    /// represents a Size of a Photo.
    /// </summary>
    public class Size
    {
        internal Size(XElement element)
        {
            this.Label = element.Attribute("label").Value;
            this.Source = new Uri(element.Attribute("source").Value);
            this.Url = new Uri(element.Attribute("url").Value);
            this.Media = element.Attribute("media").Value;
            this.Height = int.Parse(element.Attribute("height").Value);
            this.Width = int.Parse(element.Attribute("width").Value);
        }

        /// <summary>
        /// The Size category , Given in the Label.
        /// </summary>
        public string Label { get; private set; }

        /// <summary>
        /// the Width of the Photo.
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// the Height of the Photo.
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// the Type of the Media (photo, Video).
        /// </summary>
        public string Media { get; private set; }

        /// <summary>
        /// the Source file URL.
        /// </summary>
        public Uri Source { get; private set; }

        /// <summary>
        /// the URL of the Photo Size Page.
        /// </summary>
        public Uri Url { get; private set; }
    }
    
    /// <summary>
    /// represents information of a person in a photo.
    /// </summary>
    public class PersonInPhoto
    {
        internal PersonInPhoto(XElement element)
        {
            this.ID = element.Attribute("nsid").Value;
            this.RealName = element.Attribute("realname").Value;
            this.UserName = element.Attribute("username").Value;
            this.AddedByID = element.Attribute("added_by").Value;
            this.PathAlias = element.Attribute("path_alias").Value;
            this.IconServer = int.Parse(element.Attribute("iconserver").Value);
            this.Farm = int.Parse(element.Attribute("iconfarm").Value);
            this.X = element.Attribute("x") != null ? new Nullable<int>(int.Parse(element.Attribute("x").Value)) : null;
            this.Y = element.Attribute("y") != null ? new Nullable<int>(int.Parse(element.Attribute("y").Value)) : null;
            this.Height = element.Attribute("h") != null ? new Nullable<int>(int.Parse(element.Attribute("h").Value)) : null;
            this.Width = element.Attribute("w") != null ? new Nullable<int>(int.Parse(element.Attribute("w").Value)) : null;
        }

        /// <summary>
        /// the ID of the person.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the User Name of the person in the photo.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// the real name of the person.
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// the number of icon server.
        /// </summary>
        public int IconServer { get; private set; }

        /// <summary>
        /// the number of icon server Farm.
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        ///the Path Alias (used when Generating Urls ) , Could be Empty when not set by the user.
        /// </summary>
        public string PathAlias { get; private set; }

        /// <summary>
        /// the ID of the User Add the Person to the photo.
        /// </summary>
        public string AddedByID { get; private set; }

        /// <summary>
        /// the X of Person Box in the photo , Could Be Null.
        /// </summary>
        public Nullable<int> X { get; private set; }

        /// <summary>
        /// the Y of the Person Box in the photo , Could Be Null.
        /// </summary>
        public Nullable<int> Y { get; private set; }

        /// <summary>
        /// the height of the Person Box in the  photo , Could Be Null.
        /// </summary>
        public Nullable<int> Height { get; private set; }

        /// <summary>
        ///  the Width of the Person Box in the  photo  , Could Be Null.
        /// </summary>
        public Nullable<int> Width { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of PersonInPhoto Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(PersonInPhoto left, PersonInPhoto right)
        {
            if (left is PersonInPhoto)
                return left.Equals(right);
            else if (right is PersonInPhoto)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of PersonInPhoto Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(PersonInPhoto left, PersonInPhoto right)
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
            return obj is PersonInPhoto && this.ID == ((PersonInPhoto)obj).ID;
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
    /// rotation setting.
    /// </summary>
    public enum Degrees
    {
        /// <summary>
        /// 90 degrees.
        /// </summary>
        NinetyDegrees = 90,
        /// <summary>
        /// 180 degrees.
        /// </summary>
        OneHundredAndEightyDegrees = 180 ,
        /// <summary>
        /// 270 degrees.
        /// </summary>
        TwoHundredAndseventyDegress = 270 
    }

    /// <summary>
    /// represents a Comment on photo or photos set.
    /// </summary>
    public class Comment
    {
        internal Comment(XElement element)
        {
            this.ID = element.Attribute("id").Value;
            this.AuthorID = element.Attribute("author").Value;
            this.AuthorName = element.Attribute("authorname").Value;
            this.DateCreated = double.Parse(element.Attribute("datecreate").Value).ToDateTimeFromUnix();
            this.PermaLink = new Uri(element.Attribute("permalink").Value);
        }

        /// <summary>
        /// the ID of the Comment.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the ID of the User that Created the Comment.
        /// </summary>
        public string AuthorID { get; private set; }

        /// <summary>
        /// the name of the user that created the Comment.
        /// </summary>
        public string AuthorName { get; private set; }

        /// <summary>
        /// the date when the comment was created.
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// the permalink that leads to the comment directly in the photo page on Flickr.
        /// </summary>
        public Uri PermaLink { get; private set; }

        /// <summary>
        /// the Content of the Comment.
        /// </summary>
        public string Text { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Comment Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Comment left, Comment right)
        {
            if (left is Comment)
                return left.Equals(right);
            else if (right is Comment)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Comment Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Comment left, Comment right)
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
            return obj is Comment && this.ID == ((Comment)obj).ID;
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