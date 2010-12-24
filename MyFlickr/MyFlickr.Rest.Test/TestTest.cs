using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyFlickr.Rest;
using MyFlickr.Core;
namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class TestTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void EchoTest()
        {
            Test test = new Test(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var pars = test.Echo(new Parameter("hello","Hello"));
        }

        [TestMethod]
        public void EchoTest2()
        {
            Test test = new Test(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var pars = test.Echo();
        }

        [TestMethod]
        public void NullTest()
        {
            Test test = new Test(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            test.Null();
        }

        [TestMethod]
        public void LoginTest()
        {
            Test test = new Test(new Authenticator(this.data.apiKey, this.data.sharedSecret).CheckToken(this.data.token));
            var res = test.Login();
        }
    }
}
