using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Linq;

/// <summary>
/// represents a list of contact
/// </summary>
public class ContactsList : IEnumerable<Contact>
{
    private XElement data;

    private bool isPublicList;

    internal ContactsList(XElement element, bool isPublicList)
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
    /// the contacts objects
    /// </summary>
    public IEnumerable<Contact> Contacts
    {
        get
        {
            return data.Elements("contact").Select(elm => this.isPublicList
               ?
               new Contact(elm.Attribute("nsid").Value,
               elm.Attribute("username").Value, int.Parse(elm.Attribute("iconserver").Value), elm.Attribute("ignored").Value.ToBoolean())
               :
               new Contact(elm.Attribute("nsid").Value, elm.Attribute("username").Value, int.Parse(elm.Attribute("iconserver").Value)
               , elm.Attribute("ignored").Value.ToBoolean(), elm.Attribute("realname").Value, elm.Attribute("friend").Value.ToBoolean()
               , elm.Attribute("family").Value.ToBoolean()));
        }
    }

    public IEnumerator<Contact> GetEnumerator()
    {
        foreach (var contact in this.Contacts)
            yield return contact;
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

    internal Contact(string userID, string userName, int iconServer, bool isIgnored)
    {
        this.UserID = userID;
        this.UserName = userName;
        this.IconServer = iconServer;
        this.IsIgnored = isIgnored;
    }

    internal Contact(string userID, string userName, int iconServer, bool isIgnored, string realName, bool isFriend, bool isFamily)
        : this(userID, userName, iconServer, isIgnored)
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
    Family = 1,
    /// <summary>
    /// the contact is both friend and family
    /// </summary>
    Both = 2,
    /// <summary>
    /// the contact is neither family nor friend
    /// </summary>
    Niether = 3
}