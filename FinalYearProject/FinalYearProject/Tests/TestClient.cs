﻿using System.Threading;
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
            World world =  new World(1);
            Server serv = new Server(14242, world); // Need to make server first

            Client clint = new Client("127.0.0.1", 14242);
            Thread.Sleep(1000); // Wait for packet to be recieved

            clint.getMessages(world);
            int expected = 5;
            int result = clint.getStatus();

            serv.closeServer();
            Assert.AreEqual(expected, result);
        }
    }
}
