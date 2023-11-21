package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
)

type State struct {
	table     []string
	happiness int
}

func buildAdjacents() map[string]map[string]int {
	re := regexp.MustCompile("(.+) would (.+) (.+) happiness units by sitting next to (.+).")
	data, _ := os.ReadFile("aoc_2015_day_13/2015_day_13.txt")
	matches := re.FindAllStringSubmatch(string(data), -1)

	adjacent := make(map[string]map[string]int)

	for _, match := range matches {
		happiness, _ := strconv.Atoi(match[3])
		if match[2] == "lose" {
			happiness = -happiness
		}
		_, exists := adjacent[match[1]]
		if !exists {
			adjacent[match[1]] = make(map[string]int)
		}

		adjacent[match[1]][match[4]] = happiness
	}

	return adjacent
}

func contains(attendee string, state State) bool {
	for _, a := range state.table {
		if a == attendee {
			return true
		}
	}
	return false
}

func solve(adjacent map[string]map[string]int, part2 bool) int {
	if part2 {
		adjacent["me"] = make(map[string]int)
		for k, m := range adjacent {
			adjacent["me"][k] = 0
			m["me"] = 0
		}
	}

	queue := []State{}
	for attendee := range adjacent {
		queue = append(queue, State{[]string{attendee}, 0})
	}

	best := 0
	for len(queue) > 0 {
		top := queue[0]
		queue = queue[1:]

		if len(top.table) == len(adjacent) {
			first := top.table[0]
			last := top.table[len(top.table)-1]
			top.happiness += adjacent[first][last]
			top.happiness += adjacent[last][first]

			if top.happiness > best {
				best = top.happiness
			}
			continue
		}

		for next := range adjacent[top.table[len(top.table)-1]] {
			if !contains(next, top) {
				new_state := State{make([]string, len(top.table)+1), top.happiness}
				copy(new_state.table, top.table)
				new_state.table[len(top.table)] = next

				prev := top.table[len(top.table)-1]

				new_state.happiness += adjacent[prev][next]
				new_state.happiness += adjacent[next][prev]

				queue = append(queue, new_state)
			}
		}
	}

	return best
}

func main() {
	adjacent := buildAdjacents()
	fmt.Println(solve(adjacent, false))
	fmt.Println(solve(adjacent, true))
}
