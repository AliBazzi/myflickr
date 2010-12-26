using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class LicensesTest
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
            Licenses lcns = new Licenses(this.data.apiKey);
            foreach (var item in lcns.GetInfo())
            {

            }
        }

        [TestMethod]
        public void SetLicenseTest()
        {
            var res = new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token).CreateUserInstance().GetPhotos().Photos.First();
            var lid = res.GetInfo().License;
            res.SetLicense(3);
            res.SetLicense(lid);
        }
    }
}
