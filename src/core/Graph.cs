using System;
using System.Collections.Generic;
using System.Linq;

namespace core {
    internal struct Edge {
        public int From, To;
    }

    public class Graph {
        private readonly Dictionary<int, List<Edge>> _edges = new Dictionary<int, List<Edge>>();
        private readonly Dictionary<int, int> _weights = new Dictionary<int, int>();
        private readonly List<int> _inDegrees = new List<int>();

        public void AddNode(int id, int weight) {
            _weights[id] = weight;
            if (id >= _inDegrees.Count) {
                _inDegrees.AddRange(new int[id - _inDegrees.Count + 1]);
            }
        }

        public void AddEdge(int from, int to) {
            AddEdge(new Edge {
                From = from, To = to
            });
        }

        private void AddEdge(Edge edge) {
            if (!_edges.ContainsKey(edge.From)) {
                _edges.Add(edge.From, new List<Edge>());
            }

            if (!_edges.ContainsKey(edge.To)) {
                _edges.Add(edge.To, new List<Edge>());
            }

            _edges[edge.From].Add(edge);
            _inDegrees[edge.To]++;
        }

        public bool HasCircle() {
            var tempInDegrees = new List<int>(_inDegrees);
            var queue = new Queue<int>(_edges.Keys.Where(x => tempInDegrees[x] == 0));
            while (queue.Count > 0) {
                var node = queue.Dequeue();
                foreach (var edge in _edges[node]) {
                    tempInDegrees[edge.To]--;
                    if (tempInDegrees[edge.To] == 0) {
                        queue.Enqueue(edge.To);
                    }
                }
            }

            return tempInDegrees.Any(x => x != 0);
        }

        private IEnumerable<int> DagTopoSort() {
            var tempInDegrees = new List<int>(_inDegrees);
            var result = new Queue<int>();
            var queue = new Queue<int>(_edges.Keys.Where(x => tempInDegrees[x] == 0));
            while (queue.Count > 0) {
                var node = queue.Dequeue();
                result.Enqueue(node);
                foreach (var edge in _edges[node]) {
                    tempInDegrees[edge.To]--;
                    if (tempInDegrees[edge.To] == 0) {
                        queue.Enqueue(edge.To);
                    }
                }
            }

            System.Diagnostics.Debug.Assert(tempInDegrees.Any(x => x == 0));
            return result;
        }

        public List<int> FindLongestChainRecursive(Func<int, bool> head, Func<int, bool> tail) {
            var headQueue = new List<int>(_edges.Keys.Where(head));
            var longestChain = new List<int>();
            var curValue = 0;
            var maxValue = 0;
            foreach (var headNode in headQueue) {
                FindLongestChainWithSourceRecursive(headNode, tail, new List<int>(), new HashSet<int>(), longestChain, ref curValue, ref maxValue);
            }

            return longestChain.Count >= 2 ? longestChain : new List<int>();
        }

        private void FindLongestChainWithSourceRecursive(int u, Func<int, bool> tail, IList<int> curChain,
            ISet<int> chainSet, List<int> longestChain, ref int curValue, ref int maxValue) {
            curChain.Add(u);
            chainSet.Add(u);
            curValue += _weights[u];

            if (tail(u) && curChain.Count >= 2 && curValue > maxValue) {
                longestChain.Clear();
                longestChain.AddRange(curChain);
                maxValue = curValue;
            }

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var edge in _edges[u]) {
                var v = edge.To;
                if (!chainSet.Contains(v)) {
                    FindLongestChainWithSourceRecursive(v, tail, curChain, chainSet, longestChain, ref curValue, ref maxValue);
                }
            }

            curChain.RemoveAt(curChain.Count - 1);
            chainSet.Remove(u);
            curValue -= _weights[u];
        }

        public List<int> DagFindLongestChain(Func<int, bool> head, Func<int, bool> tail) {
            var visited = new HashSet<int>();
            var topo = DagTopoSort();
            var queue = new List<int>(topo.Where(head));
            List<int>? result = null;
            var maxLength = int.MinValue;
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var node in queue) {
                // ReSharper disable once InvertIf
                if (!visited.Contains(node)) {
                    var (longestPathFromNode, length) = DagFindLongestChainWithSource(node, visited, tail);
                    // ReSharper disable once InvertIf
                    if (longestPathFromNode.Count >= 2 && (result == null || maxLength < length)) {
                        maxLength = length;
                        result = longestPathFromNode;
                    }
                }
            }

            return result ?? new List<int>();
        }

        private (List<int>, int) DagFindLongestChainWithSource(int source, ISet<int> visited, Func<int, bool> tail) {
            var path = new Dictionary<int, int>();
            var queue = new Queue<int>();
            queue.Enqueue(source);
            var distance = _edges.Keys.ToDictionary(node => node, _ => int.MinValue);

            distance[source] = _weights[source];
            path[source] = int.MinValue;
            visited.Add(source);
            while (queue.Count > 0) {
                var node = queue.Dequeue();
                visited.Add(node);
                if (distance[node] == int.MinValue) {
                    continue;
                }

                foreach (var edge in _edges[node]
                             .Where(edge => distance[edge.To] < distance[node] + _weights[edge.To])) {
                    distance[edge.To] = distance[node] + _weights[edge.To];
                    path[edge.To] = node;
                    queue.Enqueue(edge.To);
                }
            }

            var filterResult = distance.Where(d => tail(d.Key) && d.Value != int.MinValue).ToList();
            if (filterResult.Count <= 0) {
                return (new List<int>(), int.MinValue);
            }

            var longestPathEnd = filterResult.First().Key;
            var maxLength = filterResult.First().Value;
            foreach (var dis in filterResult) {
                if (dis.Value > maxLength) {
                    maxLength = dis.Value;
                    longestPathEnd = dis.Key;
                }
            }

            var result = new List<int>();
            for (var n = longestPathEnd; n != int.MinValue; n = path[n]) {
                result.Add(n);
            }

            result.Reverse();
            return (result, maxLength);
        }

        private void FindPath(int u, IList<int> curChain, ISet<int> chainSet, ICollection<List<int>> chains) {
            curChain.Add(u);
            chainSet.Add(u);

            if (curChain.Count > 1) {
                chains.Add(new List<int>(curChain));
            }

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var edge in _edges[u]) {
                var v = edge.To;
                if (!chainSet.Contains(v)) {
                    FindPath(v, curChain, chainSet, chains);
                }
            }

            curChain.RemoveAt(curChain.Count - 1);
            chainSet.Remove(u);
        }

        public List<List<int>> FindAllChains() {
            var chains = new List<List<int>>();
            foreach (var node in _edges.Keys) {
                FindPath(node, new List<int>(), new HashSet<int>(), chains);
            }

            return chains;
        }
    }
}
