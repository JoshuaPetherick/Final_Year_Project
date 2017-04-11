using Microsoft.VisualStudio.TestTools.UnitTesting;
//https://msdn.microsoft.com/en-us/library/ms182532.aspx

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestServer
    {
        [TestMethod]
        public void canCreateServer()
        {
            World world = new World();
            Server serv = new Server(14242, world);
            int expected = 2; // 2 means up and running
            int result = serv.serverStatus();

            serv.closeServer();
            Assert.AreEqual(expected, result);
        }
    }
}
