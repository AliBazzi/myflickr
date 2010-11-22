using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of photosets
    /// </summary>
    public class PhotoSetsCollection : IEnumerable<PhotosSet>
    {
        private XElement data;

        internal PhotoSetsCollection(XElement element)
        {
            this.data = element;
            this.PhotosSetsCount = element.Elements("photoset").Count();
        }

        /// <summary>
        /// the number of photosets in this collection
        /// </summary>
        public int PhotosSetsCount { get; private set; }

        /// <summary>
        /// the Photosets Objects
        /// </summary>
        public IEnumerable<PhotosSet> PhotoSets
        {
            get
            {
                return data.Elements("photoset")
                    .Select(elm => new PhotosSet(Int64.Parse(elm.Attribute("id").Value), Int64.Parse(elm.Attribute("primary").Value),
                    elm.Attribute("secret").Value, int.Parse(elm.Attribute("server").Value), int.Parse(elm.Attribute("farm").Value),
                    int.Parse(elm.Attribute("photos").Value), int.Parse(elm.Attribute("videos").Value), elm.Element("title").Value,
                    elm.Element("description").Value));
            }
        }

        public IEnumerator<PhotosSet> GetEnumerator()
        {
            foreach (var photoset in this.PhotoSets)
                yield return photoset;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    /// <summary>
    /// represents photosSet Basic Information
    /// </summary>
    public class PhotosSetBasic
    {
        internal PhotosSetBasic(Int64 id, string title, string description)
        {
            this.ID = id;
            this.Title = title;
            this.Description = description;
        }

        /// <summary>
        /// the ID that identifies the photoset
        /// </summary>
        public Int64 ID { get; private set; }

        /// <summary>
        /// the title of the photoset
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the description of the photoset , could be Null
        /// </summary>
        public string Description { get; private set; }
    }

    /// <summary>
    /// represents a user PhotosSet
    /// </summary>
    public class PhotosSet : PhotosSetBasic
    {
        internal PhotosSet(Int64 id, Int64 primary, string secret, int server, int farm, int photos, int videos, string title, string description)
            : base(id, title, description)
        {
            this.Primary = primary;
            this.Secret = secret;
            this.Server = server;
            this.Farm = farm;
            this.PhotosCount = photos;
            this.VideosCount = videos;
        }

        /// <summary>
        /// the ID of default photo in this photoset
        /// </summary>
        public Int64 Primary { get; private set; }

        /// <summary>
        /// the secret used to build the URL of the default photo in this photoset
        /// </summary>
        public string Secret { get; private set; }

        /// <summary>
        /// the number of server the default photo in this set resides on
        /// </summary>
        public int Server { get; private set; }

        /// <summary>
        /// the number of servers farm the default photo in this set resides on
        /// </summary>
        public int Farm { get; private set; }

        /// <summary>
        /// the number of photos contained in this photoset
        /// </summary>
        public int PhotosCount { get; private set; }

        /// <summary>
        /// the number of videos contained in this photoset
        /// </summary>
        public int VideosCount { get; private set; }
    }
}
