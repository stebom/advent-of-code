
#include <fstream>
#include <iterator>
#include <iostream>
#include <vector>
#include <sstream>
#include <string>

namespace Aoc_2018_Day8 {

	struct node {
		int num_child_nodes;
		int num_metadata_entries;
		std::vector<node> child_nodes;
		std::vector<int> metadata_entries;
	};

	void sum_value(const node& current, long& sum) {
		if (current.num_child_nodes == 0) {
			for (const auto meta : current.metadata_entries) {
				sum += meta;
			}
		}
		else {
			// the metadata entries become indexes which refer to those child nodes.
			// A metadata entry of 1 refers to the first child node, 2 to the second, 3 to the third, and so on.
			for (const auto meta : current.metadata_entries) {
				const auto index = meta - 1;
				if (index >= 0 && index < current.num_child_nodes) {
					sum_value(current.child_nodes.at(index), sum);
				}
			}
		}
	}

	void sum_metadata(const node& current, long& sum) {
		for (const auto& child : current.child_nodes) {
			sum_metadata(child, sum);
		}

		for (const auto meta : current.metadata_entries) {
			sum += meta;
		}
	}

	void parse(const std::vector<int> &values, node& current, size_t& offset) {
		current.num_child_nodes = values[offset++];
		current.num_metadata_entries = values[offset++];

		for (auto i = 0; i < current.num_child_nodes; ++i) {
			current.child_nodes.push_back({});
			node& child = current.child_nodes[current.child_nodes.size() - 1];
			parse(values, child, offset);
		}

		for (auto i = 0; i < current.num_metadata_entries; ++i) {
			current.metadata_entries.push_back(values[offset++]);
		}
	}

	void parse(const std::string& line) {
		std::vector<int> values;

		std::string token;
		for (auto c : line) {
			if (c == ' ' && token.size() > 0) {
				values.push_back(std::stoi(token));
				token.clear();
			}
			token.push_back(c);
		}
		values.push_back(std::stoi(token));
		
		node head {};
		size_t offset = 0;
		parse(values, head, offset);

		long total_metadata = 0;
		sum_metadata(head, total_metadata);
		long total_value = 0;
		sum_value(head, total_value);
		std::cout << "total metadata: " << total_metadata << std::endl;
		std::cout << "total value: " << total_value << std::endl;
	}

	void run() {
		std::cout << "Running Aoc_2018_Day8" << std::endl;
		std::fstream f{ "2018_day_8.txt" };

		std::string line;
		while (std::getline(f, line)) {
			parse(line);
		}
	}

}
