package main

import (
	"fmt"
	"math/bits"
)

type Point struct {
	x int
	y int
}

type State struct {
	point Point
	steps int
}

func is_wall(point Point, salt int) bool {
	x, y := point.x, point.y
	val := x*x + 3*x + 2*x*y + y + y*y
	val += salt

	ones := bits.OnesCount(uint(val))
	return (ones % 2) != 0
}

func walk(p Point, salt int) []Point {
	points := make([]Point, 2)
	points[0] = Point{p.x + 1, p.y}
	points[1] = Point{p.x, p.y + 1}

	if p.y > 0 {
		points = append(points, Point{p.x, p.y - 1})
	}
	if p.x > 0 {
		points = append(points, Point{p.x - 1, p.y})
	}
	return points
}

func solve(start Point, end Point, salt int) int {
	queue := make([]State, 0)
	queue = append(queue, State{start, 0})

	visited := make(map[Point]struct{})
	walls := make(map[Point]bool)
	num_visited := 0

	for len(queue) > 0 {
		current := queue[0]
		queue = queue[1:]
		visited[current.point] = struct{}{}

		if current.point == end {
			fmt.Printf("Distinct visited cubicles: %d\n", num_visited)
			return current.steps
		}
		if current.steps == 50 {
			num_visited = len(visited)
		}

		for _, next := range walk(current.point, salt) {
			_, found := visited[next]

			wall, wpc := walls[next]
			if !wpc {
				wall = is_wall(next, salt)
				walls[next] = wall
			}

			if !found && !wall {
				queue = append(queue, State{next, current.steps + 1})
			}
		}
	}

	panic("No solution found")
}

func main() {
	fmt.Println(solve(Point{1, 1}, Point{7, 4}, 10))
	fmt.Println(solve(Point{1, 1}, Point{31, 39}, 1352))
}
