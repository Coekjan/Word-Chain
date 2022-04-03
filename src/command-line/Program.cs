using System;
using System.Collections.Generic;
using System.IO;

namespace command_line {
    internal abstract class CoreCaller {
        private static void ThrowCoreException(int code) {
            switch (code) {
                case core.WordChainCoreInterface.ErrorHasCircle:
                    throw new ProcessException("the word chain in file has circle");
                case core.WordChainCoreInterface.ErrorBufferOverflow:
                    throw new ProcessException(
                        $"result buffer overflow (buffer size is {core.WordChainCoreInterface.ResultBufferMax})");
            }
        }

        public static List<string> GenChainsAll(List<string> words) {
            var (ret, chains) = core.WordChainCoreInterface.GenChainsAll(words);
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }

        public static List<string> GenChainWord(List<string> words, char head, char tail, bool enableLoop) {
            var (ret, chains) = core.WordChainCoreInterface.GenChainWord(words, head, tail, enableLoop);
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }

        public static List<string> GenChainWordUnique(List<string> words) {
            var (ret, chains) = core.WordChainCoreInterface.GenChainWordUnique(words);
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }

        public static List<string> GenChainChar(List<string> words, char head, char tail, bool enableLoop) {
            var (ret, chains) = core.WordChainCoreInterface.GenChainChar(words, head, tail, enableLoop);
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }
    }

    public static class Program {
        private static string GetFileContent(string path) {
            if (!File.Exists(path)) {
                throw new FileException($"{path} not exists");
            }

            try {
                return File.ReadAllText(path);
            } catch (IOException) {
                throw new FileException($"fail to read {path}");
            }
        }

        public static void Main(string[] args) {
            try {
                var argParser = new ArgParser(args);
                var rawInput = GetFileContent(argParser.GetInputFileName());
                var wordList = Lexer.Lex(rawInput);
                List<string> result;
                switch (argParser.GetMode()) {
                    case Mode.All:
                        System.Diagnostics.Debug.Assert(argParser.GetHead() == null);
                        System.Diagnostics.Debug.Assert(argParser.GetTail() == null);
                        System.Diagnostics.Debug.Assert(!argParser.GetEnableLoop());
                        result = CoreCaller.GenChainsAll(wordList);
                        Console.WriteLine(result.Count);
                        result.ForEach(Console.WriteLine);
                        break;
                    case Mode.MostWordUniqueHead:
                        System.Diagnostics.Debug.Assert(argParser.GetHead() == null);
                        System.Diagnostics.Debug.Assert(argParser.GetTail() == null);
                        System.Diagnostics.Debug.Assert(!argParser.GetEnableLoop());
                        result = CoreCaller.GenChainWordUnique(wordList);
                        File.WriteAllText(argParser.GetOutputFileName(), string.Join("\n", result));
                        break;
                    case Mode.MostWord:
                        result = CoreCaller.GenChainWord(wordList,
                            argParser.GetHead() ?? '\0',
                            argParser.GetTail() ?? '\0',
                            argParser.GetEnableLoop()
                        );
                        File.WriteAllText(argParser.GetOutputFileName(), string.Join("\n", result));
                        break;
                    case Mode.MostAlpha:
                        result = CoreCaller.GenChainChar(wordList,
                            argParser.GetHead() ?? '\0',
                            argParser.GetTail() ?? '\0',
                            argParser.GetEnableLoop()
                        );
                        File.WriteAllText(argParser.GetOutputFileName(), string.Join("\n", result));
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                }
            } catch (WordChainException e) {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
