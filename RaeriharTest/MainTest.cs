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
            a.Exponent = 0;
            Assert.IsTrue(a.ToString() == "3");
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
            a.Exponent = -99998899999999999;
            Assert.IsTrue(a.ToString() == "3E-99998899999999999");
            a.Exponent = 1152921504606846975;
            Assert.IsTrue(a.ToString() == "3E+1152921504606846975");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.Exponent = 1152921504606846976);
            a.Exponent = -1152921504606846976;
            Assert.IsTrue(a.ToString() == "3E-1152921504606846976");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => a.Exponent = -1152921504606846977);
        }

        [TestMethod]
        public void ParseTest()
        {
            ArNumber sn = new ArNumber();
            string[] testStrings = {
                "-0.00589662145E-103",
                "-0.03611895678E+51",
                "-0000.0006871800",
                "35678943580000000000000000000000000000000",
                "+568.68100E-8",
                "-.00035E-20",
                "0.00",
                "-0.000",
                "+300",
                "-0.008",
                "0.00681E+98",
                "0.01",
                "0"
            };

            sn = ArNumber.Parse(testStrings[0]);
            TestContext.WriteLine(sn.ToString());
            Assert.IsTrue(sn.ToString() == "-5.89662145E-106");
            sn = ArNumber.Parse(testStrings[1]);
            Assert.IsTrue(sn.ToString() == "-3.611895678E+49");
            sn = ArNumber.Parse(testStrings[2]);
            Assert.IsTrue(sn.ToString() == "-6.8718E-4");
            //Assert.IsTrue(sn.ToString("C") == "-0.00068718");
            sn = ArNumber.Parse(testStrings[3]);
            Assert.IsTrue(sn.ToString() == "3.567894358E+40");
            sn = ArNumber.Parse(testStrings[4]);
            Assert.IsTrue(sn.ToString() == "5.68681E-6");
            //Assert.IsTrue(sn.ToString("C") == "0.00000568681");
            //TestContext.WriteLine(sn.ToString("C3"));
            //Assert.IsTrue(sn.ToString("C3") == "0.00000569");
            //Assert.IsTrue(sn.ToString("C3") == "0.00");
            sn = ArNumber.Parse(testStrings[5]);
            Assert.IsTrue(sn.ToString() == "-3.5E-24");
            sn = ArNumber.Parse(testStrings[6]);
            Assert.IsTrue(sn.ToString() == "0");
            //Assert.IsTrue(sn.ToString("C") == "0");
            sn = ArNumber.Parse(testStrings[7]);
            Assert.IsTrue(sn.ToString() == "0");
            //Assert.IsTrue(sn.ToString("C") == "0");
            sn = ArNumber.Parse(testStrings[8]);
            Assert.IsTrue(sn.ToString() == "3E+2");
            sn = ArNumber.Parse(testStrings[9]);
            Assert.IsTrue(sn.ToString() == "-8E-3");
            sn = ArNumber.Parse(testStrings[10]);
            Assert.IsTrue(sn.ToString() == "6.81E+95");
            sn = ArNumber.Parse(testStrings[11]);
            Assert.IsTrue(sn.ToString() == "1E-2");
            sn = ArNumber.Parse(testStrings[12]);
            Assert.IsTrue(sn.ToString() == "0");
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

        [TestMethod]
        public void Test()
        {
            
        }


    }
}
