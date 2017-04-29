using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Anti_Latency
{
    [TestClass]
    public class TestButton
    {
        [TestMethod]
        public void canMakeButton()
        {
            Button testButton = new Button(0, 0, "TEST");
            testButton.buttonClicked(true);
            Assert.AreEqual(testButton.getResult(), true);
        }

        [TestMethod]
        public void canClickButton()
        {
            Button testButton = new Button(0, 0, "TEST");
            if (Logic.axisAlignedBoundingBox(0,0,1,1,
                testButton.getX(), testButton.getY(), testButton.getHeight(), testButton.getWidth()) > 0)
            {
                testButton.buttonClicked(true);
            }
            else
            {
                testButton.buttonClicked(false);
            }
            Assert.AreEqual(testButton.getResult(), true);
        }
    }
}
