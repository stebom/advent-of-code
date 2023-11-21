#include "Intcode.h"

#include <algorithm>
#include <iostream>
#include <string>

Intcode::Intcode(vector<int> program)
	: _program(program.begin(), program.end()), _args(), _relative_base(0), _position(0), _debug(), _output()
{
	// Do nothing.
}

Intcode::Intcode(vector<long long> program)
	: _program(program), _args(), _relative_base(0), _position(0), _debug(), _output()
{
	// Do nothing.
}

long long Intcode::run()
{
	if (_debug) cout << "Running intcode from position " << _position << endl;

	while (_position < _program.size())
	{
		// Immediately halt
		if (_program[_position] == 99)
		{
			if (_debug) cout << "Exiting at position " << _position << "/" << _program.size()-1 << endl;
			return _program[0];
		}

		const auto mode = tomode(_program[_position]);

		// Opcode 1 adds input: a, b, output: c
		if (_program[_position] % 10 == 1)
		{
			const auto param1 = param(mode[0], _position + 1);
			const auto param2 = param(mode[1], _position + 2);
			const auto store = _program[_position + 3] + (mode[2] == 2 ? _relative_base : 0);
			write(store, param1 + param2);
			_position += 4;
		}

		// Opcode 2 multiplies input: a, b, output: c
		else if (_program[_position] % 10 == 2)
		{
			const auto param1 = param(mode[0], _position + 1);
			const auto param2 = param(mode[1], _position + 2);
			const auto store = _program[_position + 3] + (mode[2] == 2 ? _relative_base : 0);
			write(store, param1 * param2);
			_position += 4;
		}
		// Opcode 3 takes a single integer as input and saves it to the position given by its only parameter.
		else if (_program[_position] % 10 == 3)
		{
			_ASSERT(mode[0] != 1);

			if (_args.empty()) {
				return -1;
			}

			const auto input = _args.front();
			_args.pop_front();
			if (_debug) cout << "Using provided input " << input << endl;

			const auto store = _program[_position + 1] + (mode[0] == 2 ? _relative_base : 0);

			write(store, input);
			_position += 2;
		}
		// Opcode 4 outputs the value of its only parameter.
		else if (_program[_position] % 10 == 4)
		{
			const auto param1 = param(mode[0], _position + 1);
			_output.push_back(param1);
			if (_debug) cout << "Output: " << param1 << endl;
			_position += 2;
		}
		// Opcode 5 is jump-if-true
		else if (_program[_position] % 10 == 5)
		{
			// if the first parameter is non-zero,
			// it sets the instruction pointer to the value from the second parameter.
			// Otherwise, it does nothing.
			const auto param1 = param(mode[0], _position + 1);
			const auto param2 = param(mode[1], _position + 2);

			_ASSERT(param2 <= INT_MAX);
			if (param1 != 0) _position = param2;
			else _position += 3;
		}
		// Opcode 6 is jump -if-false
		else if (_program[_position] % 10 == 6)
		{
			// if the first parameter is zero,
			// it sets the instruction pointer to the value from the second parameter.
			// Otherwise, it does nothing.
			const auto param1 = param(mode[0], _position + 1);
			const auto param2 = param(mode[1], _position + 2);

			_ASSERT(param2 <= INT_MAX);
			if (param1 == 0) _position = param2;
			else _position += 3;
		}
		// Opcode 7 is less than
		else if (_program[_position] % 10 == 7)
		{
			// if the first parameter is less than the second parameter,
			// it stores 1 in the position given by the third parameter.
			// Otherwise, it stores 0.
			const auto param1 = param(mode[0], _position + 1);
			const auto param2 = param(mode[1], _position + 2);
			const auto store = _program[_position + 3] + (mode[2] == 2 ? _relative_base : 0);

			write(store, (param1 < param2) ? 1 : 0);
			_position += 4;
		}
		// Opcode 8 is equals
		else if (_program[_position] % 10 == 8)
		{
			// if the first parameter is equal to the second parameter,
			// it stores 1 in the position given by the third parameter.
			// Otherwise, it stores 0.
			const auto param1 = param(mode[0], _position + 1);
			const auto param2 = param(mode[1], _position + 2);
			const auto store = _program[_position + 3] + (mode[2] == 2 ? _relative_base : 0);

			write(store, (param1 == param2) ? 1 : 0);
			_position += 4;
		}
		// Opcode 9 adjusts the relative base
		else if (_program[_position] % 10 == 9)
		{
			_relative_base += param(mode[0], _position + 1);
			_position += 2;
		}
		// Unhandled opcode
		else
		{
			cout << "Invalid opcode " << _program[_position] << "!" << endl;
			cout << "Terminating program." << endl;
			break;
		}
	}

	return -1;
}

void Intcode::push_arg(long long arg)
{
	_args.push_back(arg);
}

const vector<long long>& Intcode::get_output() const
{
	return _output;
}

long long Intcode::get_output(int index) const
{
	return _output[index];
}

void Intcode::set_debug(bool flag)
{
	_debug = flag;
}

vector<int> Intcode::tomode(const long long instruction) const
{
	auto opcode = to_string(instruction);
	if (opcode.size() < 2) return vector<int> { 0, 0, 0 };

	opcode = opcode.substr(0, opcode.size() - 2);
	reverse(opcode.begin(), opcode.end());
	
	if (opcode.size() == 1)
	{
		opcode += "00";
	}
	else if (opcode.size() == 2)
	{
		opcode += "0";
	}

	_ASSERT(opcode.size() == 3);

	vector<int> vec;
	for (char& c : opcode)
	{
		vec.push_back(c - '0');
	}

	return vec;
}

long long Intcode::param(const int mode, const int position)
{
	_ASSERT(mode == 0 || mode == 1 || mode == 2);
	if (mode == 0) return read(read(position)); // position mode
	else if (mode == 1) return read(position); // immediate mode
	else if (mode == 2) return read(read(position) + _relative_base); // relative mode

	return -1; // fall through should never happen
}

long long Intcode::read(const long long index)
{
	_ASSERT(index >= 0);

	if (index > _program.size() - 1)
	{
		return 0;
	}

	return _program[index];
}

void Intcode::write(const long long index, const long long value)
{
	_ASSERT(index >= 0);

	if (index > _program.size() - 1)
	{
		_program.resize(index + 1);
	}

	_program[index] = value;
}