using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class PandaTest
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
            Panda panda = new Panda(new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token));
            var names = panda.GetList();
            foreach (var name in names)
            {

            }
        }

        [TestMethod]
        public void GetPhotosTest()
        {
            Panda panda = new Panda(new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token));
            var names = panda.GetList();
            var photos = panda.GetPhotos(names.First());
            foreach (var photo in photos)
            {

            }
        }
    }
}
