using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Test
{
    [TestClass]
    public class GalleryTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        //[TestMethod]
        public void CreateGalleryTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallerytoken = user.CreateGallery("test", "test");
            var gallery = gallerytoken.CreateInstance();
        }

        //[TestMethod]
        public void CreateGalleryTest2()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallerytoken = user.CreateGallery("test", "test",user.GetFavoritesList().First().ID);
            var gallery = gallerytoken.CreateInstance();
        }

        //[TestMethod]
        public void AddPhotoTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallery = user.GetGalleriesList().First();
            gallery.AddPhoto(user.GetFavoritesList().First().ID);
        }

        [TestMethod]
        public void EditMetadataTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallery = user.GetGalleriesList().First();
            var title = gallery.Title;
            var desc = gallery.Description;
            gallery.EditMetadata("test", "test");
            gallery.EditMetadata(title, desc);
        }

        [TestMethod]
        public void EditPhotoTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallery = user.GetGalleriesList().First();
            var photos = gallery.GetPhotos();
            var comment = photos.First().Comment;
            gallery.EditPhoto(photos.First().ID, "Test");
            gallery.EditPhoto(photos.First().ID, comment?? "");
        }

        [TestMethod]
        public void GetPhotosTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallery = user.GetGalleriesList().First();
            foreach (var photo in gallery.GetPhotos())
            {

            }
        }

        [TestMethod]
        public void EditPhotosTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var gallery = user.GetGalleriesList().First();
            var photos = gallery.GetPhotos();
            gallery.EditPhotos(gallery.Primary, photos.Reverse().Select(photo => photo.ID).ToArray());
            gallery.EditPhotos(gallery.Primary, photos.Select(photo => photo.ID).ToArray());
        }
    }
}
