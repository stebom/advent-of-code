#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <vector>
#include <set>
#include <algorithm>
#include <bitset>
#include <map>
#include <queue>
#include "Intcode.h"

void Day1();
int Day1_ComputeFuel(int);
int Day1_ComputeFuelRec(int);

void Day2();

void Day3();
struct Point;
std::vector<Point> CreateWire(std::string);
int ManhattanDistance(const Point&, const Point&);

void Day4();
bool Day4_Test(const std::string& str);
bool Day4_Test2(const std::string& str);

void Day5();

void Day6();
void Day6_Part1();
void Day6_Part2();

void Day7();
void Day7_Part1();
void Day7_Part2();
vector<vector<int>> Day7_FindAllCombinations(vector<int> input);

void Day8();

void Day9();

void Test_Intcode();

int main()
{
	//Test_Intcode();

	//Day1();
	//Day2();
	//Day3();
	//Day4();
	//Day5();
	//Day6();
	//Day7();
	//Day8();
	//Day9();

	return -1;
}

//====================== Day 1 ============================

void Day1()
{
	long part1 = 0, part2 = 0;
	auto file = std::ifstream("input_1.txt");
	for (std::string line; std::getline(file, line); )
	{
		part1 += Day1_ComputeFuel(std::stoi(line));
		part2 += Day1_ComputeFuelRec(std::stoi(line));
	}
	std::cout << "Part 1: " << part1 << "\n";
	std::cout << "Part 2: " << part2 << "\n";
}

// Fuel required to launch a given module is based on its mass.
int Day1_ComputeFuel(int mass) {
	// Specifically, to find the fuel required for a module, take its mass, divide by three, round down, and subtract 2.
	return (mass / 3) - 2;
}

int Day1_ComputeFuelRec(int mass) {
	auto fuel = Day1_ComputeFuel(mass);
	if (fuel <= 0) return 0;
	return fuel + Day1_ComputeFuelRec(fuel);
}

//====================== Day 2 ============================

void Day2()
{
	std::vector<int> memory;
	auto file = std::ifstream("input_2.txt");
	std::string token;
	while (std::getline(file, token, ','))
	{
		memory.push_back(std::stoi(token));
	}

	// Before running the program, replace position 1 with the value 12 and replace position 2 with the value 2.
	auto program = memory;
	program[1] = 12;
	program[2] = 2;

	auto intcode = Intcode { program };
	auto part1 = intcode.run();

	std::cout << "Day2 Part 1: " << part1 << "\n";

	// Determine what pair of inputs produces the output 19690720
	// The inputs should still be provided to the program by replacing the values at addresses 1 and 2, just like before
	int part2 = -1;
	for (auto noun = 0; noun <= 99; noun++)
	{
		for (auto verb = 0; verb <= 99; verb++)
		{
			program = memory;
			program[1] = noun;
			program[2] = verb;

			intcode = Intcode{ program };

			if (intcode.run() == 19690720) {
				part2 = 100 * noun + verb;
				goto exit;
			}
		}
	}

exit:
	std::cout << "Day2 Part 2: " << part2 << "\n";

	_ASSERT(part1 == 3101878);
	_ASSERT(part2 == 8444);
}

//====================== Day 3 ============================

struct Point
{
	Point(int _x, int _y)
		: x(_x), y(_y)
	{}

	int x;
	int y;

	bool operator==(const Point& other) const
	{
		return x == other.x && y == other.y;
	}

	bool operator<(const Point& other) const
	{
		if (x < other.x) return true;
		if (x > other.x) return false;
		if (y < other.y) return true;
		return false;
	}
};

void Day3()
{
	const auto& wire = CreateWire("R1003,U741,L919,U341,L204,U723,L113,D340,L810,D238,R750,U409,L104,U65,R119,U58,R94,D738,L543,U702,R612,D998,L580,U887,R664,D988,R232,D575,R462,U130,L386,U386,L217,U155,L68,U798,R792,U149,L573,D448,R76,U896,L745,D640,L783,D19,R567,D271,R618,U677,L449,D651,L843,D117,L636,U329,R484,U853,L523,U815,L765,U834,L500,U321,R874,U90,R473,U31,R846,U549,L70,U848,R677,D557,L702,U90,R78,U234,R282,D289,L952,D514,R308,U255,R752,D338,L134,D335,L207,U167,R746,U328,L65,D579,R894,U716,R510,D932,L396,U766,L981,D115,L668,U197,R773,U898,L22,U294,L548,D634,L31,U626,R596,U442,L103,U448,R826,U511,R732,U680,L279,D693,R292,U641,R253,U977,R699,U861,R534,D482,L481,U929,L244,U863,L951,D744,R775,U198,L658,U700,L740,U725,R286,D105,L629,D117,L991,D778,L627,D389,R942,D17,L791,D515,R231,U418,L497,D421,L508,U91,R841,D823,L88,U265,L223,D393,L399,D390,L431,D553,R40,U724,L566,U121,L436,U797,L42,U13,R19,D858,R912,D571,L207,D5,L981,D996,R814,D918,L16,U872,L5,U281,R706,U596,R827,D19,R976,D664,L930,U56,R168,D892,R661,D751,R219,U343,R120,U21,L659,U976,R498,U282,R1,U721,R475,D798,L5,U396,R268,D454,R118,U260,L709,D369,R96,D232,L320,D763,R548,U670,R102,D253,L947,U845,R888,D645,L734,D734,L459,D638,L82,U933,L485,U235,R181,D51,L45,D979,L74,D186,L513,U974,R283,D493,R128,U909,L96,D861,L291,U640,R793,D712,R421,D315,L152,U220,L252,U642,R126,D417,R137,D73,R1,D711,R880,U718,R104,U444,L36,D974,L360,U12,L890,D337,R184,D745,R164,D931,R915,D999,R452,U221,L399,D761,L987,U562,R25,D642,R411,D605,R964");
	const auto& wire2 = CreateWire("L1010,U302,L697,D105,R618,U591,R185,U931,R595,D881,L50,D744,L320,D342,L221,D201,L862,D959,R553,D135,L238,U719,L418,U798,R861,U80,L571,U774,L896,U772,L960,U368,R415,D560,R276,U33,L532,U957,R621,D137,R373,U53,L842,U118,L299,U203,L352,D531,R118,U816,R355,U678,L983,D175,R652,U230,R190,D402,R111,D842,R756,D961,L82,U206,L576,U910,R622,D494,R630,D893,L200,U943,L696,D573,L143,D640,L885,D184,L52,D96,L580,U204,L793,D806,R477,D651,L348,D318,L924,D700,R675,D689,L723,D418,L156,D215,L943,D397,L301,U350,R922,D721,R14,U399,L774,U326,L14,D465,L65,U697,R564,D4,L40,D250,R914,U901,R316,U366,R877,D222,L672,D329,L560,U882,R321,D169,R161,U891,L552,U86,L194,D274,L567,D669,L682,U60,L985,U401,R587,U569,L1,D325,L73,U814,L338,U618,L49,U67,L258,D596,R493,D249,L310,D603,R810,D735,L829,D378,R65,U85,L765,D854,L863,U989,L595,U564,L373,U76,R923,U760,L965,U458,L610,U461,R900,U151,L650,D437,L1,U464,L65,D349,R256,D376,L686,U183,L403,D354,R867,U993,R819,D333,L249,U466,L39,D878,R855,U166,L254,D532,L909,U48,L980,U652,R393,D291,L502,U230,L738,U681,L393,U935,L333,D139,L499,D813,R302,D415,L693,D404,L308,D603,R968,U753,L510,D356,L356,U620,R386,D205,R587,U212,R715,U360,L603,U792,R58,U619,R73,D958,L53,D666,L756,U71,L621,D576,L174,U779,L382,U977,R890,D830,R822,U312,R716,U767,R36,U340,R322,D175,L417,U710,L313,D526,L573,D90,L493,D257,L918,U425,R93,D552,L691,U792,R189,U43,L633,U934,L953,U817,L404,D904,L384,D15,L670,D889,L648,U751,L928,D744,L932,U761,R879,D229,R491,U902,R134,D219,L634,U423,L241");
	
	std::set<Point> wire2_set(wire2.begin(), wire2.end());

	std::vector<Point> intersection;
	for (int i = 1; i < wire.size(); i++)
	{
		if (wire2_set.count(wire[i]) > 0)
			intersection.push_back(wire[i]);
	}

	// Part 1: Find the Manhattan distance to the intersection point closest to the central port?

	std::sort(intersection.begin(), intersection.end(),
		[](const Point& p1, const Point& p2) {
			return ManhattanDistance(p1, Point(0, 0)) < ManhattanDistance(p2, Point(0, 0)); }
	);
	std::cout << "Part 1:" << ManhattanDistance(Point(0, 0), intersection[0]) << std::endl;

	// Part 2: What is the fewest combined steps the wires must take to reach an intersection?

	long long shortest_distance = INT_MAX;
	for (auto p : wire)
	{
		auto dist = std::distance(wire.begin(), std::find(wire.begin(), wire.end(), p))
				  + std::distance(wire2.begin(), std::find(wire2.begin(), wire2.end(), p));
		if (dist < shortest_distance)
			shortest_distance = dist;
	}

	std::cout << "Part 2:" << shortest_distance << " (shortest_distance)" << std::endl;

	std::sort(intersection.begin(), intersection.end(),
		[&wire, &wire2](const Point& p1, const Point& p2) {
			auto p1_dist = std::distance(wire.begin(), std::find(wire.begin(), wire.end(), p1))
						 + std::distance(wire2.begin(), std::find(wire2.begin(), wire2.end(), p1));

			auto p2_dist = std::distance(wire.begin(), std::find(wire.begin(), wire.end(), p2))
			 			 + std::distance(wire2.begin(), std::find(wire2.begin(), wire2.end(), p2));

			return p1_dist < p2_dist; }
	);

	auto best_dist = std::distance(wire.begin(), std::find(wire.begin(), wire.end(), intersection[0]))
				   + std::distance(wire2.begin(), std::find(wire2.begin(), wire2.end(), intersection[0]));

	std::cout << "Part 2:" << best_dist << " (std::sort)" << std::endl;
}

std::vector<Point> CreateWire(std::string connectors) {
	std::istringstream file;
	file.str(connectors);

	std::vector<Point> wire;

	Point point(0, 0);
	wire.push_back(point);

	std::string token;
	while (std::getline(file, token, ','))
	{
		for (int i = 0; i < std::stoi(token.substr(1)); i++)
		{
			if (token[0] == 'R') {
				point.x += 1;
			}
			if (token[0] == 'U') {
				point.y += 1;
			}
			if (token[0] == 'L') {
				point.x -= 1;
			}
			if (token[0] == 'D') {
				point.y -= 1;
			}

			wire.push_back(point);
		}
	}

	return wire;
}

int ManhattanDistance(const Point& p1, const Point& p2)
{
	return abs(p1.x - p2.x) + abs(p1.y - p2.y);
}

//====================== Day 4 ============================

void Day4()
{
	int part1 = 0;
	int part2 = 0;

	// The value is within the range given in your puzzle input.
	for (int current = 382345; current <= 843167; current++)
	{
		if (Day4_Test(std::to_string(current)))
			part1++;

		if (Day4_Test2(std::to_string(current)))
			part2++;
	}

	std::cout << "Part 1: " << part1 << std::endl;
	std::cout << "Part 2: " << part2 << std::endl;
}

bool Day4_Test(const std::string& str)
{
	// It is a six - digit number.
	if (str.size() != 6) return false;

	bool hasAdjacent = false;
	
	int previous = str[0] - '0';

	// Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
	for (int current = 1; current < str.size(); current++)
	{
		auto current_value = str[current] - '0';

		if (previous > current_value) return false;
		else if (!hasAdjacent && previous == current_value) hasAdjacent = true;

		previous = current_value;
	}
	
	// Two adjacent digits are the same (like 22 in 122345).
	return hasAdjacent;
}

bool Day4_Test2(const std::string& str)
{
	// It is a six - digit number.
	if (str.size() != 6) return false;

	int previous = str[0] - '0';

	bool hasPair = false;
	int plateau = previous;
	int plateau_count = 1;

	// Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
	for (int current = 1; current < str.size(); current++)
	{
		auto current_value = str[current] - '0';

		if (previous > current_value) return false;

		if (current_value != plateau)
		{
			if (plateau_count == 2)
				hasPair = true;

			plateau = current_value;
			plateau_count = 1;
		}
		else {
			plateau_count++;
		}

		previous = current_value;
	}

	if (plateau_count == 2)
		hasPair = true;

	// Two adjacent digits are the same (like 22 in 122345).
	return hasPair;
}

//====================== Day 5 ============================

void Day5()
{
	std::vector<int> memory;
	auto file = std::ifstream("input_5.txt");
	std::string token;
	while (std::getline(file, token, ','))
	{
		memory.push_back(std::stoi(token));
	}
	
	auto intcode = Intcode { memory };
	intcode.push_arg(1);
	intcode.run();
	auto output = intcode.get_output().back();
	cout << "Day5 Part 1: " << output << endl;

	_ASSERT(output == 15386262);

	intcode = Intcode { memory };
	intcode.push_arg(5);
	intcode.run();
	output = intcode.get_output(0);
	cout << "Day5 Part 2: " << output << endl;

	_ASSERT(output == 10376124);
}

//====================== Day 6 ============================

void Day6()
{
	Day6_Part2();
}

void Day6_Part1()
{
	using namespace std;

	multimap<string, string> adjacents;

	auto file = std::ifstream("input_6.txt");
	std::string line;
	while (std::getline(file, line))
	{
		const auto delim = line.find(')');
		const auto from = line.substr(0, delim);
		const auto to = line.substr(delim + 1, line.size());
		adjacents.insert(pair<string, string>(from, to));
	}

	queue<string> queue;

	for (auto it = adjacents.begin(), end = adjacents.end(); it != end; it = adjacents.upper_bound(it->first))
		queue.push(it->first);

	int count = 0;
	while (!queue.empty())
	{
		const auto node = queue.front();
		queue.pop();

		auto neighbours = adjacents.equal_range(node);
		for (auto i = neighbours.first; i != neighbours.second; ++i)
		{
			count++;
			queue.push(i->second);
		}
	}

	cout << "Part 1: " << count << endl;
}

void Day6_Part2()
{
	using namespace std;

	multimap<string, string> adjacents;

	auto file = std::ifstream("input_6.txt");
	std::string line;
	while (std::getline(file, line))
	{
		const auto delim = line.find(')');
		const auto from = line.substr(0, delim);
		const auto to = line.substr(delim + 1, line.size());

		adjacents.insert(pair<string, string>(from, to));
		adjacents.insert(pair<string, string>(to, from));
	}

	const auto start = adjacents.find("YOU")->second;
	const auto end = adjacents.find("SAN")->second;

	queue<pair<string, int>> queue;
	queue.push(make_pair(start, 0));

	set<string> visited;

	while (!queue.empty())
	{
		const auto node = queue.front();
		queue.pop();

		if (visited.count(node.first) > 0) continue;
		visited.insert(node.first);

		if (node.first == end)
		{
			cout << "Part 2: " << node.second << endl;
			break;
		}

		auto neighbours = adjacents.equal_range(node.first);
		for (auto i = neighbours.first; i != neighbours.second; ++i)
		{
			queue.push(make_pair(i->second, node.second + 1));
		}
	}
}

//====================== Day 7 ============================

void Day7()
{
	Day7_Part1();
	Day7_Part2();
}

void Day7_Part1()
{
	using namespace std;

	std::vector<int> memory;
	auto file = std::ifstream("input_7.txt");
	std::string token;
	while (std::getline(file, token, ','))
	{
		memory.push_back(std::stoi(token));
	}

	const auto program = memory;

	const auto combinations = Day7_FindAllCombinations(vector<int> { 0,1,2,3,4 });

	long long best = 0;

	for (const auto combination : combinations)
	{
		long long input = 0;

		for (const auto amp : combination)
		{
			Intcode intcode { program };
			intcode.push_arg(amp);
			intcode.push_arg(input);
			intcode.run();
			input = intcode.get_output(0);
		}

		if (input > best)
		{
			best = input;
		}
	}

	cout << "Part 1: " << best << endl;
}

void Day7_Part2()
{
	using namespace std;
	
	std::vector<int> memory;
	auto file = std::ifstream("input_7.txt");
	std::string token;
	while (std::getline(file, token, ','))
	{
		memory.push_back(std::stoi(token));
	}

	const auto program = memory;
	
	const auto combinations = Day7_FindAllCombinations(vector<int> { 5, 6, 7, 8, 9 });

	long long best = 0;

	for (const auto combination : combinations)
	{
		long long input = 0;

		map<int, Intcode> amplifiers;

		auto running = true;
		while (running)
		{
			for (const auto amp : combination)
			{
				auto it = amplifiers.find(amp);

				if (it != amplifiers.end())
				{
					it->second.push_arg(input);
				}
				else
				{
					auto intcode = Intcode { program };
					intcode.push_arg(amp);
					intcode.push_arg(input);

					amplifiers.insert(make_pair(amp, move(intcode)));

					it = amplifiers.find(amp);
				}

				const auto retval = it->second.run();
				input = it->second.get_output(0);

				if (amp == combination[combination.size() - 1] && retval == 0)
				{
					running = false;
					break;
				}
			}
		}

		if (input > best)
		{
			best = input;
		}
	}

	cout << "Part 2: " << best << endl;
}

vector<vector<int>> Day7_FindAllCombinations(vector<int> input)
{
	vector<vector<int>> combinations;

	queue<vector<int>> q;
	for (const auto v : input)
		q.push(vector<int> { v });

	while (!q.empty())
	{
		const auto node = q.front();
		q.pop();

		if (node.size() == 5) {
			combinations.push_back(node);
			continue;
		}

		for (const auto v : input) {
			if (count(node.begin(), node.end(), v) == 0) {
				auto branch = node;
				branch.push_back(v);
				q.push(branch);
			}
		}
	}

	return combinations;
}

//====================== Day 8 ============================

void Day8()
{
	using namespace std;

	constexpr auto wide = 25;
	constexpr auto tall = 6;
	constexpr auto layer_size = wide * tall; // 25 pixels wide and 6 pixels tall.	

	auto file = std::ifstream("input_8.txt");

	int frames = 0;
	int pixels = 0;

	int best_frame = INT_MAX;
	int lowest = INT_MAX;
	int zeroes = 0;

	char read;
	while (file >> read)
	{
		if (read == '0')
			zeroes++;

		if (pixels > 0 && pixels % layer_size == 0) {

			if (zeroes < lowest) {
				lowest = zeroes;
				best_frame = frames;
			}

			zeroes = 0;
			frames++;
		}

		pixels++;
	}

	const auto offset = best_frame * layer_size;

	file.clear();
	file.seekg(offset);

	int ones = 0, twos = 0;
	for (auto i = offset; i < offset + layer_size; i++)
	{
		file >> read;
		if (read == '1') ones++;
		else if (read == '2') twos++;
	}

	cout << "Part 1: " << ones * twos << endl;

	file.clear();
	file.seekg(0);

	char image[tall][wide] = {};
	
	pixels = 0;
	while (file >> read)
	{
		const auto col = pixels % wide;
		const auto row = (pixels / wide) % tall;

		if (image[row][col] == '\0' && read != '2')
			image[row][col] = read;

		pixels++;
	}

	cout << "Part 2:" << endl;
	for (auto row = 0; row < tall; row++)
	{
		for (auto col = 0; col < wide; col++)
			cout << (image[row][col] == '0' ? ' ' : (char)219);
		cout << endl;
	}
}

//====================== Day 9 ============================

void Test_Intcode()
{
	Day2();
	Day5();
	Day9();

	// For example, consider the program 1002, 4, 3, 4, 33.
	//
	// The first instruction, 1002, 4, 3, 4, is a multiply instruction -
	// the rightmost two digits of the first value, 02, indicate opcode 2, multiplication.
	// Then, going right to left, the parameter modes are 0 (hundreds digit), 1 (thousands digit), and 0 (ten - thousands digit, not present and therefore zero)
	
	Intcode test_multiply_im { vector<int> { 1002,4,3,4,33 } };
	test_multiply_im.run();
	_ASSERT(test_multiply_im.get_output().size() == 0);
	cout << "test_multiply_im: " << test_multiply_im.get_output().size() << endl;

	// 3,9,8,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
	Intcode test_equal_to_pm { vector<int> { 3,9,8,9,10,9,4,9,99,-1,8 } };
	test_equal_to_pm.push_arg(8);
	test_equal_to_pm.run();
	auto output = test_equal_to_pm.get_output(0);
	_ASSERT(output == 1);
	cout << "" << "test_equal_to_pm 8 == 8: " << output << endl;

	test_equal_to_pm = Intcode{ vector<int> { 3,9,8,9,10,9,4,9,99,-1,8 } };
	test_equal_to_pm.push_arg(4);
	test_equal_to_pm.run();
	output = test_equal_to_pm.get_output()[0];
	_ASSERT(output == 0);
	cout << "test_equal_to_pm 8 == 4: " << output << endl;

	// 3,9,7,9,10,9,4,9,99,-1,8 - Using position mode, consider whether the input is less than to 8; output 1 (if it is) or 0 (if it is not).
	Intcode test_less_than_pm{ vector<int> { 3,9,7,9,10,9,4,9,99,-1,8 } };
	test_less_than_pm.push_arg(8);
	test_less_than_pm.run();
	output = test_less_than_pm.get_output(0);
	_ASSERT(output == 0);
	cout << "" << "test_less_than_pm 8 > 8: " << output << endl;

	test_less_than_pm = Intcode{ vector<int> { 3,9,7,9,10,9,4,9,99,-1,8 } };
	test_less_than_pm.push_arg(4);
	test_less_than_pm.run();
	output = test_less_than_pm.get_output(0);
	_ASSERT(output == 1);
	cout << "test_less_than_pm 4 > 8: " << output << endl;

	test_less_than_pm = Intcode{ vector<int> { 3,9,7,9,10,9,4,9,99,-1,8 } };
	test_less_than_pm.push_arg(10);
	test_less_than_pm.run();
	output = test_less_than_pm.get_output(0);
	_ASSERT(output == 0);
	cout << "test_less_than_pm 10 > 8: " << output << endl;

	// 3,3,1108,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is equal to 8; output 1 (if it is) or 0 (if it is not).
	Intcode test_equal_to_im{ vector<int> { 3,3,1108,-1,8,3,4,3,99 } };
	test_equal_to_im.push_arg(8);
	test_equal_to_im.run();
	output = test_equal_to_im.get_output(0);
	_ASSERT(output == 1);
	cout << "test_equal_to_im 8 == 8: " << output << endl;

	test_equal_to_im = Intcode{ vector<int> { 3,3,1108,-1,8,3,4,3,99 } };
	test_equal_to_im.push_arg(9);
	test_equal_to_im.run();
	output = test_equal_to_im.get_output(0);
	_ASSERT(output == 0);
	cout << "test_equal_to_im 9 == 8: " << output << endl;

	// 3,3,1107,-1,8,3,4,3,99 - Using immediate mode, consider whether the input is less than 8; output 1 (if it is) or 0 (if it is not).
	Intcode test_less_than_im{ vector<int> { 3,3,1107,-1,8,3,4,3,99 } };
	test_less_than_im.push_arg(8);
	test_less_than_im.run();
	output = test_less_than_im.get_output(0);
	_ASSERT(output == 0);
	cout << "test_less_than_im 8 > 8: " << output << endl;

	// Here are some jump tests that take an input,
	// then output 0 if the input was zero or 1 if the input was non-zero:
	
	Intcode test_jump_pm { vector<int> { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 } };
	test_jump_pm.push_arg(0);
	test_jump_pm.run();
	output = test_jump_pm.get_output(0);
	_ASSERT(output == 0);
	cout << "test_jump_pm 0: " << output << endl;

	test_jump_pm = { vector<int> { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 } };
	test_jump_pm.push_arg(5);
	test_jump_pm.run();
	output = test_jump_pm.get_output(0);
	_ASSERT(output == 1);
	cout << "test_jump_pm 5: " << output << endl;

	Intcode test_jump_im { vector<int> { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 } };
	test_jump_im.push_arg(0);
	test_jump_im.run();
	output = test_jump_im.get_output(0);
	_ASSERT(output == 0);
	cout << "test_jump_im 0: " << output << endl;

	test_jump_im = { vector<int> { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 } };
	test_jump_im.push_arg(5);
	test_jump_im.run();
	output = test_jump_im.get_output(0);
	_ASSERT(output == 1);
	cout << "test_jump_im 5: " << output << endl;

	// The program will then output 999 if the input value is below 8, output 1000 if the input value is equal to 8, or output 1001 if the input value is greater than 8.
	Intcode test_less_than_im_2{ vector<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 } };
	test_less_than_im_2.push_arg(8);
	test_less_than_im_2.run();
	output = test_less_than_im_2.get_output(0);
	_ASSERT(output == 1000);
	cout << "test_less_than_im_2 8 >= 8: " << output << endl;

	test_less_than_im_2 = Intcode { vector<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 } };
	test_less_than_im_2.push_arg(5);
	test_less_than_im_2.run();
	output = test_less_than_im_2.get_output(0);
	_ASSERT(output == 999);
	cout << "test_less_than_im_2 5 >= 8: " << output << endl;

	test_less_than_im_2 = Intcode{ vector<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 } };
	test_less_than_im_2.push_arg(12);
	test_less_than_im_2.run();
	output = test_less_than_im_2.get_output(0);
	_ASSERT(output == 1001);
	cout << "test_less_than_im_2 12 >= 8: " << output << endl;

	auto test_large_numbers_1 = Intcode { vector<long long> { { 1102,34915192,34915192,7,4,7,99,0 } } };
	test_large_numbers_1.run();
	output = test_large_numbers_1.get_output(0);
	_ASSERT(output == 1219070632396864);
	cout << "test_large_numbers_1: " << output << endl;

	auto test_large_numbers_2 = Intcode{ vector<long long> { { 104,1125899906842624,99 } } };
	test_large_numbers_2.run();
	output = test_large_numbers_2.get_output(0);
	_ASSERT(output == 1125899906842624);
	cout << "test_large_numbers_2: " << output << endl;

	auto test1 = Intcode{ vector<long long> { { 109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99 } } };
	test1.run();
	cout << "copy of itself: ";
	for (auto o : test1.get_output())
		cout << o << ",";
	cout << endl;
	cout << "     should be: 109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99," << endl;

	auto test_large_multiply = Intcode{ vector<long long> { { 1102,34915192,34915192,7,4,7,99,0 } } };
	test_large_multiply.run();
	output = test_large_multiply.get_output(0);
	_ASSERT(34915192LL * 34915192LL == output);
	cout << "test_large_multiply: " << test_large_multiply.get_output(0) << endl;

	auto test_large = Intcode{ vector<long long> { { 104,1125899906842624,99 } } };
	test_large.run();
	output = test_large.get_output(0);
	_ASSERT(1125899906842624LL == output);
	cout << "test_large: " << test_large.get_output(0) << endl;

	auto t1 = Intcode{ vector<long long> { { 109,1,9,2,204,-6,99 } } };
	t1.run();
	_ASSERT(204 == t1.get_output(0));
	cout << "t1: " << t1.get_output(0) << endl;

	auto t2 = Intcode{ vector<long long> { { 109,1,109,9,204,-6,99 } } };
	t2.run();
	_ASSERT(204 == t2.get_output(0));
	cout << "t2: " << t2.get_output(0) << endl;

	auto test_rel_base = Intcode{ vector<long long> { { 109,1,203,11,209,8,204,1,99,10,0,42,0 } } };
	test_rel_base.push_arg(33);
	test_rel_base.run();
	_ASSERT(test_rel_base.get_output(0) == 33);
	cout << "test_rel_base: " << test_rel_base.get_output(0) << endl;

	auto some_test = Intcode{ vector<long long> { { 109,6,21001,9,25,1,104,0,99,49 } } };
	some_test.run();
	_ASSERT(some_test.get_output(0) == 74);
	cout << "some_test: " << some_test.get_output(0) << endl;
}

void Day9()
{
	using namespace std;
	
	std::vector<long long> memory;
	auto file = std::ifstream("input_9.txt");
	std::string token;
	while (std::getline(file, token, ','))
	{
		memory.push_back(stoll(token));
	}

	auto intcode = Intcode { memory };
	intcode.push_arg(1);
    intcode.run();
	_ASSERT(intcode.get_output(0) == 3598076521);
	cout << "Day9 Part 1: " << intcode.get_output(0) << endl;

	intcode = { memory };
	intcode.push_arg(2);
	intcode.run();
	_ASSERT(intcode.get_output(0) == 90722);
	cout << "Day9 Part 2: " << intcode.get_output(0) << endl;
}
