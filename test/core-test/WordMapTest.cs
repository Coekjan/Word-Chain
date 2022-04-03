using System.Collections.Generic;
using System.Linq;
using core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace core_test {

    [TestClass]
    public class WordMapTest {
        [TestMethod]
        public void BuildTest() {
            var wordList = new List<string>(new[] {"woo", "oom", "moon", "noox"});
            var wordMap = WordMap.Build(wordList);
            var wordsInMap = new HashSet<string>(wordMap.GetAllWords());
            Assert.AreEqual(wordList.Count, wordsInMap.Count);
            Assert.IsTrue(wordList.All(word => wordsInMap.Contains(word)));
        }

        [TestMethod]
        public void AddWordTest1() {
            var wordMap = new WordMap();

            const string word = "woo";
            var id = wordMap.AddWord(word);
            Assert.AreEqual(id, wordMap.GetId(word));
            Assert.AreEqual(word, wordMap.GetWord(id));
            Assert.AreEqual(word, wordMap.GetWord(1));
        }

        [TestMethod]
        public void AddWordTest2() {
            var wordMap = new WordMap();

            const string word = "woo";
            var id1 = wordMap.AddWord(word);
            var id2 = wordMap.AddWord(word);
            Assert.AreEqual(id1, id2);
        }
    }
}
