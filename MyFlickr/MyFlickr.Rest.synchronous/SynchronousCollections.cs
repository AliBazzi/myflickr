using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Collections.
    /// </summary>
    public static class SynchronousCollections
    {
        /// <summary>
        /// Returns information for a single collection. Currently can only be called by the collection owner, this may change.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="collection">Instance.</param>
        /// <returns>CollectionInfo Object.</returns>
        public static CollectionInfo GetInfo(this Collection collection)
        {
            FlickrSynchronousPrmitive<CollectionInfo> FSP = new FlickrSynchronousPrmitive<CollectionInfo>();

            Action<object, EventArgs<CollectionInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            collection.GetInfoCompleted += new EventHandler<EventArgs<CollectionInfo>>(handler);
            FSP.Token = collection.GetInfoAsync();
            FSP.WaitForAsynchronousCall();
            collection.GetInfoCompleted -= new EventHandler<EventArgs<CollectionInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
