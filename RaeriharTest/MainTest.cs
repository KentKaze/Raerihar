using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aritiafel.Organizations.RaeriharUniversity;
using System;
using Aritiafel.Artifacts;

namespace RaeriharTest
{
    [TestClass]
    public class MainTest
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ExponentGetSetTest()
        {
            ArNumber ar = new ArNumber(3);
            ar.Exponent = 0;
            Assert.IsTrue(ar.ToString() == "3");
            ar.Exponent = 10;
            Assert.IsTrue(ar.ToString() == "3E+10");
            ar.Exponent = -11;
            Assert.IsTrue(ar.ToString() == "3E-11");
            ar.Exponent = 15;
            Assert.IsTrue(ar.ToString() == "3E+15");
            ar.Exponent = 16;
            Assert.IsTrue(ar.ToString() == "3E+16");
            ar.Exponent = -16;
            Assert.IsTrue(ar.ToString() == "3E-16");
            ar.Exponent = -17;
            Assert.IsTrue(ar.ToString() == "3E-17");
            ar.Exponent = 4095;
            Assert.IsTrue(ar.ToString() == "3E+4095");
            ar.Exponent = 4096;
            Assert.IsTrue(ar.ToString() == "3E+4096");
            ar.Exponent = -4096;
            Assert.IsTrue(ar.ToString() == "3E-4096");
            ar.Exponent = -4097;
            Assert.IsTrue(ar.ToString() == "3E-4097");
            ar.Exponent = 65883;
            Assert.IsTrue(ar.ToString() == "3E+65883");
            ar.Exponent = 73813185;
            Assert.IsTrue(ar.ToString() == "3E+73813185");
            ar.Exponent = -3185;
            Assert.IsTrue(ar.ToString() == "3E-3185");
            ar.Exponent = 268435456;
            Assert.IsTrue(ar.ToString() == "3E+268435456");
            ar.Exponent = 268435455;
            Assert.IsTrue(ar.ToString() == "3E+268435455");
            ar.Exponent = -268435456;
            Assert.IsTrue(ar.ToString() == "3E-268435456");
            ar.Exponent = -268435457;
            Assert.IsTrue(ar.ToString() == "3E-268435457");
            ar.Exponent = 9999999999999999;
            Assert.IsTrue(ar.ToString() == "3E+9999999999999999");
            ar.Exponent = -99998899999999999;
            Assert.IsTrue(ar.ToString() == "3E-99998899999999999");
            ar.Exponent = 1152921504606846975;
            Assert.IsTrue(ar.ToString() == "3E+1152921504606846975");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ar.Exponent = 1152921504606846976);
            ar.Exponent = -1152921504606846976;
            Assert.IsTrue(ar.ToString() == "3E-1152921504606846976");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ar.Exponent = -1152921504606846977);
        }

        [TestMethod]
        public void ParseTest()
        {
            ArNumber ar = new ArNumber();

            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                double d = cb.DrawOutDiversityDouble();
                ar = new ArNumber(d);
                TestContext.WriteLine($"{d}: {ar}");
            }

            string[] testStrings = {
                "-0.00589662145E-103", //0
                "-0.03611895678E+51", //1
                "-0000.0006871800", //2
                "35678943580000000000000000000000000000000", //3
                "+568.68100E-8", //4
                "-.00035E-20", //5
                "0.00", //6
                "-0.000", //7
                "+300", //8
                "-0.008", //9
                "0.00681E+98", //10
                "0.01", //11
                "0" //12
            };

            ar = ArNumber.Parse(testStrings[0]);
            Assert.IsTrue(ar.ToString() == "-5.89662145E-106");
            ar = ArNumber.Parse(testStrings[1]);
            
            //-3.61189567800E+49
            Assert.IsTrue(ar.ToString() == "-3.611895678E+49");
            ar = ArNumber.Parse(testStrings[2]);
            Assert.IsTrue(ar.ToString() == "-6.8718E-4");
            //Assert.IsTrue(sn.ToString("C") == "-0.00068718");
            ar = ArNumber.Parse(testStrings[3]);
            Assert.IsTrue(ar.ToString() == "3.567894358E+40");
            ar = ArNumber.Parse(testStrings[4]);
            Assert.IsTrue(ar.ToString() == "5.68681E-6");
            //Assert.IsTrue(sn.ToString("C") == "0.00000568681");
            //TestContext.WriteLine(sn.ToString("C3"));
            //Assert.IsTrue(sn.ToString("C3") == "0.00000569");
            //Assert.IsTrue(sn.ToString("C3") == "0.00");
            ar = ArNumber.Parse(testStrings[5]);
            
            Assert.IsTrue(ar.ToString() == "-3.5E-24");
            ar = ArNumber.Parse(testStrings[6]);
            Assert.IsTrue(ar.ToString() == "0");
            //Assert.IsTrue(sn.ToString("C") == "0");
            ar = ArNumber.Parse(testStrings[7]);
            Assert.IsTrue(ar.ToString() == "0");
            //Assert.IsTrue(sn.ToString("C") == "0");
            ar = ArNumber.Parse(testStrings[8]);
            Assert.IsTrue(ar.ToString() == "3E+2");
            ar = ArNumber.Parse(testStrings[9]);
            Assert.IsTrue(ar.ToString() == "-8E-3");
            ar = ArNumber.Parse(testStrings[10]);
            Assert.IsTrue(ar.ToString() == "6.81E+95");
            ar = ArNumber.Parse(testStrings[11]);
            Assert.IsTrue(ar.ToString() == "1E-2");
            ar = ArNumber.Parse(testStrings[12]);
            Assert.IsTrue(ar.ToString() == "0");

            
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
