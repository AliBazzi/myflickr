using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Test
{
    [TestClass]
    public class photoSetTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetCommentsTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            foreach (var comment in photoset.GetCommentsList())
            {

            }
        }

        [TestMethod]
        public void GetCommentsTest2()
        {
            var user = new User(this.data.apiKey, "75998848@N00");
            var photoset = user.GetPhotoSetsList().First();
            foreach (var comment in photoset.GetCommentsList())
            {

            }
        }

        [TestMethod]
        public void AddRemoveEditCommentsTest() 
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var id = photoset.AddComment("test");
            photoset.EditComment(id, "test Edited!");
            photoset.DeleteComment(id);
        }

        [TestMethod]
        public void CreateDeletePhotoSetTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPhotos().Skip(10).First();
            var photosettoken = user.CreatePhotoSet("test",photo.ID,"test");
            var instance = photosettoken.CreateInstance();
            instance.Delete();
        }

        [TestMethod]
        public void AddRemovePhotosToPhotosetTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var photos = user.GetPhotos().Skip(10).Take(4);

            foreach (var photo in photos)
                photoset.AddPhoto(photo.ID);

            photoset.RemovePhoto(photos.First().ID);
            photoset.RemovePhotos(photos.Skip(1).Select(photo => photo.ID).ToArray());
        }

        [TestMethod]
        public void GetPhotosTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photos = user.GetPhotoSetsList().First().GetPhotos();
            foreach (var photo in photos)
            { }
        }

        [TestMethod]
        public void EditPhotosTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var photos = photoset.GetPhotos();
            var primary = photos.First(photo => photo.IsPrimary.Value);
            photoset.EditPhotos(primary.ID, photos.Reverse().Select(photo=>photo.ID).ToArray());
            photoset.EditPhotos(primary.ID, photos.Select(photo => photo.ID).ToArray());
        }

        [TestMethod]
        public void SetMetaTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var title = photoset.Title;
            var desc = photoset.Description;
            photoset.SetMetadata("test", "Test !");
            photoset.SetMetadata(title, desc);
        }

        [TestMethod]
        public void SetPrimaryPhotoTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var photos = photoset.GetPhotos();
            var primary = photos.First(photo => photo.IsPrimary.Value);
            photoset.SetPrimaryPhoto(photos.First().ID);
            photoset.SetPrimaryPhoto(primary.ID);
        }

        [TestMethod]
        public void ReorderPhotosTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var photos = photoset.GetPhotos();
            photoset.ReorderPhotos(photos.Reverse().Select(photo => photo.ID).ToArray());
            photoset.ReorderPhotos(photos.Select(photo => photo.ID).ToArray());
        }

        [TestMethod]
        public void GetContextTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photoset = user.GetPhotoSetsList().First();
            var photos = photoset.GetPhotos();
            var context = photos.Skip(1).First().GetContextinSet(photoset.ID);
        }

        [TestMethod]
        public void OrderSetsTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var sets = user.GetPhotoSetsList();
            user.OrderSets(sets.Reverse().Select(set => set.ID).ToArray());
            user.OrderSets(sets.Select(set => set.ID).ToArray());
        }
    }
}
