using System.Collections.Generic;

namespace graphical_interface {
    internal abstract class CoreCaller {
        private static void ThrowCoreException(int code) {
            switch (code) {
                case core.WordChainCoreInterface.ErrorHasCircle:
                    throw new ProcessException("文件中隐含单词环");
                case core.WordChainCoreInterface.ErrorBufferOverflow:
                    throw new ProcessException(
                        $"结果过大（上限长度为 {core.WordChainCoreInterface.ResultBufferMax}）");
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
}
