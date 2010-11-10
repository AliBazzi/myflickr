using System;
using System.Xml.Linq;

namespace MyFlickr.Core
{
    public class FlickrException :Exception
    {
        public int Code { get; private set; }
        internal FlickrException(XElement element)
            :base(element.Element("err").Attribute("msg").Value)
        {
            this.Code = int.Parse(element.Element("err").Attribute("code").Value);
        }
    }
}
