package main

import (
	"fmt"
	"sort"
)

type ByPair []Pair

func (u ByPair) Len() int {
	return len(u)
}
func (u ByPair) Swap(i, j int) {
	u[i], u[j] = u[j], u[i]
}
func (u ByPair) Less(i, j int) bool {
	if u[i].microchip < u[j].microchip {
		return true
	}
	return u[i].generator < u[j].generator
}

type Pair struct {
	microchip int
	generator int
}

type State struct {
	floors []int
	floor  int
	steps  int
}

func slice_equals(a, b []int) bool {
	for i := 0; i < len(a); i++ {
		if a[i] != b[i] {
			return false
		}
	}
	return true
}

func hash_state(state State) string {
	mid := len(state.floors) / 2
	pairs := make([]Pair, mid)
	for i := 0; i < mid; i++ {
		pairs[i] = Pair{state.floors[i], state.floors[i+mid]}
	}
	sort.Sort(ByPair(pairs))
	return fmt.Sprint(state.floor, fmt.Sprint(pairs))
}

func reached_goal(state State) bool {
	for _, v := range state.floors {
		if v != 3 {
			return false
		}
	}
	return true
}

func is_valid_state(state State) bool {
	floors := state.floors
	mid := int(len(floors) / 2)

	for i := 0; i < mid; i++ {
		is_chip_shielded := floors[i] == floors[i+mid]

		for y := mid; y < len(floors); y++ {
			same_floor := floors[i] == floors[y]
			is_corresponding_gen := y == i+mid

			if same_floor && !is_corresponding_gen && !is_chip_shielded {
				// microchip fried by non-corresponding RTG
				return false
			}
		}
	}

	return true
}

func explore(state State) []State {
	states := make([]State, 0)

	for i := 0; i < len(state.floors); i++ {
		// bring two items
		for j := 0; j < len(state.floors); j++ {
			if i != j {
				if state.floors[i] == state.floor && state.floors[j] == state.floor {
					if state.floor < 3 {
						up := make([]int, len(state.floors))
						copy(up, state.floors)
						up[i]++
						up[j]++
						states = append(states, State{up, state.floor + 1, state.steps + 1})
					}
					if state.floor > 0 {
						down := make([]int, len(state.floors))
						copy(down, state.floors)
						down[i]--
						down[j]--
						states = append(states, State{down, state.floor - 1, state.steps + 1})
					}
				}
			}
		}

		// bring one item
		if state.floors[i] == state.floor {
			if state.floor < 3 {
				up := make([]int, len(state.floors))
				copy(up, state.floors)
				up[i]++
				states = append(states, State{up, state.floor + 1, state.steps + 1})
			}
			if state.floor > 0 {
				down := make([]int, len(state.floors))
				copy(down, state.floors)
				down[i]--
				states = append(states, State{down, state.floor - 1, state.steps + 1})
			}
		}
	}

	return states
}

func solve_2016_day11() {

	seen := make(map[string]struct{}, 0)
	queue := make([]State, 0)

	//fmt.Println([]string{"hym", "lim", "hyg", "lig"})
	//start := State{[]int{0, 0, 1, 2}, 0, 0}

	//fmt.Println([]string{"com", "cum", "rum", "plm", "prm", "cog", "cug", "rug", "plg", "prg"})
	//start := State{[]int{2, 2, 2, 2, 0, 1, 1, 1, 1, 0}, 0, 0}
	start := State{[]int{2, 2, 2, 2, 0, 0, 0, 1, 1, 1, 1, 0, 0, 0}, 0, 0}
	seen[hash_state(start)] = struct{}{}

	queue = append(queue, start)

	for len(queue) > 0 {
		top := queue[0]
		queue = queue[1:]

		if reached_goal(top) {
			fmt.Println("Reached goal", top.steps)
			break
		}

		for _, next := range explore(top) {
			if is_valid_state(next) {
				hash := hash_state(next)
				_, found := seen[hash]
				if !found {
					seen[hash] = struct{}{}
					queue = append(queue, next)
				}
			}
		}
	}
}
