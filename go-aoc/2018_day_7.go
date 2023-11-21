package main

import (
	"fmt"
	"os"
	"sort"
	"strings"
)

type Worker struct {
	step byte
	time int
}

func remove_dep(needle byte, deps map[byte][]byte) {
	for key, dep := range deps {
		deps[key] = remove_value(needle, dep)
	}
}

func remove_value(needle byte, values []byte) []byte {
	out := make([]byte, 0, len(values))
	for _, e := range values {
		if e != needle {
			out = append(out, e)
		}
	}
	return out
}

func take_next(steps []byte, deps map[byte][]byte) byte {
	for _, step := range steps {
		if len(deps[step]) == 0 {
			return step
		}
	}
	return 0 // poor man's error handling
}

func solve_2018_day_7() {
	data, _ := os.ReadFile("2018_day_7.txt")

	var steps = make(map[byte]int)
	var deps = make(map[byte][]byte)

	for _, line := range strings.Split(string(data), "\n") {
		if line == "" {
			continue
		}

		// Step C must be finished before step A can begin.
		stepBefore := line[5]
		stepAfter := line[36]

		steps[stepBefore]++
		steps[stepAfter]++

		deps[stepAfter] = append(deps[stepAfter], stepBefore)
	}

	keys := make([]byte, 0, len(steps))
	for k := range steps {
		keys = append(keys, k)
	}
	sort.Slice(keys, func(i, j int) bool { return keys[i] < keys[j] })

	total := len(keys)
	processed := make([]byte, 0)
	workers := make([]Worker, 2)
	tick := 0

	for len(processed) < total {
		for i := range workers {
			if workers[i].time == 0 {
				if workers[i].step != 0 {
					processed = append(processed, workers[i].step)
					fmt.Printf("At tick %d: Worker %d processed %c\n", tick, i, workers[i].step)
					remove_dep(workers[i].step, deps)
					workers[i].step = 0
					workers[i].time = 0
				}

				step := take_next(keys, deps)
				if step == 0 {
					continue
				}

				keys = remove_value(step, keys)
				workers[i].step = step
				workers[i].time = int(step) - 64
				if workers[i].time < 0 {
					workers[i].time = 0
				}

				fmt.Printf("At tick %d: Worker %d started processing %c (it will take %d seconds)\n", tick, i, workers[i].step, workers[i].time)
			}
		}

		for i := range workers {
			if workers[i].time > 0 {
				workers[i].time--
			}
		}

		tick++
	}

	for _, step := range processed {
		fmt.Printf("%c", step)
	}
}
