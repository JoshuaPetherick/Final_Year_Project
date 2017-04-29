using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//https://msdn.microsoft.com/en-us/library/ms182532.aspx

namespace Anti_Latency
{
    [TestClass]
    public class TestCamera
    {
        private Viewport viewport = new Viewport();

        [TestMethod]
        public void doesCameraMove()
        {
            Camera camera = new Camera(viewport);
            World world = new World();
            world.loadLevel();
            camera.update(0, 0, 1f, world);
            Vector2 result = camera.Position;
            Vector2 expected = new Vector2(0, 0);

            Assert.AreEqual(expected, result);
        }
    }
}
