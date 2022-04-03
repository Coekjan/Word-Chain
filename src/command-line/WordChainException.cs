using System;

namespace command_line {
    internal abstract class WordChainException : Exception {
        private readonly string _details;

        protected WordChainException(string details) {
            _details = details;
        }

        protected abstract string GetMessage();

        public override string ToString() {
            return $"[Exception: {GetMessage()}] {_details}";
        }
    }

    internal class ArgumentsParseException : WordChainException {
        public ArgumentsParseException(string details) : base(details) {
        }

        protected override string GetMessage() {
            return "illegal arguments";
        }
    }

    internal class FileException : WordChainException {
        public FileException(string details) : base(details) {
        }

        protected override string GetMessage() {
            return "file content error";
        }
    }

    internal class ProcessException : WordChainException {
        public ProcessException(string details) : base(details) {
        }

        protected override string GetMessage() {
            return "process error";
        }
    }
}
