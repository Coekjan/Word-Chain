using System.Collections.Generic;
using System.Text;

namespace graphical_interface {
    internal abstract class Lexer {
        public static List<string> Lex(string sequence) {
            var wordList = new List<string>();
            var current = new StringBuilder();
            foreach (var ch in sequence) {
                if (char.IsLetter(ch)) {
                    current.Append(char.ToLower(ch));
                } else if (current.Length != 0) {
                    if (current.Length > 1) {
                        wordList.Add(current.ToString());
                    }

                    current.Clear();
                }
            }

            if (current.Length > 1) {
                wordList.Add(current.ToString());
            }

            return wordList;
        }
    }
}
