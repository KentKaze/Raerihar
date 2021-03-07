using Microsoft.VisualStudio.TestTools.UnitTesting;
using Aritiafel.Characters.Heroes;
using Aritiafel.Locations;

namespace RaeriharTest
{
    [TestClass]
    public class Backup
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void BackupTest()
        {
            string sourceDir = @"C:\Programs\Standard\Raerihar";
            string targetDir = @"E:\Backup";

            Residence rs = new Residence(targetDir);
            rs.SaveVSSolution(sourceDir, false);

            //Tina.SaveProject(ProjectChoice.Aritiafel);
        }
    }
}
