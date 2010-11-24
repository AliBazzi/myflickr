using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFlickr.Core;

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
        public void AddRemoveToFavoriteTest()
        {
           var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance()
               .GetPhotos(new User(this.data.apiKey, "53703764@N02")).Photos.First();
           res.AddToFavorite();
           res.RemoveFromFavorite();
        }

        [TestMethod]
        public void GetExifTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotos().Photos.First();
            foreach (var exif in res.GetExif())
            {

            }
        }

        [TestMethod]
        public void GetExifTest2()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPublicPhotos().Photos.First();
            foreach (var exif in res.GetExif())
            {

            }
        }

        [TestMethod]
        public void GetFavoritesTest()
        {
            var res = new User(this.data.apiKey, "36893321@N03").GetPublicPhotos().Photos.First(p => p.ID.ToString() == "5015455849");
            foreach (var person in res.GetFavorites())
            {

            }
        }

        [TestMethod]
        public void GetFavoritesTest2()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotos()
                .Photos.First(p => p.ID.ToString() == "5015455849");
            foreach (var person in res.GetFavorites())
            {

            }
        }

        [TestMethod]
        public void PostToBlogTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            photo.PostPhotoToBlog("hello", "from MyFlickr Library :)",blogID: user.GetBlogsList().First().ID);
        }

        [TestMethod]
        public void GetInfoTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPhotos().Photos.First();
            var info = photo.GetInfo();
        }

        [TestMethod]
        public void GetInfoTest2()
        {
            var user = new User(this.data.apiKey, "36893321@N03");
            var photo = user.GetPublicPhotos().Photos.First();
            var info = photo.GetInfo();
        }

        [TestMethod]
        public void AddRemoveTagsTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPhotos().Photos.First();
            photo.AddTag("test");

            foreach (var tag in photo.GetInfo().Tags)
            {
                photo.RemoveTag(tag.ID);
            }
        }

        [TestMethod]
        public void AddRemoveTagsTest2()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPhotos(new User(this.data.apiKey, "53703764@N02")).Photos.First();
            photo.AddTag("test hello bye");

            foreach (var tag in photo.GetInfo().Tags.Where(tag=> tag.AuthorID == user.UserID))
            {
                photo.RemoveTag(tag.ID);
            }
        }

        [TestMethod]
        public void GetPermsTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPhotos().Photos.First();
            var perms = photo.GetPermissions();
        }

        [TestMethod,ExpectedException(typeof(FlickrException))]
        public void GetPermsTest2()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos(new User(this.data.apiKey, "53703764@N02")).Photos.First();
            var perms = photo.GetPermissions();
        }

        [TestMethod]
        public void SetPermsTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            var perms = photo.SetPermissions(true,false,false, CommentPermission.Everybody, AddMetadataPermission.Everybody);
        }

        [TestMethod]
        public void SetMetaTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            var info = photo.GetInfo();
            photo.SetMeta("","");
            photo.SetMeta(info.Title, info.Description);
        }

        [TestMethod]
        public void SetSafetyLevelTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            photo.SetSafetyLevel(SafetyLevel.Restricted);
            photo.SetSafetyLevel(SafetyLevel.Safe);
        }

        [TestMethod, ExpectedException(typeof(FlickrException))]
        public void SetDatesTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            photo.SetDates();
        }

        [TestMethod]
        public void SetContentTypeTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            photo.SetContentType(ContentType.Photos);
        }

        [TestMethod]
        public void GetContextTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            var context = photo.GetContext();
        }

        [TestMethod]
        public void GetContextTest2()
        {
            var user = new User(this.data.apiKey, "53703764@N02");
            var photo = user.GetPublicPhotos().Photos.First();
            var context = photo.GetContext();
        }

        [TestMethod]
        public void GetAllContexts()
        {
            var user = new User(this.data.apiKey, "53703764@N02");
            var photo = user.GetPublicPhotos().Photos.First();
            var contexts = photo.GetAllContexts();
            foreach (var set in contexts.Sets)
            {

            }
            foreach (var pool in contexts.Pools)
            {

            }
        }

        [TestMethod]
        public void GetAllContexts2()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            var contexts = photo.GetAllContexts();
            foreach (var set in contexts.Sets)
            {

            }
            foreach (var pool in contexts.Pools)
            {

            }
        }

        [TestMethod]
        public void GetSizesTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().Photos.First();
            var sizes = photo.GetSizes();
            foreach (var size in sizes)
            {

            }
        }

        [TestMethod]
        public void GetSizesTest2()
        {
            var user = new User(this.data.apiKey, "53703764@N02");
            var photo = user.GetPublicPhotos().Photos.First();
            var sizes = photo.GetSizes();
            foreach (var size in sizes)
            {

            }
        }
    }
}
