package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"
	"time"
)

func parseAllNumbers(str string) []int {
	output := make([]int, 0)

	re := regexp.MustCompile("(-?[0-9]+)")
	for _, number := range re.FindAllString(str, -1) {
		val, _ := strconv.Atoi(number)
		output = append(output, val)
	}
	return output
}

func parseIngredients() [][]int {
	data, _ := os.ReadFile("aoc_2015_day_15/2015_day_15.txt")
	lines := strings.Split(string(data), "\n")

	ingredients := make([][]int, len(lines)-1)

	for i, line := range lines {
		if line == "" {
			continue
		}

		ingredients[i] = parseAllNumbers(line)
	}

	return ingredients
}

func combine(weights []int, ingredients [][]int) []int {
	score := make([]int, len(ingredients[0]))
	for i := 0; i < len(weights); i++ {
		for y := 0; y < len(ingredients[0]); y++ {
			score[y] += ingredients[i][y] * weights[i]
		}
	}
	return score
}

func count_score(score []int) int {
	total_score := score[0]
	for i := 1; i < len(score)-1; i++ {
		if score[i] < 0 {
			score[i] = 0
		}
		total_score *= score[i]
	}

	return total_score
}

func sum(weights []int) int {
	sum := 0
	for _, v := range weights {
		sum += v
	}
	return sum
}

func hash(weights []int) int {
	sh := weights[0]
	for i := 1; i < len(weights); i++ {
		sh += weights[i] << (7 * i)
	}
	return sh
}

func getAllWeights(num int) [][]int {
	start := make([]int, num)
	for i := 0; i < num; i++ {
		start[i] = 1
	}

	weights := make([][]int, 0)
	visited := make(map[int]struct{}, 0)
	queue := make([][]int, 1)
	queue[0] = start

	for len(queue) > 0 {
		top := queue[0]
		queue = queue[1:]

		if _, found := visited[hash(top)]; found {
			continue
		}

		visited[hash(top)] = struct{}{}

		if sum(top) > 100 {
			continue
		}
		if sum(top) == 100 {
			weights = append(weights, top)
			continue
		}

		for i := 0; i < num; i++ {
			next := make([]int, num)
			copy(next, top)
			next[i]++
			queue = append(queue, next)
		}

	}

	return weights
}

func bruteforce(t [][]int) int {
	score := 0
	max := 0
	for i := 0; i < 100; i++ {
		for j := 0; j < 100-i; j++ {
			for k := 0; k < 100-i-j; k++ {
				h := 100 - i - j - k
				a := t[0][0]*i + t[1][0]*j + t[2][0]*k + t[3][0]*h
				b := t[0][1]*i + t[1][1]*j + t[2][1]*k + t[3][1]*h
				c := t[0][2]*i + t[1][2]*j + t[2][2]*k + t[3][2]*h
				d := t[0][3]*i + t[1][3]*j + t[2][3]*k + t[3][3]*h
				e := t[0][4]*i + t[1][4]*j + t[2][4]*k + t[3][4]*h

				// extra condition for part b
				if e != 500 {
					continue
				}
				if a <= 0 || b <= 0 || c <= 0 || d <= 0 {
					score = 0
					continue
				}
				score = a * b * c * d
				if score > max {
					max = score
				}
			}
		}
	}
	return max
}

func getWeights() [][]int {
	weights := make([][]int, 0)
	for i := 1; i < 96; i++ {
		for j := 1; j < 96-i; j++ {
			for k := 1; k < 96-i-j; k++ {
				l := 100 - i - j - k
				weights = append(weights, []int{i, j, k, l})
			}
		}
	}
	return weights
}

func solve() {
	ingredients := parseIngredients()

	start := time.Now()
	best_score_part1 := 0
	best_score_part2 := 0

	//for _, weights := range getAllWeights(len(ingredients)) {
	for _, weights := range getWeights() {
		score := combine(weights, ingredients)
		total_score := count_score(score)
		total_calories := score[len(score)-1]

		if total_calories == 500 && total_score > best_score_part2 {
			best_score_part2 = total_score
		}
		if total_score > best_score_part1 {
			best_score_part1 = total_score
		}
	}

	fmt.Println(best_score_part1, best_score_part2)
	fmt.Println(time.Now().Sub(start))

	start = time.Now()
	fmt.Println(bruteforce(ingredients))
	fmt.Println(time.Now().Sub(start))
}

func main() {
	solve()
}
