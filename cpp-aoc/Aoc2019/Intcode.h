#pragma once
#include <bitset>
#include <vector>
#include <deque>
using namespace std;

class Intcode
{
private:
	vector<int> tomode(const long long) const;
	long long param(const int, const int);
	long long read(const long long index);
	void write(const long long index, const long long value);

	deque<long long> _args;
	vector<long long> _program;
	vector<long long> _output;
	int _relative_base;
	int _position;
	bool _debug;
	
public:
	Intcode(vector<int> program);
	Intcode(vector<long long> program);

	long long run();
	void set_debug(bool);
	const vector<long long>& get_output() const;
	long long get_output(int index) const;
	void push_arg(long long arg);
};