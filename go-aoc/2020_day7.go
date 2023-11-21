package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
	"time"
)

func find_bag(needle string, bag string, bags map[string][]string) bool {
	if bag == needle {
		return true
	}
	for _, v := range bags[bag] {
		if find_bag(needle, v, bags) {
			return true
		}
	}
	return false
}

func count_bags_ptr(bag string, bags map[string][]string, count *int) {
	for _, v := range bags[bag] {
		*count++
		count_bags_ptr(v, bags, count)
	}
}

var precalc = make(map[string]int)

func count_bags(bag string, bags map[string][]string, cache bool) int {
	if cache {
		if v, ok := precalc[bag]; ok {
			return v
		}
	}

	count := 0
	for _, v := range bags[bag] {
		count += count_bags(v, bags, cache) + 1
	}

	if cache {
		precalc[bag] = count
	}
	return count
}

func solve_2020day7_part1() {
	dat, _ := os.ReadFile("2020_day7.txt")
	lines := strings.Split(string(dat), "\n")

	m := make(map[string][]string)
	for _, line := range lines {
		if line == "" {
			continue
		}

		line = line[:len(line)-1]
		line = strings.ReplaceAll(line, " bags", "")
		line = strings.ReplaceAll(line, " bag", "")
		tokens := strings.Split(line, " contain ")

		m[tokens[0]] = []string{}
		if tokens[1] != "no other" {
			bags := strings.Split(tokens[1], ", ")
			for i := 0; i < len(bags); i++ {
				sub_bag := string(bags[i][2:])
				m[tokens[0]] = append(m[tokens[0]], sub_bag)
			}
		}
	}

	count := 0
	for k := range m {
		if k != "shiny gold" && find_bag("shiny gold", k, m) {
			count++
		}
	}

	fmt.Printf("Part 1: %d\n", count)
}

func solve_2020day7_part2() {
	dat, _ := os.ReadFile("2020_day7.txt")
	lines := strings.Split(string(dat), "\n")

	m := make(map[string][]string)
	for _, line := range lines {
		if line == "" {
			continue
		}

		line = line[:len(line)-1]
		line = strings.ReplaceAll(line, " bags", "")
		line = strings.ReplaceAll(line, " bag", "")
		tokens := strings.Split(line, " contain ")

		m[tokens[0]] = []string{}
		if tokens[1] != "no other" {
			bags := strings.Split(tokens[1], ", ")
			for i := 0; i < len(bags); i++ {
				sub_bag := string(bags[i][2:])
				num_bags, _ := strconv.Atoi(string(bags[i][0]))
				for y := 0; y < num_bags; y++ {
					m[tokens[0]] = append(m[tokens[0]], sub_bag)
				}
			}
		}
	}

	//count := 0
	//count_bags_ptr("shiny gold", m, &count)

	start := time.Now()
	count := count_bags("shiny gold", m, false)
	duration := time.Since(start)
	fmt.Printf("not using cache duration: %d μs\n", duration.Microseconds())

	start = time.Now()
	count = count_bags("shiny gold", m, true)
	duration = time.Since(start)
	fmt.Printf("using cache duration: %d μs\n", duration.Microseconds())

	fmt.Printf("Part 1: %d\n", count)
}
