using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    public static class SynchronousPhoto
    {
        /// <summary>
        /// Adds a photo to a user's favorites list.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply AddToFavorite(this Photo photo)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.AddToFavoriteCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.AddToFavoriteAsync();
            FSP.WaitForAsynchronousCall();
            photo.AddToFavoriteCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Remove a photo from a user's favorites list.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        public static NoReply RemoveFromFavorite(this Photo photo)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.RemoveFromFavoriteCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.RemoveFromFavoriteAsync();
            FSP.WaitForAsynchronousCall();
            photo.RemoveFromFavoriteCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Retrieves a list of EXIF/TIFF/GPS tags for a given photo. The calling user must have permission to view the photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>Enumerable of Exif Objects</returns>
        public static IEnumerable<Exif> GetExif(this Photo photo)
        {
            FlickrSynchronousPrmitive<IEnumerable<Exif>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Exif>>();

            Action<object, EventArgs<IEnumerable<Exif>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetExifCompleted += new EventHandler<EventArgs<IEnumerable<Exif>>>(handler);
            FSP.Token = photo.GetExifAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetExifCompleted -= new EventHandler<EventArgs<IEnumerable<Exif>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the list of people who have favorited a given photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="page">Number of users to return per page. If this argument is omitted, it defaults to 10. The maximum allowed value is 50.</param>
        /// <param name="perPage">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Enumerable of Person Objects</returns>
        public static IEnumerable<Person> GetFavorites(this Photo photo, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Person>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Person>>();

            Action<object, EventArgs<IEnumerable<Person>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetFavoritesCompleted += new EventHandler<EventArgs<IEnumerable<Person>>>(handler);
            FSP.Token = photo.GetFavoritesAsync(page,perPage);
            FSP.WaitForAsynchronousCall();
            photo.GetFavoritesCompleted -= new EventHandler<EventArgs<IEnumerable<Person>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Post the Photo to a Blog.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="title">The blog post title</param>
        /// <param name="description">The blog post body</param>
        /// <param name="blogPassword">The password for the blog (used when the blog does not have a stored password).</param>
        /// <param name="blogID">The id of the blog to post to.</param>
        /// <param name="service">A Flickr supported blogging service. Instead of passing a blog id you can pass a service id and we'll post to the first blog of that service we find.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply PostPhotoToBlog(this Photo photo , string title, string description, string blogPassword = null,
            Nullable<Int64> blogID = null, string service = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.PostPhotoToBlogCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.PostPhotoToBlogAsync(title,description,blogPassword,blogID,service);
            FSP.WaitForAsynchronousCall();
            photo.PostPhotoToBlogCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get information about a photo. The calling user must have permission to view the photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>PhotoInfo Object</returns>
        public static PhotoInfo GetInfo(this Photo photo)
        {
            FlickrSynchronousPrmitive<PhotoInfo> FSP = new FlickrSynchronousPrmitive<PhotoInfo>();

            Action<object, EventArgs<PhotoInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetInfoCompleted += new EventHandler<EventArgs<PhotoInfo>>(handler);
            FSP.Token = photo.GetInfoAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetInfoCompleted -= new EventHandler<EventArgs<PhotoInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Add tags to a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="tag">The tags to add to the photo.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply AddTag(this Photo photo, string tag)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.AddTagCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.AddTagAsync(tag);
            FSP.WaitForAsynchronousCall();
            photo.AddTagCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Remove a tag from a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="tagID">The tag to remove from the photo. This parameter should contain a tag id, as returned by flickr.photos.getInfo.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply RemoveTag(this Photo photo, string tagID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.RemoveTagCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.RemoveTagAsync(tagID);
            FSP.WaitForAsynchronousCall();
            photo.RemoveTagCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get permissions for a photo.
        /// This method requires authentication with 'read' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>Permissions Object</returns>
        public static Permissions GetPermissions(this Photo photo)
        {
            FlickrSynchronousPrmitive<Permissions> FSP = new FlickrSynchronousPrmitive<Permissions>();

            Action<object, EventArgs<Permissions>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetPermissionsCompleted += new EventHandler<EventArgs<Permissions>>(handler);
            FSP.Token = photo.GetPermissionsAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetPermissionsCompleted -= new EventHandler<EventArgs<Permissions>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set permissions for a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="isPublic">true to set the photo to public, false to set it to private.</param>
        /// <param name="isFriend">true to make the photo visible to friends when private, false to not.</param>
        /// <param name="isFamily">true to make the photo visible to family when private, false to not.</param>
        /// <param name="commentPermission">who can add comments to the photo and it's notes.</param>
        /// <param name="addMetadataPermission">who can add notes and tags to the photo.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetPermissions(this Photo photo ,bool isPublic, bool isFriend, bool isFamily, CommentPermission commentPermission, AddMetadataPermission addMetadataPermission)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.SetPermissionsCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.SetPermissionsAsync(isPublic,isFriend,isFriend,commentPermission,addMetadataPermission);
            FSP.WaitForAsynchronousCall();
            photo.SetPermissionsCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set the meta information for a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="title">The title for the photo.</param>
        /// <param name="description">The description for the photo.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetMeta(this Photo photo, string title, string description)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.SetMetaCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.SetMetaAsync(title,description);
            FSP.WaitForAsynchronousCall();
            photo.SetMetaCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set the safety level of a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="safetyLevel">The safety level of the photo.</param>
        /// <param name="isHidden">Whether or not to additionally hide the photo from public searches. Must be either True for Yes or false for No.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetSafetyLevel(this Photo photo, SafetyLevel safetyLevel, Nullable<bool> isHidden = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.SetSafetyLevelCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.SetSafetyLevelAsync(safetyLevel, isHidden);
            FSP.WaitForAsynchronousCall();
            photo.SetSafetyLevelCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set one or both of the dates for a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="datePosted">The date the photo was uploaded to flickr . more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="dateTaken">The date the photo was taken . more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <param name="dateTakenGranularity">The granularity of the date the photo was taken. more info about formats Flickr Accepts for  date : http://www.flickr.com/services/api/misc.dates.html </param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetDates(this Photo photo, string datePosted = null, string dateTaken = null, string dateTakenGranularity = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.SetDatesCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.SetDatesAsync(datePosted,dateTaken,dateTakenGranularity);
            FSP.WaitForAsynchronousCall();
            photo.SetDatesCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set the content type of a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="contentType">The content type of the photo. Must be one of: Photo, Screenshot, andOther , Only .</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetContentType(this Photo photo, ContentType contentType)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.SetContentTypeCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.SetContentTypeAsync(contentType);
            FSP.WaitForAsynchronousCall();
            photo.SetContentTypeCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Delete a photo from flickr.
        /// This method requires authentication with 'delete' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public static NoReply Delete(this Photo photo)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.DeleteCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.DeleteAsync();
            FSP.WaitForAsynchronousCall();
            photo.DeleteCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns next and previous photos for a photo in a photostream.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <returns>PhotoContext object</returns>
        public static PhotoContext GetContext(this Photo photo)
        {
            FlickrSynchronousPrmitive<PhotoContext> FSP = new FlickrSynchronousPrmitive<PhotoContext>();

            Action<object, EventArgs<PhotoContext>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetContextCompleted += new EventHandler<EventArgs<PhotoContext>>(handler);
            FSP.Token = photo.GetContextAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetContextCompleted -= new EventHandler<EventArgs<PhotoContext>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns all visible sets and pools the photo belongs to.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>PhotoContexts Object</returns>
        public static PhotoContexts GetAllContexts(this Photo photo)
        {
            FlickrSynchronousPrmitive<PhotoContexts> FSP = new FlickrSynchronousPrmitive<PhotoContexts>();

            Action<object, EventArgs<PhotoContexts>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetAllContextsCompleted += new EventHandler<EventArgs<PhotoContexts>>(handler);
            FSP.Token = photo.GetAllContextsAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetAllContextsCompleted -= new EventHandler<EventArgs<PhotoContexts>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the available sizes for a photo. The calling user must have permission to view the photo.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Enumerable of Size Objects</returns>
        public static IEnumerable<Size> GetSizes(this Photo photo)
        {
            FlickrSynchronousPrmitive<IEnumerable<Size>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Size>>();

            Action<object, EventArgs<IEnumerable<Size>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetSizesCompleted += new EventHandler<EventArgs<IEnumerable<Size>>>(handler);
            FSP.Token = photo.GetSizesAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetSizesCompleted -= new EventHandler<EventArgs<IEnumerable<Size>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
