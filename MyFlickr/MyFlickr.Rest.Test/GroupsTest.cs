using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class GroupsTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetInfoTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var groupinfo = user.GetPublicGroups().First().GetInfo();
        }

        [TestMethod]
        public void GetContextTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().First();
            var context = photo.GetContext(photo.GetAllContexts().Pools.First().ID);
        }

        [TestMethod]
        public void AddRemoveTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var photo = user.GetPublicPhotos().First();
            var group = user.GetGroups().First(grp => grp.ID == "1350286@N23");
            group.Add(photo.ID);
            group.Remove(photo.ID);
        }

        [TestMethod]
        public void GetPhotosTest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var group = user.GetGroups().First();
            var photos = group.GetPhotos();
        }

        [TestMethod]
        public void GetPhotosTest2()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var group = user.GetGroups().First();
            var photos = group.GetPhotos(user.UserID);
        }

        [TestMethod]
        public void GetMemberstest()
        {
            var user = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance();
            var group = user.GetGroups().First();
            var members = group.GetMembersList();
            foreach (var member in members)
            {

            }
        }
    }
}
