package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

var programs = make(map[string][]string)
var weights = make(map[string]int)

func get_child_weights(program string) []int {
	w := []int{}
	for _, v := range programs[program] {
		w = append(w, sum_weight(v))
	}
	return w
}

func count_relations() map[string]int {
	count := make(map[string]int)
	for k := range programs {
		count[k] = 0
	}

	for _, v := range programs {
		for _, d := range v {
			count[d]++
		}
	}
	return count
}

func sum_weight(program string) int {
	sum := weights[program]
	for _, v := range programs[program] {
		sum += sum_weight(v)
	}
	return sum
}

func traverse(program string) {
	fmt.Print(program)
	fmt.Print(" -> ")
	for _, v := range programs[program] {
		traverse(v)
	}
}

func solve_2017_day_7_part1() {
	dat, _ := os.ReadFile("2017_day_7.txt")
	for _, line := range strings.Split(string(dat), "\n") {
		if line == "" {
			continue
		}

		name := line[:strings.Index(line, " ")]
		weight, _ := strconv.Atoi(line[strings.Index(line, "(")+1 : strings.Index(line, ")")])

		programs[name] = []string{}
		weights[name] = weight

		groups := strings.Split(line, " -> ")
		if len(groups) == 2 {
			deps := strings.Split(groups[1], ", ")
			programs[name] = append(programs[name], deps...)
		}
	}

	count := count_relations()
	for k, v := range count {
		if v == 0 {
			fmt.Printf("Part 1: %s\n", k)
		}
	}

	for k, v := range programs {
		if len(v) > 2 {
			mismatch := false
			child_weights := get_child_weights(k)
			for i := 1; i < len(child_weights); i++ {
				if child_weights[i-1] != child_weights[i] {
					mismatch = true
				}
			}

			if mismatch {
				fmt.Printf("child weights of %s (%d) ", k, sum_weight(k))
				fmt.Println(v)
				fmt.Println(child_weights)
			}
		}
	}

	child_weights := get_child_weights("rfkvap")
	fmt.Println(child_weights)
	fmt.Println(weights["rfkvap"] - 9)
}
