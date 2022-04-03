using System.Collections.Generic;
using System.Linq;
using core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace core_test {

    [TestClass]
    public class GraphTest {
        [TestMethod]
        public void HasCircleTest() {
            var graph = new Graph();
            Assert.IsFalse(graph.HasCircle());
            for (var i = 1; i <= 5; i++) {
                graph.AddNode(i, 1);
                Assert.IsFalse(graph.HasCircle());
            }

            for (var i = 1; i <= 4; i++) {
                graph.AddEdge(i, i + 1);
                Assert.IsFalse(graph.HasCircle());
            }

            graph.AddEdge(4, 2);
            Assert.IsTrue(graph.HasCircle());
        }

        [TestMethod]
        public void DagFindLongestChainTest1() {
            var graph = new Graph();
            bool HeadLimit(int i) => i == 5 || i == 4;
            bool TailLimit(int i) => i == 1;
            for (var i = 1; i <= 6; i++) {
                graph.AddNode(i, i);
            }

            graph.AddEdge(5, 1);
            graph.AddEdge(2, 4);
            graph.AddEdge(4, 3);
            graph.AddEdge(6, 1);
            Assert.IsFalse(graph.HasCircle());
            var chainWithNoLimit = graph.DagFindLongestChain(_ => true, _ => true);
            Assert.IsNotNull(chainWithNoLimit);
            Assert.IsTrue(chainWithNoLimit.SequenceEqual(new List<int>(new[] {2, 4, 3})));
            var chainWithHeadLimit = graph.DagFindLongestChain(HeadLimit, _ => true);
            Assert.IsNotNull(chainWithHeadLimit);
            Assert.IsTrue(chainWithHeadLimit.SequenceEqual(new List<int>(new[] {4, 3})));
            var chainWithTailLimit = graph.DagFindLongestChain(_ => true, TailLimit);
            Assert.IsNotNull(chainWithTailLimit);
            Assert.IsTrue(chainWithTailLimit.SequenceEqual(new List<int>(new[] {6, 1})));
            var chainWithHeadAndTailLimit = graph.DagFindLongestChain(HeadLimit, TailLimit);
            Assert.IsNotNull(chainWithHeadAndTailLimit);
            Assert.IsTrue(chainWithHeadAndTailLimit.SequenceEqual(new List<int>(new[] {5, 1})));
        }

        [TestMethod]
        public void DagFindLongestChainTest2() {
            var graph = new Graph();
            for (var i = 1; i <= 6; i++) {
                graph.AddNode(i, i == 5 ? 100 : 1);
            }

            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(3, 6);
            graph.AddEdge(1, 5);
            graph.AddEdge(5, 6);
            var longestChain = graph.DagFindLongestChain(_ => true, _ => true);
            Assert.IsNotNull(longestChain);
            Assert.IsTrue(longestChain.SequenceEqual(new List<int>(new[] {1, 5, 6})));
        }

        [TestMethod]
        public void FindLongestChainRecursiveTest1() {
            var graph = new Graph();
            for (var i = 1; i <= 4; i++) {
                graph.AddNode(i, 1);
            }

            for (var i = 1; i <= 4; i++) {
                graph.AddEdge(i, i % 4 + 1);
            }

            var longestChain1 = graph.FindLongestChainRecursive(i => i == 1, _ => true);
            Assert.IsTrue(longestChain1.SequenceEqual(new List<int>(new[] {1, 2, 3, 4})));

            var longestChain2 = graph.FindLongestChainRecursive(_ => true, i => i == 4);
            Assert.IsTrue(longestChain2.SequenceEqual(new List<int>(new[] {1, 2, 3, 4})));
        }

        [TestMethod]
        public void FindLongestChainRecursiveTest2() {
            var graph = new Graph();
            for (var i = 1; i <= 5; i++) {
                graph.AddNode(i, 1);
            }

            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 1);
            graph.AddEdge(1, 5);
            graph.AddEdge(5, 4);
            var longestChain = graph.FindLongestChainRecursive(_ => true, _ => true);
            Assert.IsTrue(
                longestChain.SequenceEqual(new List<int>(new[] {5, 4, 1, 2, 3})) ||
                longestChain.SequenceEqual(new List<int>(new[] {2, 3, 4, 1, 5}))
            );
        }

        [TestMethod]
        public void FindAllChainsTest() {
            var graph = new Graph();
            for (var i = 1; i <= 5; i++) {
                graph.AddNode(i, 1);
            }

            for (var i = 1; i < 5; i++) {
                graph.AddEdge(i, i + 1);
            }

            Assert.IsFalse(graph.HasCircle());
            var allChains = graph.FindAllChains();
            Assert.IsNotNull(allChains);
            Assert.AreEqual(allChains.Count, 10);
            graph.AddEdge(2, 4);
            allChains = graph.FindAllChains();
            Assert.IsNotNull(allChains);
            Assert.AreEqual(allChains.Count, 14);
        }
    }
}
