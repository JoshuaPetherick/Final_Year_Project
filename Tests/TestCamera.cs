using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//https://msdn.microsoft.com/en-us/library/ms182532.aspx

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestCamera
    {
        private Viewport viewport = new Viewport();

        [TestMethod]
        public void doesCameraMove()
        {
            Camera camera = new Camera(viewport);
            camera.update(1, 0, 1f);
            Vector2 result = camera.Position;
            Vector2 expected = new Vector2(60, 0);

            Assert.AreEqual(expected, result);
        }
    }
}
