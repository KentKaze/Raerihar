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
        public void NewArNumber()
        {
            //Console.WriteLine("Hello World!");
            ArNumber a = new ArNumber(3);
            a.Exponent = 10;
            TestContext.WriteLine(a.ToString());
            a.Exponent = -11;
            TestContext.WriteLine(a.ToString());
            a.Exponent = 65883;
            TestContext.WriteLine(a.ToString());
            a.Exponent = 73813185;
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
