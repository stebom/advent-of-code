package main

import (
	"fmt"
	"os"
	"regexp"
	"sort"
	"strconv"
)

type DeerStats struct {
	name        string
	speed       int
	fly_period  int
	rest_period int
}

type Deer struct {
	fly_countdown  int
	rest_countdown int
	total_distance int
	points         int
}

func parse_input() []DeerStats {
	re := regexp.MustCompile("(.+) can fly (.+) km/s for (.+) seconds, but then must rest for (.+) seconds.")
	data, _ := os.ReadFile("aoc_2015_day_14/2015_day_14.txt")

	stats := make([]DeerStats, 0)

	for _, match := range re.FindAllStringSubmatch(string(data), -1) {
		name := match[1]
		speed, _ := strconv.Atoi(match[2])
		fly_period, _ := strconv.Atoi(match[3])
		rest_period, _ := strconv.Atoi(match[4])
		stats = append(stats, DeerStats{name, speed, fly_period, rest_period})
	}
	return stats
}

func get_leader_distance(deer []Deer) int {
	distances := make([]int, len(deer))
	for i, d := range deer {
		distances[i] = d.total_distance
	}
	sort.Ints(distances)
	return distances[len(distances)-1]
}

func main() {
	//Comet can fly 14 km/s for 10 seconds, but then must rest for 127 seconds.
	//Dancer can fly 16 km/s for 11 seconds, but then must rest for 162 seconds.
	//stats := []DeerStats{{"Comet", 14, 10, 127}, {"Dancer", 16, 11, 162}}
	//deer := []Deer{{stats[0].fly_period, 0, 0}, {stats[1].fly_period, 0, 0}}

	stats := parse_input()
	deer := make([]Deer, len(stats))
	for i := range stats {
		deer[i].fly_countdown = stats[i].fly_period
	}
	end_second := 2503

	for sec := 0; sec < end_second; sec++ {
		for i := 0; i < len(deer); i++ {
			if deer[i].rest_countdown == 0 {
				deer[i].total_distance += stats[i].speed
				deer[i].fly_countdown--
				if deer[i].fly_countdown == 0 {
					deer[i].rest_countdown = stats[i].rest_period
				}
			} else {
				deer[i].rest_countdown--
				if deer[i].rest_countdown == 0 {
					deer[i].fly_countdown = stats[i].fly_period
				}
			}
		}

		// part 2: award points
		best_distance := get_leader_distance(deer)
		for i := 0; i < len(deer); i++ {
			if deer[i].total_distance == best_distance {
				deer[i].points++
			}
		}
	}

	distances := make([]int, len(deer))
	for i, d := range deer {
		distances[i] = d.total_distance
	}
	sort.Ints(distances)
	fmt.Println("distances", distances)

	points := make([]int, len(deer))
	for i, d := range deer {
		points[i] = d.points
	}
	sort.Ints(points)
	fmt.Println("points", points)
}
