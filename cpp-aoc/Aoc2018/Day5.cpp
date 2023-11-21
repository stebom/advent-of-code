
#include <fstream>
#include <iterator>
#include <iostream>

#include "Tokenize.hpp"

namespace Aoc_2018_Day5 {
	using namespace std;

	struct Link {
		explicit Link(char unit) : unit(unit), prev(nullptr), next(nullptr) {}
		const char unit;
		Link* prev;
		Link* next;
	};

	bool reacts(char a, char b) {
		// A => 65, a => 97
		return max(a, b) - min(a, b) == 32;
	}

	Link* parse(const string& line) {
		Link* head = new Link('\0');
		Link* current = head;

		for (char c : line) {
			// unit must be A-Z or a-z
			if ((65 <= c && c <= 90) ||
				(97 <= c && c <= 122))
			{
				current->next = new Link(c);
				current->next->prev = current;
				current = current->next;
			}
		}

		return head;
	}

	uint16_t count(const Link* head) {
		Link* current = head->next;
		int i = 0;
		while (current) {
			current = current->next;
			++i;
		}
		return i;
	}

	void removeUnit(const Link* head, char c) {
		Link* current = head->next;
		while (current) {
			if (current->unit == c || current->unit == (char)(c + 32)) {
				Link* prev = current->prev;
				Link* next = current->next;
				delete current;

				if (next) next->prev = prev;
				prev->next = next;

				current = prev;
			}
			else {
				current = current->next;
			}
		}
	}

	void react(const Link* head) {
		Link* current = head->next;
		while (current) {
			if (current->next && reacts(current->unit, current->next->unit)) {
				Link* prev = current->prev;
				Link* next = current->next->next;

				if (next) next->prev = prev;
				prev->next = next;

				delete current->next;
				delete current;
				current = prev;
			}
			else {
				current = current->next;
			}
		}
	}

	void part1(const string& line) {
		const Link* head = parse(line);
		react(head);
		cout << "part1: " << count(head) << endl;
	}

	void part2(const string& line) {
		uint16_t best = UINT_MAX;
		
		for (char c = 'A'; c < 'Z'; c++) {
			const Link* head = parse(line);
			removeUnit(head, c);
			react(head);
			best = min(count(head), best);
		}

		cout << "part2: " << best << endl;
	}

	void run() {
		cout << "Running Aoc_2018_Day5" << endl;
		fstream f("2018_day_5.txt");
		string line;
		getline(f, line);
		part1(line);
		part2(line);
	}

}
