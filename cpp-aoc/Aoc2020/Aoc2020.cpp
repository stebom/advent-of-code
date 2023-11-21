#include <fstream>
#include <sstream>
#include <iostream>
#include <map>
#include <bitset>
#include <array>
#include <algorithm>
#include <vector>

enum class Direction { R, L };
std::pair<uint8_t, uint8_t> half(uint8_t min, uint8_t max, Direction dir);
Direction convert(char c);

std::pair<uint8_t, uint8_t> get_seat(const std::string s);
int get_seat_id(const std::pair<uint8_t, uint8_t> seat);

int main()
{
    std::map<uint8_t, uint8_t> seats{};

    int max = 0;
    auto file = std::ifstream("day5.txt");
    for (std::string line; std::getline(file, line); )
    {
        const auto seat = get_seat(line);
        const auto seatId = get_seat_id(seat);
        seats[seat.first] |= 1 << seat.second;

        if (seatId > max) max = seatId;
    }

    std::cout << "max: " << max << "\n";

    for (auto seat : seats) {
        std::bitset<8> b { seat.second };
        if (!b.all()) {
            for (int c = 0; c < 8; ++c) {
                if (b[c] == 0) {
                    std::cout << "Seat ID " << (int)get_seat_id({ seat.first, c }) << " is empty\n";
                }
            }
        }
    }
}

Direction convert(char c) {
    if (c == 'R' || c == 'B') return Direction::R;
    else if (c == 'L' || c == 'F') return Direction::L;
    throw new std::exception("Bad conversion");
}

std::pair<uint8_t, uint8_t> half(uint8_t min, uint8_t max, Direction dir) {
    const int range = max - min;
    if (dir == Direction::R) return { ceil(min + range / 2.0), max };
    else if (dir == Direction::L) return { min, floor(max - range / 2.0) };
    throw new std::exception("Bad direction");
}

std::pair<uint8_t, uint8_t> get_seat(const std::string s) {
    std::pair<uint8_t, uint8_t> row { 0, 127 };
    for (int i = 0; i < 7; i++)
        row = half(row.first, row.second, convert(s[i]));
    
    std::pair<uint8_t, uint8_t> col { 0, 7 };
    for (int i = 7; i < 10; i++)
        col = half(col.first, col.second, convert(s[i]));

    return { row.first, col.first };
}

int get_seat_id(const std::pair<uint8_t, uint8_t> seat) {
    // multiply the row by 8, then add the column
    return seat.first * 8 + seat.second;
}
