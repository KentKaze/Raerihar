using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aritiafel.Organizations.RaeriharUniversity;
using System;
using Aritiafel.Artifacts;

namespace RaeriharTest
{
    [TestClass]
    public class V4Test
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Start()
        {
            //ArNumber ar = new ArNumber();

            byte a = 20;
            byte b = 30;
            long l = long.MaxValue;
            Console.WriteLine(a + b);
            Console.WriteLine(a - b);

            ArNumberByte ab = new ArNumberByte(a);
            ArNumberByte ab2 = new ArNumberByte(b);
            Console.WriteLine(ab.Add(ab2));

            Console.WriteLine((decimal)l + a);
        }

        [TestMethod]
        public void SNTest()
        {
            ArNumberScientificNotation ar = new ArNumberScientificNotation();
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
                    "2170.6907744747728", //13
                    "-0.445064895809216518E+18", //14
                    "-1.6523221914791807E-283", //15
                    "32.135752223080374" //16
                };

            ar = ArNumberScientificNotation.Parse(testStrings[0]);
            //TestContext.WriteLine(ar.ToString("E"));            
            Assert.IsTrue(ar.ToString("E") == "-5.89662145E-106");
            ar = ArNumberScientificNotation.Parse(testStrings[1]);            
            Assert.IsTrue(ar.ToString("E") == "-3.611895678E+49");
            Assert.IsTrue(ar.ToString("D") == "-36118956780000000000000000000000000000000000000000");
            ar = ArNumberScientificNotation.Parse(testStrings[2]);
            //TestContext.WriteLine(ar.ToString("E"));            
            Assert.IsTrue(ar.ToString("E") == "-6.8718E-4");
            Assert.IsTrue(ar.ToString("D") == "0");
            Assert.IsTrue(ar.ToString("F") == "-0.00068718");
            ar = ArNumberScientificNotation.Parse(testStrings[3]);            
            Assert.IsTrue(ar.ToString("E") == "3.567894358E+40");
            Assert.IsTrue(ar.ToString("D") == "35678943580000000000000000000000000000000");
            ar = ArNumberScientificNotation.Parse(testStrings[4]);            
            Assert.IsTrue(ar.ToString("E") == "5.68681E-6");
            Assert.IsTrue(ar.ToString("F") == "0.00000568681");
            //TestContext.WriteLine(ar.ToString("C3"));            
            //TestContext.WriteLine(ar.ToString("F3"));
            Assert.IsTrue(ar.ToString("F3") == "0.000");
            Assert.IsTrue(ar.ToString("D") == "0");            
            Assert.IsTrue(ar.ToString("D3") == "000");
            ar = ArNumberScientificNotation.Parse(testStrings[5]);
            Assert.IsTrue(ar.ToString("E") == "-3.5E-24");            
            ar = ArNumberScientificNotation.Parse(testStrings[6]);            
            Assert.IsTrue(ar.ToString("E") == "0");
            Assert.IsTrue(ar.ToString("F") == "0");
            ar = ArNumberScientificNotation.Parse(testStrings[7]);
            Assert.IsTrue(ar.ToString("E") == "0");
            Assert.IsTrue(ar.ToString("F") == "0");
            ar = ArNumberScientificNotation.Parse(testStrings[8]);
            //TestContext.WriteLine(ar.ToString("E"));
            Assert.IsTrue(ar.ToString("E") == "3E+2");            
            Assert.IsTrue(ar.ToString() == "300");            
            Assert.IsTrue(ar.ToString("D5") == "00300");
            ar = ArNumberScientificNotation.Parse(testStrings[9]);
            Assert.IsTrue(ar.ToString("E") == "-8E-3");
            Assert.IsTrue(ar.ToString() == "-0.008");            
            ar = ArNumberScientificNotation.Parse(testStrings[10]);            
            Assert.IsTrue(ar.ToString("E") == "6.81E+95");
            ar = ArNumberScientificNotation.Parse(testStrings[11]);            
            Assert.IsTrue(ar.ToString("E") == "1E-2");
            ar = ArNumberScientificNotation.Parse(testStrings[12]);            
            Assert.IsTrue(ar.ToString("E") == "0");
            ar = ArNumberScientificNotation.Parse(testStrings[13]);
            Assert.IsTrue(ar.ToString("F") == "2170.6907744747728");            
            ar = ArNumberScientificNotation.Parse(testStrings[14]);
            Assert.IsTrue(ar.ToString("E") == "-4.45064895809216518E+17");
            Assert.IsTrue(ar.ToString("F") == "-445064895809216518");
            Assert.IsTrue(ar.ToString("D7") == "-445064895809216518");
            ar = ArNumberScientificNotation.Parse(testStrings[15]);
            Assert.IsTrue(ar.ToString("E") == "-1.6523221914791807E-283");            
            ar = ArNumberScientificNotation.Parse(testStrings[16]);            
            //TestContext.WriteLine(ar.ToString());
            //3.2135752223080374E+1
            Assert.IsTrue(ar.ToString() == "32.135752223080374");
            Assert.IsTrue(ar.ToString("F") == "32.135752223080374");
            Assert.IsTrue(ar.ToString("D") == "32");

            ChaosBox cb = new ChaosBox();

            for (int i = 0; i < 10000; i++)
            {
                double d = cb.DrawOutDiversityDouble();
                string s = d.ToString().Replace("E+0", "E+").Replace("E-0", "E-");
                ar = new ArNumberScientificNotation(d);
                //TestContext.WriteLine(ar.ToString().Length.ToString());
                //TestContext.WriteLine(ar.ToString());
                if (s != ar.ToString() && s != ar.ToString("E") && s != ar.ToString("F"))
                    TestContext.WriteLine($"{s}: {ar}");
            }
        }
    

        [TestMethod]
        public void DecimalTest()
        {
            ArNumberDecimal and = new ArNumberDecimal(6.6);
            TestContext.WriteLine(and.ToString());
            and = new ArNumberDecimal(900006);
            TestContext.WriteLine(and.ToString());
            and = new ArNumberDecimal(0.00054984);
            TestContext.WriteLine(and.ToString());
            
            Assert.ThrowsException<FormatException>(() => {
                and = new ArNumberDecimal(0.0000000009899898899988);
                and.ToString();
            });
            //TestContext.WriteLine(and.ToString());
        }

        [TestMethod]
        public void GeneralUse()
        {
            ArNumberScientificNotation a;

            //a = 0.1;
            //Console.WriteLine(a);
            //Console.WriteLine(a.DigitsCount);
            //a = 1.1;
            //Console.WriteLine(a.DigitsCount);
            //a = (ArNumberScientificNotation)a + 0.1;
            //Console.WriteLine(a.DigitsCount);
            //ArNumber plus = 0.1;
            //ArNumber i;
            //for (i = 100; i < 10000; i = i + plus)
            //{
            //    Console.WriteLine(i);
            //}

            //for (i = 0; i < 100; i++)
            //    ;

            //ArNumber a = 3.87699078E+98;
            //ArNumber test = a - a;
            //Console.WriteLine(test);

        }

        //[TestMethod]
        //public void GetSetTest()
        //{
        ////    TestContext.WriteLine(GC.GetTotalMemory(true).ToString());
        ////    decimal i = 33015646413218948;
        ////    TestContext.WriteLine(GC.GetTotalMemory(true).ToString());
        //    ArNumber ar = ArNumber.Parse("1894894654156489498464489498456419848948961561489489465464894");
        ////    TestContext.WriteLine(GC.GetTotalMemory(true).ToString());
        ////    ArNumber ar2 = ArNumber.Parse("5446946548648948948435489489464519848948961561489489465464894");
        ////    TestContext.WriteLine(GC.GetTotalMemory(true).ToString());

        //    ar.SetNumberBlock(0, 1);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());

        //    ar.SetNumberBlock(0, 2);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 3);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 9);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 10);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 11);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 12);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 60);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 98);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 99);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 100);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 101);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 102);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 103);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 199);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 200);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 999);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 1000);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 1999);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 2999);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 9999);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 10000);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 20000);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(0, 99999);
        //    TestContext.WriteLine(ar.GetNumberBlock(0).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 199);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 200);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 999);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 1000);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 10000);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 100000);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());
        //    ar.SetNumberBlock(1, 1000000);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());
        //    TestContext.WriteLine(ar.DigitsCount.ToString());


        //    //ar.SetNumberBlock(0, -1);
        //    //TestContext.WriteLine(ar.GetNumber().ToString());
        //    //ar.SetNumberBlock(0, -9);
        //    //TestContext.WriteLine(ar.GetNumber().ToString());
        //    //ar.SetNumberBlock(0, -100);
        //    //TestContext.WriteLine(ar.GetNumber().ToString());
        //    //ar.SetNumberBlock(0, -999);
        //    //TestContext.WriteLine(ar.GetNumber().ToString());            
        //    ar.SetNumberBlock(1, 4299);
        //    TestContext.WriteLine(ar.GetNumberBlock(1).ToString());

        //    //TestContext.WriteLine(ar.GetNumberBlock(0));
        //}


    }
}