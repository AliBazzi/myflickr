using System.Xml.Serialization;

namespace MyFlickr.Rest.Tests
{
    [XmlRoot]
    public class Data
    {
        [XmlElement]
        public string apiKey;

        [XmlElement]
        public string sharedSecret;

        [XmlElement]
        public string token;

    }
}
