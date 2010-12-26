using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class InterestingnessTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetListTest()
        {
            Interestingness interest = new Interestingness(new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token));
            var photos = interest.GetList();
            foreach (var item in photos)
            {

            }
        }
    }
}
