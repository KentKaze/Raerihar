using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aritiafel.Organizations.RaeriharUniversity;
using System;
using Aritiafel.Artifacts;

namespace RaeriharTest
{
    [TestClass]
    public class V3Test
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void Start()
        {
            ArNumber ar = new ArNumber();
            ar.SetExponent(60);
            TestContext.WriteLine(ar.Exponent.ToString());
            ar.SetExponent(-300);
            TestContext.WriteLine(ar.Exponent.ToString());
        }

        [TestMethod]
        public void GetSetTest()
        {
            ArNumber ar = new ArNumber();
            ar.SetNumberBlock(0, 1);            
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 2);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 3);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 9);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 10);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 11);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 12);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 60);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 98);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 99);
            TestContext.WriteLine(ar.GetNumber().ToString());            
            ar.SetNumberBlock(0, 100);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 101);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 102);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 103);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 199);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 200);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 999);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 1000);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 1999);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 2999);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 9999);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 10000);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 20000);
            TestContext.WriteLine(ar.GetNumber().ToString());
            ar.SetNumberBlock(0, 99999);
            TestContext.WriteLine(ar.GetNumber().ToString());
            //ar.SetNumberBlock(0, -1);
            //TestContext.WriteLine(ar.GetNumber().ToString());
            //ar.SetNumberBlock(0, -9);
            //TestContext.WriteLine(ar.GetNumber().ToString());
            //ar.SetNumberBlock(0, -100);
            //TestContext.WriteLine(ar.GetNumber().ToString());
            //ar.SetNumberBlock(0, -999);
            //TestContext.WriteLine(ar.GetNumber().ToString());
            return;
            ar.SetNumberBlock(1, 4299);
            TestContext.WriteLine(ar.GetNumberBlock(1).ToString());

            //TestContext.WriteLine(ar.GetNumberBlock(0));
        }

        [TestMethod]
        public void V3TestTest()
        {
            //0       0           0

            //1       4           1
            //2       7           1
            //3       10          2
            //4       14          2
            //5       17          3
            //6       20          3
            //7       24          3
            //8       27          4
            //9       30          4

            //10      34          5
            //11      37          5
            //12      40          5
            //13      44          6
            //14      47          6
            //15      50          7
            //16      54          7
            //17      57          8
            //18      60          8

            //19      64          8
            //20      67          9

            //digits = (bits + 2) * 4 / 15
            for (int i = 1; i < 100; i++)
            {
                int j = (i * 10 + 2) / 3;
                TestContext.WriteLine($"{i}\t{j}\t{ j / 4}");
            }

        }
    }
}