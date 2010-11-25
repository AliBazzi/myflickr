using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Test
{
    [TestClass]
    public class photosSetTest
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
    }
}
