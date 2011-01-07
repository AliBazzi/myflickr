using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class TagsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetHotListTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetHotList();
        }

        [TestMethod]
        public void GetClustersTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetClusters("old city");
        }

        [TestMethod]
        public void GetClusterPhotosTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetClusterPhotos("old city"," ");
        }

        [TestMethod]
        public void GetTagsListofPhotoTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetTagsListofPhoto("5307497148");
        }

        [TestMethod]
        public void GetTagsListofUserTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetTagsListofUser("36893321@N03");
        }

        [TestMethod]
        public void GetTagsListofUserTest2()
        {
            Tags tags = new Tags(new Authenticator(this.data.apiKey,this.data.sharedSecret).CheckToken(this.data.token));
            var res = tags.GetTagsListofUser();
        }

        [TestMethod]
        public void GetPopularTagsListofUserTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetPopularTagsListofUser("36893321@N03");
        }

        [TestMethod]
        public void GetPopularTagsListofUserTest2()
        {
            Tags tags = new Tags(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = tags.GetPopularTagsListofUser();
        }

        [TestMethod]
        public void GetRawTagsListofUserTest()
        {
            Tags tags = new Tags(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = tags.GetRawTagsListofUser();
        }

        [TestMethod]
        public void GetRelatedTagsTest()
        {
            Tags tags = new Tags(this.data.apiKey);
            var res = tags.GetRelatedTags("old city");
        }
    }
}
