using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Gallery.
    /// </summary>
    public static class SynchronousGallery
    {
        /// <summary>
        /// Add a photo to a gallery.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="gallery">Instance.</param>
        /// <param name="photoID">The photo ID to add to the gallery.</param>
        /// <param name="comment">A short comment or story to accompany the photo.</param>
        /// <returns>NoReply Represents Void.</returns>
        public static NoReply AddPhoto(this Gallery gallery, string photoID, string comment = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            gallery.AddPhotoCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = gallery.AddPhotoAsync(photoID,comment);
            FSP.WaitForAsynchronousCall();
            gallery.AddPhotoCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get an Instance of the newly created Gallery.
        /// </summary>
        /// <param name="gallerytoken">Instance.</param>
        /// <returns>Gallery Object.</returns>
        public static Gallery CreateInstance(this GalleryToken gallerytoken)
        {
            FlickrSynchronousPrmitive<Gallery> FSP = new FlickrSynchronousPrmitive<Gallery>();

            Action<object, EventArgs<Gallery>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            gallerytoken.CreateInstanceCompleted += new EventHandler<EventArgs<Gallery>>(handler);
            FSP.Token = gallerytoken.CreateInstanceAsync();
            FSP.WaitForAsynchronousCall();
            gallerytoken.CreateInstanceCompleted -= new EventHandler<EventArgs<Gallery>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Modify the meta-data for a gallery.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="gallery">Instance.</param>
        /// <param name="title">The new title for the gallery.</param>
        /// <param name="description">The new description for the gallery.</param>
        /// <returns>NoReply Represents Void.</returns>
        public static NoReply EditMetadata(this Gallery gallery, string title, string description = null)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            gallery.EditMetadataCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = gallery.EditMetadataAsync(title, description);
            FSP.WaitForAsynchronousCall();
            gallery.EditMetadataCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Edit the comment for a gallery photo.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="gallery">Instance.</param>
        /// <param name="photoID">The photo ID to edit in the gallery.</param>
        /// <param name="comment">The comment.</param>
        /// <returns>NoReply Represents Void.</returns>
        public static NoReply EditPhoto(this Gallery gallery, string photoID, string comment)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            gallery.EditPhotoCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = gallery.EditPhotoAsync(photoID, comment);
            FSP.WaitForAsynchronousCall();
            gallery.EditPhotoCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Return the list of photos for a gallery.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="gallery">Instance.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetPhotos(this Gallery gallery, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            gallery.GetPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = gallery.GetPhotosAsync(extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            gallery.GetPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Modify the photos in a gallery. Use this method to add, remove and re-order photos.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="gallery">Instance.</param>
        /// <param name="primaryPhotoID">The id of the photo to use as the 'primary' photo for the gallery.</param>
        /// <param name="photosIDs">A comma-delimited list of photo ids to include in the gallery. They will appear in the set in the order sent. This list must contain the primary photo id. This list of photos replaces the existing list.</param>
        /// <returns>NoReply Represents Void.</returns>
        public static NoReply EditPhotos(this Gallery gallery, string primaryPhotoID, params string[] photosIDs)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            gallery.EditPhotosCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = gallery.EditPhotosAsync(primaryPhotoID,photosIDs);
            FSP.WaitForAsynchronousCall();
            gallery.EditPhotosCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
