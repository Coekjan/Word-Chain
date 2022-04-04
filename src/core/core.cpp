#include "pch.h"
#include "core.h"
#include "node.h"
#include "chain.h"

#include <vector>
#include <string>
#include <cstring>
#include <algorithm>

using namespace std;

constexpr int WHITE = 0;
constexpr int GRAY = 1;
constexpr int BLACK = 2;

int status[26];
vector<Node> graph[26][26];
int result_count;
vector<string> result_storage;

inline void build_graph(char* words[], const int word_count)
{
    for (int i = 0; i < 26; i++)
    {
        for (int j = 0; j < 26; j++)
        {
            graph[i][j].clear();
        }
    }
    for (int i = 0; i < word_count; i++)
    {
        char* word = words[i];
        int len = (int)strlen(word);
        graph[word[0] - 'a'][word[len - 1] - 'a'].emplace_back(len, word);
    }
    for (int i = 0; i < 26; i++)
    {
        for (int j = 0; j < 26; j++)
        {
            sort(graph[i][j].begin(), graph[i][j].end(), [=](const Node& lhs, const Node& rhs)
            {
                if (lhs.get_len() > rhs.get_len())
                {
                    return true;
                }
                if (lhs.get_len() < rhs.get_len())
                {
                    return false;
                }
                return strcmp(lhs.get_word(), rhs.get_word()) > 0;
            });
            graph[i][j].erase(unique(graph[i][j].begin(), graph[i][j].end()), graph[i][j].end());
        }
    }
}

bool check_cycle_dfs(const int now)
{
    status[now] = GRAY;
    for (int target = 0; target < 26; target++)
    {
        if (now == target)
        {
            continue;
        }
        if (!graph[now][target].empty())
        {
            if (status[target] == GRAY)
            {
                return true;
            }
            else if (status[target] == WHITE)
            {
                if (check_cycle_dfs(target))
                {
                    return true;
                }
            }
        }
    }
    status[now] = BLACK;
    return false;
}

bool check_cycle()
{
    memset(status, WHITE, sizeof(status));
    for (int i = 0; i < 26; i++)
    {
        if (status[i] == WHITE && check_cycle_dfs(i))
        {
            return true;
        }
        if (graph[i][i].size() > 1)
        {
            return true;
        }
    }
    return false;
}

inline void replace_max_chain(const Chain& chain, Chain& max_chain, const bool by_word)
{
    if (chain.get_word_len() > 1)
    {
        if (by_word && chain.get_word_len() > max_chain.get_word_len()) {
            max_chain = chain;
        }
        if (!by_word && chain.get_char_len() > max_chain.get_char_len()) {
            max_chain = chain;
        }
    }
}

/*
// 性能优化前的 gen_chain_dfs 实现
void gen_chain_dfs(const int now, Chain& chain, Chain& max_chain, const char end, const bool by_word)
{
    for (int target = 0; target < 26; target++)
    {
        vector<Node>& nodes = graph[now][target];
        for (auto& node : nodes)
        {
            if (node.get_status())
            {
                continue;
            }
            node.set_status(1);
            chain.push_back(&node);
            if (end < 'a' || end > 'z' || target + 'a' == end)
            {
                replace_max_chain(chain, max_chain, by_word);
            }
            gen_chain_dfs(target, chain, max_chain, end, by_word);
            chain.pop_back();
            node.set_status(0);
            break;
        }
    }
}
*/

void gen_chain_dfs(const int now, Chain& chain, Chain& max_chain, const char end, const bool by_word)
{
    vector<Node>& self_nodes = graph[now][now];
    bool first_met = false;
    for (auto& node : self_nodes)
    {
        // 到达一个点之初，压入所有自环，然后再输出（否则显然更短）
        if (node.get_status() == 0) 
        {
            chain.push_back(&node);
            node.set_status(1);
            first_met = true;
        }
        else
        {
            break;
        }
    }
    if (end < 'a' || end > 'z' || (now + 'a') == end)
    {
        replace_max_chain(chain, max_chain, by_word);
    }
    for (int target = 0; target < 26; target++)
    {
        if (now == target || now >= 26)
        {
            continue;
        }
        vector<Node>& nodes = graph[now][target];
        for (auto& node : nodes)
        {
            if (node.get_status())
            {
                continue;
            }
            node.set_status(1);
            chain.push_back(&node);
            gen_chain_dfs(target, chain, max_chain, end, by_word);
            chain.pop_back();
            node.set_status(0);
            break; // 只走当前可走的最长边
        }
    }
    if (first_met)
    {
        for (auto& node : self_nodes)
        {
            chain.pop_back();
            node.set_status(0);
        }
    }
}

int abstract_gen_chain(char* words[], const int len, char* result[], const char head, const char tail
    , const bool enable_loop, const bool by_word)
{
    build_graph(words, len);
    if (!enable_loop && check_cycle())
    {
        return E_WORD_CYCLE_EXIST;
    }
    Chain chain, max_chain;
    if ('a' <= head && head <= 'z')
    {
        gen_chain_dfs(head - 'a', chain, max_chain, tail, by_word);
    }
    else
    {
        for (int i = 0; i < 26; i++)
        {
            gen_chain_dfs(i, chain, max_chain, tail, by_word);
        }
    }
    for (int i = 0; i < max_chain.get_word_len(); i++)
    {
        result[i] = max_chain.at(i);
    }
    return max_chain.get_word_len();
}

int gen_chain_word(char* words[], int len, char* result[], char head, char tail, bool enable_loop)
{
    return abstract_gen_chain(words, len, result, head, tail, enable_loop, true);
}

int gen_chain_char(char* words[], int len, char* result[], char head, char tail, bool enable_loop)
{
    return abstract_gen_chain(words, len, result, head, tail, enable_loop, false);
}

inline void output_chain(const Chain& chain)
{
    if (chain.get_word_len() > 1)
    {
        if (result_count < MAX_RESULT_LENGTH)
        {
            result_storage.push_back(chain.to_string());
        }
        result_count++;
    }
}

void chains_all_dfs(const int now, Chain& chain)
{
    Node* self_circle = nullptr;
    if (graph[now][now].size() == 1)
    {
        self_circle = &graph[now][now].at(0);
    }
    for (int target = 0; target < 26; target++)
    {
        if (now == target)
        {
            continue;
        }
        vector<Node>& nodes = graph[now][target];
        for (auto& node : nodes)
        {
            chain.push_back(&node);
            output_chain(chain);
            chains_all_dfs(target, chain);
            chain.pop_back();
            if (self_circle != nullptr)
            {
                chain.push_back(self_circle);
                output_chain(chain);
                chain.push_back(&node);
                output_chain(chain);
                chains_all_dfs(target, chain);
                chain.pop_back();
                chain.pop_back();
            }
        }
    }
}

int gen_chains_all(char* words[], int len, char* result[])
{
    build_graph(words, len);
    result_storage.clear();
    result_count = 0;
    if (check_cycle())
    {
        return E_WORD_CYCLE_EXIST;
    }
    Chain chain;
    for (int i = 0; i < 26; i++)
    {
        chains_all_dfs(i, chain);
    }
    if (result_count >= MAX_RESULT_LENGTH)
    {
        return result_count;
    }
    for (int i = 0; i < result_count; i++)
    {
        result[i] = (char*)result_storage[i].c_str();
    }
    return result_count;
}

void chain_word_unique_dfs(const int now, Chain& chain, Chain& max_chain)
{
    for (int target = 0; target < 26; target++)
    {
        if (now == target)
        {
            continue;
        }
        vector<Node>& nodes = graph[now][target];
        for (auto& node : nodes)
        {
            chain.push_back(&node);
            replace_max_chain(chain, max_chain, true);
            chain_word_unique_dfs(target, chain, max_chain);
            if (graph[target][target].size() > 0)
            {
                chain.push_back(&graph[target][target].front());
                replace_max_chain(chain, max_chain, true);
                chain.pop_back();
            }
            chain.pop_back();
            break;
        }
    }
}

int gen_chain_word_unique(char* words[], int len, char* result[])
{
    build_graph(words, len);
    if (check_cycle())
    {
        return E_WORD_CYCLE_EXIST;
    }
    Chain chain, max_chain;
    for (int i = 0; i < 26; i++)
    {
        chain_word_unique_dfs(i, chain, max_chain);
    }
    for (int i = 0; i < max_chain.get_word_len(); i++)
    {
        result[i] = max_chain.at(i);
    }
    return max_chain.get_word_len();
}
