#pragma once

class Node
{
private:
    int len;
    int status;
    char* word;

public:
    Node(int len, char* word) : len(len), word(word), status(0) {}

    int get_len() const;

    char* get_word() const;

    int get_status() const;
    
    void set_status(const int status);

    bool operator==(const Node& rhs) const;
};
