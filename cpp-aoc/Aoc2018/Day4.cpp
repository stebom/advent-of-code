#include <algorithm>
#include <iostream>
#include <fstream>
#include <map>

#include "Tokenize.hpp"

namespace Aoc_2018_Day4 {
	using namespace std;

	enum event {
		shift,
		wake,
		sleep
	};

	struct entry {
		int month;
		int day;
		int hour;
		int minute;
		int guard_id;
		event event;
	};

	vector<entry> get_entries() {
		fstream f("2018_day_4.txt");

		const string delimiters("[] -:#");

		vector<entry> entries;
		string str;

		while (getline(f, str)) {

			// Lines to be parsed:
			//   [1518 - 11 - 01 00:00] Guard #10 begins shift
			//   [1518 - 11 - 01 00:05] falls asleep
			//   [1518 - 11 - 01 00:25] wakes up

			const vector<string> tokens = tokenize(str, delimiters);

			if (tokens.size() < 7) { throw std::exception("bad format"); }

			//const auto& year = tokens.at(0);
			const int month = stoi(tokens.at(1));
			const int day = stoi(tokens.at(2));
			const int hour = stoi(tokens.at(3));
			const int minute = stoi(tokens.at(4));

			if (tokens.size() == 9) {
				const string& event = tokens.size() == 9 ? tokens.at(6) : tokens.at(5);
				const int guard_id = stoi(tokens.at(6));

				entries.push_back(entry{ month, day, hour, minute, guard_id, shift });
			}
			else {
				entries.push_back(entry{ month, day, hour, minute, -1, tokens.at(5) == "falls" ? sleep : wake });
			}
		}

		// Sort entries according to timestamp (month > day > hour > minute)
		sort(entries.begin(), entries.end(),
			[](const entry& a, const entry& b)
			{
				if (a.month != b.month) return a.month < b.month;
				if (a.day != b.day) return a.day < b.day;
				if (a.hour != b.hour) return a.hour < b.hour;
				return a.minute < b.minute;
			});

		return entries;
	}

	void part1(const vector<entry>& entries) {
		map<int, int> guard_sleep;
		int current_guard_id;
		entry last_entry = entries[0];

		for (const auto& entry : entries) {

			if (entry.event == sleep && entry.hour != 0) {
				throw std::exception("Sleep events overflow not supported");
			}

			switch (entry.event) {
			case shift:
				current_guard_id = entry.guard_id;
				break;
			case sleep:
				break;
			case wake:
				const int elapsed = entry.minute - last_entry.minute;
				guard_sleep[current_guard_id] += elapsed;

				break;
			}

			last_entry = entry;
		}

		const auto& winner_guard = std::max_element(guard_sleep.begin(), guard_sleep.end(),
			[](const auto& p1, const auto& p2) {
				return p1.second < p2.second;
			});

		cout << "WINNER: Guard " << winner_guard->first << " slept for " << winner_guard->second << " seconds" << endl;

		map<int, int> minutes;
		current_guard_id = -1;

		for (const auto& entry : entries) {
			switch (entry.event) {
			case shift:
				current_guard_id = entry.guard_id;
				break;
			case sleep:
				break;
			case wake:
				if (current_guard_id == winner_guard->first) {
					for (int i = last_entry.minute; i < entry.minute; i++) {
						minutes[i]++;
					}
				}
				break;
			}

			last_entry = entry;
		}

		const auto& winner_minute = std::max_element(minutes.begin(), minutes.end(),
			[](const auto& p1, const auto& p2) {
				return p1.second < p2.second;
			});

		cout << "WINNER: Guard slept most during minute " << winner_minute->first << ", sleeping for " << winner_minute->second << " seconds" << endl;

		// Strategy 1: Find the guard that has the most minutes asleep. What minute does that guard spend asleep the most?
		// What is the ID of the guard you chose multiplied by the minute you chose?
		cout << "Part 1: " << winner_guard->first * winner_minute->first << endl;
	}

	void part2(const vector<entry>& entries) {
		// Mapping guard id -> minute -> sleep
		map<int, map<int, int>> guards;

		int current_guard_id = -1;
		entry last_entry = entries[0];

		for (const auto& entry : entries) {
			switch (entry.event) {
			case shift:
				current_guard_id = entry.guard_id;
				break;
			case sleep:
				break;
			case wake:
				for (int minute = last_entry.minute; minute < entry.minute; minute++) {
					guards[current_guard_id][minute]++;
				}
				break;
			}

			last_entry = entry;
		}

		int winner_guard = -1;
		int winner_minute = -1;
		int winner_sleep = -1;

		for (const auto& kvp : guards) {
			const auto& best_minute = std::max_element(kvp.second.begin(), kvp.second.end(),
				[](const auto& p1, const auto& p2) {
					return p1.second < p2.second;
				});

			if (best_minute->second > winner_sleep) {
				winner_sleep = best_minute->second;
				winner_minute = best_minute->first;
				winner_guard = kvp.first;
			}
		}

		cout << "WINNER: Guard " << winner_guard << " slept most during minute " << winner_minute << ", sleeping for " << winner_sleep << " seconds" << endl;

		// Strategy 2: Of all guards, which guard is most frequently asleep on the same minute?
		// What is the ID of the guard you chose multiplied by the minute you chose ?
		cout << "Part 2: " << winner_guard * winner_minute << endl;
	}

	void run() {
		cout << "Running Aoc_2018_Day4" << endl;
		const vector<entry> entries = get_entries();
		part1(entries);
		part2(entries);
	}
};
