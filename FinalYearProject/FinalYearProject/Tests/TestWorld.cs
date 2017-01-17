using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestWorld
    {
        [TestMethod]
        public void doesAABBWork()
        {
            World world = new World(1);
            Player player1 = new Player(10, 10);
            Player player2 = new Player(10, 10);

            bool expected = true;
            bool result = world.axisAlignedBoundingBox(player1.getX(), player1.getY(), player1.getHeight(), player1.getWidth(),
                player2.getX(), player2.getY(), player2.getHeight(), player2.getWidth());
            Assert.AreEqual(expected, result);
        }
    }
}
