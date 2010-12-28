using System;
using System.Xml.Linq;

namespace MyFlickr.Core
{
    /// <summary>
    /// Represents the Exception info that is Returned From Flickr
    /// </summary>
    public class FlickrException :Exception
    {
        /// <summary>
        /// the Error Code
        /// </summary>
        public int Code { get; private set; }

        internal FlickrException(XElement element)
            :base(element.Element("err").Attribute("msg").Value)
        {
            this.Code = int.Parse(element.Element("err").Attribute("code").Value);
        }
    }
}
