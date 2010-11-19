using System;
using MyFlickr.Core;

namespace MyFlickr.Rest.Synchronous
{
    public static class SynchronousUser
    {
        /// <summary>
        /// Get a list of contacts for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="contactFilter">An optional filter of the results. The following values are valid:friends,family,both,neither</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>ContactsList Object</returns>
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
        /// <param name="userInstance"></param>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>ContactsList Object</returns>
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
        /// <param name="user"></param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>ContactsList Object</returns>
        public static ContactsList GetPublicContactsList(this User user, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            return GetPublicContactsList(user, user, page, perPage);
        }

        /// <summary>
        /// Return photos from the given user's photostream. Only photos visible to the calling user will be returned. This method must be authenticated;
        /// to return public photos for a user, use User.getPublicPhotos.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user"></param>
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
        /// <returns>PhotoCollection Object</returns>
        public static PhotosCollection GetPhotos(this User user, Nullable<SafeSearch> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
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
        /// <param name="userInstance"></param>
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
        /// <returns>PhotoCollection Object</returns>
        public static PhotosCollection GetPhotos(this User userInstance, User user, Nullable<SafeSearch> safeSearch = null, string minUploadDate = null, string maxUploadDate = null
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
        /// <param name="userInstance"></param>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotoCollection Object</returns>
        public static PhotosCollection GetPublicPhotos(this User userInstance, User user, Nullable<SafeSearch> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
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
        /// <param name="user"></param>
        /// <param name="safeSearch">Safe search setting</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotoCollection Object</returns>
        public static PhotosCollection GetPublicPhotos(this User user, Nullable<SafeSearch> safeSearch = null, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            return GetPublicPhotos(user, user, safeSearch, extras, perPage, page);
        }

        /// <summary>
        /// Get information about the user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>UserInfo Object</returns>
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
        /// <param name="userInstance"></param>
        /// <param name="user">User Object that represents a Flickr User.</param>
        /// <returns>UserInfo Object</returns>
        public static UserInfo GetInfoOf(this User userInstance, User user)
        {
            FlickrSynchronousPrmitive<UserInfo> FSP = new FlickrSynchronousPrmitive<UserInfo>();

            Action<object, EventArgs<UserInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            userInstance.GetInfoOfCompleted += new EventHandler<EventArgs<UserInfo>>(handler);
            FSP.Token = userInstance.GetInfoOfAsync(user);
            FSP.WaitForAsynchronousCall();
            userInstance.GetInfoOfCompleted -= new EventHandler<EventArgs<UserInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
