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
        public void BackupMethod()
        {
            Tina.SaveProject(ProjectChoice.RaeriharUniversity);            
        }
    }
}
