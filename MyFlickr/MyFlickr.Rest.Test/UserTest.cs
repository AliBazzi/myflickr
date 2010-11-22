using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MyFlickr.Rest.Test
{
    [TestClass]
    public class UserTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetContactsListTest()
        {
            var user = new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token).CreateUserInstance();
            var list = user.GetContactsList();
            foreach (var contact in list)
            {
                //nothing , just enumerate
            }
        }

        [TestMethod,ExpectedException(typeof(InvalidOperationException))]
        public void GetContactsListTest2()
        {
            var user = new User(data.apiKey, "36893321@N03");
            var list = user.GetContactsList();
        }

        [TestMethod]
        public void GetPublicContactsList()
        {
            var user = new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token).CreateUserInstance();
            var list = user.GetPublicContactsList();
            foreach (var contact in list)
            {
                //nothing , just enumerate
            }
        }

        [TestMethod]
        public void GetPublicContactsList2()
        {
            var user = new User(data.apiKey, "36893321@N03");
            var list = user.GetPublicContactsList();
            foreach (var contact in list) { }
        }

        [TestMethod]
        public void GetPublicContactsList3()
        {
            var user = new User(data.apiKey, "36893321@N03");
            var list = user.GetPublicContactsList(2,10);
            Assert.AreEqual<int>(list.PerPage, 10);
            foreach (var contact in list)
            {
                //nothing , just enumerate
            }
        }

        [TestMethod]
        public void GetPhotos()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotos();
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPhotos2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPhotos(contentType: ContentType.Photos,privacyFilter:PrivacyFilter.VisibleToFriendsandFamilyOnly);
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPublicPhotos()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPublicPhotos();
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPublicPhotos2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPublicPhotos(perPage:20,page:2);
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPublicPhotos3()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPublicPhotos(new User(this.data.apiKey, "36893321@N03"), SafeSearch.Safe);
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetInfo()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetInfo();
        }

        [TestMethod]
        public void GetInfo2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetInfo();
        }

        [TestMethod]
        public void GetInfoOf()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetInfo(new User(this.data.apiKey, "53703764@N02"));
        }

        [TestMethod]
        public void GetPhotoSetsList()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPhotoSetsList();
            foreach (var item in res.PhotoSets) { }
        }

        [TestMethod]
        public void GetPhotoSetsList2()
        {
            var res = new Authenticator(this.data.apiKey,this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotoSetsList();
            foreach (var item in res.PhotoSets) { }
        }

        [TestMethod]
        public void GetGalleriesList()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetGalleriesList();
            foreach (var gallery in res.Galleries) { }
        }

        [TestMethod]
        public void GetGalleriesList2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetGalleriesList();
            foreach (var gallery in res.Galleries) { }
        }

        [TestMethod]
        public void GetPublicGroups()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPublicGroups();
            foreach (var group in res.Groups) { }
        }

        [TestMethod]
        public void GetPublicGroups2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPublicGroups();
            foreach (var group in res.Groups) { }
        }

        [TestMethod]
        public void GetBlogsList()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetBlogsList();
            foreach (var blog in res.Blogs) { }
        }

        [TestMethod]
        public void GetCollectionsTree()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetCollectionsTree();
            foreach (var collection in res.Collections) 
            {
                foreach (var bphotoset in collection.PhotosSets)
                {

                }
                foreach (var col in collection.Collections.Collections)
                {

                }
            }
        }

        [TestMethod]
        public void GetCollectionsTree2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetCollectionsTree();
            foreach (var collection in res.Collections)
            {
                foreach (var bphotoset in collection.PhotosSets)
                {

                }
                foreach (var col in collection.Collections.Collections)
                {

                }
            }
        }
    }
}
