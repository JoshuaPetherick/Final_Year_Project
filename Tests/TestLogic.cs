using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestLogic
    {
        [TestMethod]
        public void doesAABBWorkPart1()
        {
            Player player1 = new Player(10, 10);
            Player player2 = new Player(10, 10);

            int expected = 4;
            int result = Logic.axisAlignedBoundingBox(player1.getX(), player1.getY(), player1.getHeight(), player1.getWidth(),
                player2.getX(), player2.getY(), player2.getHeight(), player2.getWidth());
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesAABBWorkPart2()
        {
            Player player1 = new Player(100, 100);
            Player player2 = new Player(10, 10);

            int expected = 0;
            int result = Logic.axisAlignedBoundingBox(player1.getX(), player1.getY(), player1.getHeight(), player1.getWidth(),
                player2.getX(), player2.getY(), player2.getHeight(), player2.getWidth());
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesActionsWork()
        {
            Player player = new Player(10, 10);
            World world = new World();
            Tuple<int, int> pos = Logic.actionTree(player, world, "1");

            player.setX(pos.Item1);
            player.setY(pos.Item2);

            int expected = 8;
            int result = player.getX();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void canPlayerJump()
        {
            World world = new World();
            Player play = new Player(10, 500);
            play.state = Player.playerStates.JUMPING;
            for (int i = 0; i < 3; i++)
            {
                Tuple<int, int> pos = Logic.update(play, new Tuple<int, int>(play.getX(), play.getY()), world);
                play.setX(pos.Item1);
                play.setY(pos.Item2);
            }

            int expected = 494;
            int result = play.getY();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesPlayerFall()
        {
            World world = new World();
            Player play = new Player(10, 10);
            for (int i = 0; i < 3; i++)
            {
                Tuple<int, int> pos = Logic.update(play, new Tuple<int, int>(play.getX(), play.getY()), world);
                play.setX(pos.Item1);
                play.setY(pos.Item2);
            }

            int expected = 13;
            int result = play.getY();
            Assert.AreEqual(expected, result);
        }
    }
}
