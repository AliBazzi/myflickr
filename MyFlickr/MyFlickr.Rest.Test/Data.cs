using System.Xml.Serialization;

namespace MyFlickr.Rest.Test
{
    [XmlRoot]
    public class Data
    {
        [XmlElement]
        public string apiKey;

        [XmlElement]
        public string sharedSecret;

        [XmlElement]
        public string frob;

    }
}
