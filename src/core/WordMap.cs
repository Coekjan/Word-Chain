using System.Collections.Generic;
using System.Diagnostics;

namespace core {
    public class WordMap {
        private readonly Dictionary<string, int> _wordToId = new Dictionary<string, int>();
        private readonly Dictionary<int, string> _idToWord = new Dictionary<int, string>();
        private int _wordCount;

        public static WordMap Build(List<string> wordList) {
            var wordMap = new WordMap();
            foreach (var word in wordList) {
                wordMap.AddWord(word.ToLower());
            }

            return wordMap;
        }

        public int AddWord(string word) {
            if (_wordToId.ContainsKey(word)) {
                return _wordToId[word];
            }

            ++_wordCount;
            _wordToId.Add(word, _wordCount);
            _idToWord.Add(_wordCount, word);
            return _wordCount;
        }

        public string GetWord(int id) {
            Debug.Assert(id > 0 && id <= _wordCount);
            return _idToWord[id];
        }

        public int GetId(string word) {
            Debug.Assert(_wordToId.ContainsKey(word.ToLower()));
            return _wordToId[word.ToLower()];
        }

        public IEnumerable<string> GetAllWords() {
            return _wordToId.Keys;
        }
    }
}
