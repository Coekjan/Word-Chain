#pragma once

#include "node.h"

#include <vector>
#include <string>

using namespace std;

class Chain
{
private:
    int char_len;
    vector<Node*> word_vector;

public:
    Chain(): char_len(0) {}

    char* at(const int index) const;

    int get_word_len() const;

    int get_char_len() const;

    void push_back(Node* word);

    void pop_back();

    string to_string() const;
};
