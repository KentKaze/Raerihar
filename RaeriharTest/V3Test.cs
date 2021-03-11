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

            //10      34
            //11      37
            //12      40
            //13      44
            //14      47
            //15      50
            //16      54
            //17      57
            //18      60

            //19      64

            //digits = (bits + 2) * 4 / 15
            //for (int i = 1; i < 100; i++)
            //{
            //    TestContext.WriteLine($"{i}\t{(i * 10 + 2) / 3}");
            //}
            
        }
    }
}