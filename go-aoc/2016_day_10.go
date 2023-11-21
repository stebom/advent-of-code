package main

import (
	"fmt"
	"os"
	"regexp"
	"sort"
	"strconv"
	"strings"
)

type bot struct {
	values         []int
	low_pass       int
	high_pass      int
	low_to_output  bool
	high_to_output bool
}

func solve_2016_day10() {
	dat, _ := os.ReadFile("2016_day_10.txt")
	input := strings.Split(string(dat), "\n")

	valuePattern := regexp.MustCompile("value ([0-9]+) goes to bot ([0-9]+)")
	givePattern := regexp.MustCompile("bot ([0-9]+) gives low to (bot|output) ([0-9]+) and high to (bot|output) ([0-9]+)")
	bots := make(map[int]bot)
	output := make(map[int][]int)

	for _, e := range input {
		r := valuePattern.FindStringSubmatch(e)
		if r != nil {
			fmt.Printf("value %s goes to bot %s\n", r[1], r[2])
			val, _ := strconv.Atoi(r[1])
			bid, _ := strconv.Atoi(r[2])
			bot := bots[bid]
			bot.values = append(bots[bid].values, val)
			bots[bid] = bot
		}
		r = givePattern.FindStringSubmatch(e)
		if r != nil {
			fmt.Printf("bot %s gives low to %s %s and high to %s %s\n", r[1], r[2], r[3], r[4], r[5])
			bid, _ := strconv.Atoi(r[1])
			low, _ := strconv.Atoi(r[3])
			high, _ := strconv.Atoi(r[5])
			bot := bots[bid]
			bot.low_pass = low
			bot.low_to_output = r[2] == "output"
			bot.high_pass = high
			bot.high_to_output = r[4] == "output"
			bots[bid] = bot
		}
	}

	for {
		for bid, bot := range bots {
			if len(bot.values) == 2 {
				sort.Ints(bot.values)

				if bot.values[0] == 17 && bot.values[1] == 61 {
					fmt.Printf("The bot you are looking for is %d\n", bid)
				}

				if bot.low_to_output {
					output[bot.low_pass] = append(output[bot.low_pass], bot.values[0])
				} else {
					pass_bot := bots[bot.low_pass]
					pass_bot.values = append(pass_bot.values, bot.values[0])
					bots[bot.low_pass] = pass_bot
				}
				if bot.high_to_output {
					output[bot.high_pass] = append(output[bot.high_pass], bot.values[1])
				} else {
					pass_bot := bots[bot.high_pass]
					pass_bot.values = append(pass_bot.values, bot.values[1])
					bots[bot.high_pass] = pass_bot
				}

				this_bot := bots[bid]
				this_bot.values = this_bot.values[:0]
				bots[bid] = this_bot
			}
		}

		if len(output[0]) > 0 && len(output[1]) > 0 && len(output[2]) > 0 {
			fmt.Printf("Output multiplier is %d\n", output[0][0]*output[1][0]*output[2][0])
			os.Exit(0)
		}
	}
}
