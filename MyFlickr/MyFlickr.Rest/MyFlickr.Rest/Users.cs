using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a Flickr User.
    /// </summary>
    public class User
    {
        private readonly AuthenticationTokens authTkns;

        /// <summary>
        /// the authentication tokens that are used by this user instance.
        /// </summary>
        public AuthenticationTokens AuthenticationTokens { get { return this.authTkns; } }

        /// <summary>
        /// The User ID.
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// Creates an object that represents a Flickr User.
        /// </summary>
        /// <param name="tokens">the authentication tokens to be used.</param>
        public User(AuthenticationTokens tokens)
            : this(tokens, tokens.UserID) 
        {
            if (string.IsNullOrEmpty(tokens.UserID))
                throw new InvalidOperationException("can't create an instance of user from an authentication tokens that don't have UserID");
        }

        /// <summary>
        /// Creates an object that represents a Flickr User.
        /// </summary>
        /// <param name="tokens">the authentication tokens to be used.</param>
        /// <param name="userID">the user id that you want to create instance for.</param>
        public User(AuthenticationTokens tokens, string userID)
        {
            if (tokens == null)
                throw new ArgumentNullException("tokens");
            if (string.IsNullOrEmpty(userID))
                throw new ArgumentException("userID");

            this.authTkns = tokens;
            this.UserID = userID;
        }

        /// <summary>
        /// Creates an object that represents a Flickr User.
        /// </summary>
        /// <param name="apiKey">Flickr API application key.</param>
        /// <param name="userID">The User ID.</param>
        public User(string apiKey, string userID)
            :this(new AuthenticationTokens(apiKey, null, null, Rest.AccessPermission.None, null, null, null),userID)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
        }

        /// <summary>
        /// Get a list of contacts for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="contactFilter">An optional filter of the results. The following values are valid:friends,family,both,neither</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetContactsListAsync(Nullable<ContactFilter> contactFilter = null, Nullable<int> page = null , Nullable<int> perPage = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
              element => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token,new ContactsList(element.Element("contacts")))) 
            , e => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token, e))
            , this.authTkns.SharedSecret,
            new Parameter("method", "flickr.contacts.getList"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token)
            ,new Parameter("filter", contactFilter),new Parameter("page", page),new Parameter("per_page",perPage));

            return token;
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPublicContactsListAsync(User user, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
              element => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, new ContactsList(element.Element("contacts"))))
            , e => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, e))
            , this.authTkns.SharedSecret, new Parameter("method", "flickr.contacts.getPublicList"), new Parameter("user_id", user.UserID),
            new Parameter("api_key", this.authTkns.ApiKey), new Parameter("page", page), new Parameter("per_page", perPage));

            return token;
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPublicContactsListAsync(Nullable<int> page = null, Nullable<int> perPage = null)
        {
            return this.GetPublicContactsListAsync(this, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// to return public photos for a user, use User.getPublicPhotos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="contentType">Content Type setting.</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view photos you own.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosAsync(Nullable<SafetyLevel> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
            , string minTakenDate = null , string maxTakenDate = null, Nullable<ContentType> contentType = null, Nullable<PrivacyFilter> privacyFilter = null
            , string extras = null , Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return this.GetPhotosAsync(this, safeSearch, minUploadDate, maxUploadDate, minTakenDate, maxTakenDate, contentType, privacyFilter, extras, perPage, page);            
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// to return public photos for a user, use User.getPublicPhotos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="safeSearch">Safe search setting.</param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="contentType">Content Type setting.</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view photos you own.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosAsync(User user, Nullable<SafetyLevel> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
            , string minTakenDate = null, string maxTakenDate = null, Nullable<ContentType> contentType = null, Nullable<PrivacyFilter> privacyFilter = null
            , string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                  elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns,elm.Element("photos")))),
                  e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                  new Parameter("method", "flickr.people.getPhotos"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token)
                  , new Parameter("user_id", user.UserID), new Parameter("safe_search", safeSearch.HasValue ? (object)(int)safeSearch : null),
                  new Parameter("min_upload_date", minUploadDate),
                  new Parameter("max_upload_date", maxUploadDate), new Parameter("min_taken_date", minTakenDate), new Parameter("max_taken_date", maxTakenDate),
                  new Parameter("content_type", contentType.HasValue ? (object)(int)contentType : null),
                  new Parameter("privacy_filter", privacyFilter.HasValue ? (object)(int)privacyFilter : null)
                , new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="safeSearch">Safe search setting.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPublicPhotosAsync(User user, Nullable<SafetyLevel> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                  elm => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos"))))
                , e => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null
                , new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.people.getPublicPhotos"), new Parameter("user_id", user.UserID)
                , new Parameter("safe_search", safeSearch.HasValue ? (object)(int)safeSearch: null) , new Parameter("extras", extras),
                  new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPublicPhotosAsync(Nullable<SafetyLevel> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return this.GetPublicPhotosAsync(this, safeSearch, extras, perPage, page);
        }

        /// <summary>
        /// Returns a list of photos containing a particular Flickr member.
        /// This method does not require authentication. but when called when having at least read permission ,  you will get private photos.
        /// </summary>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosOfUserAsync(string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();
            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosOfUserCollection>(token, new PhotosOfUserCollection(this.authTkns,elm.Element("photos"))))
                , e => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosOfUserCollection>(token, e)), this.authTkns.SharedSecret
                , new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.people.getPhotosOf"), new Parameter("user_id", this.UserID)
                , new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page), new Parameter("auth_token", this.authTkns.Token));
            
            return token;
        }

        /// <summary>
        /// Get information about the user.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetInfoAsync()
        {
            Token token = Core.Token.GenerateToken();
            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token,new UserInfo(elm.Element("person"))))
                , e => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, e)), this.authTkns.SharedSecret
                , new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token)
                ,new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Get information about a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetInfoAsync(User user)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            if (user == null)
                throw new ArgumentNullException("user");

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                    elm => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, new UserInfo(elm.Element("person"))))
                    , e => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, e)), this.authTkns.SharedSecret
                    , new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token)
                    , new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", user.UserID));

            return token;
        }

        /// <summary>
        /// Returns the photosets belonging to the specified user.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotoSetsListAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                  elm => this.InvokeGetPhotoSetsListCompletedEvent(new EventArgs<PhotoSetsCollection>(token,new PhotoSetsCollection(this.authTkns,elm.Element("photosets"))))
                , e => this.InvokeGetPhotoSetsListCompletedEvent(new EventArgs<PhotoSetsCollection>(token,e))
                , null, new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.photosets.getList"), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Return the list of galleries created by a user. Sorted from newest to oldest.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="perPage">Number of galleries to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetGalleriesListAsync(Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
              elm => this.InvokeGetGalleriesListCompletedEvent(new EventArgs<GalleriesCollection>(token,new GalleriesCollection(this.authTkns,elm.Element("galleries")))),
              e => this.InvokeGetGalleriesListCompletedEvent(new EventArgs<GalleriesCollection>(token,e)), null,
              new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.galleries.getList"),
              new Parameter("user_id", this.UserID), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Returns the list of public groups a user is a member of.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPublicGroupsAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPublicGroupsEvent(new EventArgs<GroupCollection>(token,new GroupCollection(this.authTkns,elm.Element("groups")))), 
                e => this.InvokeGetPublicGroupsEvent(new EventArgs<GroupCollection>(token,e)), null,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.people.getPublicGroups"), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Get a list of configured blogs for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="serviceID">Optionally only return blogs for a given service id.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetBlogsListAsync(Nullable<int> serviceID = null)
        {
            this.authTkns.ValidateGrantedPermission(Rest.AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                 elm => this.InvokeGetBlogsListEvent(new EventArgs<BlogsCollection>(token,new BlogsCollection(elm.Element("blogs"))))
               , e => this.InvokeGetBlogsListEvent(new EventArgs<BlogsCollection>(token, e)), this.authTkns.SharedSecret
               , new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.blogs.getList")
               , new Parameter("auth_token", this.authTkns.Token), new Parameter("service", serviceID));

            return token;
        }

        /// <summary>
        /// Returns a tree (or sub tree) of collections belonging to a given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="collection">The ID of the collection to fetch a tree for, or Null to fetch the root collection.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCollectionsTreeAsync(Collection collection = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetCollectionsTreeEvent(new EventArgs<CollectionsList>(token , new CollectionsList(this.authTkns,elm.Element("collections")))),
                e => this.InvokeGetCollectionsTreeEvent(new EventArgs<CollectionsList>(token,e)), null,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.collections.getTree"), 
                new Parameter("collection_id", collection != null ? collection.ID : null ), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Returns a list of favorite public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPublicFavoritesListAsync(string minFaveDate = null , string maxFaveDate = null ,string extras = null , Nullable<int> perPage = null , Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPublicFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos")))), 
                e => this.InvokeGetPublicFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null,
                new Parameter("method", "flickr.favorites.getPublicList"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("min_fave_date", minFaveDate),
                new Parameter("max_fave_date", maxFaveDate), new Parameter("per_page", perPage), new Parameter("page", page),new Parameter("user_id",this.UserID));

            return token;
        }

        /// <summary>
        /// Returns a list of the user's favorite photos. Only photos which the calling user has permission to see are returned.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">The object that represents a Flickr User.</param>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetFavoritesListAsync(User user, string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            var uri = FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns,elm.Element("photos")))),
                e => this.InvokeGetFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.favorites.getList"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("user_id", this.UserID),
                new Parameter("min_fave_date", minFaveDate),new Parameter("max_fave_date", maxFaveDate), new Parameter("per_page", perPage),
                new Parameter("page", page), new Parameter("auth_token", this.authTkns.Token));

            return token;
        }

        /// <summary>
        /// Returns a list of the user's favorite photos. Only photos which the calling user has permission to see are returned.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetFavoritesListAsync(string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return this.GetFavoritesListAsync(this, minFaveDate, maxFaveDate, extras, perPage, page);
        }

        /// <summary>
        /// Returns a list of your photos that are not part of any sets.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="mediaType">Filter results by media type.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosNotInSetAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null, Nullable<MediaType> mediaType = null, string extras = null, Nullable<int> perPage = null,
            Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotosNotInListCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos")))) ,
                e => this.InvokeGetPhotosNotInListCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getNotInSet"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("max_upload_date",maxUploadDate), new Parameter("min_upload_date",minUploadDate), new Parameter("min_taken_date",minTakenDate) ,
                new Parameter("max_taken_date",maxTakenDate), new Parameter("privacy_filter",privacyFilter.HasValue ? (object)(int)privacyFilter : null), 
                new Parameter("media",mediaType), new Parameter("extras",extras), new Parameter("page",page), new Parameter("per_page",perPage));

            return token;
        }

        /// <summary>
        /// Returns a list of your photos with no tags.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="mediaType">Filter results by media type.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetUntaggedPhotosAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null, Nullable<MediaType> mediaType = null, string extras = null, Nullable<int> perPage = null,
            Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetUntaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeGetUntaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getUntagged"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("max_upload_date", maxUploadDate), new Parameter("min_upload_date", minUploadDate), new Parameter("min_taken_date", minTakenDate),
                new Parameter("max_taken_date", maxTakenDate), new Parameter("privacy_filter", privacyFilter.HasValue ? (object)(int)privacyFilter : null),
                new Parameter("media", mediaType), new Parameter("extras", extras), new Parameter("page", page), new Parameter("per_page", perPage));

            return token;
        }

        /// <summary>
        /// Returns a list of your geo-tagged photos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="sortType">The order in which to sort returned photos. Defaults to datePostedDescending.</param>
        /// <param name="mediaType">Filter results by media type.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetGeotaggedPhotosAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null,Nullable<SortType> sortType = null , Nullable<MediaType> mediaType = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeGetGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getWithGeoData"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("max_upload_date", maxUploadDate), new Parameter("min_upload_date", minUploadDate), new Parameter("min_taken_date", minTakenDate),
                new Parameter("max_taken_date", maxTakenDate), new Parameter("privacy_filter", privacyFilter.HasValue ? (object)(int)privacyFilter : null),
                new Parameter("sort",sortType.HasValue ? sortType.Value.GetString() : null),
                new Parameter("media", mediaType), new Parameter("extras", extras), new Parameter("page", page), new Parameter("per_page", perPage));

            return token;
        }

        /// <summary>
        /// Returns a list of your photos which haven't been geo-tagged.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="sortType">The order in which to sort returned photos. Defaults to datePostedDescending.</param>
        /// <param name="mediaType">Filter results by media type.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetUnGeotaggedPhotosAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null, Nullable<SortType> sortType = null, Nullable<MediaType> mediaType = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetUnGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeGetUnGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getWithoutGeoData"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("max_upload_date", maxUploadDate), new Parameter("min_upload_date", minUploadDate), new Parameter("min_taken_date", minTakenDate),
                new Parameter("max_taken_date", maxTakenDate), new Parameter("privacy_filter", privacyFilter.HasValue ? (object)(int)privacyFilter : null),
                new Parameter("sort", sortType.HasValue ? sortType.Value.GetString() : null),
                new Parameter("media", mediaType), new Parameter("extras", extras), new Parameter("page", page), new Parameter("per_page", perPage));

            return token;
        }

        /// <summary>
        /// Gets a list of photo counts for the given date ranges for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="dates">list of dates, denoting the periods to return counts for. They should be specified smallest first.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosCountsAsync(params DateTime[] dates)
        {
            if (dates == null)
                throw new ArgumentNullException("dates");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotosCountsCompletedEvent(new EventArgs<IEnumerable<PhotosCount>>(token,elm.Element("photocounts").Elements("photocount").Select(pc=>new PhotosCount(pc)))),
                e => this.InvokeGetPhotosCountsCompletedEvent(new EventArgs<IEnumerable<PhotosCount>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getCounts"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("dates", dates.Select(date => date.ToUnixTimeStamp().ToString()).Aggregate((left, right) => string.Format("{0} , {1}", left, right))));

            return token;
        }

        /// <summary>
        /// Return a list of your photos that have been recently created or which have been recently modified.
        ///Recently modified may mean that the photo's metadata (title, description, tags) may have been changed or a comment has been added (or just modified somehow :-)
        ///This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="minDate">a timestamp indicating the date from which modifications should be compared.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetRecentlyUpdatedPhotosAsync(DateTime minDate,string extras = null , Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetRecentlyUpdatedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos")))),
                e => this.InvokeGetRecentlyUpdatedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.recentlyUpdated"), new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("min_date", minDate.ToUnixTimeStamp()), new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Fetch a list of recent photos from the calling users' contacts.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="count">Number of photos to return. Defaults to 10, maximum 50. This is only used if single_photo is not passed.</param>
        /// <param name="justFriends">set as true to only show photos from friends and family (excluding regular contacts).</param>
        /// <param name="singlePhoto">set as true to Only fetch one photo (the latest) per contact, instead of all photos in chronological order.</param>
        /// <param name="includeSelf">Set to true to include photos from the calling user.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields include: license, date_upload, date_taken, owner_name, icon_server, original_format, last_update.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetContactsPhotosAsync(Nullable<int> count = null, Nullable<bool> justFriends = null, Nullable<bool> singlePhoto = null,
            Nullable<bool> includeSelf = null, string extras = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetContactsPhotosCompletedEvent
                    (new EventArgs<IEnumerable<Photo>>(token,elm.Element("photos").Elements("photo").Select(photo=>new Photo(this.authTkns,photo)))),
                e => this.InvokeGetContactsPhotosCompletedEvent(new EventArgs<IEnumerable<Photo>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.photos.getContactsPhotos"), new Parameter("api_key", this.authTkns.ApiKey), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("count",count),new Parameter("just_friends",justFriends),
                new Parameter("single_photos",singlePhoto), new Parameter("iclude_self",includeSelf), new Parameter("extras",extras));

            return token;
        }

        /// <summary>
        /// Fetch a list of recent public photos from a users' contacts.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="count">Number of photos to return. Defaults to 10, maximum 50. This is only used if single_photo is not passed.</param>
        /// <param name="justFriends">set as true to only show photos from friends and family (excluding regular contacts).</param>
        /// <param name="singlePhoto">set as true to Only fetch one photo (the latest) per contact, instead of all photos in chronological order.</param>
        /// <param name="includeSelf">Set to true to include photos from the user specified by user object.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields include: license, date_upload, date_taken, owner_name, icon_server, original_format, last_update.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetContactsPublicPhotosAsync(Nullable<int> count = null, Nullable<bool> justFriends = null, Nullable<bool> singlePhoto = null,
            Nullable<bool> includeSelf = null, string extras = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetContactsPublicPhotosCompletedEvent
                    (new EventArgs<IEnumerable<Photo>>(token, elm.Element("photos").Elements("photo").Select(photo => new Photo(this.authTkns, photo)))),
                e => this.InvokeGetContactsPublicPhotosCompletedEvent(new EventArgs<IEnumerable<Photo>>(token, e)), null,
                new Parameter("method", "flickr.photos.getContactsPublicPhotos"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("user_id", this.UserID), new Parameter("count", count), new Parameter("just_friends", justFriends),
                new Parameter("single_photos", singlePhoto), new Parameter("iclude_self", includeSelf), new Parameter("extras", extras));

            return token;
        }

        /// <summary>
        /// Return a list of contacts for a user who have recently uploaded photos along with the total count of photos uploaded.
        /// This method is still considered experimental. We don't plan for it to change or to go away but so long as this notice is present you should write your code accordingly.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="dateLastUpload">Limits the resultset to contacts that have uploaded photos since this date. The date should be in the form of a Unix timestamp. The default offset is (1) hour and the maximum (24) hours. </param>
        /// <param name="filter">Limit the result set to all contacts or only those who are friends or family. Valid options are:
        ///* ff friends and family
        ///* all all your contacts
        ///Default value is "all".</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetListRecentlyUploadedAsync(string dateLastUpload = null, string filter = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetListRecentlyUploadedCompletedEvent(new EventArgs<IEnumerable<Contact>>(token,elm.Element("contacts").Elements("contact").Select(cont=>new Contact(cont)))),
                e => this.InvokeGetListRecentlyUploadedCompletedEvent(new EventArgs<IEnumerable<Contact>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.contacts.getListRecentlyUploaded"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("date_lastupload", dateLastUpload), new Parameter("filter", filter));

            return token;
        }

        /// <summary>
        /// Create a new photoset for the calling user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="title">A title for the photoset.</param>
        /// <param name="primaryPhotoID">The id of the photo to represent this set. The photo must belong to the calling user.</param>
        /// <param name="description">A description of the photoset. May contain limited html.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token CreatePhotoSetAsync(string title, string primaryPhotoID, string description = null)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("title");
            if (string.IsNullOrEmpty(primaryPhotoID))
                throw new ArgumentException("primaryPhotoID");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);

            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeCreatePhotoSetCompletedEvent(new EventArgs<PhotoSetToken>(token,new PhotoSetToken(this.authTkns,elm.Element("photoset")))), 
                e => this.InvokeCreatePhotoSetCompletedEvent(new EventArgs<PhotoSetToken>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),new Parameter("method", "flickr.photosets.create"),
                new Parameter("title", title), new Parameter("description", description), new Parameter("primary_photo_id", primaryPhotoID));

            return token;
        }

        /// <summary>
        /// Set the order of photosets for the calling user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photosetsIDs">A comma delimited list of photoset IDs, ordered with the set to show first, first in the list. Any set IDs not given in the list will be set to appear at the end of the list, ordered by their IDs.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token OrderSetsAsync(params string[] photosetsIDs)
        {
            if (photosetsIDs == null)
                throw new ArgumentNullException("photosetsIDs");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeOrderSetsCompeltedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeOrderSetsCompeltedEvent(new EventArgs<NoReply>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), new Parameter("method", "flickr.photosets.orderSets"), 
                new Parameter("photoset_ids", photosetsIDs.Aggregate((left, right) => string.Format("{0},{1}", left, right))));

            return token;
        }

        /// <summary>
        /// Create a new gallery for the calling user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="title">The name of the gallery</param>
        /// <param name="description">A short description for the gallery.</param>
        /// <param name="primaryPhotoID">The first photo to add to your gallery.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token CreateGalleryAsync(string title, string description, string primaryPhotoID = null)
        {
            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("title");
            if (string.IsNullOrEmpty(description))
                throw new ArgumentException("description");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeCreateGalleryCompletedEvent(new EventArgs<GalleryToken>(token,new GalleryToken(this.authTkns,elm.Element("gallery")))), 
                e => this.InvokeCreateGalleryCompletedEvent(new EventArgs<GalleryToken>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token), 
                new Parameter("method", "flickr.galleries.create"), new Parameter("title", title), 
                new Parameter("description", description), new Parameter("primary_photo_id", primaryPhotoID));

            return token;
        }

        /// <summary>
        /// Returns a list of groups to which you can add photos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of groups to return per page. If this argument is omitted, it defaults to 400. The maximum allowed value is 400.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetGroupsAsync(Nullable<int> page = null, Nullable<int> perPage = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeGetGroupsCompletedEvent(new EventArgs<GroupCollection>(token, new GroupCollection(this.authTkns, elm.Element("groups")))), 
                e => this.InvokeGetGroupsCompletedEvent(new EventArgs<GroupCollection>(token,e)), this.authTkns.SharedSecret, 
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.groups.pools.getGroups"), new Parameter("page", page), new Parameter("per_page", perPage));

            return token;
        }

        /// <summary>
        /// Returns a list of recent activity on photos belonging to the calling user. Do not poll this method more than once an hour (as Flickr Team Recommends , Not me ;) ).
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="timeFrame">The timeframe in which to return updates for. This can be specified in days ('2d') or hours ('4h'). The default behavoir is to return changes since the beginning of the previous user session.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of items to return per page. If this argument is omitted, it defaults to 10. The maximum allowed value is 50.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetPhotosActivitesAsync(string timeFrame = null, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetPhotosActivitiesCompletedEvent(new EventArgs<ItemsCollection>(token, new ItemsCollection(this.authTkns, elm.Element("items")))),
                e => this.InvokeGetPhotosActivitiesCompletedEvent(new EventArgs<ItemsCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.activity.userPhotos"), new Parameter("per_page", perPage), 
                new Parameter("page", page), new Parameter("timeframe", timeFrame));

            return token;
        }

        /// <summary>
        /// Returns a list of recent activity on photos commented on by the calling user. Do not poll this method more than once an hour (as Flickr Team Recommends , Not me ;) ).
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of items to return per page. If this argument is omitted, it defaults to 10. The maximum allowed value is 50.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetCommentsActivitiesAsync(Nullable<int> page = null, Nullable<int> perPage = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetCommentsActivitiesCompletedEvent(new EventArgs<ItemsCollection>(token, new ItemsCollection(this.authTkns, elm.Element("items")))),
                e => this.InvokeGetCommentsActivitiesCompletedEvent(new EventArgs<ItemsCollection>(token, e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("auth_token", this.authTkns.Token),
                new Parameter("method", "flickr.activity.userComments"), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        #region Events
        private void InvokeGetCommentsActivitiesCompletedEvent(EventArgs<ItemsCollection> args)
        {
            if (this.GetCommentsActivitiesCompleted != null)
            {
                this.GetCommentsActivitiesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCommentsActivitiesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ItemsCollection>> GetCommentsActivitiesCompleted;
        private void InvokeGetPhotosActivitiesCompletedEvent(EventArgs<ItemsCollection> args)
        {
            if (this.GetPhotosActivitiesCompleted != null)
            {
                this.GetPhotosActivitiesCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosActivitiesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ItemsCollection>> GetPhotosActivitiesCompleted;
        private void InvokeGetGroupsCompletedEvent(EventArgs<GroupCollection> args)
        {
            if (this.GetGroupsCompleted != null)
            {
                this.GetGroupsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetGroupsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GroupCollection>> GetGroupsCompleted;
        private void InvokeCreateGalleryCompletedEvent(EventArgs<GalleryToken> args)
        {
            if (this.CreateGalleryCompleted != null)
            {
                this.CreateGalleryCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when CreateGalleryAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GalleryToken>> CreateGalleryCompleted;
        private void InvokeOrderSetsCompeltedEvent(EventArgs<NoReply> args)
        {
            if (this.OrderSetsCompleted != null)
            {
                this.OrderSetsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when OrderSetsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> OrderSetsCompleted;
        private void InvokeCreatePhotoSetCompletedEvent(EventArgs<PhotoSetToken> args)
        {
            if (this.CreatePhotoSetCompleted != null)
            {
                this.CreatePhotoSetCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when CreatePhotoSetAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotoSetToken>> CreatePhotoSetCompleted;
        private void InvokeGetListRecentlyUploadedCompletedEvent(EventArgs<IEnumerable<Contact>> args)
        {
            if (this.GetListRecentlyUploadedCompleted != null)
            {
                this.GetListRecentlyUploadedCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetListRecentlyUploadedAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Contact>>> GetListRecentlyUploadedCompleted;
        private void InvokeGetContactsPublicPhotosCompletedEvent(EventArgs<IEnumerable<Photo>> args)
        {
            if (this.GetContactsPublicPhotosCompleted != null)
            {
                this.GetContactsPublicPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetContactsPublicPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Photo>>> GetContactsPublicPhotosCompleted;
        private void InvokeGetContactsPhotosCompletedEvent(EventArgs<IEnumerable<Photo>> args)
        {
            if (this.GetContactsPhotosCompleted != null)
            {
                this.GetContactsPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetContactsPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<Photo>>> GetContactsPhotosCompleted;
        private void InvokeGetRecentlyUpdatedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetRecentlyUpdatedPhotosCompleted != null)
            {
                this.GetRecentlyUpdatedPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetRecentlyUpdatedPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetRecentlyUpdatedPhotosCompleted;
        private void InvokeGetPhotosCountsCompletedEvent(EventArgs<IEnumerable<PhotosCount>> args)
        {
            if (this.GetPhotosCountsCompleted != null)
            {
                this.GetPhotosCountsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosCountsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<PhotosCount>>> GetPhotosCountsCompleted;
        private void InvokeGetUnGeotaggedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetUnGeotaggedPhotosCompleted != null)
            {
                this.GetUnGeotaggedPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetUnGeotaggedPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetUnGeotaggedPhotosCompleted;
        private void InvokeGetGeotaggedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetGeotaggedPhotosCompleted != null)
            {
                this.GetGeotaggedPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetGeotaggedPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetGeotaggedPhotosCompleted;
        private void InvokeGetUntaggedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetUntaggedPhotosCompleted != null)
            {
                this.GetUntaggedPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetUntaggedPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetUntaggedPhotosCompleted;
        private void InvokeGetPhotosNotInListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPhotosNotInSetCompleted != null)
            {
                this.GetPhotosNotInSetCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosNotInSetAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetPhotosNotInSetCompleted;
        private void InvokeGetFavoritesListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetFavoritesListCompleted != null)
            {
                this.GetFavoritesListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetFavoritesListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetFavoritesListCompleted;
        private void InvokeGetPublicFavoritesListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPublicFavoritesListCompleted != null)
            {
                this.GetPublicFavoritesListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPublicFavoritesListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetPublicFavoritesListCompleted;
        private void InvokeGetCollectionsTreeEvent(EventArgs<CollectionsList> args)
        {
            if (this.GetCollectionsTreeCompleted != null)
            {
                this.GetCollectionsTreeCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetCollectionsTreeAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<CollectionsList>> GetCollectionsTreeCompleted;
        private void InvokeGetBlogsListEvent(EventArgs<BlogsCollection> args)
        {
            if (this.GetBlogsListCompleted != null)
            {
                this.GetBlogsListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetBlogsListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<BlogsCollection>> GetBlogsListCompleted;
        private void InvokeGetPublicGroupsEvent(EventArgs<GroupCollection> args)
        {
            if (this.GetPublicGroupsCompleted != null)
            {
                this.GetPublicGroupsCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPublicGroupsAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GroupCollection>> GetPublicGroupsCompleted;
        private void InvokeGetGalleriesListCompletedEvent(EventArgs<GalleriesCollection> args)
        {
            if (this.GetGalleriesListCompleted != null)
            {
                this.GetGalleriesListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetGalleriesListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GalleriesCollection>> GetGalleriesListCompleted;
        private void InvokeGetPhotoSetsListCompletedEvent(EventArgs<PhotoSetsCollection> args)
        {
            if (this.GetPhotoSetsListCompleted != null)
            {
                this.GetPhotoSetsListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotoSetsListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotoSetsCollection>> GetPhotoSetsListCompleted;
        private void InvokeGetInfoCompletedEvent(EventArgs<UserInfo> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetInfoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<UserInfo>> GetInfoCompleted;
        private void InvokeGetUserPhotosCompletedEvent(EventArgs<PhotosOfUserCollection> args)
        {
            if (this.GetUserPhotosCompleted != null)
            {
                this.GetUserPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetUserPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosOfUserCollection>> GetUserPhotosCompleted;
        private void InvokeGetPublicPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPublicPhotosCompleted != null)
            {
                this.GetPublicPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPublicPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetPublicPhotosCompleted;
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
        private void InvokeGetPublicContactsListCompletedEvent(EventArgs<ContactsList> args)
        {
            if (this.GetPublicContactsListCompleted != null)
            {
                this.GetPublicContactsListCompleted(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPublicContactsListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ContactsList>> GetPublicContactsListCompleted;
        private void InvokeGetContactsListCompletedEvent(EventArgs<ContactsList> args)
        {
            if (this.GetContactsListCompleted != null)
            {
                this.GetContactsListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetGetContsctsListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<ContactsList>> GetContactsListCompleted;
        #endregion

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of User Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(User left, User right)
        {
            if (left is User)
                return left.Equals(right);
            else if (right is User)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of User Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(User left, User right)
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
            return obj is User && this.UserID == ((User)obj).UserID;
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
    /// Content Type setting.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        ///  for photos only.
        /// </summary>
        Photos =1 ,
        /// <summary>
        ///  for screenshots only.
        /// </summary>
        ScreenShots = 2 ,
        /// <summary>
        /// for 'other' only.
        /// </summary>
        Others =3 ,
        /// <summary>
        /// for photos and screenshots.
        /// </summary>
        PhotosScreenShots = 4 ,
        /// <summary>
        /// for screenshots and 'other'.
        /// </summary>
        ScreenShotsandOthers = 5 ,
        /// <summary>
        /// for photos and 'other'.
        /// </summary>
        PhotosOthers = 6 ,
        /// <summary>
        /// for photos, screenshots, and 'other' (all).
        /// </summary>
        PhotosScreenShotsOthers = 7
    }

    /// <summary>
    /// Privacy Filter Setting.
    /// </summary>
    public enum PrivacyFilter
    {
        /// <summary>
        /// public photos.
        /// </summary>
        PublicPhotos =1 ,
        /// <summary>
        /// private photos visible to friends.
        /// </summary>
        VisibleToFriendsOnly = 2,
        /// <summary>
        /// private photos visible to family.
        /// </summary>
        VisibleToFamilyOnly = 3,
        /// <summary>
        /// private photos visible to friends and family.
        /// </summary>
        VisibleToFriendsandFamilyOnly = 4,
        /// <summary>
        /// completely private photos.
        /// </summary>
        PrivatePhotos = 5
    }

    /// <summary>
    /// Media Type Setting.
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// Photos and Videos.
        /// </summary>
        All=0 ,
        /// <summary>
        /// Photos only.
        /// </summary>
        Photos=1 ,
        /// <summary>
        /// Videos only.
        /// </summary>
        Videos =2
    }

    /// <summary>
    /// The order in which to sort returned photos.
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// Date Posted Descending.
        /// </summary>
        DatePostedDescending,
        /// <summary>
        /// Date Posted Ascending.
        /// </summary>
        DatePostedAscending,
        /// <summary>
        /// Date Taken Ascending.
        /// </summary>
        DateTakenAscending,
        /// <summary>
        /// Date Taken Descending.
        /// </summary>
        DateTakenDescending,
        /// <summary>
        /// Interestingness Descending.
        /// </summary>
        InterestingnessDescending,
        /// <summary>
        /// Interestingness Ascending.
        /// </summary>
        InterestingnessAscending        
    }

    internal static class SortTypeExtensions
    {
        public static string GetString(this SortType sortType)
        {
            switch (sortType)
            {
                case SortType.DatePostedDescending:
                    return "date-posted-desc";
                case SortType.DatePostedAscending:
                    return "date-posted-asc";
                case SortType.DateTakenAscending:
                    return "date-taken-asc";
                case SortType.DateTakenDescending:
                    return "date-taken-desc";
                case SortType.InterestingnessDescending:
                    return "interestingness-desc";
                case SortType.InterestingnessAscending:
                    return "interestingness-asc";
                default:
                    throw new ArgumentException("sortType");
            }
        }
    }

    /// <summary>
    /// represents the information of Flickr User.
    /// </summary>
    public class UserInfo
    {
        private XElement data;

        internal UserInfo(XElement element)
        {
            this.data = element;
        }

        /// <summary>
        /// the user name.
        /// </summary>
        public string UserName { get { return data.Attribute("username").Value; } }

        /// <summary>
        /// the Real user name.
        /// </summary>
        public string RealName { get { return data.Element("realname").Value;} }

        /// <summary>
        /// the User ID.
        /// </summary>
        public string UserID { get { return data.Attribute("id").Value; } }

        /// <summary>
        /// determine if the user has Pro Account.
        /// </summary>
        public bool IsProUser { get { return data.Attribute("ispro").Value.ToBoolean(); } }

        /// <summary>
        /// the number of the server the icon resides on.
        /// </summary>
        public int IconServer { get { return int.Parse(data.Attribute("iconserver").Value); } }

        /// <summary>
        /// the number of server farm  the icon resides on.
        /// </summary>
        public int IconFarm { get { return int.Parse(data.Attribute("iconfarm").Value); } }

        /// <summary>
        /// the Path Alias (used when Generating Urls ) , Could be Empty when not set by the user.
        /// </summary>
        public string PathAlias { get { return data.Attribute("path_alias").Value; } }
        
        /// <summary>
        /// http://markmail.org/message/2poskzlsgdjjt7ow , could be Null.
        /// </summary>
        public string mbox_sha1sum { get { return data.Element("mbox_sha1sum") != null ? data.Element("mbox_sha1sum").Value : null; } }

        /// <summary>
        /// The Location of the User , Could be Null.
        /// </summary>
        public string Location { get { return data.Element("location").Value; } }

        /// <summary>
        /// the url that leads to Photostream of the user.
        /// </summary>
        public Uri PhotosUrl { get { return new Uri(data.Element("photosurl").Value); } }

        /// <summary>
        /// the url that leads to the user profile.
        /// </summary>
        public Uri ProfileUrl { get { return new Uri(data.Element("profileurl").Value); } }

        /// <summary>
        /// the url that leads to the Photostream page of the user that ready to be displayed on mobile device .
        /// </summary>
        public Uri MobileUrl { get { return new Uri(data.Element("mobileurl").Value); } }

        /// <summary>
        /// contains the datetime of the first photo taken by the user.
        /// </summary>
        public DateTime FirstDateTaken { get { return DateTime.Parse(data.Element("photos").Element("firstdatetaken").Value); } }

        /// <summary>
        /// contains the timestamp of the first photo uploaded by the user.
        /// </summary>
        public DateTime FirstDate { get { return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(data.Element("photos").Element("firstdate").Value)); } }

        /// <summary>
        /// the number of photos the user has Uploaded.
        /// </summary>
        public int PhotosCount { get { return int.Parse(data.Element("photos").Element("count").Value); } }

        /// <summary>
        /// the number of views by Flickr users for the Photostream page of the User, could be Null.
        /// </summary>
        public Nullable<int> PhotoStreamViews { get { return data.Element("photos").Element("views") != null ? new Nullable<int>(int.Parse(data.Element("photos").Element("views").Value)) : null; } }

        /// <summary>
        /// the Gender of User , could be Null.
        /// </summary>
        public string Geneder { get { return data.Attribute("geneder") != null ? data.Element("gender").Value : null; } }

        /// <summary>
        /// determine if the user is ignored by you.
        /// </summary>
        public Nullable<bool> IsIgnored { get { return data.Attribute("igonred") != null ? new Nullable<bool>(data.Attribute("ignored").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether the user is a contact in your contact list or Not.
        /// </summary>
        public Nullable<bool> IsContact { get { return data.Attribute("contact") != null ? new Nullable<bool>(data.Attribute("contact").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether you are marking the user as a family in you contact list or Not.
        /// </summary>
        public Nullable<bool> IsFamily { get { return data.Attribute("family") != null ? new Nullable<bool>(data.Attribute("family").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether you are marking the user as a friend  in you contact list or Not.
        /// </summary>
        public Nullable<bool> IsFriend { get { return data.Attribute("friend") != null ? new Nullable<bool>(data.Attribute("friend").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether the user is marking you as a contact or Not.
        /// </summary>
        public Nullable<bool> IsConsideringYouAsContact { get { return data.Attribute("revcontact") != null ? new Nullable<bool>(data.Attribute("revcontact").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether the user is marking you as a family or Not.
        /// </summary>
        public Nullable<bool> IsConsideringYouAsFriend { get { return data.Attribute("revfriend") != null ? new Nullable<bool>(data.Attribute("revfriend").Value.ToBoolean()) : null;  } }

        /// <summary>
        /// determine whether the user is marking you as a friend or Not.
        /// </summary>
        public Nullable<bool> IsConsideringYouAsFamily { get { return data.Attribute("revfamily") != null ? new Nullable<bool>(data.Attribute("revfamily").Value.ToBoolean()) : null; } }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of UserInfo Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(UserInfo left, UserInfo right)
        {
            if (left is UserInfo)
                return left.Equals(right);
            else if (right is UserInfo)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of UserInfo Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(UserInfo left, UserInfo right)
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
            return obj is UserInfo && this.UserID == ((UserInfo)obj).UserID;
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
    /// represents  photo counts in a given time period.
    /// </summary>
    public class PhotosCount
    {
        internal PhotosCount(XElement element)
        {
            this.Count = int.Parse(element.Attribute("count").Value);
            this.FromDate = double.Parse(element.Attribute("fromdate").Value).ToDateTimeFromUnix();
            this.ToDate = double.Parse(element.Attribute("todate").Value).ToDateTimeFromUnix();
        }

        /// <summary>
        /// the number of photo in this period.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// start time of counting.
        /// </summary>
        public DateTime FromDate { get; private set; }

        /// <summary>
        /// end time of counting.
        /// </summary>
        public DateTime ToDate { get; private set; }
    }

    /// <summary>
    /// Tag Mode in Searching.
    /// </summary>
    public enum TagMode
	{
        /// <summary>
        /// 'any' for an OR combination of tags.
        /// </summary>
        Any = 0,
        /// <summary>
        /// 'all' for an AND combination.
        /// </summary>
        All = 1
	}

    /// <summary>
    /// Geo Context Setting .
    /// </summary>
    public enum GeoContext
	{
        /// <summary>
        /// not defined.
        /// </summary>
        NotDefined = 0,
        /// <summary>
        /// indoors.
        /// </summary>
        InDoors = 1,
        /// <summary>
        /// outdoors.
        /// </summary>
        OutDoors = 2
	}

    /// <summary>
    /// Radius Unit Setting.
    /// </summary>
    public enum RadiusUnits
    {
        /// <summary>
        /// Miles.
        /// </summary>
        Mi = 0,
        /// <summary>
        /// Kilometers.
        /// </summary>
        Km = 1
    }

    /// <summary>
    /// represents collection of items.
    /// </summary>
    public class ItemsCollection : IEnumerable<Item>
    {
        private XElement data;

        private readonly AuthenticationTokens authtkns;

        internal ItemsCollection(AuthenticationTokens authtkns,XElement element)
        {
            this.authtkns = authtkns;
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
        /// the number of items per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the Total Number of items .
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Enumerable of Item Objects.
        /// </summary>
        public IEnumerable<Item> Items { get { return this.data.Elements("item").Select(item => new Item(this.authtkns,item)); } }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Item> GetEnumerator()
        {
            foreach (var item in this.Items)
                yield return item;
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
    /// represents an item that has an activity.
    /// </summary>
    public class Item : IEnumerable<Event>
    {
        private XElement data;

        private readonly AuthenticationTokens authtkns;

        internal Item(AuthenticationTokens authtkns,XElement element)
        {
            this.authtkns = authtkns;
            this.data = element;
            this.ID = element.Attribute("id").Value;
            this.OwnerID = element.Attribute("owner").Value;
            this.OwnerName = element.Attribute("ownername").Value;
            this.Primary = element.Attribute("primary") != null ? element.Attribute("primary").Value : null;
            this.Secret = element.Attribute("secret").Value;
            this.Server = int.Parse(element.Attribute("server").Value);
            this.Farm = int.Parse(element.Attribute("farm").Value);
            this.CommentsCount = int.Parse(element.Attribute("comments").Value);
            this.ViewsCount = int.Parse(element.Attribute("views").Value);
            this.FavesCount = element.Attribute("faves") != null ? new Nullable<int>(int.Parse(element.Attribute("faves").Value)) : null;
            this.NotesCount = element.Attribute("notes") != null ? new Nullable<int>(int.Parse(element.Attribute("notes").Value)) : null;
            this.PhotosCount = element.Attribute("photos") != null ? new Nullable<int>(int.Parse(element.Attribute("photos").Value)) : null;
            this.Type = ItemTypeExtensions.GetValue(element.Attribute("type").Value);
        }

        /// <summary>
        /// the Type of the Item.
        /// </summary>
        public ItemType Type { get; private set; }

        /// <summary>
        /// the ID of the Item.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Owner ID.
        /// </summary>
        public string OwnerID { get; private set; }

        /// <summary>
        /// the Owner Name.
        /// </summary>
        public string OwnerName { get; private set; }

        /// <summary>
        /// the Secret of the Item.
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Server number which the item is on.
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// The server Farm number which the item is on.
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the Total Number of Comments on the Item.
        /// </summary>
        public int CommentsCount { get; private set; }

        /// <summary>
        /// the Total Number of Notes on the Item , Could Be Null.
        /// </summary>
        public Nullable<int> NotesCount { get; private set; }

        /// <summary>
        /// the Total Number of Views of the Item.
        /// </summary>
        public int ViewsCount { get; private set; }

        /// <summary>
        /// the Total Number of favorites of this Item , Could Be Null.
        /// </summary>
        public Nullable<int> FavesCount { get; private set; }

        /// <summary>
        /// the Number of the Photos in the Photoset , Could Be Null.
        /// </summary>
        public Nullable<int> PhotosCount { get; private set; }

        /// <summary>
        /// the primary photo ID of the Photoset , Could Be Null
        /// </summary>
        public string Primary { get; private set; }

        /// <summary>
        /// Enumerable of Event Objects that represents the Activity on the Item.
        /// </summary>
        public IEnumerable<Event> Activities { get { return this.data.Element("activity").Elements("event").Select(evnt => new Event(this.authtkns,evnt)); } }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Event> GetEnumerator()
        {
            foreach (var evnt in this.Activities)
                yield return evnt;
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Item Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Item left, Item right)
        {
            if (left is Item)
                return left.Equals(right);
            else if (right is Item)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Item Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Item left, Item right)
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
            return obj is Item && this.ID == ((Item)obj).ID;
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
    /// represents an Event Info on an Item.
    /// </summary>
    public class Event
    {
        private XElement data;

        private readonly AuthenticationTokens authtkns;

        internal Event(AuthenticationTokens authtkns,XElement element)
        {
            this.data = element;
            this.authtkns = authtkns;
            this.UserID = element.Attribute("user").Value;
            this.UserName = element.Attribute("username").Value;
            this.DateCreated = double.Parse(element.Attribute("dateadded").Value).ToDateTimeFromUnix();
            this.CommentID = element.Attribute("commentid") != null ? element.Attribute("commentid").Value : null;
            this.CommentID = element.Attribute("noteid") != null ? element.Attribute("noteid").Value : null;
            this.Content = element.Value;
            this.type = EventTypeExtensions.GetValue(element.Attribute("type").Value);
        }

        /// <summary>
        /// the User that Created the Event.
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// the Name of the User that Created the Event.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// the date when the Event was Created.
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// the ID of the Comment , Could Be Null.
        /// </summary>
        public string CommentID { get; private set; }

        /// <summary>
        /// the Content of the Event.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// the Type of the Event.
        /// </summary>
        public EventType type { get; private set; }

        /// <summary>
        /// the Note ID , Could Be Null.
        /// </summary>
        public string NoteID { get; private set; }
    }

    /// <summary>
    /// represents the Type of an Event.
    /// </summary>
    public enum EventType 
    {
        /// <summary>
        /// a Comment Event.
        /// </summary>
        Comment = 0,
        /// <summary>
        /// a Favorite Event.
        /// </summary>
        Favorite =1,
        /// <summary>
        /// a Tag Addition.
        /// </summary>
        Tag =2,
        /// <summary>
        /// a Note Addition.
        /// </summary>
        Note =3
    }

    /// <summary>
    /// represents the Item types.
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// a photo.
        /// </summary>
        Photo = 0,
        /// <summary>
        /// a Photoset.
        /// </summary>
        Photoset = 1
    }

    internal static class EventTypeExtensions
    {
        public static EventType GetValue(string value)
        {
            switch (value)
            {
                case "fave":
                    return EventType.Favorite;
                case "comment":
                    return EventType.Comment;
                case "tag":
                    return EventType.Tag;
                case "note":
                    return EventType.Note;
                default:
                    throw new ArgumentException("value");
            }
        }
    }

    internal static class ItemTypeExtensions
    {
        public static ItemType GetValue(string value)
        {
            switch (value)
            {
                case "photo":
                    return ItemType.Photo;
                case "photoset":
                    return ItemType.Photoset;
                default:
                    throw new ArgumentException("value");
            }
        }
    }
}
