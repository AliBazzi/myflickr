using System;
using MyFlickr.Core;
using System.Collections.Generic;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extensions Methods for Users.
    /// </summary>
    public static class SynchronousUser
    {
        /// <summary>
        /// Get a list of contacts for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="contactFilter">An optional filter of the results. The following values are valid:friends,family,both,neither.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>ContactsList Object.</returns>
        public static ContactsList GetContactsList(this User user, Nullable<ContactFilter> contactFilter = null, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<ContactsList> FSP = new FlickrSynchronousPrmitive<ContactsList>();

            Action<object, EventArgs<ContactsList>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetContactsListCompleted += new EventHandler<EventArgs<ContactsList>>(handler);
            FSP.Token = user.GetContactsListAsync(contactFilter, page, perPage);
            FSP.WaitForAsynchronousCall();
            user.GetContactsListCompleted -= new EventHandler<EventArgs<ContactsList>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userInstance">Instance.</param>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>ContactsList Object.</returns>
        public static ContactsList GetPublicContactsList(this User userInstance, User user, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<ContactsList> FSP = new FlickrSynchronousPrmitive<ContactsList>();

            Action<object, EventArgs<ContactsList>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            userInstance.GetPublicContactsListCompleted += new EventHandler<EventArgs<ContactsList>>(handler);
            FSP.Token = userInstance.GetPublicContactsListAsync(user, page, perPage);
            FSP.WaitForAsynchronousCall();
            userInstance.GetPublicContactsListCompleted -= new EventHandler<EventArgs<ContactsList>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>ContactsList Object.</returns>
        public static ContactsList GetPublicContactsList(this User user, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            return GetPublicContactsList(user, user, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// to return public photos for a user, use User.getPublicPhotos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
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
        /// <returns>PhotoCollection Object.</returns>
        public static PhotosCollection GetPhotos(this User user, Nullable<SafetyLevel> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
            , string minTakenDate = null, string maxTakenDate = null, Nullable<ContentType> contentType = null, Nullable<PrivacyFilter> privacyFilter = null
            , string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return GetPhotos(user, user, safeSearch, minUploadDate, maxUploadDate, minTakenDate, maxTakenDate, contentType, privacyFilter, extras, perPage, page);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// to return public photos for a user, use User.getPublicPhotos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="userInstance">Instance.</param>
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
        /// <returns>PhotoCollection Object.</returns>
        public static PhotosCollection GetPhotos(this User userInstance, User user, Nullable<SafetyLevel> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
            , string minTakenDate = null, string maxTakenDate = null, Nullable<ContentType> contentType = null, Nullable<PrivacyFilter> privacyFilter = null
            , string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            userInstance.GetPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = userInstance.GetPhotosAsync(user, safeSearch, minUploadDate, maxUploadDate, minTakenDate, maxTakenDate, contentType, privacyFilter, extras, perPage, page);
            FSP.WaitForAsynchronousCall();
            userInstance.GetPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userInstance">Instance.</param>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="safeSearch">Safe search setting.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotoCollection Object.</returns>
        public static PhotosCollection GetPublicPhotos(this User userInstance, User user, Nullable<SafetyLevel> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            userInstance.GetPublicPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = userInstance.GetPublicPhotosAsync(user, safeSearch, extras, perPage, page);
            FSP.WaitForAsynchronousCall();
            userInstance.GetPublicPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="safeSearch">Safe search setting.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotoCollection Object.</returns>
        public static PhotosCollection GetPublicPhotos(this User user, Nullable<SafetyLevel> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return GetPublicPhotos(user, user, safeSearch, extras, perPage, page);
        }

        /// <summary>
        /// Returns a list of photos containing a particular Flickr member.
        /// This method does not require authentication. but when called when having at least read permission ,  you will get private photos.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosOfUserCollection GetPhotosOfUser(this User user, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosOfUserCollection> FSP = new FlickrSynchronousPrmitive<PhotosOfUserCollection>();

            Action<object, EventArgs<PhotosOfUserCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetUserPhotosCompleted += new EventHandler<EventArgs<PhotosOfUserCollection>>(handler);
            FSP.Token = user.GetPhotosOfUserAsync(extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            user.GetUserPhotosCompleted -= new EventHandler<EventArgs<PhotosOfUserCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get information about the user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <returns>UserInfo Object.</returns>
        public static UserInfo GetInfo(this User user)
        {
            FlickrSynchronousPrmitive<UserInfo> FSP = new FlickrSynchronousPrmitive<UserInfo>();

            Action<object, EventArgs<UserInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetInfoCompleted += new EventHandler<EventArgs<UserInfo>>(handler);
            FSP.Token = user.GetInfoAsync();
            FSP.WaitForAsynchronousCall();
            user.GetInfoCompleted -= new EventHandler<EventArgs<UserInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get information about a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userInstance">Instance.</param>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <returns>UserInfo Object.</returns>
        public static UserInfo GetInfo(this User userInstance, User user)
        {
            FlickrSynchronousPrmitive<UserInfo> FSP = new FlickrSynchronousPrmitive<UserInfo>();

            Action<object, EventArgs<UserInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            userInstance.GetInfoCompleted += new EventHandler<EventArgs<UserInfo>>(handler);
            FSP.Token = userInstance.GetInfoAsync(user);
            FSP.WaitForAsynchronousCall();
            userInstance.GetInfoCompleted -= new EventHandler<EventArgs<UserInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the photosets belonging to the specified user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <returns>PhotoSetsCollection Object.</returns>
        public static PhotoSetsCollection GetPhotoSetsList(this User user)
        {
            FlickrSynchronousPrmitive<PhotoSetsCollection> FSP = new FlickrSynchronousPrmitive<PhotoSetsCollection>();

            Action<object, EventArgs<PhotoSetsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPhotoSetsListCompleted += new EventHandler<EventArgs<PhotoSetsCollection>>(handler);
            FSP.Token = user.GetPhotoSetsListAsync();
            FSP.WaitForAsynchronousCall();
            user.GetPhotoSetsListCompleted -= new EventHandler<EventArgs<PhotoSetsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Return the list of galleries created by a user. Sorted from newest to oldest.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="perPage">Number of galleries to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>GalleriesCollection Object.</returns>
        public static GalleriesCollection GetGalleriesList(this User user, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<GalleriesCollection> FSP = new FlickrSynchronousPrmitive<GalleriesCollection>();

            Action<object, EventArgs<GalleriesCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetGalleriesListCompleted += new EventHandler<EventArgs<GalleriesCollection>>(handler);
            FSP.Token = user.GetGalleriesListAsync(perPage,page);
            FSP.WaitForAsynchronousCall();
            user.GetGalleriesListCompleted -= new EventHandler<EventArgs<GalleriesCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the list of public groups a user is a member of.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <returns>GroupCollection Object.</returns>
        public static GroupCollection GetPublicGroups(this User user)
        {
            FlickrSynchronousPrmitive<GroupCollection> FSP = new FlickrSynchronousPrmitive<GroupCollection>();

            Action<object, EventArgs<GroupCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPublicGroupsCompleted += new EventHandler<EventArgs<GroupCollection>>(handler);
            FSP.Token = user.GetPublicGroupsAsync();
            FSP.WaitForAsynchronousCall();
            user.GetPublicGroupsCompleted -= new EventHandler<EventArgs<GroupCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of configured blogs for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="serviceID">Optionally only return blogs for a given service id.</param>
        /// <returns>BlogsCollection Object.</returns>
        public static BlogsCollection GetBlogsList(this User user, Nullable<int> serviceID = null)
        {
            FlickrSynchronousPrmitive<BlogsCollection> FSP = new FlickrSynchronousPrmitive<BlogsCollection>();

            Action<object, EventArgs<BlogsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetBlogsListCompleted += new EventHandler<EventArgs<BlogsCollection>>(handler);
            FSP.Token = user.GetBlogsListAsync(serviceID);
            FSP.WaitForAsynchronousCall();
            user.GetBlogsListCompleted -= new EventHandler<EventArgs<BlogsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a tree (or sub tree) of collections belonging to a given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="collection">The ID of the collection to fetch a tree for, or Null to fetch the root collection.</param>
        /// <returns>CollectionsList Object.</returns>   
        public static CollectionsList GetCollectionsTree(this User user, Collection collection = null)
        {
            FlickrSynchronousPrmitive<CollectionsList> FSP = new FlickrSynchronousPrmitive<CollectionsList>();

            Action<object, EventArgs<CollectionsList>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetCollectionsTreeCompleted += new EventHandler<EventArgs<CollectionsList>>(handler);
            FSP.Token = user.GetCollectionsTreeAsync(collection);
            FSP.WaitForAsynchronousCall();
            user.GetCollectionsTreeCompleted -= new EventHandler<EventArgs<CollectionsList>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of favorite public photos for the given user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetPublicFavoritesList(this User user ,string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPublicFavoritesListCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = user.GetPublicFavoritesListAsync(minFaveDate,maxFaveDate,extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            user.GetPublicFavoritesListCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of the user's favorite photos. Only photos which the calling user has permission to see are returned.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="userInstance">Instance.</param>
        /// <param name="user">The object that represents a Flickr User.</param>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetFavoritesList(this User userInstance ,User user ,string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            userInstance.GetFavoritesListCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = userInstance.GetFavoritesListAsync(user,minFaveDate,maxFaveDate,extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            userInstance.GetFavoritesListCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of the user's favorite photos. Only photos which the calling user has permission to see are returned.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="userInstance">Instance.</param>
        /// <param name="minFaveDate">Minimum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxFaveDate">Maximum date that a photo was favorited on. The date should be in the form of a unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>    
        public static PhotosCollection GetFavoritesList(this User userInstance, string minFaveDate = null, string maxFaveDate = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return GetFavoritesList(userInstance, userInstance, minFaveDate, maxFaveDate, extras, perPage, page);
        }

        /// <summary>
        /// Returns a list of your photos that are not part of any sets.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="mediaType">Filter results by media type.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetPhotosNotInSet(this User user, string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null,
            string maxTakenDate = null, Nullable<PrivacyFilter> privacyFilter = null, Nullable<MediaType> mediaType = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPhotosNotInSetCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = user.GetPhotosNotInSetAsync(maxUploadDate, minUploadDate, minTakenDate, maxTakenDate, privacyFilter, mediaType, extras, perPage, page);
            FSP.WaitForAsynchronousCall();
            user.GetPhotosNotInSetCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of your photos with no tags.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="maxUploadDate">Maximum upload date. Photos with an upload date less than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minUploadDate">Minimum upload date. Photos with an upload date greater than or equal to this value will be returned. The date can be in the form of a unix timestamp or mysql datetime.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="minTakenDate">Minimum taken date. Photos with an taken date greater than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="maxTakenDate">Maximum taken date. Photos with an taken date less than or equal to this value will be returned. The date can be in the form of a mysql datetime or unix timestamp.  more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level.</param>
        /// <param name="mediaType">Filter results by media type.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetUntaggedPhotos(this User user, string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null,
            string maxTakenDate = null, Nullable<PrivacyFilter> privacyFilter = null, Nullable<MediaType> mediaType = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetUntaggedPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = user.GetUntaggedPhotosAsync(maxUploadDate, minUploadDate, minTakenDate, maxTakenDate, privacyFilter, mediaType, extras, perPage, page);
            FSP.WaitForAsynchronousCall();
            user.GetUntaggedPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of your geo-tagged photos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
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
        ///<returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetGeotaggedPhotos(this User user, string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null,
            string maxTakenDate = null, Nullable<PrivacyFilter> privacyFilter = null, Nullable<SortType> sortType = null, Nullable<MediaType> mediaType = null,
            string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetGeotaggedPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = user.GetGeotaggedPhotosAsync(maxUploadDate, minUploadDate, minTakenDate, maxTakenDate, privacyFilter,sortType, mediaType, extras, perPage, page);
            FSP.WaitForAsynchronousCall();
            user.GetGeotaggedPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of your photos which haven't been geo-tagged.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
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
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetUnGeotaggedPhotos(this User user ,string maxUploadDate = null, string minUploadDate = null, string minTakenDate = null,
            string maxTakenDate = null, Nullable<PrivacyFilter> privacyFilter = null, Nullable<SortType> sortType = null, Nullable<MediaType> mediaType = null,
            string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetUnGeotaggedPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = user.GetUnGeotaggedPhotosAsync(maxUploadDate, minUploadDate, minTakenDate, maxTakenDate, privacyFilter, sortType, mediaType, extras, perPage, page);
            FSP.WaitForAsynchronousCall();
            user.GetUnGeotaggedPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Gets a list of photo counts for the given date ranges for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="dates">list of dates, denoting the periods to return counts for. They should be specified smallest first.</param>
        /// <returns>Enumerable of PhotosCount Objects.</returns>
        public static IEnumerable<PhotosCount> GetPhotosCounts(this User user , params DateTime[] dates)
        {
            FlickrSynchronousPrmitive<IEnumerable<PhotosCount>> FSP = new FlickrSynchronousPrmitive<IEnumerable<PhotosCount>>();

            Action<object, EventArgs<IEnumerable<PhotosCount>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPhotosCountsCompleted += new EventHandler<EventArgs<IEnumerable<PhotosCount>>>(handler);
            FSP.Token = user.GetPhotosCountsAsync(dates);
            FSP.WaitForAsynchronousCall();
            user.GetPhotosCountsCompleted -= new EventHandler<EventArgs<IEnumerable<PhotosCount>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Return a list of your photos that have been recently created or which have been recently modified.
        ///Recently modified may mean that the photo's metadata (title, description, tags) may have been changed or a comment has been added (or just modified somehow :-)
        ///This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="minDate">a timestamp indicating the date from which modifications should be compared.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetRecentlyUpdatedPhotos(this User user, DateTime minDate, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetRecentlyUpdatedPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = user.GetRecentlyUpdatedPhotosAsync(minDate,extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            user.GetRecentlyUpdatedPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Fetch a list of recent photos from the calling users' contacts.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="count">Number of photos to return. Defaults to 10, maximum 50. This is only used if single_photo is not passed.</param>
        /// <param name="justFriends">set as true to only show photos from friends and family (excluding regular contacts).</param>
        /// <param name="singlePhoto">set as true to Only fetch one photo (the latest) per contact, instead of all photos in chronological order.</param>
        /// <param name="includeSelf">Set to true to include photos from the calling user.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields include: license, date_upload, date_taken, owner_name, icon_server, original_format, last_update.</param>
        /// <returns>Enumerable of Photos Objects.</returns>
        public static IEnumerable<Photo> GetContactsPhotos(this User user, Nullable<int> count = null, Nullable<bool> justFriends = null,
            Nullable<bool> singlePhoto = null, Nullable<bool> includeSelf = null, string extras = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Photo>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Photo>>();

            Action<object, EventArgs<IEnumerable<Photo>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetContactsPhotosCompleted += new EventHandler<EventArgs<IEnumerable<Photo>>>(handler);
            FSP.Token = user.GetContactsPhotosAsync(count,justFriends,singlePhoto,includeSelf,extras);
            FSP.WaitForAsynchronousCall();
            user.GetContactsPhotosCompleted -= new EventHandler<EventArgs<IEnumerable<Photo>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Fetch a list of recent public photos from a users' contacts.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="count">Number of photos to return. Defaults to 10, maximum 50. This is only used if single_photo is not passed.</param>
        /// <param name="justFriends">set as true to only show photos from friends and family (excluding regular contacts).</param>
        /// <param name="singlePhoto">set as true to Only fetch one photo (the latest) per contact, instead of all photos in chronological order.</param>
        /// <param name="includeSelf">Set to true to include photos from the user specified by user object.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields include: license, date_upload, date_taken, owner_name, icon_server, original_format, last_update.</param>
        /// <returns>Enumerable of Photos Objects.</returns>
        public static IEnumerable<Photo> GetContactsPublicPhotos(this User user, Nullable<int> count = null, Nullable<bool> justFriends = null,
            Nullable<bool> singlePhoto = null, Nullable<bool> includeSelf = null, string extras = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Photo>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Photo>>();

            Action<object, EventArgs<IEnumerable<Photo>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetContactsPublicPhotosCompleted += new EventHandler<EventArgs<IEnumerable<Photo>>>(handler);
            FSP.Token = user.GetContactsPublicPhotosAsync(count,justFriends,singlePhoto,includeSelf,extras);
            FSP.WaitForAsynchronousCall();
            user.GetContactsPublicPhotosCompleted -= new EventHandler<EventArgs<IEnumerable<Photo>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Return a list of contacts for a user who have recently uploaded photos along with the total count of photos uploaded.
        /// This method is still considered experimental. We don't plan for it to change or to go away but so long as this notice is present you should write your code accordingly.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="dateLastUpload">Limits the resultset to contacts that have uploaded photos since this date. The date should be in the form of a Unix timestamp. The default offset is (1) hour and the maximum (24) hours. </param>
        /// <param name="filter">Limit the result set to all contacts or only those who are friends or family. Valid options are:
        ///* ff friends and family
        ///* all all your contacts
        ///Default value is "all".</param>
        /// <returns>Enumerable of Contact Objects.</returns>
        public static IEnumerable<Contact> GetListRecentlyUploaded(this User user, string dateLastUpload = null, string filter = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Contact>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Contact>>();

            Action<object, EventArgs<IEnumerable<Contact>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetListRecentlyUploadedCompleted += new EventHandler<EventArgs<IEnumerable<Contact>>>(handler);
            FSP.Token = user.GetListRecentlyUploadedAsync(dateLastUpload,filter);
            FSP.WaitForAsynchronousCall();
            user.GetListRecentlyUploadedCompleted -= new EventHandler<EventArgs<IEnumerable<Contact>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Create a new photoset for the calling user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="title">A title for the photoset.</param>
        /// <param name="primaryPhotoID">The id of the photo to represent this set. The photo must belong to the calling user.</param>
        /// <param name="description">A description of the photoset. May contain limited html.</param>
        /// <returns>PhotoSetToken object.</returns>
        public static PhotoSetToken CreatePhotoSet(this User user, string title, string primaryPhotoID, string description = null)
        {
            FlickrSynchronousPrmitive<PhotoSetToken> FSP = new FlickrSynchronousPrmitive<PhotoSetToken>();

            Action<object, EventArgs<PhotoSetToken>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.CreatePhotoSetCompleted += new EventHandler<EventArgs<PhotoSetToken>>(handler);
            FSP.Token = user.CreatePhotoSetAsync(title,primaryPhotoID,description);
            FSP.WaitForAsynchronousCall();
            user.CreatePhotoSetCompleted -= new EventHandler<EventArgs<PhotoSetToken>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set the order of photosets for the calling user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="photosetsIDs">A comma delimited list of photoset IDs, ordered with the set to show first, first in the list. Any set IDs not given in the list will be set to appear at the end of the list, ordered by their IDs.</param>
        /// <returns>NoReply represents void.</returns>
        public static NoReply OrderSets(this User user ,params string[] photosetsIDs)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.OrderSetsCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = user.OrderSetsAsync(photosetsIDs);
            FSP.WaitForAsynchronousCall();
            user.OrderSetsCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Create a new gallery for the calling user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="title">The name of the gallery.</param>
        /// <param name="description">A short description for the gallery.</param>
        /// <param name="primaryPhotoID">The first photo to add to your gallery.</param>
        /// <returns>GalleryToken Object.</returns>
        public static GalleryToken CreateGallery(this User user, string title, string description, string primaryPhotoID = null)
        {
            FlickrSynchronousPrmitive<GalleryToken> FSP = new FlickrSynchronousPrmitive<GalleryToken>();

            Action<object, EventArgs<GalleryToken>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.CreateGalleryCompleted += new EventHandler<EventArgs<GalleryToken>>(handler);
            FSP.Token = user.CreateGalleryAsync(title,description,primaryPhotoID);
            FSP.WaitForAsynchronousCall();
            user.CreateGalleryCompleted -= new EventHandler<EventArgs<GalleryToken>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of groups to which you can add photos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of groups to return per page. If this argument is omitted, it defaults to 400. The maximum allowed value is 400.</param>
        /// <returns>GroupCollection Object.</returns>
        public static GroupCollection GetGroups(this User user,Nullable<int> page = null ,Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<GroupCollection> FSP = new FlickrSynchronousPrmitive<GroupCollection>();

            Action<object, EventArgs<GroupCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetGroupsCompleted += new EventHandler<EventArgs<GroupCollection>>(handler);
            FSP.Token = user.GetGroupsAsync(page,perPage);
            FSP.WaitForAsynchronousCall();
            user.GetGroupsCompleted -= new EventHandler<EventArgs<GroupCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of recent activity on photos belonging to the calling user. Do not poll this method more than once an hour (as Flickr Team Recommends).
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="timeFrame">The timeframe in which to return updates for. This can be specified in days ('2d') or hours ('4h'). The default behavoir is to return changes since the beginning of the previous user session.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of items to return per page. If this argument is omitted, it defaults to 10. The maximum allowed value is 50.</param>
        /// <returns>ItemsCollection Object.</returns>
        public static ItemsCollection GetPhotosActivities(this User user, string timeFrame = null, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<ItemsCollection> FSP = new FlickrSynchronousPrmitive<ItemsCollection>();

            Action<object, EventArgs<ItemsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetPhotosActivitiesCompleted += new EventHandler<EventArgs<ItemsCollection>>(handler);
            FSP.Token = user.GetPhotosActivitesAsync(timeFrame,page, perPage);
            FSP.WaitForAsynchronousCall();
            user.GetPhotosActivitiesCompleted -= new EventHandler<EventArgs<ItemsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of recent activity on photos commented on by the calling user. Do not poll this method more than once an hour (as Flickr Team Recommends , Not me ;) ).
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user">Instance.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of items to return per page. If this argument is omitted, it defaults to 10. The maximum allowed value is 50.</param>
        /// <returns>ItemsCollection Object.</returns>
        public static ItemsCollection GetCommentsActivities(this User user, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<ItemsCollection> FSP = new FlickrSynchronousPrmitive<ItemsCollection>();

            Action<object, EventArgs<ItemsCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            user.GetCommentsActivitiesCompleted += new EventHandler<EventArgs<ItemsCollection>>(handler);
            FSP.Token = user.GetCommentsActivitiesAsync( page, perPage);
            FSP.WaitForAsynchronousCall();
            user.GetCommentsActivitiesCompleted -= new EventHandler<EventArgs<ItemsCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
