using Microsoft.VisualStudio.TestTools.UnitTesting;
//https://msdn.microsoft.com/en-us/library/ms182532.aspx

namespace FinalYearProject
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

        [TestMethod]
        public void canPlayerJump()
        {
            World world = new World(1);
            Player play = new Player(10, 500);
            play.state = Player.playerStates.JUMPING;
            for (int i = 0; i < 3; i++)
            {
                play.playerUpdate(world);
            }

            int expected = 494;
            int result = play.getY();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesPlayerFall()
        {
            World world = new World(1);
            Player play = new Player(10, 10);
            for (int i = 0; i < 3; i++)
            {
                play.playerUpdate(world);
            }

            int expected = 13;
            int result = play.getY();
            Assert.AreEqual(expected, result);
        }
    }
}
