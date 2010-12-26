using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    public static class SynchronousInterestingness
    {
        /// <summary>
        /// Returns the list of interesting photos for the most recent day or a user-specified date.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="interestingness"></param>
        /// <param name="date">A specific date, formatted as YYYY-MM-DD, to return interesting photos for.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object</returns>
        public static PhotosCollection GetList(this Interestingness interestingness, string date = null, string extras = null ,
            Nullable<int> perPage = null , Nullable<int> page = null )
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            interestingness.GetListCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = interestingness.GetListAsync(date,extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            interestingness.GetListCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow(); 
        }
    }
}
