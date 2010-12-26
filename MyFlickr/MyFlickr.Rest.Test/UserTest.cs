using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
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
        public void GetPublicContactsListTest()
        {
            var user = new Authenticator(data.apiKey, data.sharedSecret).CheckToken(data.token).CreateUserInstance();
            var list = user.GetPublicContactsList();
            foreach (var contact in list)
            {
                //nothing , just enumerate
            }
        }

        [TestMethod]
        public void GetPublicContactsListTest2()
        {
            var user = new User(data.apiKey, "36893321@N03");
            var list = user.GetPublicContactsList();
            foreach (var contact in list) { }
        }

        [TestMethod]
        public void GetPublicContactsListTest3()
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
        public void GetPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotos();
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPhotosTest2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPhotos(contentType: ContentType.Photos,privacyFilter:PrivacyFilter.VisibleToFriendsandFamilyOnly);
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPublicPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPublicPhotos();
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPublicPhotosTest2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPublicPhotos(perPage:20,page:2);
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetPublicPhotosTest3()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetPublicPhotos(new User(this.data.apiKey, "36893321@N03"), SafetyLevel.Safe);
            foreach (var photo in res) { }
        }

        [TestMethod]
        public void GetInfoTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetInfo();
        }

        [TestMethod]
        public void GetInfoTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetInfo();
        }

        [TestMethod]
        public void GetInfoOfTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetInfo(new User(this.data.apiKey, "53703764@N02"));
        }

        [TestMethod]
        public void GetPhotoSetsListTest()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPhotoSetsList();
            foreach (var item in res.PhotoSets) { }
        }

        [TestMethod]
        public void GetPhotoSetsListTest2()
        {
            var res = new Authenticator(this.data.apiKey,this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotoSetsList();
            foreach (var item in res.PhotoSets) { }
        }

        [TestMethod]
        public void GetGalleriesListTest()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetGalleriesList();
            foreach (var gallery in res.Galleries) { }
        }

        [TestMethod]
        public void GetGalleriesListTest2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetGalleriesList();
            foreach (var gallery in res.Galleries) { }
        }

        [TestMethod]
        public void GetPublicGroupsTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPublicGroups();
            foreach (var group in res.Groups) { }
        }

        [TestMethod]
        public void GetPublicGroupsTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPublicGroups();
            foreach (var group in res.Groups) { }
        }

        [TestMethod]
        public void GetBlogsListTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetBlogsList();
            foreach (var blog in res.Blogs) { }
        }

        [TestMethod]
        public void GetCollectionsTreeTest()
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
        public void GetCollectionsTreeTest2()
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

        [TestMethod]
        public void GetPhotosOfUserTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotosOfUser();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetPhotosOfUserTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPhotosOfUser();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetPublicFavoritesTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPublicFavoritesList();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetPublicFavoritesTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPublicFavoritesList();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetFavoritesTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetFavoritesList();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod,ExpectedException(typeof(InvalidOperationException))]
        public void GetFavoritesTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetFavoritesList();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetFavoritesTest3()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                .GetFavoritesList(new User(this.data.apiKey, "53703764@N02"));
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetPhotosNotInSetTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                           .GetPhotosNotInSet();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetGeotaggedPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                                      .GetGeotaggedPhotos();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetUnGeotaggedPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                                      .GetUnGeotaggedPhotos();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetUntaggedPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                                      .GetUntaggedPhotos();
            foreach (var item in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetPhotosCountsTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                      .GetPhotosCounts(new DateTime(2008,1,1),new DateTime(2009,1,1),new DateTime(2010,1,1));
            foreach (var range in res)
            {

            }
        }

        [TestMethod]
        public void GetRecentlyUpdatedTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                                 .GetRecentlyUpdatedPhotos(new DateTime(2010, 9, 1));
            foreach (var photo in res.Photos)
            {

            }
        }

        [TestMethod]
        public void GetContactsPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                                            .GetContactsPhotos();
            foreach (var photo in res)
            {

            }
        }

        [TestMethod]
        public void GetContactsPublicPhotosTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                                                       .GetContactsPublicPhotos();
            foreach (var photo in res)
            {

            }
        }

        [TestMethod]
        public void GetContactsPublicPhotosTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetContactsPublicPhotos();
            foreach (var photo in res)
            {

            }
        }

        [TestMethod]
        public void GetListContactsRecentlyUploadedTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
                        .GetListRecentlyUploaded((DateTime.Now - new TimeSpan(23,0,0)).ToUnixTimeStamp().ToString());
            foreach (var contact in res)
            {

            }
        }

        [TestMethod]
        public void GetGroupsTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetGroups();
            foreach (var group in res.Groups) { }
        }

        [TestMethod]
        public void GetPhotosActivitiesTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var items = user.GetPhotosActivities("24h");
            foreach (var item in items)
            {
                foreach (var Event in item)
                {

                }
            }
         }

        [TestMethod]
        public void GetCommentsActivitesTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var items = user.GetCommentsActivities();
            foreach (var item in items)
            {
                foreach (var Event in item)
                {

                }
            }
        }
    }
}
