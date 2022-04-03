using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace core {
    internal abstract class CharConverter {
        public static unsafe List<string> ReadFromBytePtrArray(byte** bytes, int len) {
            var buffer = new List<string>();
            for (var i = 0; i < len; i++) {
                var word = new StringBuilder();
                for (var p = bytes[i]; *p != 0; p++) {
                    word.Append((char) *p);
                }

                buffer.Add(word.ToString());
            }

            return buffer;
        }

        public static unsafe void WriteToBytePtrArray(List<string> buffer, byte** bytes) {
            for (var i = 0; i < buffer.Count; i++) {
                var p = bytes[i];
                for (var j = 0; j < buffer[i].Length; j++) {
                    *p++ = (byte) buffer[i][j];
                }

                *p = 0;
            }
        }
    }

    public abstract class WordChainCoreInterface {
        // ReSharper disable once MemberCanBePrivate.Global
        public const int ResultBufferMax = 20000;

        // ReSharper disable once MemberCanBePrivate.Global
        public const int ErrorHasCircle = -1;

        // ReSharper disable once MemberCanBePrivate.Global
        public const int ErrorBufferOverflow = -2;

        public static unsafe int gen_chains_all(byte** words, int len, byte** result) {
            var wordList = CharConverter.ReadFromBytePtrArray(words, len);
            var (ret, res) = GenChainsAll(wordList);
            if (ret < 0) {
                return ret;
            }

            CharConverter.WriteToBytePtrArray(res, result);
            return ret;
        }

        public static (int, List<string>) GenChainsAll(List<string> words) {
            var wordMap = WordMap.Build(words);
            var graph = WordChainHandler.BuildWordGraph(wordMap, _ => 1);
            if (graph.HasCircle()) {
                return (ErrorHasCircle, new List<string>());
            }

            var allChains = graph.FindAllChains();
            if (allChains.Count > ResultBufferMax) {
                return (ErrorBufferOverflow, new List<string>());
            }

            var wordChains =
                allChains.ConvertAll(chain => string.Join(" ", chain.ConvertAll(wordId => wordMap.GetWord(wordId))));
            return (wordChains.Count, wordChains);
        }

        public static unsafe int gen_chain_word(byte** words, int len, byte** result, byte head, byte tail,
            bool enableLoop) {
            var wordList = CharConverter.ReadFromBytePtrArray(words, len);
            var (ret, chain) = GenChainWord(wordList, (char) head, (char) tail, enableLoop);
            if (ret < 0) {
                return ret;
            }

            CharConverter.WriteToBytePtrArray(chain, result);
            return ret;
        }

        public static (int, List<string>) GenChainWord(List<string> words, char head, char tail, bool enableLoop) {
            return GenChainMaxLength(words, head, tail, enableLoop, _ => 1);
        }

        public static unsafe int gen_chain_word_unique(byte** words, int len, byte** result) {
            var wordList = CharConverter.ReadFromBytePtrArray(words, len);
            var (ret, uniqueChain) = GenChainWordUnique(wordList);
            if (ret < 0) {
                return ret;
            }

            CharConverter.WriteToBytePtrArray(uniqueChain, result);
            return uniqueChain.Count;
        }

        public static (int, List<string>) GenChainWordUnique(List<string> words) {
            var wordMap = WordMap.Build(words);
            var graph = WordChainHandler.BuildAlphaGraph(wordMap);
            if (graph.HasCircle()) {
                return (ErrorHasCircle, new List<string>());
            }

            var longestAlphaChain = graph.DagFindLongestChain(_ => true, _ => true);
            var headTailMap = new Dictionary<(int, int), string>();
            foreach (var word in words) {
                headTailMap[(word.First(), word.Last())] = word;
            }

            var uniqueChain = new List<string>();
            for (var i = 0; i < longestAlphaChain.Count - 1; i++) {
                var word = longestAlphaChain[i + 1] == '$'
                    ? headTailMap[(longestAlphaChain[i], longestAlphaChain[i])]
                    : headTailMap[(longestAlphaChain[i], longestAlphaChain[i + 1])];
                uniqueChain.Add(word);
            }

            if (uniqueChain.Count == 1) {
                return (0, new List<string>());
            }

            if (uniqueChain.Count > ResultBufferMax) {
                return (ErrorBufferOverflow, new List<string>());
            }

            return (uniqueChain.Count, uniqueChain);
        }

        public static unsafe int gen_chain_char(byte** words, int len, byte** result, byte head, byte tail,
            bool enableLoop) {
            var wordList = CharConverter.ReadFromBytePtrArray(words, len);
            var (ret, chain) = GenChainChar(wordList, (char) head, (char) tail, enableLoop);
            if (ret < 0) {
                return ret;
            }

            CharConverter.WriteToBytePtrArray(chain, result);
            return ret;
        }

        public static (int, List<string>) GenChainChar(List<string> words, char head, char tail, bool enableLoop) {
            return GenChainMaxLength(words, head, tail, enableLoop, s => s.Length);
        }

        private static (int, List<string>) GenChainMaxLength(List<string> words, char head, char tail, bool enableLoop,
            Func<string, int> calcWeight) {
            var wordMap = WordMap.Build(words);
            var graph = WordChainHandler.BuildWordGraph(wordMap, calcWeight);
            if (!enableLoop && graph.HasCircle()) {
                return (ErrorHasCircle, new List<string>());
            }

            var headLimit = head == 0
                ? (Func<int, bool>) (_ => true)
                : i => wordMap.GetWord(i).StartsWith(head.ToString());
            var tailLimit = tail == 0
                ? (Func<int, bool>) (_ => true)
                : i => wordMap.GetWord(i).EndsWith(tail.ToString());
            var longestChain = !enableLoop
                ? graph.DagFindLongestChain(headLimit, tailLimit)
                : graph.FindLongestChainRecursive(headLimit, tailLimit);
            if (longestChain.Count > ResultBufferMax) {
                return (ErrorBufferOverflow, new List<string>());
            }

            var wordChain = longestChain.ConvertAll(i => wordMap.GetWord(i));
            return (wordChain.Count, wordChain);
        }
    }

    internal abstract class WordChainHandler {
        internal static Graph BuildAlphaGraph(WordMap wordMap) {
            var graph = new Graph();
            for (var ch = 'a'; ch <= 'z'; ch++) {
                graph.AddNode(ch, 1);
            }

            graph.AddNode('$', 1);

            foreach (var word in wordMap.GetAllWords()) {
                int fromId = word.First();
                int toId = word.Last();
                graph.AddEdge(fromId, fromId != toId ? toId : '$');
            }

            return graph;
        }

        internal static Graph BuildWordGraph(WordMap wordMap, Func<string, int> calcWeight) {
            var graph = new Graph();
            var headMap = new Dictionary<char, List<string>>();
            for (var ch = 'a'; ch <= 'z'; ch++) {
                headMap[ch] = new List<string>();
            }

            foreach (var word in wordMap.GetAllWords()) {
                var head = word.First();
                headMap[head].Add(word);
                graph.AddNode(wordMap.GetId(word), calcWeight(word));
            }

            foreach (var fromWord in wordMap.GetAllWords()) {
                var tail = fromWord.Last();
                var fromWordId = wordMap.GetId(fromWord);

                foreach (var toWordId in headMap[tail].Select(wordMap.GetId)
                             .Where(toWordId => fromWordId != toWordId)) {
                    graph.AddEdge(fromWordId, toWordId);
                }
            }

            return graph;
        }
    }
}
