package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

type marker struct {
	chunk       int
	multiplier  int
	marker_size int
}

func parse_marker(str string, offset int) marker {
	groupEnd := strings.IndexRune(str[offset:], ')')
	tokens := strings.Split(str[offset+1:offset+groupEnd], "x")
	size, _ := strconv.Atoi(tokens[0])
	multiplier, _ := strconv.Atoi(tokens[1])
	return marker{size, multiplier, groupEnd}
}

func process_p1(str string) int {
	decompress_size := 0
	for i := 0; i < len(str); i++ {
		if str[i] == '(' {
			marker := parse_marker(str, i)
			decompress_size += marker.multiplier * marker.chunk
			i += marker.marker_size + marker.chunk
		} else {
			decompress_size++
		}
	}

	return decompress_size
}

func sub_process_p2(str string, offset int, end int) int {
	decompress_size := 0
	for offset < end {
		if str[offset] == '(' {
			marker := parse_marker(str, offset)
			offset += marker.marker_size + 1
			decompress_size += marker.multiplier * sub_process_p2(str, offset, offset+marker.chunk)
			offset += marker.chunk
		} else {
			decompress_size++
			offset++
		}
	}
	return decompress_size
}

func process_p2(str string) int {
	return sub_process_p2(str, 0, len(str))
}

func solve_2016_day_9() {
	dat, _ := os.ReadFile("2016_day9.txt")
	lines := strings.Split(string(dat), "\n")
	fmt.Printf("part 1: %d\n", process_p1(lines[0]))
	fmt.Printf("part 2: %d\n", process_p2(lines[0]))
}
