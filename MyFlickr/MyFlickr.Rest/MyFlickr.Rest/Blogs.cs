using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

/// <summary>
/// represents a Collection of Blogs
/// </summary>
public class BlogsCollection : IEnumerable<Blog>
{
    private XElement data;

    internal BlogsCollection(XElement element)
    {
        this.data = element;
        this.BlogsCount = element.Elements("blog").Count();
    }

    /// <summary>
    /// the number of blogs in this collection
    /// </summary>
    public int BlogsCount { get; private set; }

    /// <summary>
    /// Blogs Objects
    /// </summary>
    public IEnumerable<Blog> Blogs
    {
        get
        {
            return this.data.Elements("blog").Select(elm => new Blog(elm));
        }
    }

    public IEnumerator<Blog> GetEnumerator()
    {
        foreach (var blog in this.Blogs)
            yield return blog;
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}

/// <summary>
/// Represents a blog
/// </summary>
public class Blog
{
    internal Blog(XElement element)
    {
        this.ID = Int64.Parse(element.Attribute("id").Value);
        this.URL = new Uri(element.Attribute("url").Value);
        this.Name = element.Attribute("name").Value;
        this.NeedsPassword = element.Attribute("needspassword").Value.ToBoolean();
    }

    /// <summary>
    /// the ID of the Blog
    /// </summary>
    public Int64 ID { get; private set; }

    /// <summary>
    /// the URL that leads to the blog Service
    /// </summary>
    public Uri URL { get; private set; }

    /// <summary>
    /// determine whether the blog needs password
    /// </summary>
    public bool NeedsPassword { get; private set; }

    /// <summary>
    /// the name of the Blog Service
    /// </summary>
    public string Name { get; private set; }
}