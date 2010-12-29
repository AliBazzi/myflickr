using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of Flickr Collections.
    /// </summary>
    public class CollectionsList : IEnumerable<Collection>
    {
        private XElement data;
        private IEnumerable<XElement> iEnumerable;
        private AuthenticationTokens authTkns;

        internal CollectionsList(AuthenticationTokens authTkns,XElement element)
        {
            this.authTkns = authTkns;
            this.data = element;
        }

        internal CollectionsList(AuthenticationTokens authTkns,IEnumerable<XElement> iEnumerable)
        {
            this.authTkns = authTkns;
            this.iEnumerable = iEnumerable;
        }

        /// <summary>
        /// Flickr Collections Objects.
        /// </summary>
        public IEnumerable<Collection> Collections
        {
            get
            {
                if (this.data != null)
                    return this.data.Elements("collection").Select(elm => new Collection(this.authTkns,elm));
                else
                    return this.iEnumerable.Select(elm => new Collection(this.authTkns,elm));
            }
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Collection> GetEnumerator()
        {
            foreach (var item in this.Collections)
                yield return item;
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a Flickr Collection.
    /// </summary>
    public class Collection
    {
        private XElement data;
        private AuthenticationTokens authTkns;
        internal Collection(AuthenticationTokens authTkns,XElement element)
        {
            this.authTkns = authTkns;
            this.data = element;
            this.ID = element.Attribute("id").Value;
            this.Title = element.Attribute("title").Value;
            this.Description = element.Attribute("description").Value;
            this.IconLarge = element.Attribute("iconlarge").Value;
            this.IconSmall = element.Attribute("iconsmall").Value;
        }

        /// <summary>
        /// Flickr Collections List.
        /// </summary>
        public CollectionsList Collections { get { return new CollectionsList(this.authTkns,this.data.Elements("collection")); } }

        /// <summary>
        /// PhotoSets Objects.
        /// </summary>
        public IEnumerable<PhotoSetBasic> PhotosSets
        {
            get
            {
                return this.data.Elements("set").Select(set => new PhotoSetBasic(this.authTkns,set));
            }
        }

        /// <summary>
        /// the ID of the Collection.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Title of the Collection.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Description of the collection.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// the URL of the large icon of this Collection , Note : could be relative path for a default when not set by the User.
        /// </summary>
        public string IconLarge { get; private set; }

        /// <summary>
        /// the URL of the small icon of this Collection , Note : could be relative path for a default when not set by the User.
        /// </summary>
        public string IconSmall { get; private set; }

        /// <summary>
        /// Returns information for a single collection. Currently can only be called by the collection owner, this may change.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetInfoAsync()
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
                elm => this.InvokeGetInfoCompletedEvent(new EventArgs<CollectionInfo>(token,new CollectionInfo(this.authTkns,elm.Element("collection")))), 
                e => this.InvokeGetInfoCompletedEvent(new EventArgs<CollectionInfo>(token,e)), this.authTkns.SharedSecret,
                new Parameter("api_key", this.authTkns.ApiKey), new Parameter("method", "flickr.collections.getInfo"), 
                new Parameter("auth_token", this.authTkns.Token), new Parameter("collection_id", this.ID));

            return token;
        }

        private void InvokeGetInfoCompletedEvent(EventArgs<CollectionInfo> args)
        {
            if (this.GetInfoCompleted != null)
            {
                this.GetInfoCompleted.Invoke(this, args);
            }
        }
        /// <summary>
        /// Raised when GetInfoAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<CollectionInfo>> GetInfoCompleted;

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Collection Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Collection left, Collection right)
        {
            if (left is Collection)
                return left.Equals(right);
            else if (right is Collection)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Collection Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Collection left, Collection right)
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
            return obj is Collection && this.ID == ((Collection)obj).ID;
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
    /// represents Collection Info
    /// </summary>
    public class CollectionInfo
    {
        internal CollectionInfo(AuthenticationTokens authtkns,XElement element)
        {
            this.ID = element.Attribute("id").Value;
            this.ChildCount = int.Parse(element.Attribute("child_count").Value);
            this.DateCreated = double.Parse(element.Attribute("datecreate").Value).ToDateTimeFromUnix();
            this.IconLarge = element.Attribute("iconlarge").Value;
            this.IconSmall = element.Attribute("iconsmall").Value;
            this.Server = element.Attribute("server")!= null ? new Nullable<int>(int.Parse(element.Attribute("server").Value)): null;
            this.Secret = element.Attribute("secret") != null ? element.Attribute("secret").Value : null;
            this.Title = element.Element("title").Value;
            this.Description = element.Element("description").Value;
            this.IconPhotos = element.Element("iconphotos")!= null ? element.Element("iconphotos").Elements("photo").Select(ph => new Photo(authtkns, ph)) : null;
        }

        /// <summary>
        /// the ID of the Collection.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Title of the Collection.
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Description of the Collection.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// the Number of Childs of this collection.
        /// </summary>
        public int ChildCount { get; private set; }

        /// <summary>
        /// the Date where this Collection was Created.
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// the Url of the Icon that represents this Collection (large) , Could Be Partial Url.
        /// </summary>
        public string IconLarge { get; private set; }

        /// <summary>
        /// the Url of the Icon that represents this Collection (small) , Could Be Partial Url.
        /// </summary>
        public string IconSmall { get; private set; }

        /// <summary>
        /// the Server Number , Could Be Null.
        /// </summary>
        public Nullable<int> Server { get; private set; }

        /// <summary>
        /// Secret of the Collection , Could Be Null.
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// Enumerable of Photos that Represents the Icon Photos of the Collection , Could Be Null.
        /// </summary>
        public IEnumerable<Photo> IconPhotos { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of CollectionInfo Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(CollectionInfo left, CollectionInfo right)
        {
            if (left is CollectionInfo)
                return left.Equals(right);
            else if (right is CollectionInfo)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of CollectionInfo Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(CollectionInfo left, CollectionInfo right)
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
            return obj is CollectionInfo && this.ID == ((CollectionInfo)obj).ID;
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
}