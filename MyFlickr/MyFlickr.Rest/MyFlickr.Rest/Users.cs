using System;
using System.Collections.Generic;
using System.Linq;
using MyFlickr.Core;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a Flickr User
    /// </summary>
    public class User
    {
        private AuthenticationTokens authTkns;

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
        public Token GetContactsListAsync(Nullable<ContactFilter> contactFilter = null,Nullable<int> page = null , Nullable<int> perPage = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = MyFlickr.Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
              element => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token,new ContactsList(element.Element("contacts"),false))) 
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
              element => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, new ContactsList(element.Element("contacts"), true)))
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
        public Token GetPhotosAsync(Nullable<SafeSearch> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
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
        public Token GetPhotosAsync(User user, Nullable<SafeSearch> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
            , string minTakenDate = null, string maxTakenDate = null, Nullable<ContentType> contentType = null, Nullable<PrivacyFilter> privacyFilter = null
            , string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                  elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(this.authTkns,elm.Element("photos")))),
                  e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                  new Parameter("method", "flickr.people.getPhotos"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token)
                , new Parameter("user_id", user.UserID), new Parameter("safe_search", safeSearch), new Parameter("min_upload_date", minUploadDate)
                , new Parameter("max_upload_date", maxUploadDate), new Parameter("min_taken_date", minTakenDate), new Parameter("max_taken_date", maxTakenDate)
                , new Parameter("content_type", contentType), new Parameter("privacy_filter", privacyFilter)
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
        public Token GetPublicPhotosAsync(User user, Nullable<SafeSearch> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                  elm => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authTkns,elm.Element("photos"))))
                , e => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null
                , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPublicPhotos"), new Parameter("user_id", user.UserID)
                , new Parameter("safe_search", safeSearch) , new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));

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
        public Token GetPublicPhotosAsync(Nullable<SafeSearch> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
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
                  elm => this.InvokeGetPhotoSetsListCompletedEvent(new EventArgs<PhotoSetsCollection>(token,new PhotoSetsCollection(elm.Element("photosets"))))
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
                elm => this.InvokeGetCollectionsTreeEvent(new EventArgs<CollectionsList>(token , new CollectionsList(elm.Element("collections")))),
                e => this.InvokeGetCollectionsTreeEvent(new EventArgs<CollectionsList>(token,e)), null,
                new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.collections.getTree"), 
                new Parameter("collection_id", collection != null ? collection.ID : null ), new Parameter("user_id", this.UserID));

            return token;
        }

        /// <summary>
        /// Returns a list of favorite public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.</param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.</param>
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
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.</param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.</param>
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
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.</param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetFavoritesListAsync(string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return this.GetFavoritesListAsync(this, minFaveDate, maxFaveDate, extras, perPage, page);
        }
        
        #region Events
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
    /// Safe search setting
    /// </summary>
    public enum SafeSearch
    {
        /// <summary>
        /// for safe
        /// </summary>
        Safe = 0 , 
        /// <summary>
        /// for moderate
        /// </summary>
        Moderate = 1,
        /// <summary>
        /// for restricted
        /// </summary>
        Restricted =2
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
}
