package main

import (
	"os"
	"sort"
	"strconv"
	"strings"
)

func evaluate_precondition(operator string, register int, term int) bool {
	switch operator {
	case ">":
		return register > term
	case "<":
		return register < term
	case "<=":
		return register <= term
	case ">=":
		return register >= term
	case "==":
		return register == term
	case "!=":
		return register != term
	default:
		panic(operator)
	}
}

func evaluate_expression(operator string, register int, term int) int {
	switch operator {
	case "inc":
		return register + term
	case "dec":
		return register - term
	default:
		panic(operator)
	}
}

func solve_2017_day_8_part1() {
	highest_reg := 0
	registers := make(map[string]int)
	data, _ := os.ReadFile("2017_day_8.txt")
	for _, line := range strings.Split(string(data), "\n") {
		if line == "" {
			continue
		}

		tokens := strings.Split(line, " ")

		// b inc 5 if a > 1
		expr_reg := registers[tokens[0]]
		expr_operator := tokens[1]
		expr_term, _ := strconv.Atoi(tokens[2])
		precond_reg := registers[tokens[4]]
		precond_operator := tokens[5]
		precond_term, _ := strconv.Atoi(tokens[6])

		if evaluate_precondition(precond_operator, precond_reg, precond_term) {
			new_value := evaluate_expression(expr_operator, expr_reg, expr_term)
			registers[tokens[0]] = new_value
			if new_value > highest_reg {
				highest_reg = new_value
			}
		}
	}

	values := make([]int, 0)
	for _, v := range registers {
		values = append(values, v)
	}
	sort.Ints(values)

	println(values[len(values)-1])
	println(highest_reg)
}
