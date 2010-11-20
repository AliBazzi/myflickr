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
        private AuthenticationTokens authTkns { get; set; }

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

            FlickrCore.IntiateRequest(
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

            FlickrCore.IntiateRequest(
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

            FlickrCore.IntiateRequest(
                  elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, new PhotosCollection(elm.Element("photos")))),
                  e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token, e)), this.SharedSecret,
                  new Parameter("method", "flickr.people.getPhotos"), new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token)
                , new Parameter("user_id", user.UserID)
                , new Parameter("safe_search", safeSearch.HasValue ? (object)(int)safeSearch.Value : null), new Parameter("min_upload_date", minUploadDate)
                , new Parameter("max_upload_date", maxUploadDate), new Parameter("min_taken_date", minTakenDate), new Parameter("max_taken_date", maxTakenDate)
                , new Parameter("content_type", contentType.HasValue ? (object)(int)contentType.Value : null)
                , new Parameter("privacy_filter", privacyFilter.HasValue ? (object)(int)privacyFilter.Value : null)
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

            FlickrCore.IntiateRequest(elm => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(elm.Element("photos"))))
                , e => this.InvokeGetPublicPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null
                , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPublicPhotos"), new Parameter("user_id", user.UserID)
                , new Parameter("safe_search", safeSearch.HasValue ? (object)(int)safeSearch.Value : null)
                , new Parameter("extras", extras), new Parameter("per_page", perPage.HasValue ? (object)(int)perPage.Value : null)
                , new Parameter("page", page.HasValue ? (object)(int)page.Value : null));

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
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetUserPhotos(User user, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateRequest(
                elm => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(elm.Element("photos"))))
                , e => this.InvokeGetUserPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), null
                , new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPhotosOf"), new Parameter("user_id", user.UserID)
                , new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));

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
                FlickrCore.IntiateRequest(
                    elm => this.InokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token,new UserInfo(elm.Element("person"))))
                    , e => this.InokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token,e)), this.SharedSecret
                    , new Parameter("api_key", this.ApiKey),new Parameter("auth_token",this.Token)
                    ,new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", this.UserID));
            }
            else
            {
                FlickrCore.IntiateRequest(
                    elm => this.InokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, new UserInfo(elm.Element("person"))))
                    , e => this.InokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, e)), null,
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

            FlickrCore.IntiateRequest(
                    elm => this.InokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, new UserInfo(elm.Element("person"))))
                    , e => this.InokeGetInfoCompletedEvent(new EventArgs<UserInfo>(token, e)), this.SharedSecret
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

            FlickrCore.IntiateRequest(
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

            FlickrCore.IntiateRequest(
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

            FlickrCore.IntiateRequest(
                elm => this.InvokeGetPublicGroupsEvent(new EventArgs<GroupCollection>(token,new GroupCollection(elm.Element("groups")))), 
                e => this.InvokeGetPublicGroupsEvent(new EventArgs<GroupCollection>(token,e)), null,
                new Parameter("api_key", this.ApiKey), new Parameter("method", "flickr.people.getPublicGroups"), new Parameter("user_id", this.UserID));

            return token;
        }

        #region Events
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
        private void InokeGetInfoCompletedEvent(EventArgs<UserInfo> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<UserInfo>> GetInfoCompleted;
        private void InvokeGetUserPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetUserPhotosCompleted != null)
            {
                this.GetUserPhotosCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<PhotosCollection>> GetUserPhotosCompleted;
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
    /// represents a list of contact
    /// </summary>
    public class ContactsList : IEnumerable<Contact>
    {
        private XElement data;

        private bool isPublicList;

        internal ContactsList(XElement element,bool isPublicList)
        {
            this.isPublicList = isPublicList;
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.PerPage = int.Parse(element.Attribute("per_page").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
        }

        /// <summary>
        /// the page number
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the available pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of contacts per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of contacts
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the contacts objects
        /// </summary>
        public IEnumerable<Contact> Contacts
        {
            get
            {
                return data.Elements("contact").Select(elm => this.isPublicList
                   ?
                   new Contact(elm.Attribute("nsid").Value,
                   elm.Attribute("username").Value, int.Parse(elm.Attribute("iconserver").Value), elm.Attribute("ignored").Value.ToBoolean())
                   :
                   new Contact(elm.Attribute("nsid").Value, elm.Attribute("username").Value, int.Parse(elm.Attribute("iconserver").Value)
                   , elm.Attribute("ignored").Value.ToBoolean(), elm.Attribute("realname").Value, elm.Attribute("friend").Value.ToBoolean()
                   , elm.Attribute("family").Value.ToBoolean()));
            }
        }

        public IEnumerator<Contact> GetEnumerator()
        {
            foreach (var contact in this.Contacts)
                yield return contact;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Represents the Contact information
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// the User ID of the Contact
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// the real Name of the Contact , Could be Null
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// the Name of the Contact
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// is the contact marked as friend , could be Null
        /// </summary>
        public Nullable<bool> IsFriend { get; private set; }

        /// <summary>
        /// is the contact marked as family , could be Null
        /// </summary>
        public Nullable<bool> IsFamily { get; private set; }

        /// <summary>
        /// the number of icon server
        /// </summary>
        public int IconServer { get; private set; }

        /// <summary>
        /// the contact is ignored
        /// </summary>
        public bool IsIgnored { get; private set; }

        internal Contact(string userID , string userName , int iconServer ,bool isIgnored)
        {
            this.UserID = userID;
            this.UserName = userName;
            this.IconServer = iconServer;
            this.IsIgnored = isIgnored;
        }

        internal Contact(string userID, string userName, int iconServer, bool isIgnored, string realName, bool isFriend, bool isFamily)
            :this(userID,userName,iconServer,isIgnored)
        {
            this.RealName = realName;
            this.IsFamily = isFamily;
            this.IsFriend = isFriend;
        }
    }

    /// <summary>
    /// The Filters that could be Applied on Contacts List when getting them
    /// </summary>
    public enum ContactFilter
	{
        /// <summary>
        /// the contact is Friend
        /// </summary>
        Friends = 0,
        /// <summary>
        /// the contact is Family
        /// </summary>
        Family =1, 
        /// <summary>
        /// the contact is both friend and family
        /// </summary>
        Both =2,
        /// <summary>
        /// the contact is neither family nor friend
        /// </summary>
        Niether =3
	}

    /// <summary>
    /// represents a collection of photos
    /// </summary>
    public class PhotosCollection : IEnumerable<Photo>
    {
        private XElement data ;

        internal PhotosCollection(XElement data)
        {
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
                return data.Elements("photo")
                    .Select(elm => new Photo(elm.Attribute("isfriend").Value.ToBoolean(), elm.Attribute("isfamily").Value.ToBoolean(),
                    elm.Attribute("ispublic").Value.ToBoolean(), Int64.Parse(elm.Attribute("id").Value), elm.Attribute("title").Value, elm.Attribute("owner").Value,
                    elm.Attribute("secret").Value, int.Parse(elm.Attribute("server").Value), int.Parse(elm.Attribute("farm").Value)));
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
        internal Photo(bool isFriend, bool isFamily, bool isPublic, Int64 id, string title, string ownerID, string secret, int server, int farm)
        {
            this.IsFriend = isFriend;
            this.IsFamily = isFamily;
            this.IsPublic = isPublic;
            this.ID = id;
            this.Title = title;
            this.OwnerID = ownerID;
            this.Secret = secret;
            this.Server = server;
            this.Farm = farm;
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
            return new Uri(string.Format("http://www.flickr.com/photos/{0}/{1}",this.OwnerID,this.ID));
        }

        //to add :
        // get sizes

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

    /// <summary>
    /// represents a collection of photosets
    /// </summary>
    public class PhotoSetsCollection:IEnumerable<PhotoSet>
    {
        private XElement data;

        internal PhotoSetsCollection(XElement element)
        {
            this.data = element;
            this.PhotoSetsCount = element.Elements("photoset").Count();
        }

        /// <summary>
        /// the number of photosets in this collection
        /// </summary>
        public int PhotoSetsCount { get; private set; }

        /// <summary>
        /// the Photosets Objects
        /// </summary>
        public IEnumerable<PhotoSet> PhotoSets
        {
            get
            {
                return data.Elements("photoset")
                    .Select(elm => new PhotoSet(Int64.Parse(elm.Attribute("id").Value), Int64.Parse(elm.Attribute("primary").Value),
                    elm.Attribute("secret").Value, int.Parse(elm.Attribute("server").Value), int.Parse(elm.Attribute("farm").Value),
                    int.Parse(elm.Attribute("photos").Value), int.Parse(elm.Attribute("videos").Value), elm.Element("title").Value,
                    elm.Element("description").Value));
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
    /// represents a user photoset
    /// </summary>
    public class PhotoSet
    {
        internal PhotoSet(Int64 id,Int64 primary,string secret , int server,int farm ,int photos,int videos,string title,string description) 
        {
            this.ID = id;
            this.Primary = primary;
            this.Secret = secret;
            this.Server = server;
            this.Farm = farm;
            this.PhotosCount = photos;
            this.VideosCount = videos;
            this.Title = title;
            this.Description = description;
        }

        /// <summary>
        /// the ID that identifies the photoset
        /// </summary>
        public Int64 ID { get; private set; }

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

        /// <summary>
        /// the title of the photoset
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the description of the photoset , could be Null
        /// </summary>
        public string Description { get; private set; }
    }

    /// <summary>
    /// represents a collection of galleries
    /// </summary>
    public class GalleriesCollection:IEnumerable<Gallery>
    {
        private XElement data;

        internal GalleriesCollection(XElement element)
        {
            this.data = element;
        }

        /// <summary>
        /// the Total Number of Galleries
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Number of Galleries per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the number of pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the current page number
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the Galleries Objects
        /// </summary>
        public IEnumerable<Gallery> Galleries 
        { 
            get 
            {
                return data.Elements("gallery").Select(
                    elm => new Gallery(elm.Attribute("id").Value, elm.Attribute("url").Value, elm.Attribute("owner").Value,
                        Int64.Parse(elm.Attribute("primary_photo_id").Value), int.Parse(elm.Attribute("count_photos").Value),
                        int.Parse(elm.Attribute("count_videos").Value), int.Parse(elm.Attribute("primary_photo_farm").Value),
                    int.Parse(elm.Attribute("primary_photo_server").Value), elm.Attribute("primary_photo_secret").Value, elm.Attribute("date_create").Value,
                    elm.Attribute("date_update").Value, elm.Element("title").Value, elm.Element("description").Value)); 
            }
        }

        public IEnumerator<Gallery> GetEnumerator()
        {
            foreach (var gallery in this.Galleries)
                yield return gallery; 
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a gallery
    /// </summary>
    public class Gallery
    {
        internal Gallery(string id, string url, string ownerID, Int64 primary, int photosCount, int videosCount, int farm, int server, string secret,
            string dateCreated, string dateUpdated, string title, string description)
        {
            this.ID = id;
            this.Url = new Uri(url);
            this.OwnerID = ownerID;
            this.Primary = primary;
            this.PhotosCount = photosCount;
            this.VideosCount = videosCount;
            this.Farm = farm;
            this.Server = server;
            this.Secret = secret;
            this.DateCreated = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(dateCreated));
            this.DateUpdated = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(dateCreated));
            this.Title = title;
            this.Description = description;
        }

        /// <summary>
        /// the ID of the Gallery
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the URL that leads to the page of the gallery on Flickr
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// The Owner ID of the Gallery 
        /// </summary>
        public string OwnerID { get; private set; }

        /// <summary>
        /// the ID of the primary photo of the gallery
        /// </summary>
        public Int64 Primary { get; private set; }

        /// <summary>
        /// the number of photos in this gallery
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of videos in this gallery
        /// </summary>
        public int VideosCount { get; private set; }

        /// <summary>
        /// the number of server farm that the primary photo of this gallery resides on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the number of server that the primary photo of this gallery resides on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// the secret used to build the URL of primary photo of this gallery
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Date and time of the creation of this gallery
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// the Date and time of the last modification of this gallery
        /// </summary>
        public DateTime DateUpdated { get; private set; }

        /// <summary>
        /// the Title of this gallery
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Description of this gallery , could Be Null
        /// </summary>
        public string Description { get; private set; }
    }

    /// <summary>
    /// represents a Collection of Groups
    /// </summary>
    public class GroupCollection : IEnumerable<Group>
    {
        private XElement data;

        internal GroupCollection(XElement element)
        {
            this.data = element;
            this.GroupsCount = element.Elements("group").Count();
        }

        /// <summary>
        /// the Number of the Groups in the collection
        /// </summary>
        public int GroupsCount { get; private set; }

        /// <summary>
        /// the Groups Objects
        /// </summary>
        public IEnumerable<Group> Groups { 
            get
            {
                return this.data.Elements("group").Select(elm => 
                    new Group(elm.Attribute("nsid").Value,elm.Attribute("name").Value, elm.Attribute("admin").Value.ToBoolean()
                        , elm.Attribute("eighteenplus").Value.ToBoolean())); 
            }
        }

        public IEnumerator<Group> GetEnumerator()
        {
            foreach (var group in this.Groups)
                yield return group;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represent a Flickr Group
    /// </summary>
    public class Group
    {
        internal Group(string id, string name, bool isAdmin, bool isOver18)
        {
            this.ID = id;
            this.Name = name;
            this.IsAdmin = isAdmin;
            this.IsOver18 = isOver18;
        }
        /// <summary>
        /// the ID that identifies the group
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the name of the Group
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// determine whether the calling user is an administrator of the group.
        /// </summary>
        public bool IsAdmin { get; private set; }

        /// <summary>
        /// determine if the group is visible to members over 18 only.
        /// </summary>
        public bool IsOver18 { get; private set; }
    }
}
