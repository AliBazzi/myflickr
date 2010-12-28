using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for PhotoSet
    /// </summary>
    public static class SynchronousPhotoSets
    {
        /// <summary>
        /// Returns the comments for a photoset.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photoset"></param>
        /// <returns>Enumerable of comments</returns>
        public static IEnumerable<Comment> GetCommentsList(this PhotoSetBasic photoset)
        {
            FlickrSynchronousPrmitive<IEnumerable<Comment>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Comment>>();

            Action<object, EventArgs<IEnumerable<Comment>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.GetCommentsListCompleted += new EventHandler<EventArgs<IEnumerable<Comment>>>(handler);
            FSP.Token = photoset.GetCommentsListAsync();
            FSP.WaitForAsynchronousCall();
            photoset.GetCommentsListCompleted -= new EventHandler<EventArgs<IEnumerable<Comment>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Add a comment to a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="text">Text of the comment.</param>
        /// <returns>the ID of the Added Comment</returns>
        public static string AddComment(this PhotoSetBasic photoset,string text)
        {
            FlickrSynchronousPrmitive<string> FSP = new FlickrSynchronousPrmitive<string>();

            Action<object, EventArgs<string>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.AddCommentCompleted += new EventHandler<EventArgs<string>>(handler);
            FSP.Token = photoset.AddCommentAsync(text);
            FSP.WaitForAsynchronousCall();
            photoset.AddCommentCompleted -= new EventHandler<EventArgs<string>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Delete a photoset comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="commentID">The id of the comment to delete from a photoset.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply DeleteComment(this PhotoSetBasic photoset, string commentID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.DeleteCommentCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.DeleteCommentAsync(commentID);
            FSP.WaitForAsynchronousCall();
            photoset.DeleteCommentCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Edit the text of a comment as the currently authenticated user.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="commentID">The id of the comment to edit.</param>
        /// <param name="text">Update the comment to this text.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply EditComment(this PhotoSetBasic photoset, string commentID, string text)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.EditCommentCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.EditCommentAsync(commentID,text);
            FSP.WaitForAsynchronousCall();
            photoset.EditCommentCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Delete a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply Delete(this PhotoSetBasic photoset)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.DeleteCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.DeleteAsync();
            FSP.WaitForAsynchronousCall();
            photoset.DeleteCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Get a Photoset Instance
        /// </summary>
        /// <param name="PST"></param>
        /// <returns>PhotoSet Object</returns>
        public static PhotoSet CreateInstance(this PhotoSetToken PST)
        {
            FlickrSynchronousPrmitive<PhotoSet> FSP = new FlickrSynchronousPrmitive<PhotoSet>();

            Action<object, EventArgs<PhotoSet>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            PST.CreateInstanceCompleted += new EventHandler<EventArgs<PhotoSet>>(handler);
            FSP.Token = PST.CreateInstanceAsync();
            FSP.WaitForAsynchronousCall();
            PST.CreateInstanceCompleted -= new EventHandler<EventArgs<PhotoSet>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Add a photo to the end of an existing photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="photoID">The id of the photo to add to the set.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply AddPhoto(this PhotoSetBasic photoset,string photoID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.AddPhotoCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.AddPhotoAsync(photoID);
            FSP.WaitForAsynchronousCall();
            photoset.AddPhotoCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Remove a photo from a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="photoID">The id of the photo to remove from the set.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply RemovePhoto(this PhotoSetBasic photoset, string photoID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.RemovePhotoCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.RemovePhotoAsync(photoID);
            FSP.WaitForAsynchronousCall();
            photoset.RemovePhotoCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }

        /// <summary>
        /// Remove multiple photos from a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="photoIDs">list of photo ids to remove from the photoset.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply RemovePhotos(this PhotoSetBasic photoset,params string[] photoIDs)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.RemovePhotosCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.RemovePhotosAsync(photoIDs);
            FSP.WaitForAsynchronousCall();
            photoset.RemovePhotosCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the list of photos in a set.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_o</param>
        /// <param name="privacyFilter">Return photos only matching a certain privacy level. This only applies when making an authenticated call to view a photoset you own.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 500. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="mediaType">Filter results by media type. Possible values are all (default), photos or videos</param>
        /// <returns></returns>
        public static PhotosCollection GetPhotos(this PhotoSetBasic photoset, string extras = null, Nullable<PrivacyFilter> privacyFilter = null,
            Nullable<int> perPage = null, Nullable<int> page = null, Nullable<MediaType> mediaType = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.GetPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = photoset.GetPhotosAsync(extras,privacyFilter,perPage,page,mediaType);
            FSP.WaitForAsynchronousCall();
            photoset.GetPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Modify the photos in a photoset. Use this method to add, remove and re-order photos.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="primaryPhotoID">The id of the photo to use as the 'primary' photo for the set. This id must also be passed along in photo_ids list argument.</param>
        /// <param name="photosIDs">A comma-delimited list of photo ids to include in the set. They will appear in the set in the order sent. This list must contain the primary photo id. All photos must belong to the owner of the set. This list of photos replaces the existing list. Call flickr.photosets.addPhoto to append a photo to a set.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply EditPhotos(this PhotoSetBasic photoset, string primaryPhotoID, string[] photosIDs)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.EditPhotosCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.EditPhotosAsync(primaryPhotoID,photosIDs);
            FSP.WaitForAsynchronousCall();
            photoset.EditPhotosCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Modify the meta-data for a photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="title">The new title for the photoset.</param>
        /// <param name="description">A description of the photoset. May contain limited html.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetMetadata(this PhotoSetBasic photoset,string title, string description = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.SetMetadataCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.SetMetadataAsync(title, description);
            FSP.WaitForAsynchronousCall();
            photoset.SetMetadataCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Set photoset primary photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="photoID">The id of the photo to set as primary.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply SetPrimaryPhoto(this PhotoSetBasic photoset, string photoID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.SetPrimaryPhotoCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.SetPrimaryPhotoAsync(photoID);
            FSP.WaitForAsynchronousCall();
            photoset.SetPrimaryPhotoCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Reorder photos in the photoset.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoset"></param>
        /// <param name="photosIDs">Ordered, comma-delimited list of photo ids. Photos that are not in the list will keep their original order.</param>
        /// <returns>NoReply Represents Void</returns>
        public static NoReply ReorderPhotos(this PhotoSetBasic photoset, params string[] photosIDs)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            photoset.ReorderPhotosCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = photoset.ReorderPhotosAsync(photosIDs);
            FSP.WaitForAsynchronousCall();
            photoset.ReorderPhotosCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
