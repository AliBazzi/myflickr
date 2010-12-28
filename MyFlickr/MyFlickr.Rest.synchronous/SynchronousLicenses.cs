using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Licenses
    /// </summary>
    public static class SynchronousLicenses
    {
        /// <summary>
        /// Fetches a list of available photo licenses for Flickr.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="licenses"></param>
        /// <returns>Enumerable of License Objects</returns>
        public static IEnumerable<License> GetInfo(this Licenses licenses)
        {
            FlickrSynchronousPrmitive<IEnumerable<License>> FSP = new FlickrSynchronousPrmitive<IEnumerable<License>>();

            Action<object, EventArgs<IEnumerable<License>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            licenses.GetInfoCompleted += new EventHandler<EventArgs<IEnumerable<License>>>(handler);
            FSP.Token = licenses.GetInfoAsync();
            FSP.WaitForAsynchronousCall();
            licenses.GetInfoCompleted -= new EventHandler<EventArgs<IEnumerable<License>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
