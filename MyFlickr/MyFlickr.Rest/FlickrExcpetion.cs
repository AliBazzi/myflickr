using System;
using System.Xml.Linq;

namespace MyFlickr.Rest
{
    public class FlickrException :Exception
    {
        public int Code { get; private set; }
        internal FlickrException(XElement element)
            :base(element.Element("err").Attribute("code").Value)
        {
            this.Code = int.Parse(element.Element("err").Attribute("message").Value);
        }
    }
}
