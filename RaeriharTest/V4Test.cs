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
            
            Console.WriteLine(a + b);
            Console.WriteLine(a - b);

            ArNumberByte ab = new ArNumberByte(a);
            ArNumberByte ab2 = new ArNumberByte(b);
            Console.WriteLine( ab.Add(ab2));
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