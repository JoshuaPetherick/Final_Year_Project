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
            technique.update(7);

            int expected = 7;
            int result = technique.getLastAction();
            Assert.AreEqual(expected, result);
        }
    }
}
