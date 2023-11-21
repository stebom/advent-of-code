package main

import (
	"fmt"
	"os"
	"strconv"
)

type context struct {
	sum    int
	ignore bool
}

func solve_2015_day_12() {
	data, _ := os.ReadFile("2015_day_12.txt")

	//solve("[1,2,3]")
	//solve(`[1,{"c":"red","b":2},3]`)
	//solve(`{"d":"red","e":[1,2,3,4],"f":5}`)
	//solve(`[1,"red",5]`)
	solve(string(data))
}

func solve(data string) {
	stack := make([]context, 1)
	sum := 0
	token := ""
	for i := 0; i < len(data); i++ {
		e := data[i]

		is_numeric_token := e == '-' || ('0' <= e && e <= '9')
		if !is_numeric_token && token != "" {
			val, err := strconv.Atoi(token)
			if err != nil {
				panic(err)
			}
			stack[len(stack)-1].sum += val
			token = ""
		} else if is_numeric_token {
			token += string(e)
		}

		if e == '{' {
			stack = append(stack, context{})
		} else if e == '}' {
			top := stack[len(stack)-1]
			stack = stack[:len(stack)-1]
			if !top.ignore {
				stack[len(stack)-1].sum += top.sum
			}
		} else if e == ':' && len(data)-i > 6 {
			if string(data[i:i+6]) == `:"red"` {
				stack[len(stack)-1].ignore = true
				i += 5
			}
		}
	}

	for len(stack) > 0 {
		top := stack[len(stack)-1]
		if !top.ignore {
			fmt.Printf("popping end stack with value %d\n", top.sum)
			sum += top.sum
		}
		stack = stack[:len(stack)-1]
	}

	fmt.Printf("%s has a sum of %d\n", data, sum)
}
