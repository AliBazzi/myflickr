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
                return data.Elements("photoset").Select(elm => new PhotosSet(elm));
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
        internal PhotosSetBasic(XElement elm)
        {
            this.ID = Int64.Parse(elm.Attribute("id").Value);
            this.Title = elm.Attribute("title").Value;
            this.Description = elm.Attribute("description") != null ?  elm.Attribute("description").Value : null;
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
        public PhotosSet(XElement elm)
            :base(elm)
        {
            this.Primary = Int64.Parse(elm.Attribute("primary").Value);
            this.Secret = elm.Attribute("secret").Value;
            this.Server = int.Parse(elm.Attribute("server").Value);
            this.Farm = int.Parse(elm.Attribute("farm").Value);
            this.PhotosCount = int.Parse(elm.Attribute("photos").Value);
            this.VideosCount = int.Parse(elm.Attribute("videos").Value);
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
