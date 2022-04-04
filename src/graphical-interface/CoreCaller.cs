﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace graphical_interface {
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

        private static unsafe void Dim2ArrayToDoublePointer(byte[][] array, byte** result) {
            for (var i = 0; i < array.Length; i++) {
                fixed (byte* ptr = array[i]) {
                    result[i] = ptr;
                }
            }
        }

        public static unsafe (int, List<string>) Call(string[] words, Func<IntPtr, IntPtr, int> simple) {
            var wordsCharArray = ConvertStringArrayToSByteDim2Array(words);
            var selfManagedWordSpace = Marshal.AllocHGlobal(sizeof(byte*) * wordsCharArray.Length).ToPointer();
            var selfManagedResultSpace = Marshal.AllocHGlobal(sizeof(byte*) * 20000).ToPointer();
            Dim2ArrayToDoublePointer(wordsCharArray, (byte**) selfManagedWordSpace);
            var result = new List<string>();
            var backendResult = simple((IntPtr) selfManagedWordSpace, (IntPtr) selfManagedResultSpace);
            if (backendResult >= 0 && backendResult < 20000) {
                for (var i = 0; i < backendResult; i++) {
                    var bytes = ((byte**) selfManagedResultSpace)[i];
                    var str = new StringBuilder();
                    for (var p = bytes; *p != 0; p++) {
                        str.Append((char) *p);
                    }
                    result.Add(str.ToString());
                }
            }
            Marshal.FreeHGlobal((IntPtr) selfManagedWordSpace);
            Marshal.FreeHGlobal((IntPtr) selfManagedResultSpace);
            return (backendResult, result);
        }
    }

    internal abstract class CoreCaller {
        [DllImport("core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int gen_chains_all(byte** words, int len, byte** result);

        [DllImport("core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int gen_chain_word(byte** words, int len, byte** result, byte head, byte tail, bool enableLoop);

        [DllImport("core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int gen_chain_word_unique(byte** words, int len, byte** result);

        [DllImport("core.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int gen_chain_char(byte** words, int len, byte** result, byte head, byte tail, bool enableLoop);

        private static void ThrowCoreException(int code) {
            System.Diagnostics.Debug.Assert((uint) code == 0x80000001);
            throw new ProcessException("the word chain in file has circle");
        }

        public static unsafe List<string> GenChainsAll(List<string> words) {
            var (ret, chains) = Adapter.Call(words.ToArray(), (s, r) => {
                return gen_chains_all((byte**) s, words.Count, (byte**) r);
            });
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }

        public static unsafe List<string> GenChainWord(List<string> words, char head, char tail, bool enableLoop) {
            var (ret, chains) = Adapter.Call(words.ToArray(), (s, r) => {
                return gen_chain_word((byte**) s, words.Count, (byte**) r, (byte) head, (byte) tail, enableLoop);
            });
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }

        public static unsafe List<string> GenChainWordUnique(List<string> words) {
            var (ret, chains) = Adapter.Call(words.ToArray(), (s, r) => {
                return gen_chain_word_unique((byte**) s, words.Count, (byte**) r);
            });
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }

        public static unsafe List<string> GenChainChar(List<string> words, char head, char tail, bool enableLoop) {
            var (ret, chains) = Adapter.Call(words.ToArray(), (s, r) => {
                return gen_chain_char((byte**) s, words.Count, (byte**) r, (byte) head, (byte) tail, enableLoop);
            });
            if (ret < 0) {
                ThrowCoreException(ret);
            }

            return chains;
        }
    }
}
