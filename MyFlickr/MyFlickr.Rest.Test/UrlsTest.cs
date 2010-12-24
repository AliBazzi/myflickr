using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class UrlsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetGroupTest()
        {
            Urls urls = new Urls(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var group = user.GetGroups().First();
            var url = urls.GetGroup(group.ID);
        }

        [TestMethod]
        public void GetUserPhotosTest()
        {
            Urls urls = new Urls(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var url = urls.GetUserPhotos("36893321@N03");
        }

        [TestMethod]
        public void GetUserProfileTest()
        {
            Urls urls = new Urls(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var url = urls.GetUserProfile();
        }

        [TestMethod]
        public void GalleryLookupTest()
        {
            Urls urls = new Urls(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var gallery = urls.LookupGallery("http://www.flickr.com/photos/alibazzi/galleries/72157624737485558/");
        }

        [TestMethod]
        public void LookupGroupTest()
        {
            Urls urls = new Urls(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = urls.LookupGroup("http://www.flickr.com/groups/omayadmosque/");
        }

        [TestMethod]
        public void LookupUserTest()
        {
            Urls urls = new Urls(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = urls.LookupUser("http://www.flickr.com/photos/alibazzi");
        }
    }
}
