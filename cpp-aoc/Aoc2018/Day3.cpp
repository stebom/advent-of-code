#include <algorithm>
#include <fstream>
#include <iostream>
#include <map>
#include <set>
#include <string>
#include <sstream>
#include <vector>

#include "Tokenize.hpp"

namespace Aoc_2018_Day3 {

	using namespace std;

	int run()
	{
		fstream f("2018_day_3.txt");

		const string delimiters("# @,:x");

		map<pair<int, int>, int> cells;
		map<int, set<pair<int, int>>> claims;

		string str;
		while (getline(f, str)) {
			// "#123 @ 3, 2: 5x4"
			const vector<string> tokens = tokenize(str, delimiters);
			const int id = stoi(tokens.at(0));
			const int col = stoi(tokens.at(1));
			const int row = stoi(tokens.at(2));
			const int width = stoi(tokens.at(3));
			const int height = stoi(tokens.at(4));

			set<pair<int, int>> squares;
			for (int r = 0; r < height; r++) {
				for (int c = 0; c < width; c++) {
					const auto p = make_pair(r + row, c + col);
					cells[p]++;
					squares.insert(p);
				}
			}
			claims.emplace(id, squares);
		}

		f.close();

		const auto num_overlapping = std::count_if(cells.begin(), cells.end(), [](pair<pair<int, int>, int> p) { return p.second > 1; });
		std::cout << "num_overlapping: " << num_overlapping << endl;

		for (auto& claim : claims) {

			bool intersects = false;
			for (auto& other_claim : claims) {
				if (claim.first == other_claim.first) continue;
				if (intersects) continue;

				for (auto& cell : claim.second) {
					if (other_claim.second.count(cell) > 0) {
						intersects = true;
						break;
					}
				}
			}

			if (!intersects) {
				std::cout << "claim " << claim.first << " never intersects" << endl;
			}
		}

		return 0;
	}

	vector<string> tokenize(const string& str, const string& delim)
	{
		size_t tokenStart = 0;
		size_t delimPos = str.find_first_of(delim);
		vector<string> tokens;

		while (delimPos != string::npos)
		{
			const string tok = str.substr(tokenStart, delimPos - tokenStart);
			if (tok.size() > 0) tokens.push_back(tok);
			delimPos++;
			tokenStart = delimPos;
			delimPos = str.find_first_of(delim, delimPos);

			if (delimPos == string::npos)
			{
				const string tok = str.substr(tokenStart, delimPos - tokenStart);
				if (tok.size() > 0) tokens.push_back(tok);
			}
		}

		return tokens;
	}

}
