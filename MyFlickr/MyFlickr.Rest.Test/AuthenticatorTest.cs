using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFlickr.Rest.Synchronous;
using MyFlickr.Core;

namespace MyFlickr.Rest.Test
{
    [TestClass]
    public class AuthenticatorTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        void SaveData()
        {
            new XmlSerialization.Serialization<Data>().Serialize(data, "C:\\data.xml");
        }

        [TestMethod]
        public void GetFrobTest()
        {
            var res = new Authenticator(data.apiKey, data.sharedSecret).GetFrob(AccessPermission.Read);
        }
        [TestMethod,ExpectedException(typeof(FlickrException))]
        public void GetFullTokenTest()
        {
            var res = new Authenticator(data.apiKey, data.sharedSecret).GetFullToken("fake");
        }
        [TestMethod,ExpectedException(typeof(FlickrException))]
        public void GetTokenTest()
        {
            var res = new Authenticator(data.apiKey, data.sharedSecret).GetToken("fake");
        }
        [TestMethod, ExpectedException(typeof(FlickrException))]
        public void CheckTokenTest()
        {
            var res = new Authenticator(data.apiKey, data.sharedSecret).CheckToken("fake");
        }
    }
}
