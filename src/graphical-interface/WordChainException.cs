using System;

namespace graphical_interface {
    internal abstract class WordChainException : Exception {
        private readonly string _details;

        protected WordChainException(string details) {
            _details = details;
        }

        protected abstract string GetDetails();

        public override string ToString() {
            return _details;
        }
    }

    internal class ProcessException : WordChainException {
        public ProcessException(string details) : base(details) {
        }

        protected override string GetDetails() {
            return "处理数据过程中发现异常";
        }
    }
}
