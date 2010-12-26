using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Method that exist in flickr.blogs namespace
    /// </summary>
    public class Blogs
    {
        private readonly string apiKey;

        /// <summary>
        /// Create Blogs Object
        /// </summary>
        /// <param name="apiKey">The API Key of your Application</param>
        public Blogs(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            this.apiKey = apiKey;
        }

        /// <summary>
        /// Return a list of Flickr supported blogging services.
        /// This method does not require authentication.
        /// </summary>
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised</returns>
        public Token GetServicesAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.IntiateGetRequest(
                elm => this.InvokeGetServicesCompletedEvent(new EventArgs<IEnumerable<BloggingService>>(token,
                    elm.Element("services").Elements("service").Select(srvc => new BloggingService(srvc)))), 
                e => this.InvokeGetServicesCompletedEvent(new EventArgs<IEnumerable<BloggingService>>(token,e)),
                null, new Parameter("api_key", this.apiKey), new Parameter("method", "flickr.blogs.getServices"));

            return token;
        }

        private void InvokeGetServicesCompletedEvent(EventArgs<IEnumerable<BloggingService>> args)
        {
            if (this.GetServicesCompleted != null)
            {
                this.GetServicesCompleted.Invoke(this, args);
            }
        }
        public event EventHandler<EventArgs<IEnumerable<BloggingService>>> GetServicesCompleted;
    }

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

    /// <summary>
    /// represents Blogging Service Info
    /// </summary>
    public class BloggingService
    {
        internal BloggingService(XElement element)
        {
            this.ID = element.Attribute("id").Value;
            this.Name = element.Value;
        }

        /// <summary>
        /// the ID of the Service
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Name of the Service
        /// </summary>
        public string Name { get; private set; }
    }
}