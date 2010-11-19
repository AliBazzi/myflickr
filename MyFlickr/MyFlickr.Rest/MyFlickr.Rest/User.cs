﻿using System;
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
        public Token GetInfoOfAsync(User user)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            if (user == null)
                throw new ArgumentNullException("user");

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateRequest(
                    elm => this.InokeGetInfoOfCompletedEvent(new EventArgs<UserInfo>(token, new UserInfo(elm.Element("person"))))
                    , e => this.InokeGetInfoOfCompletedEvent(new EventArgs<UserInfo>(token, e)), this.SharedSecret
                    , new Parameter("api_key", this.ApiKey), new Parameter("auth_token", this.Token)
                    , new Parameter("method", "flickr.people.getInfo"), new Parameter("user_id", user.UserID));

            return token;
        }

        #region Events
        private void InokeGetInfoOfCompletedEvent(EventArgs<UserInfo> args)
        {
            if (this.GetInfoOfCompleted != null)
            {
                this.GetInfoOfCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<UserInfo>> GetInfoOfCompleted;
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
        private XElement data { get; set; }

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
        private XElement data { get;set; }

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
        internal UserInfo(XElement element)
        {
            this.UserID = element.Attribute("id").Value;
            this.UserName = element.Element("username").Value;
            this.RealName = element.Element("realname").Value;
            this.IsProUser = element.Attribute("ispro").Value.ToBoolean();
            this.IconServer = int.Parse(element.Attribute("iconserver").Value);
            this.IconFarm = int.Parse(element.Attribute("iconfarm").Value);
            this.PathAlias = element.Attribute("path_alias").Value;
            this.Geneder = element.Attribute("geneder") != null ? element.Element("gender").Value : null;
            this.IsIgnored = element.Attribute("igonred") != null ? new Nullable<bool>(element.Attribute("ignored").Value.ToBoolean()) : null;
            this.IsContact = element.Attribute("contact") != null ? new Nullable<bool>(element.Attribute("contact").Value.ToBoolean()) : null;
            this.IsFamily = element.Attribute("family") != null ? new Nullable<bool>(element.Attribute("family").Value.ToBoolean()) : null;
            this.IsFriend = element.Attribute("friend") != null ? new Nullable<bool>(element.Attribute("friend").Value.ToBoolean()) : null;
            this.IsConsideringYouAsContact = element.Attribute("revcontact") != null ? new Nullable<bool>(element.Attribute("revcontact").Value.ToBoolean()) : null;
            this.IsConsideringYouAsFamily = element.Attribute("revfamily") != null ? new Nullable<bool>(element.Attribute("revfamily").Value.ToBoolean()) : null;
            this.IsConsideringYouAsFriend = element.Attribute("revfriend") != null ? new Nullable<bool>(element.Attribute("revfriend").Value.ToBoolean()) : null;
            this.mbox_sha1sum = element.Element("mbox_sha1sum") !=null ? element.Element("mbox_sha1sum").Value : null;
            this.Location = element.Element("location").Value;
            this.PhotosUrl = new Uri(element.Element("photosurl").Value);
            this.ProfileUrl = new Uri(element.Element("profileurl").Value);
            this.MobileUrl = new Uri(element.Element("mobileurl").Value);
            this.FirstDateTaken = DateTime.Parse(element.Element("photos").Element("firstdatetaken").Value);
            this.FirstDate = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(element.Element("photos").Element("firstdate").Value));
            this.PhotosCount = int.Parse(element.Element("photos").Element("count").Value);
            this.PhotoStreamViews = element.Element("photos").Element("views") != null ? new Nullable<int>(int.Parse(element.Element("photos").Element("views").Value)) : null;
        }

        /// <summary>
        /// the user name
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// the Real user name
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// the User ID
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// determine if the user has Pro Account
        /// </summary>
        public bool IsProUser { get; private set; }

        /// <summary>
        /// the number of the server the icon resides on
        /// </summary>
        public int IconServer { get; private set; }

        /// <summary>
        /// the number of server farm  the icon resides on
        /// </summary>
        public int IconFarm { get; private set; }

        /// <summary>
        /// the Path Alias (used when Generating Urls ) , Could be Empty when not set by the user
        /// </summary>
        public string PathAlias { get; private set; }
        
        /// <summary>
        /// http://markmail.org/message/2poskzlsgdjjt7ow , could be Null
        /// </summary>
        public string mbox_sha1sum { get; private set; }

        /// <summary>
        /// The Location of the User , Could be Null
        /// </summary>
        public string Location { get; private set; }

        /// <summary>
        /// the url that leads to Photostream of the user
        /// </summary>
        public Uri PhotosUrl { get; private set; }

        /// <summary>
        /// the url that leads to the user profile
        /// </summary>
        public Uri ProfileUrl { get; private set; }

        /// <summary>
        /// the url that leads to the Photostream page of the user that ready to be displayed on mobile device 
        /// </summary>
        public Uri MobileUrl { get; private set; }

        /// <summary>
        /// contains the datetime of the first photo taken by the user.
        /// </summary>
        public DateTime FirstDateTaken { get; private set; }

        /// <summary>
        /// contains the timestamp of the first photo uploaded by the user.
        /// </summary>
        public DateTime FirstDate { get; private set; }

        /// <summary>
        /// the number of photos the user has Uploaded
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of views by Flickr users for the Photostream page of the User, could be Null
        /// </summary>
        public Nullable<int> PhotoStreamViews { get; private set; }

        /// <summary>
        /// the Gender of User , could be Null
        /// </summary>
        public string Geneder { get; private set; }

        /// <summary>
        /// determine if the user is ignored by you
        /// </summary>
        public Nullable<bool> IsIgnored { get; private set; }

        /// <summary>
        /// determine whether the user is a contact in your contact list or Not
        /// </summary>
        public Nullable<bool> IsContact { get; private set; }

        /// <summary>
        /// determine whether you are marking the user as a family in you contact list or Not
        /// </summary>
        public Nullable<bool> IsFamily { get; private set; }

        /// <summary>
        /// determine whether you are marking the user as a friend  in you contact list or Not
        /// </summary>
        public Nullable<bool> IsFriend { get; private set; }

        /// <summary>
        /// determine whether the user is marking you as a contact or Not
        /// </summary>
        public Nullable<bool> IsConsideringYouAsContact { get; private set; }

        /// <summary>
        /// determine whether the user is marking you as a family or Not
        /// </summary>
        public Nullable<bool> IsConsideringYouAsFriend { get; private set; }

        /// <summary>
        /// determine whether the user is marking you as a friend or Not
        /// </summary>
        public Nullable<bool> IsConsideringYouAsFamily { get; private set; }
    }
}
