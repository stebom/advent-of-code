package main

import (
	"fmt"
	"os"
	"strings"
)

func solve_2020day6_part1() {
	dat, _ := os.ReadFile("2020_day6.txt")
	groups := strings.Split(string(dat), "\n\n")

	count := 0
	for i := 0; i < len(groups); i++ {
		group := strings.ReplaceAll(groups[i], "\n", "")

		m := make(map[byte]int)
		for x := 0; x < len(group); x++ {
			m[group[x]]++
		}

		count += len(m)
	}

	fmt.Printf("Part 1: %d\n", count)
}

func solve_2020day6_part2() {
	dat, _ := os.ReadFile("2020_day6.txt")
	groups := strings.Split(string(dat), "\n\n")

	count := 0
	for i := 0; i < len(groups); i++ {
		group := strings.Split(groups[i], "\n")
		group_count := 0

		m := make(map[byte]int)
		for y := 0; y < len(group); y++ {
			if group[y] != "" {
				group_count += 1

				for x := 0; x < len(group[y]); x++ {
					m[group[y][x]]++
				}
			}
		}

		for _, value := range m {
			if value == group_count {
				count += 1
			}
		}
	}

	fmt.Printf("Part 2: %d\n", count)
}
