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
        public void AddMinus()
        {
            ArNumberScientificNotation ar1 = 3;
            ArNumberScientificNotation ar2 = 5;
            ArNumberScientificNotation ar3 = (ArNumberScientificNotation)(ar1 + ar2);

            if (ArNumberScientificNotation.Negate(ar1) < ArNumberScientificNotation.Negate(ar2))
                Console.WriteLine("!!?");
            Assert.IsTrue(ar3.ToString() == "8");
            ar3 = (ArNumberScientificNotation)(ar1 - ar2);
            Assert.IsTrue(ar3.ToString() == "-2");
            ar1 = 300;
            ar2 = 25;
            ar3 = (ArNumberScientificNotation)(ar1 + ar2);
            Assert.IsTrue(ar3.ToString() == "325");
            ar3 = (ArNumberScientificNotation)(ar1 - ar2);
            Assert.IsTrue(ar3.ToString() == "275");

            //To Do
            ar1 = 870000000000;
            ar2 = 850;
            ar3 = (ArNumberScientificNotation)(ar1 + ar2);
            Assert.IsTrue(ar3.ToString() == "870000000850");            
            ar3 = (ArNumberScientificNotation)(ar1 - ar2);
            Assert.IsTrue(ar3.ToString() == "869999999150");
            

            ar1 = 1917854895357220849;
            ar2 = 8593185756644802792;
            ar3 = (ArNumberScientificNotation)(ar1 + ar2);
            Assert.IsTrue(ar3.ToString("D") == "10511040652002023641");
            //TestContext.WriteLine(ar3.ToString("D"));
            ar3 = (ArNumberScientificNotation)(ar1 - ar2);
            Assert.IsTrue(ar3.ToString("D") == "-6675330861287581943");
            //TestContext.WriteLine(ar3.ToString("D"));


            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                long a = cb.DrawOutLong();
                long b = cb.DrawOutLong();
                decimal m = (decimal)a + (decimal)b;
                decimal m2 = (decimal)a - (decimal)b;
                ar1 = a;
                ar2 = b;
                ar3 = (ArNumberScientificNotation)(ar1 + ar2);

                if (ar1.ToString("D") != a.ToString() || ar2.ToString("D") != b.ToString() ||
                   ar3.ToString("D") != m.ToString())
                {
                    TestContext.WriteLine("Wrong Detected");
                    TestContext.WriteLine($"{a}+{b}={m}");
                    TestContext.WriteLine($"{ar1.ToString("D")}+{ar2.ToString("D")}={ar3.ToString("D")}");
                }
                ar3 = (ArNumberScientificNotation)(ar1 - ar2);
                if (ar3.ToString("D") != m2.ToString())
                {
                    TestContext.WriteLine("Wrong Detected");
                    TestContext.WriteLine($"{m2}");
                    TestContext.WriteLine($"{ar3.ToString("D")}");
                }
            }
        }

        [TestMethod]
        public void AddMinus2()
        {
            ArNumberScientificNotation ar1;
            ArNumberScientificNotation ar2;
            ArNumberScientificNotation ar3;
            ar1 = 10.88483022032963E+307;
            ar2 = 2.3934165709915166E+307;
            //+  = 1.71705924567000363134E+308
            //= 1.32782467913211466E+308;
            //34818995930244796
            ar3 = (ArNumberScientificNotation)(ar1 + ar2);
            TestContext.WriteLine(ar3.ToString());

            ar1 = 90;
            ar2 = 90.5;
            ar3 = (ArNumberScientificNotation)(ar1 + ar2);
            Assert.IsTrue(ar3.ToString() == "180.5");
            //TestContext.WriteLine(ar3.ToString());
            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 1000; i++)
            {
                double d1 = cb.DrawOutDouble(true);
                double d2 = cb.DrawOutDouble(true);
                double d3 = d1 + d2;
                ar1 = d1;
                ar2 = d2;
                ar3 = (ArNumberScientificNotation)(ar1 + ar2);
                if (!double.IsInfinity(d3) && d3.ToString().Substring(0, 17) != ar3.ToString().Substring(0, 17))
                {
                    TestContext.WriteLine("");
                    TestContext.WriteLine($"{d3.ToString().Substring(0, 17)}!={ar3.ToString().Substring(0, 17)}");
                    //TestContext.WriteLine($"{d1.ToString("G17")}+{d2.ToString("G17")}={ar3}");
                }
                else
                    TestContext.Write("O");

            }

        }

        [TestMethod]
        public void Multiply()
        {
            ArNumberScientificNotation ar1;
            ArNumberScientificNotation ar2;
            ArNumberScientificNotation ar3;

            ar1 = 20;
            ar2 = 30;
            ar3 = (ArNumberScientificNotation)(ar1 * ar2);
            Assert.IsTrue(ar3.ToString() == "600");

            ar1 = 0.2;
            ar2 = 30;
            ar3 = (ArNumberScientificNotation)(ar1 * ar2);
            Assert.IsTrue(ar3.ToString() == "6");

            ar1 = 1.2;
            ar2 = 3.5;
            ar3 = (ArNumberScientificNotation)(ar1 * ar2);
            Assert.IsTrue(ar3.ToString() == "4.2");

            ar1 = 0.000000002;
            ar2 = 500;
            ar3 = (ArNumberScientificNotation)(ar1 * ar2);
            Assert.IsTrue(ar3.ToString() == "0.000001");

            ChaosBox cb = new ChaosBox();
            for (int i = 0; i < 10000; i++)
            {
                int a = cb.DrawOutInteger(true);
                int b = cb.DrawOutInteger(true);
                long c = (long)a * b;
                ar1 = a;
                ar2 = b;
                ar3 = (ArNumberScientificNotation)(ar1 * ar2);
                if (c.ToString() != ar3.ToString())
                {
                    TestContext.WriteLine("WrongDetected:");
                    TestContext.WriteLine($"{a} * {b} = {c}");
                    TestContext.WriteLine($"{ar3}");
                    //TestContext.WriteLine($"{d1.ToString("G17")}+{d2.ToString("G17")}={ar3}");
                } 
            }

        }

        [TestMethod]
        public void GeneralUse()
        {
            ArNumberScientificNotation a;
            //a++;
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