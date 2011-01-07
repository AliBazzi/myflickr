using System;
using System.Collections.Generic;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extensions Methods for Tags.
    /// </summary>
    public static class SynchronousTags
    {
        /// <summary>
        /// Returns a list of hot tags for the given period.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="period">The period for which to fetch hot tags. Valid values are day and week (defaults to day).</param>
        /// <param name="count">The number of tags to return. Defaults to 20. Maximum allowed value is 200.</param>
        /// <returns>Enumerable of Tuple that holds the Score and the Value of the Tag.</returns>
        public static IEnumerable<Tuple<int, string>> GetHotList(this Tags tags, string period = null, Nullable<int> count = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Tuple<int, string>>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Tuple<int, string>>>();

            Action<object, EventArgs<IEnumerable<Tuple<int, string>>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetHotListCompleted += new EventHandler<EventArgs<IEnumerable<Tuple<int, string>>>>(handler);
            FSP.Token = tags.GetHotListAsync(period,count);
            FSP.WaitForAsynchronousCall();
            tags.GetHotListCompleted -= new EventHandler<EventArgs<IEnumerable<Tuple<int, string>>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Gives you a list of tag clusters for the given tag.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="tag">The tag to fetch clusters for.</param>
        /// <returns>Enumerable of Cluster Objects.</returns>
        public static IEnumerable<Cluster> GetClusters(this Tags tags, string tag)
        {
            FlickrSynchronousPrmitive<IEnumerable<Cluster>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Cluster>>();

            Action<object, EventArgs<IEnumerable<Cluster>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetClustersCompleted += new EventHandler<EventArgs<IEnumerable<Cluster>>>(handler);
            FSP.Token = tags.GetClustersAsync(tag);
            FSP.WaitForAsynchronousCall();
            tags.GetClustersCompleted -= new EventHandler<EventArgs<IEnumerable<Cluster>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns the first 24 photos for a given tag cluster.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="tag">The tag that this cluster belongs to.</param>
        /// <param name="clusterID">The top three tags for the cluster, separated by dashes (just like the url).</param>
        /// <returns>Enumerable of Photo Objects..</returns>
        public static IEnumerable<Photo> GetClusterPhotos(this Tags tags, string tag, string clusterID)
        {
            FlickrSynchronousPrmitive<IEnumerable<Photo>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Photo>>();

            Action<object, EventArgs<IEnumerable<Photo>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetClusterPhotosCompleted += new EventHandler<EventArgs<IEnumerable<Photo>>>(handler);
            FSP.Token = tags.GetClusterPhotosAsync(tag,clusterID);
            FSP.WaitForAsynchronousCall();
            tags.GetClusterPhotosCompleted -= new EventHandler<EventArgs<IEnumerable<Photo>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the tag list for a given photo.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="photoID">The id of the photo to return tags for.</param>
        /// <returns>Enumerable of Tag Objects.</returns>
        public static IEnumerable<Tag> GetTagsListofPhoto(this Tags tags, string photoID)
        {
            FlickrSynchronousPrmitive<IEnumerable<Tag>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Tag>>();

            Action<object, EventArgs<IEnumerable<Tag>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetTagsListofPhotoCompleted += new EventHandler<EventArgs<IEnumerable<Tag>>>(handler);
            FSP.Token = tags.GetTagsListofPhotoAsync(photoID);
            FSP.WaitForAsynchronousCall();
            tags.GetTagsListofPhotoCompleted -= new EventHandler<EventArgs<IEnumerable<Tag>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the tag list for a given user (or the currently logged in user).
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="userID">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <returns>Enumerable of Tags Values.</returns>
        public static IEnumerable<string> GetTagsListofUser(this Tags tags, string userID = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<string>> FSP = new FlickrSynchronousPrmitive<IEnumerable<string>>();

            Action<object, EventArgs<IEnumerable<string>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetTagsListofUserCompleted += new EventHandler<EventArgs<IEnumerable<string>>>(handler);
            FSP.Token = tags.GetTagsListofUserAsync(userID);
            FSP.WaitForAsynchronousCall();
            tags.GetTagsListofUserCompleted -= new EventHandler<EventArgs<IEnumerable<string>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the popular tags for a given user (or the currently logged in user).
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="userID">The NSID of the user to fetch the tag list for. If this argument is not specified, the currently logged in user (if any) is assumed.</param>
        /// <param name="count">Number of popular tags to return. defaults to 10 when this argument is not present.</param>
        /// <returns>Enumerable of Tuple that holds the count and the Value of the Tag.</returns>
        public static IEnumerable<Tuple<int, string>> GetPopularTagsListofUser(this Tags tags, string userID = null, Nullable<int> count = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Tuple<int, string>>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Tuple<int, string>>>();

            Action<object, EventArgs<IEnumerable<Tuple<int, string>>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetPopularTagsListofUserCompleted += new EventHandler<EventArgs<IEnumerable<Tuple<int, string>>>>(handler);
            FSP.Token = tags.GetPopularTagsListofUserAsync(userID, count);
            FSP.WaitForAsynchronousCall();
            tags.GetPopularTagsListofUserCompleted -= new EventHandler<EventArgs<IEnumerable<Tuple<int, string>>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get the raw versions of a given tag (or all tags) for the currently logged-in user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="tag">The tag you want to retrieve all raw versions for.</param>
        /// <returns>Enumerable of Tuple that holds the clean tag value and the an Enumerable of Raw values of the Tag.</returns>
        public static IEnumerable<Tuple<string, IEnumerable<string>>> GetRawTagsListofUser(this Tags tags, string tag = null)
        {
            FlickrSynchronousPrmitive<IEnumerable<Tuple<string, IEnumerable<string>>>> FSP = new FlickrSynchronousPrmitive<IEnumerable<Tuple<string, IEnumerable<string>>>>();

            Action<object, EventArgs<IEnumerable<Tuple<string, IEnumerable<string>>>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetRawTagsListofUserCompleted += new EventHandler<EventArgs<IEnumerable<Tuple<string, IEnumerable<string>>>>>(handler);
            FSP.Token = tags.GetRawTagsListofUserAsync(tag);
            FSP.WaitForAsynchronousCall();
            tags.GetRawTagsListofUserCompleted -= new EventHandler<EventArgs<IEnumerable<Tuple<string, IEnumerable<string>>>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of tags 'related' to the given tag, based on clustered usage analysis.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="tags">Instance.</param>
        /// <param name="tag">The tag to fetch related tags for.</param>
        /// <returns>Tuple that holds the source tag value and the an Enumerable of related tags values.</returns>
        public static Tuple<string, IEnumerable<string>> GetRelatedTags(this Tags tags, string tag)
        {
            FlickrSynchronousPrmitive<Tuple<string, IEnumerable<string>>> FSP = new FlickrSynchronousPrmitive<Tuple<string, IEnumerable<string>>>();

            Action<object, EventArgs<Tuple<string, IEnumerable<string>>>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            tags.GetRelatedTagsCompleted += new EventHandler<EventArgs<Tuple<string, IEnumerable<string>>>>(handler);
            FSP.Token = tags.GetRelatedTagsAsync(tag);
            FSP.WaitForAsynchronousCall();
            tags.GetRelatedTagsCompleted -= new EventHandler<EventArgs<Tuple<string, IEnumerable<string>>>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
