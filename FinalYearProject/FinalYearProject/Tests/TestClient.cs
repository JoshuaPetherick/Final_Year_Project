using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//https://msdn.microsoft.com/en-us/library/ms182532.aspx

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestClient
    {
        [TestMethod]
        public void canClientConnect()
        {
            Server serv = new Server(14242); // Need to make server first
            Client clint = new Client("127.0.0.1", 14242);
            Thread.Sleep(1000); // Wait for packet to be recieved

            clint.getMessages();
            int expected = 5;

            serv.closeServer();
            Assert.AreEqual(expected, clint.getStatus());
        }
    }
}
