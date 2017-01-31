using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestTechnique
    {
        [TestMethod]
        public void doesClientSidePredictionUpdateRun()
        {
            Technique technique = new ClientSidePrediction();
            technique.update("7", new Client("127.0.0.1", 14242));

            string expected = "7";
            string result = technique.getLastAction();
            Assert.AreEqual(expected, result);
        }
    }
}
