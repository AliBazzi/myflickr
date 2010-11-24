using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Linq;

namespace MyFlickr.Rest
{
    /// <summary>
    /// represents a collection of Flickr Collections
    /// </summary>
    public class CollectionsList
    {
        private XElement data;
        private IEnumerable<XElement> iEnumerable;

        internal CollectionsList(XElement element)
        {
            this.data = element;
        }

        public CollectionsList(IEnumerable<XElement> iEnumerable)
        {
            this.iEnumerable = iEnumerable;
        }

        /// <summary>
        /// Flickr Collections Objects
        /// </summary>
        public IEnumerable<Collection> Collections
        {
            get
            {
                if (this.data != null)
                    return this.data.Elements("collection").Select(elm => new Collection(elm));
                else
                    return this.iEnumerable.Select(elm => new Collection(elm));
            }
        }
    }

    /// <summary>
    /// represents a Flickr Collection
    /// </summary>
    public class Collection
    {
        private XElement data;

        internal Collection(XElement element)
        {
            this.data = element;
            this.ID = element.Attribute("id").Value;
            this.Title = element.Attribute("title").Value;
            this.Description = element.Attribute("description").Value;
            this.IconLarge = element.Attribute("iconlarge").Value;
            this.IconSmall = element.Attribute("iconsmall").Value;
        }

        /// <summary>
        /// Flickr Collections List
        /// </summary>
        public CollectionsList Collections { get { return new CollectionsList(this.data.Elements("collection")); } }

        /// <summary>
        /// PhotoSets Objects
        /// </summary>
        public IEnumerable<PhotosSetBasic> PhotosSets
        {
            get
            {
                return this.data.Elements("set").Select(set => new PhotosSetBasic(set));
            }
        }

        /// <summary>
        /// the ID of the Collection
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// the Title of the Collection
        /// </summary>
        public string Title { get; private set; }

        /// <summary>
        /// the Description of the collection
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// the URL of the large icon of this Collection , Note : could be relative path for a default when not set by the User
        /// </summary>
        public string IconLarge { get; private set; }

        /// <summary>
        /// the URL of the small icon of this Collection , Note : could be relative path for a default when not set by the User
        /// </summary>
        public string IconSmall { get; private set; }
    }
}