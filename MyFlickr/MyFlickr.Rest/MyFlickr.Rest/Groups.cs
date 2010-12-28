using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Method that exist in flickr.groups namespace
    /// </summary>
    public class Groups
    {
        private readonly AuthenticationTokens authtkns;

        /// <summary>
        /// Create Groups Object
        /// </summary>
        /// <param name="authenticationTokens">Authentication Tokens Object</param>
        public Groups(AuthenticationTokens authenticationTokens)
        {
            if (authenticationTokens == null)
                throw new ArgumentNullException("authenticationTokens");
            this.authtkns = authenticationTokens;
        }

        /// <summary>
        /// Search for groups. 18+ groups will only be returned for authenticated calls where the authenticated user is over 18.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="text">The text to search for.</param>
        /// <param name="page">The page of results to return. If this argument is ommited, it defaults to 1. </param>
        /// <param name="perPage">Number of groups to return per page. If this argument is ommited, it defaults to 100. The maximum allowed value is 500.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token SearchAsync(string text,Nullable<int> page = null , Nullable<int> perPage= null )
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeSearchCompletedEvent(new EventArgs<GroupCollection>(token,new GroupCollection(this.authtkns,elm.Element("groups")))), 
                e => this.InvokeSearchCompletedEvent( new EventArgs<GroupCollection>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.groups.search"), new Parameter("text", text), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        private void InvokeSearchCompletedEvent(EventArgs<GroupCollection> args)
        {
            if (this.SearchCompleted != null)
            {
                this.SearchCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when SearchAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GroupCollection>> SearchCompleted;
    }

    /// <summary>
    /// represents a Collection of Groups
    /// </summary>
    public class GroupCollection : IEnumerable<Group>
    {
        private XElement data;

        private readonly AuthenticationTokens authkns;

        internal GroupCollection(AuthenticationTokens authtkns,XElement element)
        {
            this.authkns = authtkns;
            this.data = element;
            this.Total = element.Attribute("total")!=null ? new Nullable<int>(int.Parse(element.Attribute("total").Value)) : null;
            this.Page = element.Attribute("page") != null ? new Nullable<int>(int.Parse(element.Attribute("page").Value)) : null;
            this.Pages = element.Attribute("pages") != null ? new Nullable<int>(int.Parse(element.Attribute("pages").Value)) : null;
            this.PerPage = element.Attribute("perpage") != null ? new Nullable<int>(int.Parse(element.Attribute("perpage").Value)) : null;
        }

        /// <summary>
        /// the Total Number of group , could be Null
        /// </summary>
        public Nullable<int> Total { get; private set; }

        /// <summary>
        /// the Number of groups per page , could be Null
        /// </summary>
        public Nullable<int> PerPage { get; private set; }

        /// <summary>
        /// the number of pages , could be Null
        /// </summary>
        public Nullable<int> Pages { get; private set; }

        /// <summary>
        /// the current page number , could be Null
        /// </summary>
        public Nullable<int> Page { get; private set; }

        /// <summary>
        /// the Groups Objects
        /// </summary>
        public IEnumerable<Group> Groups
        {
            get
            {
                return this.data.Elements("group").Select(elm =>new Group(this.authkns,elm));
            }
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator</returns>
        public IEnumerator<Group> GetEnumerator()
        {
            foreach (var group in this.Groups)
                yield return group;
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a group Pool
    /// </summary>
    public class Pool
    {
        private readonly AuthenticationTokens authtkns;

        internal Pool(AuthenticationTokens authtkns,XElement element)
        {
            this.authtkns = authtkns;
            this.ID = element.Attribute("nsid") != null ? element.Attribute("nsid").Value : element.Attribute("id").Value;
            this.Title = element.Attribute("title")!=null ? element.Attribute("title").Value : element.Attribute("name").Value;
        }

        /// <summary>
        /// the ID of the Group
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Title of the Group
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// Get information about a group.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="language">The language of the group name and description to fetch. If the language is not found, the primary language of the group will be returned. Valid values are the same as in feeds.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetInfoAsync(string language = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeGetInfoCompletedEvent(new EventArgs<GroupInfo>(token, new GroupInfo(elm.Element("group")))),
                e => this.InvokeGetInfoCompletedEvent(new EventArgs<GroupInfo>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.groups.getInfo"), new Parameter("lang", language), new Parameter("group_id", this.ID));

            return token;
        }

        /// <summary>
        /// Add a photo to a group's pool.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The id of the photo to add to the group pool. The photo must belong to the calling user.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token AddAsync(string photoID)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authtkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeAddCompletedEvent(new EventArgs<NoReply>(token,NoReply.Empty)), 
                e => this.InvokeAddCompletedEvent(new EventArgs<NoReply>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.groups.pools.add"), new Parameter("photo_id", photoID), new Parameter("group_id", this.ID));

            return token;
        }

        /// <summary>
        /// Remove a photo from a group pool.
        /// This method requires authentication with 'write' permission.
        /// </summary>
        /// <param name="photoID">The id of the photo to remove from the group pool. The photo must either be owned by the calling user of the calling user must be an administrator of the group.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token RemoveAsync(string photoID)
        {
            if (string.IsNullOrEmpty(photoID))
                throw new ArgumentException("photoID");

            this.authtkns.ValidateGrantedPermission(AccessPermission.Write);
            Token token = Token.GenerateToken();

            FlickrCore.InitiatePostRequest(
                elm => this.InvokeRemoveCompletedEvent(new EventArgs<NoReply>(token, NoReply.Empty)),
                e => this.InvokeRemoveCompletedEvent(new EventArgs<NoReply>(token, e)), this.authtkns.SharedSecret,
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.groups.pools.remove"), new Parameter("photo_id", photoID), new Parameter("group_id", this.ID));

            return token;
        }

        /// <summary>
        /// Returns a list of pool photos for a given group, based on the permissions of the group and the user logged in (if any).
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">The nsid of a user. Specifiying this parameter will retrieve for you only those photos that the user has contributed to the group pool.</param>
        /// <param name="tags">A tag to filter the pool with. At the moment only one tag at a time is supported.</param>
        /// <param name="extras">A comma-delimited list of extra information to fetch for each returned record. Currently supported fields are: description, license, date_upload, date_taken, owner_name, icon_server, original_format, last_update, geo, tags, machine_tags, o_dims, views, media, path_alias, url_sq, url_t, url_s, url_m, url_z, url_l, url_o</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPhotosAsync(string userID = null ,string tags = null ,string extras = null , Nullable<int> perPage= null ,Nullable<int> page = null)
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,new PhotosCollection(this.authtkns,elm.Element("photos")))), 
                e => this.InvokeGetPhotosCompletedEvent(new EventArgs<PhotosCollection>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.groups.pools.getPhotos"), new Parameter("group_id", this.ID), new Parameter("tags", tags), 
                new Parameter("user_id", userID), new Parameter("extras", extras), new Parameter("per_page", perPage), new Parameter("page", page));

            return token;
        }

        /// <summary>
        /// Get a list of the members of a group. The call must be signed on behalf of a Flickr member, and the ability to see the group membership will be determined by the Flickr member's group privileges.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="membersTypes">Comma separated list of member types     
        /// * 2: member
        ///* 3: moderator
        ///* 4: admin
        ///By default returns all types.</param>
        /// <param name="perPage">Number of members to return per page. If this argument is omitted, it defaults to 100. The maximum allowed value is 500.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetMembersListAsync(string membersTypes = null, Nullable<int> perPage = null, Nullable<int> page= null)
        {
            this.authtkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetMembersListCompletedEvent(new EventArgs<MembersList>(token,new MembersList(elm.Element("members")))), 
                e => this.InvokeGetMembersListCompletedEvent(new EventArgs<MembersList>(token,e)), this.authtkns.SharedSecret, 
                new Parameter("api_key", this.authtkns.ApiKey), new Parameter("auth_token", this.authtkns.Token),
                new Parameter("method", "flickr.groups.members.getList"), new Parameter("per_page", perPage), new Parameter("page", page),
                new Parameter("membertypes", membersTypes),new Parameter("group_id",this.ID));

            return token;
        }

        #region Events
        private void InvokeGetMembersListCompletedEvent(EventArgs<MembersList> args)
        {
            if (this.GetMembersListCompleted != null)
            {
                this.GetMembersListCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetMembersListAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<MembersList>> GetMembersListCompleted;
        private void InvokeGetPhotosCompletedEvent(EventArgs<PhotosCollection> args)
        {
            if (this.GetPhotosCompleted != null)
            {
                this.GetPhotosCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetPhotosAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<PhotosCollection>> GetPhotosCompleted;
        private void InvokeRemoveCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.RemoveCompleted != null)
            {
                this.RemoveCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when RemoveAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> RemoveCompleted;
        private void InvokeAddCompletedEvent(EventArgs<NoReply> args)
        {
            if (this.AddCompleted != null)
            {
                this.AddCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised whenAddAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<NoReply>> AddCompleted;
        private void InvokeGetInfoCompletedEvent(EventArgs<GroupInfo> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetInfoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<GroupInfo>> GetInfoCompleted;
        #endregion

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Pool Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Pool left, Pool right)
        {
            if (left is Pool)
                return left.Equals(right);
            else if (right is Pool)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Pool Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Pool left, Pool right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determine whether a Given Object Equals this Object.
        /// </summary>
        /// <param name="obj">Instance</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            return obj is Pool && this.ID == ((Pool)obj).ID;
        }

        /// <summary>
        /// Serve as Hash Function for a Particular Type.
        /// </summary>
        /// <returns>Hashed Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// represent a Flickr Group
    /// </summary>
    public class Group : Pool
    {
        internal Group(AuthenticationTokens authtkns,XElement element)
            :base(authtkns,element)
        {
            this.IsAdmin = element.Attribute("admin")!=null ? new Nullable<bool>(element.Attribute("admin").Value.ToBoolean()): null ;
            this.IsOver18 = element.Attribute("eighteenplus")!= null ? new Nullable<bool>(element.Attribute("eighteenplus").Value.ToBoolean()) : null ;
        }

        /// <summary>
        /// determine whether the calling user is an administrator of the group.
        /// </summary>
        public Nullable<bool> IsAdmin { get; private set; }

        /// <summary>
        /// determine if the group is visible to members over 18 only.
        /// </summary>
        public Nullable<bool> IsOver18 { get; private set; }
    }

    /// <summary>
    /// represents Group Info
    /// </summary>
    public class GroupInfo
    {
        internal GroupInfo(XElement element)
        {
            this.ID = element.Attribute("id").Value;
            this.Description = element.Element("description").Value;
            this.Name = element.Element("name").Value;
            this.MembersCount = int.Parse(element.Element("members").Value);
            this.Privacy = int.Parse(element.Element("privacy").Value);
            this.Server = int.Parse(element.Attribute("iconserver").Value);
            this.Farm = int.Parse(element.Attribute("iconfarm").Value);
            this.Language = element.Attribute("lang").Value;
            this.IsPoolModerated = element.Attribute("ispoolmoderated").Value.ToBoolean();
            this.Throttle = new Throttle(element.Element("throttle"));
            this.Blast = element.Element("blast") != null ? new Blast(element.Element("blast")) : null;
            this.Restrictions = new Restrictions(element.Element("restrictions"));
        }

        /// <summary>
        /// the ID that identifies the group
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the name of the Group
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// description of the group
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// the Server number which the icon of the group is on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// The server Farm number which the icon of the group is on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the Language of the Group , could be empty if not specified
        /// </summary>
        public string Language { get; private set; }

        /// <summary>
        /// determine whether the  the Group pool is moderated by the calling user
        /// </summary>
        public bool IsPoolModerated { get; private set; }

        /// <summary>
        /// the number of members in the group
        /// </summary>
        public int MembersCount { get; private set; }

        /// <summary>
        /// Determine whether the Group is Public , Secretive ...
        /// </summary>
        public int Privacy { get; private set; }

        /// <summary>
        /// holds info about group policy with content addition
        /// </summary>
        public Throttle Throttle { get; private set; }

        /// <summary>
        /// a Blast that is written by admin of the group , Could be Null
        /// </summary>
        public Blast Blast { get; private set; }

        /// <summary>
        /// the Restrictions of this group
        /// </summary>
        public Restrictions Restrictions { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of GroupInfo Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(GroupInfo left, GroupInfo right)
        {
            if (left is GroupInfo)
                return left.Equals(right);
            else if (right is GroupInfo)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of GroupInfo Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(GroupInfo left, GroupInfo right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determine whether a Given Object Equals this Object.
        /// </summary>
        /// <param name="obj">Instance</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            return obj is GroupInfo && this.ID == ((GroupInfo)obj).ID;
        }

        /// <summary>
        /// Serve as Hash Function for a Particular Type.
        /// </summary>
        /// <returns>Hashed Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// represents Restrictions info
    /// </summary>
    public class Restrictions
    {
        internal Restrictions(XElement element)
        {
            this.AcceptPhotos = element.Attribute("photos_ok").Value.ToBoolean();
            this.AcceptVideos = element.Attribute("videos_ok").Value.ToBoolean();
            this.AcceptImages = element.Attribute("images_ok").Value.ToBoolean();
            this.AcceptScreens = element.Attribute("screens_ok").Value.ToBoolean();
            this.AcceptArt = element.Attribute("art_ok").Value.ToBoolean();
            this.AcceptSafe = element.Attribute("safe_ok").Value.ToBoolean();
            this.AcceptModerate = element.Attribute("moderate_ok").Value.ToBoolean();
            this.AcceptRestricted = element.Attribute("restricted_ok").Value.ToBoolean();
            this.HasGeo = element.Attribute("has_geo").Value.ToBoolean();
        }

        /// <summary>
        /// determine whether you can Add Photos or Not
        /// </summary>
        public bool AcceptPhotos { get; private set; }

        /// <summary>
        /// determine whether you can Add Videos or Not
        /// </summary>
        public bool AcceptVideos { get; private set; }

        /// <summary>
        /// determine whether you can Add Images or Not
        /// </summary>
        public bool AcceptImages { get; private set; }

        /// <summary>
        /// determine whether you can Add Screens or Not
        /// </summary>
        public bool AcceptScreens { get; private set; }

        /// <summary>
        /// determine whether you can Add Art or Not
        /// </summary>
        public bool AcceptArt { get; private set; }

        /// <summary>
        /// determine whether you can Add Safe content or Not
        /// </summary>
        public bool AcceptSafe { get; private set; }

        /// <summary>
        /// determine whether you can Add Moderate content or Not
        /// </summary>
        public bool AcceptModerate { get; private set; }

        /// <summary>
        /// determine whether you can Add Restricted content or Not
        /// </summary>
        public bool AcceptRestricted { get; private set; }
        
        /// <summary>
        /// Determine whether Items Must Hold with Geo Tags or Not.
        /// </summary>
        public bool HasGeo { get; private set; }
    }

    /// <summary>
    /// represents Blast info
    /// </summary>
    public class Blast
    {
        internal Blast(XElement element)
        {
            this.Content = element.Value;
            this.UserID = element.Attribute("user_id").Value;
            this.DateAdded = double.Parse(element.Attribute("date_blast_added").Value).ToDateTimeFromUnix();
        }

        /// <summary>
        /// the Content of blast
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// the id of the User that wrote the blast
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// the date when the blast was added
        /// </summary>
        public DateTime DateAdded { get; private set; }
    }

    /// <summary>
    /// represents throttling info
    /// </summary>
    public class Throttle
    {
        internal Throttle(XElement element)
        {
            this.Mode = ThrottleModeExtensions.GetValue(element.Attribute("mode").Value);
            this.count = element.Attribute("count") != null ? new Nullable<int>(int.Parse(element.Attribute("count").Value)) : null;
            this.count = element.Attribute("remaining") != null ? new Nullable<int>(int.Parse(element.Attribute("remaining").Value)) : null;
        }

        /// <summary>
        /// the maximum allowed photos number to be added , Could be Null
        /// </summary>
        public Nullable<int> count { get; private set; }

        /// <summary>
        /// The Mode of throttling
        /// </summary>
        public ThrottleMode Mode { get; private set; }

        /// <summary>
        /// the remaining allowed number of photos to be added , Could be Null
        /// </summary>
        public Nullable<int> Remaining { get; private set; }
    }

    /// <summary>
    /// the Throttling Mode
    /// </summary>
    public enum ThrottleMode
    {
        /// <summary>
        /// Unlimited
        /// </summary>
        None = 0,
        /// <summary>
        /// Per day
        /// </summary>
        Day = 1,
        /// <summary>
        /// Per week
        /// </summary>
        Week = 2,
        /// <summary>
        /// Per month
        /// </summary>
        Month = 3,
        /// <summary>
        /// Addition is Disabled
        /// </summary>
        Disabled=4,
        /// <summary>
        /// the limit is reached
        /// </summary>
        Ever=5
    }

    internal static class ThrottleModeExtensions
    {
        public static ThrottleMode GetValue(string value)
        {
            switch (value)
            {
                case "none":
                    return ThrottleMode.None;
                case "day":
                    return ThrottleMode.Day;
                case "week":
                    return ThrottleMode.Week;
                case "month":
                    return ThrottleMode.Month;
                case "disabled":
                    return ThrottleMode.Disabled;
                case "ever":
                    return ThrottleMode.Ever;
                default:
                    throw new ArgumentException("value");
            }
        }
    }

    /// <summary>
    /// represents a list of members in a group
    /// </summary>
    public class MembersList : IEnumerable<Member>
    {
        private XElement data;

        internal MembersList(XElement element)
        {
            this.data = element;
            this.PerPage = int.Parse(element.Attribute("perpage").Value);
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
        }

        /// <summary>
        /// the Total Number of members
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Number of members per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the number of pages 
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the current page number
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// Enumerable of members
        /// </summary>
        public IEnumerable<Member> Members { get { return this.data.Elements("member").Select(elm => new Member(elm)); } }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator</returns>
        public IEnumerator<Member> GetEnumerator()
        {
            foreach (var member in this.Members)
                yield return member;
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Represent Member info in a Group
    /// </summary>
    public class Member
    {
        internal Member(XElement element)
        {
            this.ID = element.Attribute("nsid").Value;
            this.UserName = element.Attribute("username").Value;
            this.Server = int.Parse(element.Attribute("iconserver").Value);
            this.Farm = int.Parse(element.Attribute("iconfarm").Value);
            this.MemberType = MemberTypeExtensions.GetValue(int.Parse(element.Attribute("membertype").Value));
        }

        /// <summary>
        /// the ID of the member
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the name of the Member
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// the server number that the icon of the user is on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// the server farm number that the icon of the user is on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the type of the member
        /// </summary>
        public MemberType MemberType { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Member Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Member left, Member right)
        {
            if (left is Member)
                return left.Equals(right);
            else if (right is Member)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Member Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Member left, Member right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determine whether a Given Object Equals this Object.
        /// </summary>
        /// <param name="obj">Instance</param>
        /// <returns>True or False</returns>
        public override bool Equals(object obj)
        {
            return obj is Member && this.ID == ((Member)obj).ID;
        }

        /// <summary>
        /// Serve as Hash Function for a Particular Type.
        /// </summary>
        /// <returns>Hashed Value</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

    /// <summary>
    /// represents member type options
    /// </summary>
    public enum MemberType
    {
        /// <summary>
        ///  usual member
        /// </summary>
        Member = 2,
        /// <summary>
        /// a moderator
        /// </summary>
        Moderator = 3,
        /// <summary>
        /// an Admin
        /// </summary>
        Admin = 4
    }

    internal static class MemberTypeExtensions
    {
        public static MemberType GetValue(int value)
        {
            switch (value)
            {
                case 2:
                    return MemberType.Member;
                case 3:
                    return MemberType.Moderator;
                case 4:
                    return MemberType.Admin;
                default:
                    throw new ArgumentException("value");
            }
        }
    }
}