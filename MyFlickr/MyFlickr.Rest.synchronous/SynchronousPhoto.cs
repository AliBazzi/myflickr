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
    }
}
