using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aritiafel.Organizations.RaeriharUniversity;
using System;

namespace RaeriharTest
{
    [TestClass]
    public class MainTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ExponentGetSetTest()
        {
            ArNumber a = new ArNumber(3);
            a.Exponent = 10;
            Assert.IsTrue(a.ToString() == "3E+10");
            a.Exponent = -11;
            Assert.IsTrue(a.ToString() == "3E-11");
            a.Exponent = 15;
            Assert.IsTrue(a.ToString() == "3E+15");
            a.Exponent = 16;
            Assert.IsTrue(a.ToString() == "3E+16");
            a.Exponent = -16;
            Assert.IsTrue(a.ToString() == "3E-16");
            a.Exponent = -17;
            Assert.IsTrue(a.ToString() == "3E-17");
            a.Exponent = 4095;
            Assert.IsTrue(a.ToString() == "3E+4095");
            a.Exponent = 4096;
            Assert.IsTrue(a.ToString() == "3E+4096");
            a.Exponent = -4096;
            Assert.IsTrue(a.ToString() == "3E-4096");
            a.Exponent = -4097;
            Assert.IsTrue(a.ToString() == "3E-4097");
            a.Exponent = 65883;
            Assert.IsTrue(a.ToString() == "3E+65883");
            a.Exponent = 73813185;
            Assert.IsTrue(a.ToString() == "3E+73813185");
            a.Exponent = -3185;
            Assert.IsTrue(a.ToString() == "3E-3185");
            a.Exponent = 268435456;
            Assert.IsTrue(a.ToString() == "3E+268435456");
            a.Exponent = 268435455;
            Assert.IsTrue(a.ToString() == "3E+268435455");
            a.Exponent = -268435456;
            Assert.IsTrue(a.ToString() == "3E-268435456");
            a.Exponent = -268435457;
            Assert.IsTrue(a.ToString() == "3E-268435457");
            a.Exponent = 9999999999999999;
            Assert.IsTrue(a.ToString() == "3E+9999999999999999");
        }

        [TestMethod]
        public void NewArNumber()
        {   
            ArNumber a = new ArNumber(3);
            TestContext.WriteLine(a.ToString());
            a = 20;
            TestContext.WriteLine(a.ToString());
            a = 56;
            TestContext.WriteLine(a.ToString());
            a = 127;
            TestContext.WriteLine(a.ToString());
            a = -13;
            TestContext.WriteLine(a.ToString());

        }
    }
}
