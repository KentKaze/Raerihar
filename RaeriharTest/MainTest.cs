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

        //[TestMethod]
        //public void CompareTest()
        //{
        //    ChaosBox cb = new ChaosBox();
        //    ArNumber ar;

        //    ArNumber br;
        //    for(int i = 0; i < 10000; i++)
        //    {
        //        ar = cb.DrawOutDiversityDouble();
        //        br = cb.DrawOutDecimal();
        //        if (ar > br)
        //            TestContext.WriteLine($"{ar}>{br}");
        //        else
        //            TestContext.WriteLine($"{ar}<{br}");
        //    }

        //}

        //[TestMethod]
        //public void ExponentGetSetTest()
        //{
        //    ArNumber ar = new ArNumber(3);
        //    ar.Exponent = 0;
        //    Assert.IsTrue(ar.ToString("E") == "3");
        //    ar.Exponent = 10;
        //    Assert.IsTrue(ar.ToString("E") == "3E+10");
        //    ar.Exponent = -11;
        //    Assert.IsTrue(ar.ToString("E") == "3E-11");
        //    ar.Exponent = 63;
        //    Assert.IsTrue(ar.ToString("E") == "3E+63");
        //    ar.Exponent = 64;
        //    Assert.IsTrue(ar.ToString("E") == "3E+64");
        //    ar.Exponent = -64;
        //    Assert.IsTrue(ar.ToString("E") == "3E-64");
        //    ar.Exponent = -65;
        //    Assert.IsTrue(ar.ToString("E") == "3E-65");
        //    ar.Exponent = 16383;
        //    Assert.IsTrue(ar.ToString("E") == "3E+16383");
        //    ar.Exponent = 16384;
        //    Assert.IsTrue(ar.ToString("E") == "3E+16384");
        //    ar.Exponent = -16384;
        //    Assert.IsTrue(ar.ToString("E") == "3E-16384");
        //    ar.Exponent = -16385;
        //    Assert.IsTrue(ar.ToString("E") == "3E-16385");
        //    ar.Exponent = 65883;
        //    Assert.IsTrue(ar.ToString("E") == "3E+65883");
        //    ar.Exponent = 73813185;
        //    Assert.IsTrue(ar.ToString("E") == "3E+73813185");
        //    ar.Exponent = -3185;
        //    Assert.IsTrue(ar.ToString("E") == "3E-3185");
        //    ar.Exponent = 1073741823;
        //    Assert.IsTrue(ar.ToString("E") == "3E+1073741823");
        //    ar.Exponent = 1073741824;
        //    Assert.IsTrue(ar.ToString() == "3E+1073741824");
        //    ar.Exponent = -1073741824;
        //    Assert.IsTrue(ar.ToString() == "3E-1073741824");
        //    ar.Exponent = -1073741825;
        //    Assert.IsTrue(ar.ToString() == "3E-1073741825");
        //    ar.Exponent = 9999999999999999;
        //    Assert.IsTrue(ar.ToString() == "3E+9999999999999999");
        //    ar.Exponent = -99998899999999999;
        //    Assert.IsTrue(ar.ToString() == "3E-99998899999999999");
        //    ar.Exponent = 4611686018427387903;
        //    Assert.IsTrue(ar.ToString() == "3E+4611686018427387903");
        //    Assert.ThrowsException<ArgumentOutOfRangeException>(() => ar.Exponent = 4611686018427387904);
        //    ar.Exponent = -4611686018427387904;
        //    Assert.IsTrue(ar.ToString() == "3E-4611686018427387904");
        //    Assert.ThrowsException<ArgumentOutOfRangeException>(() => ar.Exponent = -4611686018427387905);
        //}

        [TestMethod]
        public void ParseTest()
        {
            ArNumber ar = new ArNumber();
            //9.810030755230452E-231: 9.81375523452E-231
            //-4.970069320087064E-275: -4.97693287064E-275


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
                "0", //12
                "2170.6907744747728" //13
            };

            ar = ArNumber.Parse(testStrings[0]);
            //TestContext.WriteLine(ar.ToString("E"));
            Assert.IsTrue(ar.ToString("E") == "-5.89662145E-106");
            ar = ArNumber.Parse(testStrings[1]);
            //TestContext.WriteLine(ar.ToString());
            Assert.IsTrue(ar.ToString("E") == "-3.611895678E+49");
            //TestContext.WriteLine(ar.ToString("D"));
            Assert.IsTrue(ar.ToString("D") == "-36118956780000000000000000000000000000000000000000");            
            ar = ArNumber.Parse(testStrings[2]);
            //TestContext.WriteLine(ar.ToString("E"));
            //TestContext.WriteLine(ar.ToString());
            Assert.IsTrue(ar.ToString("E") == "-6.8718E-4");
            Assert.IsTrue(ar.ToString("D") == "0");
            Assert.IsTrue(ar.ToString("C") == "-0.00068718");
            ar = ArNumber.Parse(testStrings[3]);
            Assert.IsTrue(ar.ToString("E") == "3.567894358E+40");
            Assert.IsTrue(ar.ToString("D") == "35678943580000000000000000000000000000000");

            ar = ArNumber.Parse(testStrings[4]);
            Assert.IsTrue(ar.ToString("E") == "5.68681E-6");
            Assert.IsTrue(ar.ToString("C") == "0.00000568681");
            //TestContext.WriteLine(ar.ToString("C3"));
            Assert.IsTrue(ar.ToString("C3") == "0.00000568681"); // TO DO
            //Assert.IsTrue(sn.ToString("C3") == "0.00");            
            Assert.IsTrue(ar.ToString("D") == "0");


            ar = ArNumber.Parse(testStrings[5]);
            Assert.IsTrue(ar.ToString("E") == "-3.5E-24");
            ar = ArNumber.Parse(testStrings[6]);
            Assert.IsTrue(ar.ToString("E") == "0");
            Assert.IsTrue(ar.ToString("C") == "0");
            ar = ArNumber.Parse(testStrings[7]);
            Assert.IsTrue(ar.ToString("E") == "0");
            Assert.IsTrue(ar.ToString("C") == "0");
            ar = ArNumber.Parse(testStrings[8]);
            Assert.IsTrue(ar.ToString("E") == "3E+2");
            Assert.IsTrue(ar.ToString() == "300");
            ar = ArNumber.Parse(testStrings[9]);
            Assert.IsTrue(ar.ToString("E") == "-8E-3");
            Assert.IsTrue(ar.ToString() == "-0.008");
            ar = ArNumber.Parse(testStrings[10]);
            Assert.IsTrue(ar.ToString("E") == "6.81E+95");
            ar = ArNumber.Parse(testStrings[11]);
            Assert.IsTrue(ar.ToString("E") == "1E-2");
            ar = ArNumber.Parse(testStrings[12]);
            Assert.IsTrue(ar.ToString("E") == "0");
            ar = ArNumber.Parse(testStrings[13]);
            //TestContext.WriteLine(ar.ToString());

            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                double d = cb.DrawOutDiversityDouble();
                string s = d.ToString().Replace("E+0", "E+").Replace("E-0", "E-");
                ar = new ArNumber(d);
                TestContext.WriteLine(ar.ToString().Length.ToString());
                TestContext.WriteLine(ar.ToString());
                if (s != ar.ToString())
                    TestContext.WriteLine($"{s}: {ar}");
            }
        }

        [TestMethod]
        public void NewArNumber()
        {
            ArNumber a = new ArNumber((sbyte)-3);
            TestContext.WriteLine(a.ToString());
            a = new ArNumber((sbyte)20);
            TestContext.WriteLine(a.ToString());
            a = new ArNumber((sbyte)100);
            TestContext.WriteLine(a.ToString());
            a = new ArNumber((sbyte)-100);
            TestContext.WriteLine(a.ToString());
            a = new ArNumber((sbyte)-87);
            TestContext.WriteLine(a.ToString());
            a = 20;
            TestContext.WriteLine(a.ToString());
            a = 56;
            TestContext.WriteLine(a.ToString());
            a = 127;
            TestContext.WriteLine(a.ToString());
            a = -13;
            TestContext.WriteLine(a.ToString());
            ArNumber b = new ArNumber(50.7);
            b.Negative = true;

            ArNumber c = b;
            TestContext.WriteLine(c.ToString());
        }

        //[TestMethod]
        //public void CastTest()
        //{
        //    byte b = 3;
        //    ArNumber ar = new ArNumber(3);
        //    ArNumber br = ar.Clone() as ArNumber;
        //    br.Negative = true;
        //    TestContext.WriteLine(ar.ToString());
        //    TestContext.WriteLine(br.ToString());
        //    sbyte c = (sbyte)ar;
        //    TestContext.WriteLine(c.ToString());
        //    br = 200;
        //    Assert.ThrowsException<OverflowException>(() =>
        //    {
        //        sbyte d = (sbyte)br;
        //    });
        //    //TestContext.WriteLine(d.ToString());

        //    byte a1 = (byte)br;
        //    short a2 = (short)br;
        //    ushort a3 = (ushort)br;
        //    int a4 = (int)br;
        //    uint a5 = (uint)br;
        //    long a6 = (long)br;
        //    ulong a7 = (ulong)br;
        //    decimal a8 = (decimal)br;            
        //    float a9 = (float)br;            
        //    double a11 = (double)br;
        //    char a10 = (char)br;

        //    Assert.ThrowsException<OverflowException>(() =>
        //    {
        //        br = -60;
        //        byte aa1 = (byte)br;
        //    });
        //}


    }
}
