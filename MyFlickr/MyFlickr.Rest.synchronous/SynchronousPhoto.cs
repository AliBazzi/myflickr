using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
