using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    public static class SynchronousBlogs
    {
        /// <summary>
        /// Return a list of Flickr supported blogging services.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="blogs"></param>
        /// <returns>Enumerable of  BloggingService Objects</returns>
        public static IEnumerable<BloggingService> GetServices(this Blogs blogs)
        {
            FlickrSynchronousPrmitive<IEnumerable<BloggingService>> FSP = new FlickrSynchronousPrmitive<IEnumerable<BloggingService>>();

            Action<object, EventArgs<IEnumerable<BloggingService>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            blogs.GetServicesCompleted += new EventHandler<EventArgs<IEnumerable<BloggingService>>>(handler);
            FSP.Token = blogs.GetServicesAsync();
            FSP.WaitForAsynchronousCall();
            blogs.GetServicesCompleted -= new EventHandler<EventArgs<IEnumerable<BloggingService>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
