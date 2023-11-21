#include <sstream>
#include <fstream>
#include <iostream>
#include <string>
#include <vector>
#include <regex>
#include <set>
#include <algorithm>
#include <map>

void day1()
{
	using namespace std;

	auto file = ifstream("day1_input.txt");
	string line;
	getline(file, line);

	auto part1 = 0;
	auto part2 = 0;
	for (auto i = 0; i < line.size(); i++)
	{
		// part 1
		{
			auto next = i == line.size() - 1 ? 0 : i + 1;
			if (line[i] == line[next]) {
				part1 += line[i] - '0';
			}
		}

		// part 2
		{
			auto next = (i + line.size() / 2) % line.size();
			if (line[i] == line[next]) {
				part2 += line[i] - '0';
			}
		}
	}

	cout << "day 1 part1: " << part1 << endl;
	cout << "      part2: " << part2 << endl;
}

void day2()
{
	using namespace std;

	const std::regex whitespace("\\s+");

	auto file = ifstream("day2_input.txt");

	auto part1 = 0;
	auto part2 = 0;

	string line;
	while (getline(file, line))
	{
		line = std::regex_replace(line, whitespace, " ");

		set<int> values {};
		int high = -1;
		int low = INT_MAX;
		stringstream input(line);
		string token;

		while (getline(input, token, ' '))
		{
			const auto val = stoi(token);
			if (val > high) high = val;
			if (val < low) low = val;

			values.emplace(val);
		}

		for (const auto val1 : values) {
			for (const auto val2 : values) {
				if (val1 == val2) continue;
				if (val1 % val2 == 0) part2 += val1 / val2;
			}
		}

		part1 += (high - low);
	}

	cout << "day 2 part1: " << part1 << endl;
	cout << "      part2: " << part2 << endl;
}

void day3_part1()
{
	using namespace std;

	const int target = 289326;
	
	int layer = 2;
	int range = 10;
	while (true)
	{
		const auto side = (layer * 2 + 1);
		const auto cells = side * 2 + (side - 2) * 2;

		const auto layer_min = range;
		const auto layer_max = range + cells - 1;

		if (layer_max > target) {

			const auto steps = target - layer_min;
			cout << steps << " steps from layer_min " << layer << endl;
			
			cout << "layer " << layer << endl;
			cout << "  side: " << side << endl;
			cout << "  cells: " << cells << endl;
			cout << "  layer_min: " << layer_min << endl;
			cout << "  layer_max: " << layer_max << endl;

			auto current = layer_min;

			// { X,Y }
			auto position = make_pair(layer, layer - 1);

			while (current != target && position.second > -layer) {
				position.second -= 1;
				current++;
			}

			while (current != target && position.first > -layer) {
				position.first -= 1;
				current++;
			}

			while (current != target && position.second < layer) {
				position.second += 1;
				current++;
			}

			while (current != target && position.first < layer) {
				position.first += 1;
				current++;
			}

			const auto distance = abs(position.first) + abs(position.second);
			cout << "day 3 part1: " << distance << endl;
			break;
		}

		range += cells;
		layer++;
	}
	cerr << "No solution found!" << endl;
}

void day3_part2()
{
	using namespace std;
	enum struct Direction { North, West, South, East };

	auto turnLeft = [](Direction direction) {
		if (direction == Direction::North) return Direction::West;
		if (direction == Direction::West) return Direction::South;
		if (direction == Direction::South) return Direction::East;
		if (direction == Direction::East) return Direction::North;
		return direction;
	};

	auto walk = [](pair<short, short> pos, Direction direction) {
		if (direction == Direction::North) pos.second--;
		if (direction == Direction::West) pos.first--;
		if (direction == Direction::South) pos.second++;
		if (direction == Direction::East) pos.first++;
		return pos;
	};


	set< pair<short, short>> visited { };
	Direction direction = Direction::East;
	pair<short, short> position { 0,0 };
	visited.insert(position);

	map<pair<short, short>, int> values {
		{ position, 1 }
	};

	auto score = [walk, &values](pair<short, short> pos) {
		/*
			cout << "West " << values[walk(pos, Direction::West)] << endl;
			cout << "North " << values[walk(pos, Direction::North)] << endl;
			cout << "East " << values[walk(pos, Direction::East)] << endl;
			cout << "South " << values[walk(pos, Direction::South)] << endl;
			cout << "NW " << values[{ pos.first - 1, pos.second - 1}] << endl; // NW
			cout << "NE " << values[{ pos.first + 1, pos.second - 1}] << endl; // NE
			cout << "SW " << values[{ pos.first - 1, pos.second - 1}] << endl; // SW
			cout << "SE " << values[{ pos.first + 1, pos.second - 1}] << endl; // SE
		*/
		return values[walk(pos, Direction::West)] +
			   values[walk(pos, Direction::North)] +
			   values[walk(pos, Direction::East)] +
			   values[walk(pos, Direction::South)] +
			   values[{ pos.first - 1, pos.second - 1}] + // NW
			   values[{ pos.first + 1, pos.second - 1}] + // NE
			   values[{ pos.first - 1, pos.second + 1}] + // SW
			   values[{ pos.first + 1, pos.second + 1}]; // SE
	};

	const int steps = 289326 - 1;
	for (auto i = 0; i < steps; i++)
	{
		position = walk(position, direction);
		visited.insert(position);

		values[position] = score(position);
		cout << "position is scored " << values[position] << endl;
		if (values[position] > 289326) break;

		const bool canWalkLeft = visited.count(walk(position, turnLeft(direction))) == 0;
		if (canWalkLeft) direction = turnLeft(direction);
	}

	const auto distance = abs(position.first) + abs(position.second);
	cout << "day 3 part1: " << distance << endl;
}

int main()
{
	//day1();
	//day2();
	//day3_part1();
	day3_part2();
}
