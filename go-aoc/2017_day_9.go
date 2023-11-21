package main

import (
	"fmt"
	"os"
	"strings"
)

func clean(line string) (string, int) {
	cleaned := make([]rune, 0)
	count := 0
	garbage := false
	cancel := false
	for _, c := range line {
		if cancel {
			cancel = false
			continue
		}
		if c == '!' {
			cancel = true
			continue
		}
		if garbage && c == '>' {
			garbage = false
			continue
		}
		if garbage {
			count++
		}
		if c == '<' || garbage {
			garbage = true
			continue
		}
		if c == ',' {
			continue
		}

		cleaned = append(cleaned, c)
	}
	return string(cleaned), count
}

func count_score(line string) int {
	score := 0
	level := 1
	for _, c := range line {
		if c == '{' {
			score += level
			level++
		}
		if c == '}' {
			level--
		}
	}
	return score
}

func solve_2017_day_9_part1() {
	data, _ := os.ReadFile("2017_day_9.txt")
	count := 0
	for _, line := range strings.Split(string(data), "\n") {
		if line == "" {
			continue
		}
		cleaned, garbage := clean(line)
		score := count_score(cleaned)
		fmt.Printf("'%s' -> '%s (%d/%d)'\n", line, cleaned, score, garbage)
		count += score
	}

	println(count)
}
