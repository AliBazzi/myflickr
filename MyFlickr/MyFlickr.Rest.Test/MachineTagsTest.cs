using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class MachineTagsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetPredicatesTest()
        {
            MachineTags mt = new MachineTags(this.data.apiKey);
            var predicates = mt.GetPredicates();
            foreach (var pred in predicates)
            {

            }
        }

        [TestMethod]
        public void GetRecentValuesTest()
        {
            MachineTags mt = new MachineTags(this.data.apiKey);
            var values = mt.GetRecentValues(addedSince:"213123");
            foreach (var val in values)
            {

            }
        }

        [TestMethod]
        public void GetValuesTest()
        {
            MachineTags mt = new MachineTags(this.data.apiKey);
            var values = mt.GetValues("upcoming", "event");
            foreach (var val in values)
            {

            }
        }

        [TestMethod]
        public void GetPairsTest()
        {
            MachineTags mt = new MachineTags(this.data.apiKey);
            var pairs = mt.GetPairs();
            foreach (var pair in pairs)
            {

            }
        }

        [TestMethod]
        public void GetNamespacesTest()
        {
            MachineTags mt = new MachineTags(this.data.apiKey);
            var nss = mt.GetNamespaces();
            foreach (var ns in nss)
            {

            }
        }
    }
}
