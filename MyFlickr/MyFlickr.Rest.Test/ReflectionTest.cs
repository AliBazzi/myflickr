using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyFlickr.Rest.Tests
{
    [TestClass]
    public class ReflectionTest
    {
        private Data data;

        [TestInitialize]
        public void GetData()
        {
            data = new XmlSerialization.Serialization<Data>().Deserialize("C:\\data.xml");
        }

        [TestMethod]
        public void GetMethodsTest()
        {
            Reflection reflection = new Reflection(this.data.apiKey);
            var methods = reflection.GetMethods();
            foreach (var method in methods)
            {

            }
        }

        [TestMethod]
        public void GetMethodInfoTest()
        {
            Reflection reflection = new Reflection(this.data.apiKey);
            foreach (var method in reflection.GetMethods().Skip(1).Take(5))
            {
                var mi = reflection.GetMethodInfo(method.Name);
            }
        }
    }
}
