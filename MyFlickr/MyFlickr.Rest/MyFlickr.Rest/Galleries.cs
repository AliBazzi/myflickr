using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of galleries
    /// </summary>
    public class GalleriesCollection : IEnumerable<Gallery>
    {
        private XElement data;

        internal GalleriesCollection(XElement element)
        {
            this.data = element;
        }

        /// <summary>
        /// the Total Number of Galleries
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// the Number of Galleries per page
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
        /// the Galleries Objects
        /// </summary>
        public IEnumerable<Gallery> Galleries
        {
            get
            {
                return data.Elements("gallery").Select(
                    elm => new Gallery(elm.Attribute("id").Value, elm.Attribute("url").Value, elm.Attribute("owner").Value,
                        Int64.Parse(elm.Attribute("primary_photo_id").Value), int.Parse(elm.Attribute("count_photos").Value),
                        int.Parse(elm.Attribute("count_videos").Value), int.Parse(elm.Attribute("primary_photo_farm").Value),
                    int.Parse(elm.Attribute("primary_photo_server").Value), elm.Attribute("primary_photo_secret").Value, elm.Attribute("date_create").Value,
                    elm.Attribute("date_update").Value, elm.Element("title").Value, elm.Element("description").Value));
            }
        }

        public IEnumerator<Gallery> GetEnumerator()
        {
            foreach (var gallery in this.Galleries)
                yield return gallery;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents a gallery
    /// </summary>
    public class Gallery
    {
        internal Gallery(string id, string url, string ownerID, Int64 primary, int photosCount, int videosCount, int farm, int server, string secret,
            string dateCreated, string dateUpdated, string title, string description)
        {
            this.ID = id;
            this.Url = new Uri(url);
            this.OwnerID = ownerID;
            this.Primary = primary;
            this.PhotosCount = photosCount;
            this.VideosCount = videosCount;
            this.Farm = farm;
            this.Server = server;
            this.Secret = secret;
            this.DateCreated = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(dateCreated));
            this.DateUpdated = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(double.Parse(dateCreated));
            this.Title = title;
            this.Description = description;
        }

        /// <summary>
        /// the ID of the Gallery
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the URL that leads to the page of the gallery on Flickr
        /// </summary>
        public Uri Url { get; private set; }

        /// <summary>
        /// The Owner ID of the Gallery 
        /// </summary>
        public string OwnerID { get; private set; }

        /// <summary>
        /// the ID of the primary photo of the gallery
        /// </summary>
        public Int64 Primary { get; private set; }

        /// <summary>
        /// the number of photos in this gallery
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of videos in this gallery
        /// </summary>
        public int VideosCount { get; private set; }

        /// <summary>
        /// the number of server farm that the primary photo of this gallery resides on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the number of server that the primary photo of this gallery resides on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// the secret used to build the URL of primary photo of this gallery
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the Date and time of the creation of this gallery
        /// </summary>
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// the Date and time of the last modification of this gallery
        /// </summary>
        public DateTime DateUpdated { get; private set; }

        /// <summary>
        /// the Title of this gallery
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Description of this gallery , could Be Null
        /// </summary>
        public string Description { get; private set; }
    }
}