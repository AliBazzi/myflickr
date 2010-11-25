using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a Flickr User
    /// </summary>
    public class User
    {
        private readonly AuthenticationTokens authTkns;

        /// <summary>
        /// Flickr API application key
        /// </summary>
        public string ApiKey { get { return this.authTkns.ApiKey; } }

        /// <summary>
        /// A shared secret for the api key that is issued by flickr , Could Be Null
        /// </summary>
        public string SharedSecret { get { return this.authTkns.SharedSecret; } }

        /// <summary>
        /// The User ID
        /// </summary>
        public string UserID { get { return this.authTkns.UserID; } }

        /// <summary>
        /// The Access Permission
        /// </summary>
        public AccessPermission AccessPermission { get { return this.authTkns.AccessPermission ; } }

        /// <summary>
        /// the Full Name of the User , Could Be Null
        /// </summary>
        public string FullName { get { return this.authTkns.FullName ; } }

        /// <summary>
        /// the UserName , Could be Null
        /// </summary>
        public string UserName { get { return this.authTkns.UserName ; } }

        /// <summary>
        /// the Authentication Token , Could Be Null
        /// </summary>
        public string Token { get { return this.authTkns.Token ; } }

        internal User(AuthenticationTokens tokens)
        {
            this.authTkns = tokens;
        }

        /// <summary>
        /// Creates an object that represents a Flickr User
        /// </summary>
        /// <param name="apiKey">Flickr API application key</param>
        /// <param name="userID">The User ID</param>
        public User(string apiKey, string userID)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (string.IsNullOrEmpty(userID))
                throw new ArgumentException("userID");
            this.authTkns = new AuthenticationTokens(apiKey, null, null, Rest.AccessPermission.None, userID, null, null);
        }

        /// <summary>
        /// Get a list of contacts for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="contactFilter">An optional filter of the results. The following values are valid:friends,family,both,neither</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetContactsListAsync(Nullable<ContactFilter> contactFilter = null, Nullable<int> page = null , Nullable<int> perPage = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = MyFlickr.Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
              element => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token,new ContactsList(element.Element("contacts")))) 
            , e => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token, e))
            , this.SharedSecret,
            new Parameter("method", "flickr.contacts.getList"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.authTkns.Token)
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicContactsListAsync(User user, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
              element => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, new ContactsList(element.Element("contacts"))))
            , e => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, e))
            , this.SharedSecret, new Parameter("method", "flickr.contacts.getPublicList"),new Parameter("user_id",user.UserID),
            new Parameter("api_key", this.ApiKey) , new Parameter("page", page), new Parameter("per_page", perPage));

            return token;
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicContactsListAsync(Nullable<int> page = null, Nullable<int> perPage = null)
        {
            return this.GetPublicContactsListAsync(this, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// to return public photos for a user, use User.getPublicPhotos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="userID">The NSID of the user who's photos to return. A value of "me" will return the calling user's photos.</param>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="contentType">Content Type setting</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view photos you own.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
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
        /// <param name="user">User Object that represents a Flickr User</param>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date should be in the form of a mysql datetime. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="contentType">Content Type setting</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view photos you own.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotosAsync(User user, Nullable<SafetyLevel> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
            , string minTakenDate = null, string maxTakenDate = null, Nullable<ContentType> contentType = null, Nullable<PrivacyFilter> privacyFilter = null
            , string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                  elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns,elm.Element("photos")))),
                  e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                  new Parameter("method", "flickr.people.getPhotos"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token)
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
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicPhotosAsync(User user, Nullable<SafetyLevel> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                  elm => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos"))))
                , e => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null
                , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPublicPhotos"), new Parameter("user_id", user.UserID)
                , new Parameter("safe_search", safeSearch.HasValue ? (object)(int)safeSearch: null) , new Parameter("extras", extras),
                  new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicPhotosAsync(Nullable<SafetyLevel> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return this.GetPublicPhotosAsync(this, safeSearch, extras, perPage, page);
        }

        /// <summary>
        /// Returns a list of photos containing a particular Flickr member.
        /// This method does not require authentication. but when called when having at least read permission ,  you will get private photos
        /// </summary>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotosOfUserAsync(string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();
            if (this.AccessPermission > AccessPermission.None)
            {
                FlickrCore.IntiateGetRequest(
                    elm => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosOfUserCollection>(token, new PhotosOfUserCollection(this.authTkns,elm.Element("photos"))))
                    , e => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosOfUserCollection>(token, e)), this.SharedSecret
                    , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPhotosOf"), new Parameter("user_id", this.UserID)
                    , new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page),new Parameter("auth_token",this.Token));
            }
            else
            {
                FlickrCore.IntiateGetRequest(
                    elm => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosOfUserCollection>(token, new PhotosOfUserCollection(this.authTkns,elm.Element("photos"))))
                    , e => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosOfUserCollection>(token, e)), null
                    , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPhotosOf"), new Parameter("user_id", this.UserID)
                    , new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));
            }
            return token;
        }

        /// <summary>
        /// Get information about the user.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetInfoAsync()
        {
            Token token = Core.Token.GenerateToken();

            if (this.AccessPermission > AccessPermission.None)
            {
                FlickrCore.IntiateGetRequest(
                    elm => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token,new UserInfo(elm.Element("person"))))
                    , e => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token,e)), this.SharedSecret
                    , new Parameter("api_key", this.ApiKey),new Parameter("auth_token",this.Token)
                    ,new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", this.UserID));
            }
            else
            {
                FlickrCore.IntiateGetRequest(
                    elm => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, new UserInfo(elm.Element("person"))))
                    , e => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, e)), null,
                    new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", this.UserID));
            }

            return token;
        }

        /// <summary>
        /// Get information about a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetInfoAsync(User user)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            if (user == null)
                throw new ArgumentNullException("user");

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                    elm => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, new UserInfo(elm.Element("person"))))
                    , e => this.InvokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, e)), this.SharedSecret
                    , new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token)
                    , new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", user.UserID));

            return token;
        }

        /// <summary>
        /// Returns the photosets belonging to the specified user.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotoSetsListAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                  elm => this.InvokeGetPhotoSetsListCompletedEvent(new EventArgs<PhotoSetsCollection>(token,new PhotoSetsCollection(this.authTkns,elm.Element("photosets"))))
                , e => this.InvokeGetPhotoSetsListCompletedEvent(new EventArgs<PhotoSetsCollection>(token,e))
                , null, new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.photosets.getList"), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Return the list of galleries created by a user. Sorted from newest to oldest.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="perPage">Number of galleries to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetGalleriesListAsync(Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
              elm => this.InvokeGetGalleriesListCompletedEvent(new EventArgs<GalleriesCollection>(token,new GalleriesCollection(elm.Element("galleries")))),
              e => this.InvokeGetGalleriesListCompletedEvent(new EventArgs<GalleriesCollection>(token,e)), null,
              new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.galleries.getList"),
              new Parameter("user_id", this.UserID), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Returns the list of public groups a user is a member of.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicGroupsAsync()
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPublicGroupsEvent(new EventArgs<GroupCollection>(token,new GroupCollection(elm.Element("groups")))), 
                e => this.InvokeGetPublicGroupsEvent(new EventArgs<GroupCollection>(token,e)), null,
                new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPublicGroups"), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Get a list of configured blogs for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="serviceID">Optionally only return blogs for a given service id.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetBlogsListAsync(Nullable<int> serviceID = null)
        {
            this.authTkns.ValidateGrantedPermission(Rest.AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                 elm => this.InvokeGetBlogsListEvent(new EventArgs<BlogsCollection>(token,new BlogsCollection(elm.Element("blogs"))))
               , e => this.InvokeGetBlogsListEvent(new EventArgs<BlogsCollection>(token,e)), this.SharedSecret
               , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.blogs.getList")
               , new Parameter("auth_token", this.Token), new Parameter("service", serviceID));

            return token;
        }

        /// <summary>
        /// Returns a tree (or sub tree) of collections belonging to a given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="collectionID">The ID of the collection to fetch a tree for, or Null to fetch the root collection.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetCollectionsTreeAsync(Collection collection = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetCollectionsTreeEvent(new EventArgs<CollectionsList>(token , new CollectionsList(this.authTkns,elm.Element("collections")))),
                e => this.InvokeGetCollectionsTreeEvent(new EventArgs<CollectionsList>(token,e)), null,
                new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.collections.getTree"), 
                new Parameter("collection_id", collection != null ? collection.ID : null ), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Returns a list of favorite public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicFavoritesListAsync(string minFaveDate = null , string maxFaveDate = null ,string extras = null , Nullable<int> perPage = null , Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPublicFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos")))), 
                e => this.InvokeGetPublicFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null,
                new Parameter("method", "flickr.favorites.getPublicList"), new Parameter("api_key", this.ApiKey), new Parameter("min_fave_date", minFaveDate),
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
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetFavoritesListAsync(User user, string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            var uri = FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns,elm.Element("photos")))),
                e => this.InvokeGetFavoritesListCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                new Parameter("method", "flickr.favorites.getList"), new Parameter("api_key", this.ApiKey),new Parameter("user_id", this.UserID),
                new Parameter("min_fave_date", minFaveDate),new Parameter("max_fave_date", maxFaveDate), new Parameter("per_page", perPage), 
                new Parameter("page", page), new Parameter("auth_token",this.Token));

            return token;
        }

        /// <summary>
        /// Returns a list of the user's favorite photos. Only photos which the calling user has permission to see are returned.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
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
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotosNotInSetAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null, Nullable<MediaType> mediaType = null, string extras = null, Nullable<int> perPage = null,
            Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPhotosNotInListCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos")))) ,
                e => this.InvokeGetPhotosNotInListCompletedEvent(new EventArgs<PhotosCollection>(token,e)), this.SharedSecret ,
                new Parameter("method","flickr.photos.getNotInSet"), new Parameter("api_key",this.ApiKey), new Parameter("auth_token",this.Token) ,
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
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetUntaggedPhotosAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null, Nullable<MediaType> mediaType = null, string extras = null, Nullable<int> perPage = null,
            Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetUntaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeGetUntaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                new Parameter("method", "flickr.photos.getUntagged"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token),
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
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetGeotaggedPhotosAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null,Nullable<SortType> sortType = null , Nullable<MediaType> mediaType = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeGetGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                new Parameter("method", "flickr.photos.getWithGeoData"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token),
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
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetUnGeotaggedPhotosAsync(string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null, string maxTakenDate = null,
            Nullable<PrivacyFilter> privacyFilter = null, Nullable<SortType> sortType = null, Nullable<MediaType> mediaType = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetUnGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns, elm.Element("photos")))),
                e => this.InvokeGetUnGeotaggedPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                new Parameter("method", "flickr.photos.getWithoutGeoData"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token),
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotosCountsAsync(params DateTime[] dates)
        {
            if (dates == null)
                throw new ArgumentNullException("dates");

            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
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
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetRecentlyUpdatedPhotosAsync(DateTime minDate,string extras = null , Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetContactsPhotosAsync(Nullable<int> count = null, Nullable<bool> justFriends = null, Nullable<bool> singlePhoto = null,
            Nullable<bool> includeSelf = null, string extras = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetContactsPublicPhotosAsync(Nullable<int> count = null, Nullable<bool> justFriends = null, Nullable<bool> singlePhoto = null,
            Nullable<bool> includeSelf = null, string extras = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
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
        /// <param name="date_lastUpload">Limits the resultset to contacts that have uploaded photos since this date. The date should be in the form of a Unix timestamp. The default offset is (1) hour and the maximum (24) hours. </param>
        /// <param name="filter">Limit the result set to all contacts or only those who are friends or family. Valid options are:
        ///* ff friends and family
        ///* all all your contacts
        ///Default value is "all".</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetListRecentlyUploadedAsync(string dateLastUpload = null, string filter = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetListRecentlyUploadedCompletedEvent(new EventArgs<IEnumerable<Contact>>(token,elm.Element("contacts").Elements("contact").Select(cont=>new Contact(cont)))),
                e => this.InvokeGetListRecentlyUploadedCompletedEvent(new EventArgs<IEnumerable<Contact>>(token,e)), this.authTkns.SharedSecret,
                new Parameter("method", "flickr.contacts.getListRecentlyUploaded"), new Parameter("api_key", this.authTkns.ApiKey),
                new Parameter("auth_token", this.authTkns.Token), new Parameter("date_lastupload", dateLastUpload), new Parameter("filter", filter));

            return token;
        }

        #region Events
        private void InvokeGetListRecentlyUploadedCompletedEvent(EventArgs<IEnumerable<Contact>> args)
        {
            if (this.GetListRecentlyUploadedCompleted != null)
            {
                this.GetListRecentlyUploadedCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Contact>>> GetListRecentlyUploadedCompleted;
        private void InvokeGetContactsPublicPhotosCompletedEvent(EventArgs<IEnumerable<Photo>> args)
        {
            if (this.GetContactsPublicPhotosCompleted != null)
            {
                this.GetContactsPublicPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Photo>>> GetContactsPublicPhotosCompleted;
        private void InvokeGetContactsPhotosCompletedEvent(EventArgs<IEnumerable<Photo>> args)
        {
            if (this.GetContactsPhotosCompleted != null)
            {
                this.GetContactsPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<Photo>>> GetContactsPhotosCompleted;
        private void InvokeGetRecentlyUpdatedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetRecentlyUpdatedPhotosCompleted != null)
            {
                this.GetRecentlyUpdatedPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetRecentlyUpdatedPhotosCompleted;
        private void InvokeGetPhotosCountsCompletedEvent(EventArgs<IEnumerable<PhotosCount>> args)
        {
            if (this.GetPhotosCountsCompleted != null)
            {
                this.GetPhotosCountsCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<PhotosCount>>> GetPhotosCountsCompleted;
        private void InvokeGetUnGeotaggedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetUnGeotaggedPhotosCompleted != null)
            {
                this.GetUnGeotaggedPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetUnGeotaggedPhotosCompleted;
        private void InvokeGetGeotaggedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetGeotaggedPhotosCompleted != null)
            {
                this.GetGeotaggedPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetGeotaggedPhotosCompleted;
        private void InvokeGetUntaggedPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetUntaggedPhotosCompleted != null)
            {
                this.GetUntaggedPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetUntaggedPhotosCompleted;
        private void InvokeGetPhotosNotInListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPhotosNotInSetCompleted != null)
            {
                this.GetPhotosNotInSetCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetPhotosNotInSetCompleted;
        private void InvokeGetFavoritesListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetFavoritesListCompleted != null)
            {
                this.GetFavoritesListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetFavoritesListCompleted;
        private void InvokeGetPublicFavoritesListCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPublicFavoritesListCompleted != null)
            {
                this.GetPublicFavoritesListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetPublicFavoritesListCompleted;
        private void InvokeGetCollectionsTreeEvent(EventArgs<CollectionsList> args)
        {
            if (this.GetCollectionsTreeCompleted != null)
            {
                this.GetCollectionsTreeCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<CollectionsList>> GetCollectionsTreeCompleted;
        private void InvokeGetBlogsListEvent(EventArgs<BlogsCollection> args)
        {
            if (this.GetBlogsListCompleted != null)
            {
                this.GetBlogsListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<BlogsCollection>> GetBlogsListCompleted;
        private void InvokeGetPublicGroupsEvent(EventArgs<GroupCollection> args)
        {
            if (this.GetPublicGroupsCompleted != null)
            {
                this.GetPublicGroupsCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<GroupCollection>> GetPublicGroupsCompleted;
        private void InvokeGetGalleriesListCompletedEvent(EventArgs<GalleriesCollection> args)
        {
            if (this.GetGalleriesListCompleted != null)
            {
                this.GetGalleriesListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<GalleriesCollection>> GetGalleriesListCompleted;
        private void InvokeGetPhotoSetsListCompletedEvent(EventArgs<PhotoSetsCollection> args)
        {
            if (this.GetPhotoSetsListCompleted != null)
            {
                this.GetPhotoSetsListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotoSetsCollection>> GetPhotoSetsListCompleted;
        private void InvokeGetInfoCompletedEvent(EventArgs<UserInfo> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<UserInfo>> GetInfoCompleted;
        private void InvokeGetUserPhotosCompletedEvent(EventArgs<PhotosOfUserCollection> args)
        {
            if (this.GetUserPhotosCompleted != null)
            {
                this.GetUserPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosOfUserCollection>> GetUserPhotosCompleted;
        private void InvokeGetPublicPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPublicPhotosCompleted != null)
            {
                this.GetPublicPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetPublicPhotosCompleted;
        private void InvokeGetPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPhotosCompleted != null)
            {
                this.GetPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetPhotosCompleted;
        private void InvokeGetPublicContactsListCompletedEvent(EventArgs<ContactsList> args)
        {
            if (this.GetPublicContactsListCompleted != null)
            {
                this.GetPublicContactsListCompleted(this, args);
            }
        }
        public event EventHandler<EventArgs<ContactsList>> GetPublicContactsListCompleted;
        private void InvokeGetContactsListCompletedEvent(EventArgs<ContactsList> args)
        {
            if (this.GetContactsListCompleted != null)
            {
                this.GetContactsListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<ContactsList>> GetContactsListCompleted;
        #endregion
    }

    /// <summary>
    /// Content Type setting
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
    /// Privacy Filter Setting
    /// </summary>
    public enum PrivacyFilter
    {
        /// <summary>
        /// public photos
        /// </summary>
        PublicPhotos =1 ,
        /// <summary>
        /// private photos visible to friends
        /// </summary>
        VisibleToFriendsOnly = 2,
        /// <summary>
        /// private photos visible to family
        /// </summary>
        VisibleToFamilyOnly = 3,
        /// <summary>
        /// private photos visible to friends & family
        /// </summary>
        VisibleToFriendsandFamilyOnly = 4,
        /// <summary>
        /// completely private photos
        /// </summary>
        PrivatePhotos = 5
    }

    /// <summary>
    /// Media Type Setting
    /// </summary>
    public enum MediaType
    {
        /// <summary>
        /// Photos and Videos
        /// </summary>
        All=0 ,
        /// <summary>
        /// Photos only
        /// </summary>
        Photos=1 ,
        /// <summary>
        /// Videos only
        /// </summary>
        Videos =2
    }

    /// <summary>
    /// The order in which to sort returned photos.
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// Date Posted Descending
        /// </summary>
        DatePostedDescending,
        /// <summary>
        /// Date Posted Ascending
        /// </summary>
        DatePostedAscending,
        /// <summary>
        /// Date Taken Ascending
        /// </summary>
        DateTakenAscending,
        /// <summary>
        /// Date Taken Descending
        /// </summary>
        DateTakenDescending,
        /// <summary>
        /// Interestingness Descending
        /// </summary>
        InterestingnessDescending,
        /// <summary>
        /// Interestingness Ascending
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
    /// represents the information of Flickr User
    /// </summary>
    public class UserInfo
    {
        private XElement data;

        internal UserInfo(XElement element)
        {
            this.data = element;
        }

        /// <summary>
        /// the user name
        /// </summary>
        public string UserName { get { return data.Attribute("username").Value; } }

        /// <summary>
        /// the Real user name
        /// </summary>
        public string RealName { get { return data.Element("realname").Value;} }

        /// <summary>
        /// the User ID
        /// </summary>
        public string UserID { get { return data.Attribute("id").Value; } }

        /// <summary>
        /// determine if the user has Pro Account
        /// </summary>
        public bool IsProUser { get { return data.Attribute("ispro").Value.ToBoolean(); } }

        /// <summary>
        /// the number of the server the icon resides on
        /// </summary>
        public int IconServer { get { return int.Parse(data.Attribute("iconserver").Value); } }

        /// <summary>
        /// the number of server farm  the icon resides on
        /// </summary>
        public int IconFarm { get { return int.Parse(data.Attribute("iconfarm").Value); } }

        /// <summary>
        /// the Path Alias (used when Generating Urls ) , Could be Empty when not set by the user
        /// </summary>
        public string PathAlias { get { return data.Attribute("path_alias").Value; } }
        
        /// <summary>
        /// http://markmail.org/message/2poskzlsgdjjt7ow , could be Null
        /// </summary>
        public string mbox_sha1sum { get { return data.Element("mbox_sha1sum") != null ? data.Element("mbox_sha1sum").Value : null; } }

        /// <summary>
        /// The Location of the User , Could be Null
        /// </summary>
        public string Location { get { return data.Element("location").Value; } }

        /// <summary>
        /// the url that leads to Photostream of the user
        /// </summary>
        public Uri PhotosUrl { get { return new Uri(data.Element("photosurl").Value); } }

        /// <summary>
        /// the url that leads to the user profile
        /// </summary>
        public Uri ProfileUrl { get { return new Uri(data.Element("profileurl").Value); } }

        /// <summary>
        /// the url that leads to the Photostream page of the user that ready to be displayed on mobile device 
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
        /// the number of photos the user has Uploaded
        /// </summary>
        public int PhotosCount { get { return int.Parse(data.Element("photos").Element("count").Value); } }

        /// <summary>
        /// the number of views by Flickr users for the Photostream page of the User, could be Null
        /// </summary>
        public Nullable<int> PhotoStreamViews { get { return data.Element("photos").Element("views") != null ? new Nullable<int>(int.Parse(data.Element("photos").Element("views").Value)) : null; } }

        /// <summary>
        /// the Gender of User , could be Null
        /// </summary>
        public string Geneder { get { return data.Attribute("geneder") != null ? data.Element("gender").Value : null; } }

        /// <summary>
        /// determine if the user is ignored by you
        /// </summary>
        public Nullable<bool> IsIgnored { get { return data.Attribute("igonred") != null ? new Nullable<bool>(data.Attribute("ignored").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether the user is a contact in your contact list or Not
        /// </summary>
        public Nullable<bool> IsContact { get { return data.Attribute("contact") != null ? new Nullable<bool>(data.Attribute("contact").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether you are marking the user as a family in you contact list or Not
        /// </summary>
        public Nullable<bool> IsFamily { get { return data.Attribute("family") != null ? new Nullable<bool>(data.Attribute("family").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether you are marking the user as a friend  in you contact list or Not
        /// </summary>
        public Nullable<bool> IsFriend { get { return data.Attribute("friend") != null ? new Nullable<bool>(data.Attribute("friend").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether the user is marking you as a contact or Not
        /// </summary>
        public Nullable<bool> IsConsideringYouAsContact { get { return data.Attribute("revcontact") != null ? new Nullable<bool>(data.Attribute("revcontact").Value.ToBoolean()) : null; } }

        /// <summary>
        /// determine whether the user is marking you as a family or Not
        /// </summary>
        public Nullable<bool> IsConsideringYouAsFriend { get { return data.Attribute("revfriend") != null ? new Nullable<bool>(data.Attribute("revfriend").Value.ToBoolean()) : null;  } }

        /// <summary>
        /// determine whether the user is marking you as a friend or Not
        /// </summary>
        public Nullable<bool> IsConsideringYouAsFamily { get { return data.Attribute("revfamily") != null ? new Nullable<bool>(data.Attribute("revfamily").Value.ToBoolean()) : null; } }
    }

    /// <summary>
    /// represents  photo counts in a given time period
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
        /// the number of photo in this period
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// start time of counting
        /// </summary>
        public DateTime FromDate { get; private set; }

        /// <summary>
        /// end time of counting
        /// </summary>
        public DateTime ToDate { get; private set; }
    }

    /// <summary>
    /// Tag Mode in Searching
    /// </summary>
    public enum TagMode
	{
        /// <summary>
        /// 'any' for an OR combination of tags
        /// </summary>
        Any = 0,
        /// <summary>
        /// 'all' for an AND combination
        /// </summary>
        All = 1
	}

    /// <summary>
    /// Geo Context Setting 
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
    /// Radius Unit Setting
    /// </summary>
    public enum RadiusUnits
    {
        /// <summary>
        /// Miles
        /// </summary>
        Mi = 0,
        /// <summary>
        /// Kilometers
        /// </summary>
        Km = 1
    }
}
