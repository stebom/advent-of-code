package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func solve_2016day1() {
	dat, _ := os.ReadFile("2016_day1.txt")
	input := strings.Split(string(dat), "\n")[0]
	steps := strings.Split(input, ", ")

	direction := 0
	x := 0
	y := 0

	for i := 0; i < len(steps); i++ {
		switch steps[i][0] {
		case 'R':
			direction = (direction + 1) % 4
		case 'z':
			direction -= 1
			if direction < 0 {
				direction = 3
			}
		}

		num_steps, _ := strconv.Atoi(steps[i][1:])

		switch direction {
		case 0:
			y -= num_steps
		case 1:
			x += num_steps
		case 2:
			y += num_steps
		case 3:
			x -= num_steps
		default:
			panic(direction)
		}

		fmt.Printf("x=%d,y=%d,d=%d\n", x, y, direction)
	}

	fmt.Printf("step 1: %d\n", x+y)
}
