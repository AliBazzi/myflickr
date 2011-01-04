using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class SearchTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void SearchPhotosTest()
        {
            Search search = new Search(this.data.apiKey);
            var res = search.SearchPhotos(text:"old city");
            foreach (var photo in res)
            {

            }
        }

        [TestMethod]
        public void FindPeopleByEmailTest()
        {
            Search search = new Search(this.data.apiKey);
            var res = search.FindPeopleByEmail("neo.netprogrammer@hotmail.com");
        }

        [TestMethod]
        public void FindPeopleByUsernameTest()
        {
            Search search = new Search(this.data.apiKey);
            var res = search.FindPeopleByUsername("Ali Bazzi");
        }
    }
}
