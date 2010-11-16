using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyFlickr.Core;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a Flickr User
    /// </summary>
    public class User
    {
        private AuthenticationTokens authTkns { get; set; }

        /// <summary>
        /// Flickr API application key
        /// </summary>
        public string ApiKey { get ;private set; }

        /// <summary>
        /// A shared secret for the api key that is issued by flickr
        /// </summary>
        public string SharedSecret { get ; private set; }

        /// <summary>
        /// The User ID
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// The Access Permission
        /// </summary>
        public AccessPermission AccessPermission { get { return this.authTkns.AccessPermission; } }

        /// <summary>
        /// the Full Name of the User
        /// </summary>
        public string FullName { get { return this.authTkns.FullName; } }

        /// <summary>
        /// the UserName
        /// </summary>
        public string UserName { get { return this.authTkns.UserName; } }

        /// <summary>
        /// the Authentication Token
        /// </summary>
        public string Token { get { return this.authTkns.Token ; } }

        internal User(string apiKey , string sharedSecret , AuthenticationTokens tokens)
        {
            this.authTkns = tokens;
            this.ApiKey = apiKey;
            this.SharedSecret = sharedSecret;
            this.UserID = tokens.UserID;
        }

        /// <summary>
        /// Creates an object that represents a Flickr User
        /// </summary>
        /// <param name="apiKey">Flickr API application key</param>
        /// <param name="userID">The User ID</param>
        public User(string apiKey, string userID)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (string.IsNullOrEmpty(userID))
                throw new ArgumentException("userID");
            this.ApiKey = apiKey;
            this.UserID = userID;
        }

        /// <summary>
        /// Get a list of contacts for the calling user.
        /// This method requires authentication with 'read' permission.
        /// </summary>
        /// <param name="contactFilter">An optional filter of the results. The following values are valid:friends,family,both,neither</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetContactsListAsync(ContactFilter contactFilter = ContactFilter.Both,int page =1 , int perPage = 1000)
        {
            this.authTkns.ValidateGrantedPermission(AccessPermission.Read);
            Token token = MyFlickr.Core.Token.GenerateToken();

            FlickrCore.IntiateRequest(element => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token,new ContactsList(element.Element("contacts"),false))) 
            , e => this.InvokeGetContactsListCompletedEvent(new EventArgs<ContactsList>(token, e))
            , this.SharedSecret, new Parameter("method", "flickr.contacts.getList") , new Parameter("api_key", this.ApiKey)
            , new Parameter("filter", contactFilter.ToString().ToLower()) , new Parameter("page", page.ToString()) , new Parameter("per_page",perPage.ToString())
            , new Parameter("auth_token",this.authTkns.Token));

            return token;
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="userID">The NSID of the user to fetch the contact list for.</param>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicContactsListAsync(string userID, int page = 1, int perPage = 1000)
        {
            if (string.IsNullOrEmpty(userID))
                throw new ArgumentException("userID");

            Token token = Core.Token.GenerateToken();

            FlickrCore.IntiateRequest(element => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, new ContactsList(element.Element("contacts"), true)))
            , e => this.InvokeGetPublicContactsListCompletedEvent(new EventArgs<ContactsList>(token, e))
            , this.SharedSecret, new Parameter("method", "flickr.contacts.getPublicList"),new Parameter("user_id",this.UserID),
            new Parameter("api_key", this.ApiKey) , new Parameter("page", page.ToString()), new Parameter("per_page", perPage.ToString()));

            return token;
        }

        /// <summary>
        /// Get the contact list for a user.
        /// This method does not require authentication.
        /// </summary>
        /// <param name="page">The page of results to return. If this argument is omitted, it defaults to 1.</param>
        /// <param name="perPage">Number of photos to return per page. If this argument is omitted, it defaults to 1000. The maximum allowed value is 1000.</param>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetPublicContactsListAsync(int page = 1, int perPage = 1000)
        {
            return this.GetPublicContactsListAsync(this.UserID, page, perPage);
        }

        #region Events
        private void InvokeGetPublicContactsListCompletedEvent(EventArgs<ContactsList> args)
        {
            if (this.GetPublicContactsListCompleted != null)
            {
                this.GetPublicContactsListCompleted(this, args);
            }
        }
        public event EventHandler<EventArgs<ContactsList>> GetPublicContactsListCompleted;
        private void InvokeGetContactsListCompletedEvent(EventArgs<ContactsList> args)
        {
            if (this.GetContactsListCompleted != null)
            {
                this.GetContactsListCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<ContactsList>> GetContactsListCompleted;
        #endregion
    }

    /// <summary>
    /// represents a list of contact
    /// </summary>
    public class ContactsList : IEnumerable<Contact>
    {
        private XElement data { get; set; }

        private bool isPublicList;

        internal ContactsList(XElement element,bool isPublicList)
        {
            this.isPublicList = isPublicList;
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.PerPage = int.Parse(element.Attribute("per_page").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
        }

        /// <summary>
        /// the page number
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the available pages
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of contacts per page
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of contacts
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Contacts objects
        /// </summary>
        public IEnumerable<Contact> Contacts 
        {
            get 
            {
                return data.Elements("contact").Select(elm => this.isPublicList 
                    ? 
                    new Contact(elm.Attribute("nsid").Value, 
                    elm.Attribute("username").Value,int.Parse(elm.Attribute("iconserver").Value),bool.Parse(elm.Attribute("ignored").Value))
                    : 
                    new Contact(elm.Attribute("nsid").Value, elm.Attribute("username").Value,int.Parse(elm.Attribute("iconserver").Value)
                    ,bool.Parse(elm.Attribute("ignored").Value)
                    ,elm.Attribute("realname").Value,bool.Parse(elm.Attribute("friend").Value),bool.Parse(elm.Attribute("family").Value)));
            }
        }

        public IEnumerator<Contact> GetEnumerator()
        {
            foreach (var item in this.Contacts)
                yield return item;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// Represents the Contact information
    /// </summary>
    public class Contact
    {
        /// <summary>
        /// the User ID of the Contact
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// the real Name of the Contact , Could be Null
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// the Name of the Contact
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// is the contact marked as friend , could be Null
        /// </summary>
        public Nullable<bool> IsFriend { get; private set; }

        /// <summary>
        /// is the contact marked as family , could be Null
        /// </summary>
        public Nullable<bool> IsFamily { get; private set; }

        /// <summary>
        /// the number of icon server
        /// </summary>
        public int IconServer { get; private set; }

        /// <summary>
        /// the contact is ignored
        /// </summary>
        public bool IsIgnored { get; private set; }

        internal Contact(string userID , string userName , int iconServer ,bool isIgnored)
        {
            this.UserID = userID;
            this.UserName = userName;
            this.IconServer = iconServer;
            this.IsIgnored = isIgnored;
        }

        internal Contact(string userID, string userName, int iconServer, bool isIgnored, string realName, bool isFriend, bool isFamily)
            :this(userID,userName,iconServer,isIgnored)
        {
            this.RealName = realName;
            this.IsFamily = isFamily;
            this.IsFriend = isFriend;
        }
    }

    /// <summary>
    /// The Filters that could be Applied on Contacts List when getting them
    /// </summary>
    public enum ContactFilter
	{
        /// <summary>
        /// the contact is Friend
        /// </summary>
        Friends = 0,
        /// <summary>
        /// the contact is Family
        /// </summary>
        Family =1, 
        /// <summary>
        /// the contact is both friend and family
        /// </summary>
        Both =2,
        /// <summary>
        /// the contact is neither family nor friend
        /// </summary>
        Niether =3
	}
}
