using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a list of contact.
    /// </summary>
    public class ContactsList : IEnumerable<Contact>
    {
        private XElement data;

        internal ContactsList(XElement element)
        {
            this.data = element;
            this.Page = int.Parse(element.Attribute("page").Value);
            this.Pages = int.Parse(element.Attribute("pages").Value);
            this.PerPage = int.Parse(element.Attribute("per_page").Value);
            this.Total = int.Parse(element.Attribute("total").Value);
        }

        /// <summary>
        /// the page number.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// the available pages.
        /// </summary>
        public int Pages { get; private set; }

        /// <summary>
        /// the number of contacts per page.
        /// </summary>
        public int PerPage { get; private set; }

        /// <summary>
        /// the total number of contacts.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the contacts objects.
        /// </summary>
        public IEnumerable<Contact> Contacts
        {
            get
            {
                return data.Elements("contact").Select(elm => new Contact(elm));
            }
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Contact> GetEnumerator()
        {
            foreach (var contact in this.Contacts)
                yield return contact;
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
    /// Represents the Contact information.
    /// </summary>
    public class Contact
    {
        internal Contact(XElement elm)
        {
            this.UserID = elm.Attribute("nsid").Value;
            this.UserName = elm.Attribute("username").Value;
            this.IconServer = int.Parse(elm.Attribute("iconserver").Value);
            this.IsIgnored = elm.Attribute("ignored") != null ? new Nullable<bool>(elm.Attribute("ignored").Value.ToBoolean()) : null;
            this.RealName = elm.Attribute("realname") != null ? elm.Attribute("realname").Value : null;
            this.IsFriend = elm.Attribute("friend") != null ? new Nullable<bool>(elm.Attribute("friend").Value.ToBoolean()) : null;
            this.IsFamily = elm.Attribute("family") != null ? new Nullable<bool>(elm.Attribute("family").Value.ToBoolean()) : null;
            this.PathAlias = elm.Attribute("path_alias") != null ? elm.Attribute("path_alias").Value : null;
            this.PhotosUploaded = elm.Attribute("photos_uploaded") != null ? new Nullable<int>(int.Parse(elm.Attribute("photos_uploaded").Value)) : null;
        }

        /// <summary>
        /// the User ID of the Contact.
        /// </summary>
        public string UserID { get; private set; }

        /// <summary>
        /// the real Name of the Contact , Could be Null.
        /// </summary>
        public string RealName { get; private set; }

        /// <summary>
        /// the Name of the Contact.
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// is the contact marked as friend , could be Null.
        /// </summary>
        public Nullable<bool> IsFriend { get; private set; }

        /// <summary>
        /// is the contact marked as family , could be Null.
        /// </summary>
        public Nullable<bool> IsFamily { get; private set; }

        /// <summary>
        /// the number of icon server.
        /// </summary>
        public int IconServer { get; private set; }

        /// <summary>
        /// the contact is ignored , Could Be Null.
        /// </summary>
        public Nullable<bool> IsIgnored { get; private set; }

        /// <summary>
        ///the Path Alias (used when Generating Urls ) , Could be Empty when not set by the user.
        /// </summary>
        public string PathAlias { get; private set; }

        /// <summary>
        /// the Number of photos Uploaded recently by the Contact , Could Be null.
        /// </summary>
        public Nullable<int> PhotosUploaded { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Contact Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Contact left, Contact right)
        {
            if (left is Contact)
                return left.Equals(right);
            else if (right is Contact)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Contact Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Contact left, Contact right)
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
            return obj is Contact && this.UserID == ((Contact)obj).UserID;
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
    /// The Filters that could be Applied on Contacts List when getting them.
    /// </summary>
    public enum ContactFilter
    {
        /// <summary>
        /// the contact is Friend.
        /// </summary>
        Friends = 0,
        /// <summary>
        /// the contact is Family.
        /// </summary>
        Family = 1,
        /// <summary>
        /// the contact is both friend and family.
        /// </summary>
        Both = 2,
        /// <summary>
        /// the contact is neither family nor friend.
        /// </summary>
        Niether = 3
    }
}