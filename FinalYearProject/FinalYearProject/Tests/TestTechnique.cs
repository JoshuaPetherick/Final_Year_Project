using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestTechnique
    {
        [TestMethod]
        public void doesClientSidePredictionUpdateRun()
        {
            Player player = new Player(10, 10);
            World world = new World(1);
            Technique technique = new ClientSidePrediction();
            technique.update(new Client("127.0.0.1", 14242), player, world, "7");

            string expected = "7";
            string result = technique.getLastAction();
            Assert.AreEqual(expected, result);
        }
    }
}
