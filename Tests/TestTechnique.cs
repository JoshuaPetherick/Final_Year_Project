using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FinalYearProject.Tests
{
    [TestClass]
    public class TestTechnique
    {
        [TestMethod]
        public void doesClientSidePredictionUpdate()
        {
            World world = new World();
            Player player = new Player(10, 10);
            Technique technique = new ClientSidePrediction();
            technique.update(new Client(true), player, world, "7");

            string expected = "7";
            string result = technique.getLastAction();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesClientSidePredictionProcess()
        {
            World world = new World();
            Player player = new Player(0, 0);
            Technique technique = new ClientSidePrediction();
            Client clnt = new Client(true);

            technique.update(clnt, player, world, "1");
            Thread.Sleep(1000); // Wait for packet to be recieved

            Tuple<int, int> expected = new Tuple<int, int>(0, 1);
            Tuple<int, int> result = technique.process(clnt, world);
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesServerReconcilliationUpdate()
        {
            World world = new World();
            Player player = new Player(10, 10);
            Technique technique = new ServerReconcilliation();
            technique.update(new Client(true), player, world, "7");

            string expected = "7";
            string result = technique.getLastAction();
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void doesServerReconcilliationProcess()
        {
            World world = new World();
            Player player = new Player(0, 0);
            Technique technique = new ServerReconcilliation();
            Client clnt = new Client(true);

            technique.update(clnt, player, world, "1");
            Thread.Sleep(1000); // Wait for packet to be recieved

            Tuple<int, int> expected = null; // Should match and therefore return nothing
            Tuple<int, int> result = technique.process(clnt, world);
            Assert.AreEqual(expected, result);
        }
    }
}
