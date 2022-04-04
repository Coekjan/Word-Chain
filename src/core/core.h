#pragma once

constexpr int MAX_RESULT_LENGTH = 20000;

constexpr int E_WORD_CYCLE_EXIST = 0x80000001;

extern "C" _declspec(dllexport) int gen_chain_word(char* words[], int len, char* result[], char head, char tail, bool enable_loop);

extern "C" _declspec(dllexport) int gen_chains_all(char* words[], int len, char* result[]);

extern "C" _declspec(dllexport) int gen_chain_word_unique(char* words[], int len, char* result[]);

extern "C" _declspec(dllexport) int gen_chain_char(char* words[], int len, char* result[], char head, char tail, bool enable_loop);
