using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Photo
    /// </summary>
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

        /// <summary>
        /// Add a note to a photo. Coordinates and sizes are in pixels, based on the 500px image size shown on individual photo pages.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="x">The left coordinate of the note.</param>
        /// <param name="y">The top coordinate of the note.</param>
        /// <param name="height">The height of the note.</param>
        /// <param name="width">The width of the note</param>
        /// <param name="text">The description of the note</param>
        /// <returns>the ID of the new Note</returns>
        public static string AddNote(this Photo photo, int x, int y, int height, int width, string text)
        {
            FlickrSynchronousPrmitive<string> FSP = new FlickrSynchronousPrmitive<string>();

            Action<object, EventArgs<string>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.AddNoteCompleted += new EventHandler<EventArgs<string>>(handler);
            FSP.Token = photo.AddNoteAync(x,y,height,width,text);
            FSP.WaitForAsynchronousCall();
            photo.AddNoteCompleted -= new EventHandler<EventArgs<string>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Delete a note from a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="noteID">The id of the note to delete.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply DeleteNote(this Photo photo, string noteID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.DeleteNoteCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.DeleteNoteAsync(noteID);
            FSP.WaitForAsynchronousCall();
            photo.DeleteNoteCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Edit a note on a photo. Coordinates and sizes are in pixels, based on the 500px image size shown on individual photo pages. 
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="noteID">The id of the note to edit.</param>
        /// <param name="x">The left coordinate of the note.</param>
        /// <param name="y">The top coordinate of the note.</param>
        /// <param name="height">The height of the note</param>
        /// <param name="width">The width of the note.</param>
        /// <param name="text">The description of the note.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply EditNote(this Photo photo, string noteID, int x, int y, int height, int width, string text)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.EditNoteCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.EditNoteAsync(noteID,x,y,height,width,text);
            FSP.WaitForAsynchronousCall();
            photo.EditNoteCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Add a person to a photo. Coordinates and sizes of boxes are optional; they are measured in pixels, based on the 500px image size shown on individual photo pages.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="personID">The NSID of the user to add to the photo.</param>
        /// <param name="x">The left-most pixel co-ordinate of the box around the person.</param>
        /// <param name="y">The top-most pixel co-ordinate of the box around the person.</param>
        /// <param name="height">The height (in pixels) of the box around the person.</param>
        /// <param name="width">The width (in pixels) of the box around the person.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply AddPerson(this Photo photo, string personID, Nullable<int> x = null, Nullable<int> y = null, Nullable<int> height = null, Nullable<int> width = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.AddPersonCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.AddPersonAync(personID,x,y,height,width);
            FSP.WaitForAsynchronousCall();
            photo.AddPersonCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Remove a person from a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="personID">The NSID of the person to remove from the photo.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply RemovePerson(this Photo photo, string personID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.RemovePersonCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.RemovePersonAsync(personID);
            FSP.WaitForAsynchronousCall();
            photo.RemovePersonCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Remove the bounding box from a person in a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="personID">The NSID of the person whose bounding box you want to remove.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply RemovePersonCoords(this Photo photo, string personID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.RemovePersonCoordsCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.RemovePersonCoordsAsync(personID);
            FSP.WaitForAsynchronousCall();
            photo.RemovePersonCoordsCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Edit the bounding box of an existing person on a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="personID">The NSID of the person to edit in a photo.</param>
        /// <param name="x">The left-most pixel co-ordinate of the box around the person.</param>
        /// <param name="y">The top-most pixel co-ordinate of the box around the person.</param>
        /// <param name="height">The width (in pixels) of the box around the person.</param>
        /// <param name="width">The width (in pixels) of the box around the person.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply EditPersonCoords(this Photo photo, string personID, int x, int y, int height, int width)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.EditPersonCoordsCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.EditPersonCoordsAsync(personID,x,y,height,width);
            FSP.WaitForAsynchronousCall();
            photo.EditPersonCoordsCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of people in a given photo.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Enumerable of PersonInPhoto Objects</returns>
        public static IEnumerable<PersonInPhoto> GetPersonsList(this Photo photo)
        {
            FlickrSynchronousPrmitive<IEnumerable<PersonInPhoto>> FSP = new FlickrSynchronousPrmitive<IEnumerable<PersonInPhoto>>();

            Action<object, EventArgs<IEnumerable<PersonInPhoto>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetPersonsListCompleted += new EventHandler<EventArgs<IEnumerable<PersonInPhoto>>>(handler);
            FSP.Token = photo.GetPersonsListAsync();
            FSP.WaitForAsynchronousCall();
            photo.GetPersonsListCompleted -= new EventHandler<EventArgs<IEnumerable<PersonInPhoto>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 

        }

        /// <summary>
        /// Rotate a photo.
        /// This method requires authentication with 'write' permission.
        /// the Photo Should belong to the calling user.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="degrees">The amount of degrees by which to rotate the photo (clockwise) from it's current orientation. Valid values are 90, 180 and 270.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply Rotate(this Photo photo,Degrees degrees)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.RotateCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.RotateAsync(degrees);
            FSP.WaitForAsynchronousCall();
            photo.RotateCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Add comment to a photo as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="text">Text of the comment.</param>
        /// <returns>the ID of the Added Comment</returns>
        public static string AddComment(this Photo photo, string text)
        {
            FlickrSynchronousPrmitive<string> FSP = new FlickrSynchronousPrmitive<string>();

            Action<object, EventArgs<string>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.AddCommentCompleted += new EventHandler<EventArgs<string>>(handler);
            FSP.Token = photo.AddCommentAsync(text);
            FSP.WaitForAsynchronousCall();
            photo.AddCommentCompleted -= new EventHandler<EventArgs<string>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Delete a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="commentID">The id of the comment to delete.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply DeleteComment(this Photo photo, string commentID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.DeleteCommentCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.DeleteCommentAsync(commentID);
            FSP.WaitForAsynchronousCall();
            photo.DeleteCommentCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Edit the text of a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="commentID">The id of the comment to edit.</param>
        /// <param name="text">Update the comment to this text.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply EditComment(this Photo photo, string commentID, string text)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.EditCommentCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.EditCommentAsync(commentID,text);
            FSP.WaitForAsynchronousCall();
            photo.EditCommentCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the comments for a photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="minCommentDate">Minimum date that a a comment was added. The date should be in the form of a unix timestamp.</param>
        /// <param name="maxCommentDate">Maximum date that a comment was added. The date should be in the form of a unix timestamp.</param>
        /// <returns>Enumerable of Comments Object</returns>
        public static IEnumerable<Comment> GetCommentsList(this Photo photo, string minCommentDate = null, string maxCommentDate = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Comment>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Comment>>();

            Action<object, EventArgs<IEnumerable<Comment>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetCommentsListCompleted += new EventHandler<EventArgs<IEnumerable<Comment>>>(handler);
            FSP.Token = photo.GetCommentsListAsync(minCommentDate,maxCommentDate);
            FSP.WaitForAsynchronousCall();
            photo.GetCommentsListCompleted -= new EventHandler<EventArgs<IEnumerable<Comment>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Returns next and previous photos for a photo in a set.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="photosetID">The id of the photoset for which to fetch the photo's context.</param>
        /// <returns>PhotoContext object</returns>
        public static PhotoContext GetContextinSet(this Photo photo, string photosetID)
        {
            FlickrSynchronousPrmitive<PhotoContext> FSP = new FlickrSynchronousPrmitive<PhotoContext>();

            Action<object, EventArgs<PhotoContext>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetContextCompleted += new EventHandler<EventArgs<PhotoContext>>(handler);
            FSP.Token = photo.GetContextinSetAsync(photosetID);
            FSP.WaitForAsynchronousCall();
            photo.GetContextCompleted -= new EventHandler<EventArgs<PhotoContext>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Returns next and previous photos for a photo in a group pool.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="groupID">The nsid of the group who's pool to fetch the photo's context for.</param>
        /// <returns>PhotoContext object</returns>
        public static PhotoContext GetContext(this Photo photo, string groupID)
        {
            FlickrSynchronousPrmitive<PhotoContext> FSP = new FlickrSynchronousPrmitive<PhotoContext>();

            Action<object, EventArgs<PhotoContext>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetContextCompleted += new EventHandler<EventArgs<PhotoContext>>(handler);
            FSP.Token = photo.GetContextAsync(groupID);
            FSP.WaitForAsynchronousCall();
            photo.GetContextCompleted -= new EventHandler<EventArgs<PhotoContext>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Return the list of galleries to which a photo has been added. Galleries are returned sorted by date which the photo was added to the gallery.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="perPage">Number of galleries to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public static GalleriesCollection GetGalleriesList(this Photo photo,Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<GalleriesCollection> FSP = new FlickrSynchronousPrmitive<GalleriesCollection>();

            Action<object, EventArgs<GalleriesCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.GetGalleriesCompleted += new EventHandler<EventArgs<GalleriesCollection>>(handler);
            FSP.Token = photo.GetGalleriesListAsync(perPage,page);
            FSP.WaitForAsynchronousCall();
            photo.GetGalleriesCompleted -= new EventHandler<EventArgs<GalleriesCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Sets the license for a photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photo"></param>
        /// <param name="licenseID">The license to apply, or 0 (zero) to remove the current license. Note : as of this writing the "no known copyright restrictions" license (7) is not a valid argument.</param>
        /// <returns>NoReply represents Void</returns>
        public static NoReply SetLicense(this Photo photo, int licenseID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photo.SetLicenseCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photo.SetLicenseAsync(licenseID);
            FSP.WaitForAsynchronousCall();
            photo.SetLicenseCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
