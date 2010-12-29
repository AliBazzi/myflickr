using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MyFlickr.Core;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents the Method that exist in flickr.blogs namespace.
    /// </summary>
    public class Blogs
    {
        private readonly string apiKey;

        /// <summary>
        /// Create Blogs Object.
        /// </summary>
        /// <param name="apiKey">The API Key of your Application.</param>
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
        /// <returns>Token that represents unique identifier that identifies your Call when the corresponding Event is raised.</returns>
        public Token GetServicesAsync()
        {
            Token token = Token.GenerateToken();

            FlickrCore.InitiateGetRequest(
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
        /// <summary>
        /// Raised when GetServicesAsync call is Finished.
        /// </summary>
        public event EventHandler<EventArgs<IEnumerable<BloggingService>>> GetServicesCompleted;
    }

    /// <summary>
    /// represents a Collection of Blogs.
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
        /// the number of blogs in this collection.
        /// </summary>
        public int BlogsCount { get; private set; }

        /// <summary>
        /// Blogs Objects.
        /// </summary>
        public IEnumerable<Blog> Blogs
        {
            get
            {
                return this.data.Elements("blog").Select(elm => new Blog(elm));
            }
        }

        /// <summary>
        /// Returns Enumerator for the Current Instance.
        /// </summary>
        /// <returns>an Enumerator.</returns>
        public IEnumerator<Blog> GetEnumerator()
        {
            foreach (var blog in this.Blogs)
                yield return blog;
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
    /// Represents a blog.
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
        /// the ID of the Blog.
        /// </summary>
        public Int64 ID { get; private set; }

        /// <summary>
        /// the URL that leads to the blog Service.
        /// </summary>
        public Uri URL { get; private set; }

        /// <summary>
        /// determine whether the blog needs password.
        /// </summary>
        public bool NeedsPassword { get; private set; }

        /// <summary>
        /// the name of the Blog Service.
        /// </summary>
        public string Name { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of Blog Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(Blog left, Blog right)
        {
            if (left is Blog)
                return left.Equals(right);
            else if (right is Blog)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of Blog Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(Blog left, Blog right)
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
            return obj is Blog && this.ID == ((Blog)obj).ID;
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
    /// represents Blogging Service Info.
    /// </summary>
    public class BloggingService
    {
        internal BloggingService(XElement element)
        {
            this.ID = element.Attribute("id").Value;
            this.Name = element.Value;
        }

        /// <summary>
        /// the ID of the Service.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Name of the Service.
        /// </summary>
        public string Name { get; private set; }

        #region Equality
        /// <summary>
        /// Determine whether Two Instances of BloggingService Are Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator ==(BloggingService left, BloggingService right)
        {
            if (left is BloggingService)
                return left.Equals(right);
            else if (right is BloggingService)
                return right.Equals(left);
            return true;
        }

        /// <summary>
        /// Determine whether Two Instances of BloggingService Are Not Equal or Not.
        /// </summary>
        /// <param name="left">instance</param>
        /// <param name="right">instance</param>
        /// <returns>True or False</returns>
        public static bool operator !=(BloggingService left, BloggingService right)
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
            return obj is BloggingService && this.ID == ((BloggingService)obj).ID;
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