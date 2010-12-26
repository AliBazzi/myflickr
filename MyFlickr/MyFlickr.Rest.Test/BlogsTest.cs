using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class BlogsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetServicesTest()
        {
            Blogs blgs = new Blogs(this.data.apiKey);
            var blogs = blgs.GetServices();
            foreach (var srvc in blogs)
            {

            }
        }
    }
}
