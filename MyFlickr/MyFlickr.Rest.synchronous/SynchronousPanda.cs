using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Panda
    /// </summary>
    public static class SynchronousPanda
    {
        /// <summary>
        /// Return a list of Flickr pandas, from whom you can request photos using the flickr.panda.getPhotos API method. 
        /// This method does not require authentication.
        /// </summary>
        /// <param name="panda"></param>
        /// <returns>Enumerable of Panda names</returns>
        public static IEnumerable<string> GetList(this Panda panda)
        {
            FlickrSynchronousPrmitive<IEnumerable<string>> FSP = new FlickrSynchronousPrmitive<IEnumerable<string>>();

            Action<object, EventArgs<IEnumerable<string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            panda.GetListCompleted += new EventHandler<EventArgs<IEnumerable<string>>>(handler);
            FSP.Token = panda.GetListAsync();
            FSP.WaitForAsynchronousCall();
            panda.GetListCompleted -= new EventHandler<EventArgs<IEnumerable<string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Ask the Flickr Pandas for a list of recent public (and "safe") photos. 
        /// This method does not require authentication.
        /// </summary>
        /// <param name="panda"></param>
        /// <param name="pandaName">The name of the panda to ask for photos from.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PandaPhotosCollection Object</returns>
        public static PandaPhotosCollection GetPhotos(this Panda panda, string pandaName, string extras = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PandaPhotosCollection> FSP = new FlickrSynchronousPrmitive<PandaPhotosCollection>();

            Action<object, EventArgs<PandaPhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            panda.GetPhotosCompleted += new EventHandler<EventArgs<PandaPhotosCollection>>(handler);
            FSP.Token = panda.GetPhotosAsync(pandaName,extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            panda.GetPhotosCompleted -= new EventHandler<EventArgs<PandaPhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
