using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class StatsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetPhotoDomainsTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetPhotoDomains("2011-01-05");
        }

        [TestMethod]
        public void GetCollectionDomainsTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetCollectionDomains("2011-01-05");
        }

        [TestMethod]
        public void GetPhotostreamsDomainsTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetPhotostreamsDomains("2011-01-05");
        }

        [TestMethod]
        public void GetPhotostreamStatsTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetPhotostreamStats("2011-01-05");
        }

        [TestMethod]
        public void GetTotalViewsTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetTotalViews();
        }

        [TestMethod]
        public void GetCSVFilesTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetCSVFiles();
        }

        [TestMethod]
        public void GetCollectionStatsTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            var user = new User(auth);
            Stats stats = new Stats(auth);
            var id = user.GetCollectionsTree().First().ID.Split(new char[]{'-'}).Last();
            var res = stats.GetCollectionStats("2011-01-06",id);
        }

        [TestMethod]
        public void GetPhotosetStatsTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            var user = new User(auth);
            Stats stats = new Stats(auth);
            var id = user.GetPhotoSetsList().First().ID;
            var res = stats.GetPhotosetStats("2011-01-06", id);
        }

        [TestMethod]
        public void GetPhotostreamReferrersTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            Stats stats = new Stats(auth);
            var refs = stats.GetPhotostreamReferrers("2011-01-05", stats.GetPhotostreamsDomains("2011-01-05").First().Name);
        }

        [TestMethod]
        public void GetPhotoReferrersTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            Stats stats = new Stats(auth);
            var refs = stats.GetPhotoReferrers("2011-01-05", stats.GetPhotoDomains("2011-01-05").First().Name);
        }

        [TestMethod]
        public void GetPhotosetReferrersTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            Stats stats = new Stats(auth);
            var refs = stats.GetPhotosetReferrers("2011-01-05", "flickr.com");
        }

        [TestMethod]
        public void GetCollectionReferrersTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            Stats stats = new Stats(auth);
            var refs = stats.GetPhotoReferrers("2011-01-05", "flickr.com");
        }

        [TestMethod]
        public void GetPhotosetDomainsTest()
        {
            Stats stats = new Stats(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = stats.GetPhotosetDomains("2011-01-05");
        }

        [TestMethod]
        public void GetPhotoStatsTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            var stat = new Stats(auth);
            var res = stat.GetPhotoStats("2011-01-06", "4801682014");
        }

        [TestMethod]
        public void GetPopularPhotosTest()
        {
            var auth = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token);
            var stat = new Stats(auth);
            var res = stat.GetPopularPhotos();
        }
    }
}
