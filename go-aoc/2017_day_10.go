package main

import "strconv"

func las(input []int) (result []int) {
	cur := input[0]
	counter := 0
	for i := 0; i < len(input); i++ {
		if input[i] != cur {
			result = append(result, counter, cur)
			cur = input[i]
			counter = 1
		} else {
			counter++
		}
	}
	result = append(result, counter, cur)
	return result
}

func lookAndSay(input string) string {
	ret := ""

	cur := input[0]
	counter := 0
	for i := 0; i < len(input); i++ {
		if input[i] != cur {
			ret += strconv.Itoa(counter)
			ret += string(cur)

			cur = input[i]
			counter = 1
		} else {
			counter++
		}
	}

	ret += strconv.Itoa(counter)
	ret += string(cur)
	return ret
}

/*
For example:

1 becomes 11 (1 copy of digit 1).
11 becomes 21 (2 copies of digit 1).
21 becomes 1211 (one 2 followed by one 1).
1211 becomes 111221 (one 1, one 2, and two 1s).
111221 becomes 312211 (three 1s, two 2s, and one 1).
Starting with the digits in your puzzle input, apply this
*/

func solve_str(input string, iterations int) string {
	sequence := input
	for i := 0; i < iterations; i++ {
		sequence = lookAndSay(sequence)
	}
	return sequence
}

func solve_int(input []int, iterations int) []int {
	sequence := input
	for i := 0; i < iterations; i++ {
		sequence = las(sequence)
	}
	return sequence
}

func itoa(input string) (result []int) {
	for _, e := range input {
		n, _ := strconv.Atoi(string(e))
		result = append(result, n)
	}
	return result
}

func solve_2017_day_10() {
	input := itoa("1113222113")

	seq_int := solve_int(input, 40)
	println(len(seq_int))

	seq_int = solve_int(seq_int, 10)
	println(len(seq_int))

	println("starting compute of 100 iterations...")
	seq_int = solve_int(input, 100)
	println(len(seq_int))
}
