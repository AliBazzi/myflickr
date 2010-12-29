using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Urls.
    /// </summary>
    public static class SynchronousUrls
    {
        /// <summary>
        /// /// Returns the url to a group's page.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="urls">Instance.</param>
        /// <param name="groupID">The NSID of the group to fetch the url for.</param>
        /// <returns>Url of the Group on Flickr.</returns>
        public static Uri GetGroup(this Urls urls, string groupID)
        {
            FlickrSynchronousPrmitive<Uri> FSP = new FlickrSynchronousPrmitive<Uri>();

            Action<object, EventArgs<Uri>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            urls.GetGroupCompleted += new EventHandler<EventArgs<Uri>>(handler);
            FSP.Token = urls.GetGroupAsync(groupID);
            FSP.WaitForAsynchronousCall();
            urls.GetGroupCompleted -= new EventHandler<EventArgs<Uri>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// /// Returns the url to a user's photos.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="urls">Instance.</param>
        /// <param name="userID">The NSID of the user to fetch the url for. If omitted, the calling user is assumed.</param>
        /// <returns>Url of the User photostream on Flickr.</returns>
        public static Uri GetUserPhotos(this Urls urls, string userID = null)
        {
            FlickrSynchronousPrmitive<Uri> FSP = new FlickrSynchronousPrmitive<Uri>();

            Action<object, EventArgs<Uri>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            urls.GetUserPhotosCompleted += new EventHandler<EventArgs<Uri>>(handler);
            FSP.Token = urls.GetUserPhotosAsync(userID);
            FSP.WaitForAsynchronousCall();
            urls.GetUserPhotosCompleted -= new EventHandler<EventArgs<Uri>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the url to a user's profile.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="urls">Instance.</param>
        /// <param name="userID">The NSID of the user to fetch the url for. If omitted, the calling user is assumed.</param>
        /// <returns>Url of the User profile on Flickr.</returns>
        public static Uri GetUserProfile(this Urls urls, string userID = null)
        {
            FlickrSynchronousPrmitive<Uri> FSP = new FlickrSynchronousPrmitive<Uri>();

            Action<object, EventArgs<Uri>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            urls.GetUserProfileCompleted += new EventHandler<EventArgs<Uri>>(handler);
            FSP.Token = urls.GetUserProfileAsync(userID);
            FSP.WaitForAsynchronousCall();
            urls.GetUserProfileCompleted -= new EventHandler<EventArgs<Uri>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        ///  Returns gallery , by url.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="urls">Instance.</param>
        /// <param name="url">The gallery's URL.</param>
        /// <returns>Gallery Object.</returns>
        public static Gallery LookupGallery(this Urls urls, string url)
        {
            FlickrSynchronousPrmitive<Gallery> FSP = new FlickrSynchronousPrmitive<Gallery>();

            Action<object, EventArgs<Gallery>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            urls.LookupGalleryCompleted += new EventHandler<EventArgs<Gallery>>(handler);
            FSP.Token = urls.LookupGalleryAsync(url);
            FSP.WaitForAsynchronousCall();
            urls.LookupGalleryCompleted -= new EventHandler<EventArgs<Gallery>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a group NSID, given the url to a group's page or photo pool.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="url">The url to the group's page or photo pool.</param>
        /// <returns>Tuple that Holds the ID and the Group Name</returns>
        public static Tuple<string, string> LookupGroup(this Urls urls, string url)
        {
            FlickrSynchronousPrmitive<Tuple<string, string>> FSP = new FlickrSynchronousPrmitive<Tuple<string, string>>();

            Action<object, EventArgs<Tuple<string, string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            urls.LookupGroupCompleted += new EventHandler<EventArgs<Tuple<string, string>>>(handler);
            FSP.Token = urls.LookupGroupAsync(url);
            FSP.WaitForAsynchronousCall();
            urls.LookupGroupCompleted -= new EventHandler<EventArgs<Tuple<string, string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a user NSID, given the url to a user's photos or profile.
        /// This method does not require authentication. 
        /// </summary>
        /// <param name="urls">Instance.</param>
        /// <param name="url">The url to the user's profile or photos page.</param>
        /// <returns>Tuple that Holds the ID and the User Name.</returns>
        public static Tuple<string, string> LookupUser(this Urls urls, string url)
        {
            FlickrSynchronousPrmitive<Tuple<string, string>> FSP = new FlickrSynchronousPrmitive<Tuple<string, string>>();

            Action<object, EventArgs<Tuple<string, string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            urls.LookupUserCompleted += new EventHandler<EventArgs<Tuple<string, string>>>(handler);
            FSP.Token = urls.LookupUserAsync(url);
            FSP.WaitForAsynchronousCall();
            urls.LookupUserCompleted -= new EventHandler<EventArgs<Tuple<string, string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
