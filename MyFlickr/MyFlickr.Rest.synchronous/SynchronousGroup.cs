using System;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// Extension Methods for Group.
    /// </summary>
    public static class SynchronousGroup
    {
        /// <summary>
        /// Get information about a group.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="group">Instance.</param>
        /// <param name="language">The language of the group name and description to fetch. If the language is not found, the primary language of the group will be returned. Valid values are the same as in feeds.</param>
        /// <returns>GroupInfo object.</returns>
        public static GroupInfo GetInfo(this Group group,string language = null)
        {
            FlickrSynchronousPrmitive<GroupInfo> FSP = new FlickrSynchronousPrmitive<GroupInfo>();

            Action<object, EventArgs<GroupInfo>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            group.GetInfoCompleted += new EventHandler<EventArgs<GroupInfo>>(handler);
            FSP.Token = group.GetInfoAsync(language);
            FSP.WaitForAsynchronousCall();
            group.GetInfoCompleted -= new EventHandler<EventArgs<GroupInfo>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Add a photo to a group's pool.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="photoID">The id of the photo to add to the group pool. The photo must belong to the calling user.</param>
        /// <returns>NoReply Represents Void.</returns>
        public static NoReply Add(this Group group, string photoID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            group.AddCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = group.AddAsync(photoID);
            FSP.WaitForAsynchronousCall();
            group.AddCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Remove a photo from a group pool.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="group">Instance.</param>
        /// <param name="photoID">The id of the photo to remove from the group pool. The photo must either be owned by the calling user of the calling user must be an administrator of the group.</param>
        /// <returns>NoReply Represents Void.</returns>
        public static NoReply Remove(this Group group, string photoID)
        {
            FlickrSynchronousPrmitive<NoReply> FSP = new FlickrSynchronousPrmitive<NoReply>();

            Action<object, EventArgs<NoReply>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            group.RemoveCompleted += new EventHandler<EventArgs<NoReply>>(handler);
            FSP.Token = group.RemoveAsync(photoID);
            FSP.WaitForAsynchronousCall();
            group.RemoveCompleted -= new EventHandler<EventArgs<NoReply>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Returns a list of pool photos for a given group, based on the permissions of the group and the user logged in (if any).
        /// This method does not require authentication.
        /// </summary>
        /// <param name="group">Instance.</param>
        /// <param name="userID">The nsid of a user. Specifying this parameter will retrieve for you only those photos that the user has contributed to the group pool.</param>
        /// <param name="tags">A tag to filter the pool with. At the moment only one tag at a time is supported.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>PhotosCollection Object.</returns>
        public static PhotosCollection GetPhotos(this Group group, string userID = null, string tags = null, string extras = null,
            Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<PhotosCollection> FSP = new FlickrSynchronousPrmitive<PhotosCollection>();

            Action<object, EventArgs<PhotosCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            group.GetPhotosCompleted += new EventHandler<EventArgs<PhotosCollection>>(handler);
            FSP.Token = group.GetPhotosAsync(userID,tags,extras,perPage,page);
            FSP.WaitForAsynchronousCall();
            group.GetPhotosCompleted -= new EventHandler<EventArgs<PhotosCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Get a list of the members of a group. The call must be signed on behalf of a Flickr member, and the ability to see the group membership will be determined by the Flickr member's group privileges.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="membersTypes">Comma separated list of member types     
        /// * 2: member
        ///* 3: moderator
        ///* 4: admin
        ///By default returns all types.</param>
        /// <param name="perPage">Number of members to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>MembersList Object.</returns>
        public static MembersList GetMembersList(this Group group, string membersTypes = null, Nullable<int> perPage = null, Nullable<int> page = null)
        {
            FlickrSynchronousPrmitive<MembersList> FSP = new FlickrSynchronousPrmitive<MembersList>();

            Action<object, EventArgs<MembersList>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            group.GetMembersListCompleted += new EventHandler<EventArgs<MembersList>>(handler);
            FSP.Token = group.GetMembersListAsync(membersTypes,perPage,page);
            FSP.WaitForAsynchronousCall();
            group.GetMembersListCompleted -= new EventHandler<EventArgs<MembersList>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }

        /// <summary>
        /// Search for groups. 18+ groups will only be returned for authenticated calls where the authenticated user is over 18.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="groups">Instance.</param>
        /// <param name="text">The text to search for.</param>
        /// <param name="page">The page of results to return. If this argument is ommited, it defaults to 1. </param>
        /// <param name="perPage">Number of groups to return per page. If this argument is ommited, it defaults to 100. The maximum allowed value is 500.</param>
        /// <returns>GroupCollection Object.</returns>
        public static GroupCollection Search(this Groups groups, string text, Nullable<int> page = null, Nullable<int> perPage = null)
        {
            FlickrSynchronousPrmitive<GroupCollection> FSP = new FlickrSynchronousPrmitive<GroupCollection>();

            Action<object, EventArgs<GroupCollection>> handler = (o, e) => e.Token.IfEqualSetValueandResume(FSP, e);
            groups.SearchCompleted += new EventHandler<EventArgs<GroupCollection>>(handler);
            FSP.Token = groups.SearchAsync(text, page, perPage);
            FSP.WaitForAsynchronousCall();
            groups.SearchCompleted -= new EventHandler<EventArgs<GroupCollection>>(handler);

            return FSP.ResultHolder.ReturnOrThrow();
        }
    }
}
