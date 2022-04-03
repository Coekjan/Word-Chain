using System.Collections.Generic;
using System.Diagnostics;

namespace command_line {
    internal enum Mode {
        All = 1,
        MostWordUniqueHead = 2,
        MostWord = 3,
        MostAlpha = 4
    }

    internal class ArgParser {
        private readonly bool _enableLoop;
        private Mode? _mode;
        private char? _head;
        private char? _tail;

        // ReSharper disable once ConvertToConstant.Local
        private readonly string? _outputFileName = "solution.txt";
        private string? _inputFileName;

        public bool GetEnableLoop() {
            return _enableLoop;
        }

        public Mode GetMode() {
            Debug.Assert(_mode != null);
            return _mode!.Value;
        }

        public char? GetHead() {
            return _head;
        }

        public char? GetTail() {
            return _tail;
        }

        public string GetOutputFileName() {
            Debug.Assert(_outputFileName != null);
            return _outputFileName!;
        }

        public string GetInputFileName() {
            Debug.Assert(_inputFileName != null);
            return _inputFileName!;
        }

        private void SetMode(Mode m) {
            if (_mode != null) {
                throw new ArgumentsParseException("duplicated mode");
            }

            _mode = m;
        }

        private void SetInputFileName(string fileName) {
            if (_inputFileName != null) {
                throw new ArgumentsParseException("multiple input files specified");
            }

            _inputFileName = fileName;
        }

        private void SetHead(char h) {
            if (_head != null) {
                throw new ArgumentsParseException("multiple heads specified");
            }

            _head = h;
        }

        private void SetTail(char t) {
            if (_tail != null) {
                throw new ArgumentsParseException("multiple tails specified");
            }

            _tail = t;
        }

        private static void Check(bool prediction, string message) {
            if (prediction) {
                throw new ArgumentsParseException(message);
            }
        }

        public ArgParser(IReadOnlyList<string> args) {
            for (var i = 0; i < args.Count; i++) {
                if (IsInputFileName(args[i])) {
                    SetInputFileName(args[i]);
                } else if (args[i].StartsWith("-") && args[i].Length == 2) {
                    switch (args[i][1]) {
                        case 'n':
                        case 'm':
                        case 'w':
                        case 'c':
                            var mode = ParseModeFromArg(args[i][1]);
                            Debug.Assert(mode != null);
                            SetMode(mode!.Value);
                            break;
                        case 'h':
                            if (i + 1 < args.Count && args[i + 1].Length == 1 && char.IsLetter(args[i + 1][0])) {
                                i++;
                                SetHead(char.ToLower(args[i][0]));
                            } else {
                                throw new ArgumentsParseException("no letter specified after -h");
                            }

                            break;
                        case 't':
                            if (i + 1 < args.Count && args[i + 1].Length == 1 && char.IsLetter(args[i + 1][0])) {
                                i++;
                                SetTail(char.ToLower(args[i][0]));
                            } else {
                                throw new ArgumentsParseException("no letter specified after -h");
                            }

                            break;
                        case 'r':
                            _enableLoop = true;
                            break;
                        default:
                            throw new ArgumentsParseException($"cannot recognize -{args[i][0]}");
                    }
                } else {
                    throw new ArgumentsParseException($"command parse error at {args[i]}");
                }
            }

            Check(_mode == null, "mode not specified");
            Check(_inputFileName == null, "input file not specified");
            Check(_mode == Mode.All && _head != null, "-n and -h used at the same time");
            Check(_mode == Mode.All && _tail != null, "-n and -t used at the same time");
            Check(_mode == Mode.All && _enableLoop, "-n and -r used at the same time");
            Check(_mode == Mode.MostWordUniqueHead && _head != null, "-m and -h used at the same time");
            Check(_mode == Mode.MostWordUniqueHead && _tail != null, "-m and -t used at the same time");
            Check(_mode == Mode.MostWordUniqueHead && _enableLoop, "-m and -r used at the same time");
        }

        private static Mode? ParseModeFromArg(char arg) {
            return arg switch {
                'n' => Mode.All,
                'm' => Mode.MostWordUniqueHead,
                'w' => Mode.MostWord,
                'c' => Mode.MostAlpha,
                _ => null
            };
        }

        private static bool IsInputFileName(string inputFileName) {
            return inputFileName.EndsWith(".txt");
        }
    }
}
