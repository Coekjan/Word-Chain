using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace core_test {
    internal abstract class Adapter {
        private const int MaxSpace = 1024;

        private static byte[][] ConvertStringArrayToSByteDim2Array(string[] stringArray) {
            var result = new byte[stringArray.Length][];
            for (var i = 0; i < stringArray.Length; i++) {
                var byteArray = Encoding.ASCII.GetBytes(stringArray[i]);
                var byteArrayWithEnd = new byte[byteArray.Length + 1];
                Array.Copy(byteArray, byteArrayWithEnd, byteArray.Length);
                result[i] = byteArrayWithEnd;
            }

            return result;
        }

        private static IEnumerable<string> ConvertSByteDim2ArrayToStringArray(byte[][] sbyteDim2Array, int length) {
            var result = new string[length];
            var str = new StringBuilder();
            for (var i = 0; i < length; i++) {
                str.Clear();
                for (var j = 0; sbyteDim2Array[i][j] != 0; j++) {
                    str.Append((char) sbyteDim2Array[i][j]);
                }

                result[i] = str.ToString();
            }

            return result;
        }

        private static unsafe void Dim2ArrayToDoublePointer(byte[][] array, byte** result) {
            for (var i = 0; i < array.Length; i++) {
                fixed (byte* ptr = array[i]) {
                    result[i] = ptr;
                }
            }
        }

        private static byte[][] MakeByteDim2Space() {
            var result = new byte[WordChainCoreInterface.ResultBufferMax][];
            for (var i = 0; i < result.Length; i++) {
                result[i] = new byte[MaxSpace];
            }

            return result;
        }

        public static unsafe (int, List<string>) Call(string[] words, Func<IntPtr, IntPtr, int> simple) {
            var resultCharArray = MakeByteDim2Space();
            var wordsCharArray = ConvertStringArrayToSByteDim2Array(words);
            var selfManagedWordSpace = Marshal.AllocHGlobal(sizeof(byte*) * wordsCharArray.Length).ToPointer();
            var selfManagedResultSpace = Marshal.AllocHGlobal(sizeof(byte*) * resultCharArray.Length).ToPointer();
            Dim2ArrayToDoublePointer(wordsCharArray, (byte**) selfManagedWordSpace);
            Dim2ArrayToDoublePointer(resultCharArray, (byte**) selfManagedResultSpace);
            var backendResult = simple((IntPtr) selfManagedWordSpace, (IntPtr) selfManagedResultSpace);
            Marshal.FreeHGlobal((IntPtr) selfManagedWordSpace);
            Marshal.FreeHGlobal((IntPtr) selfManagedResultSpace);
            return backendResult < 0
                ? (backendResult, new List<string>())
                : (backendResult, ConvertSByteDim2ArrayToStringArray(resultCharArray, backendResult).ToList());
        }
    }

    [TestClass]
    public class WordChainCoreInterfaceTest {
        [TestMethod]
        public unsafe void gen_chains_all_Test1() {
            var words = new[] {
                "woo",
                "oom",
                "moon",
                "noox"
            };
            var (ret, result) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chains_all(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer()
            ));
            var answer = new List<string>(new[] {
                "woo oom",
                "moon noox",
                "oom moon",
                "woo oom moon",
                "oom moon noox",
                "woo oom moon noox"
            });
            Assert.IsTrue(ret == 6 && result.Count == 6 && result.All(answer.Contains));
        }

        [TestMethod]
        public unsafe void gen_chains_all_Test2() {
            var words = new[] {
                "woo",
                "oow"
            };
            var (ret, _) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chains_all(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer()
            ));
            Assert.IsTrue(ret == WordChainCoreInterface.ErrorHasCircle);
        }

        [TestMethod]
        public unsafe void gen_chain_word_Test1() {
            var words = new[] {
                "algebra",
                "apple",
                "zoo",
                "elephant",
                "under",
                "fox",
                "dog",
                "moon",
                "leaf",
                "trick",
                "pseudopseudohypoparathyroidism"
            };
            var (ret, result) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer(),
                0,
                0,
                false
            ));
            var answer = new List<string>(new[] {
                "algebra",
                "apple",
                "elephant",
                "trick"
            });
            Assert.IsTrue(ret == 4 && result.SequenceEqual(answer));
        }

        [TestMethod]
        public unsafe void gen_chain_word_Test2() {
            var words = new[] {
                "algebra",
                "apple",
                "zoo",
                "elephant",
                "under",
                "fox",
                "dog",
                "moon",
                "leaf",
                "trick",
                "pseudopseudohypoparathyroidism"
            };
            var (ret, result) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer(),
                (byte) 'e',
                0,
                false
            ));
            var answer = new List<string>(new[] {
                "elephant",
                "trick"
            });
            Assert.IsTrue(ret == 2 && result.SequenceEqual(answer));
        }

        [TestMethod]
        public unsafe void gen_chain_word_Test3() {
            var words = new[] {
                "algebra",
                "apple",
                "zoo",
                "elephant",
                "under",
                "fox",
                "dog",
                "moon",
                "leaf",
                "trick",
                "pseudopseudohypoparathyroidism"
            };
            var (ret, result) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer(),
                0,
                (byte) 't',
                false
            ));
            var answer = new List<string>(new[] {
                "algebra",
                "apple",
                "elephant"
            });
            Assert.IsTrue(ret == 3 && result.SequenceEqual(answer));
        }

        [TestMethod]
        public unsafe void gen_chain_word_Test4() {
            var words = new[] {
                "algebra",
                "appla"
            };
            var (ret, _) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer(),
                0,
                0,
                false
            ));
            Assert.IsTrue(ret == WordChainCoreInterface.ErrorHasCircle);
        }

        [TestMethod]
        public unsafe void gen_chain_word_unique_Test1() {
            var words = new[] {
                "algebra",
                "apple",
                "zoo",
                "elephant",
                "under",
                "fox",
                "dog",
                "moon",
                "leaf",
                "trick",
                "pseudopseudohypoparathyroidism"
            };
            var (ret, result) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word_unique(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer()
            ));
            var answer = new List<string>(new[] {
                "apple",
                "elephant",
                "trick"
            });
            Assert.IsTrue(ret == 3 && result.SequenceEqual(answer));
        }

        [TestMethod]
        public unsafe void gen_chain_word_unique_Test2() {
            var words = new[] {
                "algebra",
                "apple",
                "egg",
                "ga"
            };
            var (ret, _) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word_unique(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer()
            ));
            Assert.IsTrue(ret == WordChainCoreInterface.ErrorHasCircle);
        }

        [TestMethod]
        public unsafe void gen_chain_word_unique_Test3() {
            var words = new[] {
                "algebra"
            };
            var (ret, _) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_word_unique(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer()
            ));
            Assert.IsTrue(ret == 0);
        }

        [TestMethod]
        public unsafe void gen_chain_char_Test1() {
            var words = new[] {
                "algebra",
                "apple",
                "zoo",
                "elephant",
                "under",
                "fox",
                "dog",
                "moon",
                "leaf",
                "trick",
                "pseudopseudohypoparathyroidism"
            };
            var (ret, result) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_char(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer(),
                0,
                0,
                false
            ));
            var answer = new List<string>(new[] {
                "pseudopseudohypoparathyroidism",
                "moon"
            });
            Assert.IsTrue(ret == 2 && result.SequenceEqual(answer));
        }
        
        [TestMethod]
        public unsafe void gen_chain_char_Test2() {
            var words = new[] {
                "wps",
                "saw"
            };
            var (ret, _) = Adapter.Call(words, (s, r) => WordChainCoreInterface.gen_chain_char(
                (byte**) s.ToPointer(),
                words.Length,
                (byte**) r.ToPointer(),
                0,
                0,
                false
            ));
            Assert.IsTrue(ret == WordChainCoreInterface.ErrorHasCircle);
        }
    }
}
