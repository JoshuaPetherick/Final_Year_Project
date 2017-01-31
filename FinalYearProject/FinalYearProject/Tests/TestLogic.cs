using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestLogic
    {
        [TestMethod]
        public void doesAABBWork()
        {
            Player player1 = new Player(10, 10);
            Player player2 = new Player(10, 10);

            bool expected = true;
            bool result = Logic.axisAlignedBoundingBox(player1.getX(), player1.getY(), player1.getHeight(), player1.getWidth(),
                player2.getX(), player2.getY(), player2.getHeight(), player2.getWidth());
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesActionsWork()
        {
            Player player = new Player(10, 10);

            Tuple<int, int> pos = Logic.actionTree(new Tuple<int, int>(player.getX(), player.getY()), "1");
            player.setX(pos.Item1);
            player.setY(pos.Item2);

            int expected = 9;
            int result = player.getX();
            Assert.AreEqual(expected, result);
        }
    }
}
