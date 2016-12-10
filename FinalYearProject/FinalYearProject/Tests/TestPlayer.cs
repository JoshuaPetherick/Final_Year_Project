﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            int expected = 9;

            play.setX(9);
            Assert.AreEqual(expected, play.getX());
        }

        [TestMethod]
        public void canPlayerMoveRight()
        {
            Player play = new Player(10, 10);
            int expected = 11;

            play.setX(11);
            Assert.AreEqual(expected, play.getX());
        }

        [TestMethod]
        public void canPlayerMoveUp()
        {
            Player play = new Player(10, 10);
            int expected = 9;

            play.setY(9);
            Assert.AreEqual(expected, play.getY());
        }

        [TestMethod]
        public void canPlayerMoveDown()
        {
            Player play = new Player(10, 10);
            int expected = 11;

            play.setY(11);
            Assert.AreEqual(expected, play.getY());
        }
    }
}
