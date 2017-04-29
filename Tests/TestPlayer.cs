using Microsoft.VisualStudio.TestTools.UnitTesting;
//https://msdn.microsoft.com/en-us/library/ms182532.aspx

namespace Anti_Latency
{
    [TestClass]
    public class TestPlayer
    {
        [TestMethod]
        public void canPlayerMoveLeft()
        {
            Player play = new Player(10, 10);
            play.setX(9);

            int expected = 9;
            int result = play.getX();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void canPlayerMoveRight()
        {
            Player play = new Player(10, 10);
            play.setX(11);

            int expected = 11;
            int result = play.getX();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void canPlayerMoveUp()
        {
            Player play = new Player(10, 10);
            play.setY(9);

            int expected = 9;
            int result = play.getY();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void canPlayerMoveDown()
        {
            Player play = new Player(10, 10);
            play.setY(11);

            int expected = 11;
            int result = play.getY();
            Assert.AreEqual(expected, result);
        }
    }
}
