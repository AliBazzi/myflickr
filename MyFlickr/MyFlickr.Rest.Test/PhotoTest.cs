using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Test
{
    [TestClass]
    public class PhotoTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void AddRemoveToFavorite()
        {
           var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
               .GetPhotos(new User(this.data.apiKey, "53703764@N02")).Photos.First();
           res.AddToFavorite();
           res.RemoveFromFavorite();
        }
    }
}
