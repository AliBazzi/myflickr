using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class CommonsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetInstitutionsTest()
        {
            Commons cmns = new Commons(this.data.apiKey);
            var insts = cmns.GetInstitutions();
            foreach (var inst in insts)
            {

            }
        }
    }
}
