package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
)

func is_register(token string) bool {
	switch token {
	case "a", "b", "c", "d":
		return true
	}
	return false
}

func solve_2016_day12() {
	inc := regexp.MustCompile("inc ([a-d])")
	dec := regexp.MustCompile("dec ([a-d])")
	cpy := regexp.MustCompile("cpy (.+) ([a-d])")
	jnz := regexp.MustCompile("jnz (.+) (.+)")

	registers := make(map[string]int)
	registers["a"] = 0
	registers["b"] = 0
	registers["c"] = 1
	registers["d"] = 0

	data, _ := os.ReadFile("2016_day_12.txt")
	instructions := strings.Split(string(data), "\n")

	ip := 0
	for ip < len(instructions)-1 {
		line := instructions[ip]

		r := inc.FindStringSubmatch(line)
		if r != nil {
			registers[r[1]]++
			ip++
			continue
		}

		r = dec.FindStringSubmatch(line)
		if r != nil {
			registers[r[1]]--
			ip++
			continue
		}

		r = cpy.FindStringSubmatch(line)
		if r != nil {
			if is_register(r[1]) {
				registers[r[2]] = registers[r[1]]
			} else {
				x, _ := strconv.Atoi(r[1])
				registers[r[2]] = x
			}
			ip++
			continue
		}

		r = jnz.FindStringSubmatch(line)
		if r != nil {
			y, _ := strconv.Atoi(r[2])
			eval := false

			if is_register(r[1]) {
				eval = registers[r[1]] != 0
			} else {
				x, _ := strconv.Atoi(r[1])
				eval = x != 0
			}

			if eval {
				ip += y
			} else {
				ip++
			}

			continue
		}

		panic("unhandled case")
	}

	fmt.Println(registers)
}
