package main

import "fmt"

type Link struct {
	value int
	prev  *Link
	next  *Link
}

type LinkedList struct {
	current *Link
	size    int
}

func create_list() LinkedList {
	current := &Link{value: 0}
	current.next = current
	current.prev = current
	return LinkedList{current: current, size: 1}
}

func (list *LinkedList) insert(value int) {
	if list.current == nil {
		panic("List empty!")
	}

	new_node := &Link{value: value, prev: list.current.next, next: list.current.next.next}
	list.current.next.next.prev = new_node
	list.current.next.next = new_node
	list.current = new_node
	list.size++
}

func print(i int, start *Link, list *LinkedList) {
	fmt.Printf("[%d]", i+1)
	current := start
	stop := false
	for !stop {
		if current != list.current {
			fmt.Printf("  %d", current.value)
		} else {
			fmt.Printf(" (%d)", current.value)
		}
		current = current.next
		stop = current == start
	}
	println()
}

func solve_2018_day_9() {

	marble(9, 25)
	marble(10, 1618)       // 10 players; last marble is worth 1618 points: high score is 8317
	marble(13, 7999)       // 13 players; last marble is worth 7999 points: high score is 146373
	marble(17, 1104)       // 17 players; last marble is worth 1104 points: high score is 2764
	marble(21, 6111)       // 21 players; last marble is worth 6111 points: high score is 54718
	marble(30, 5807)       // 30 players; last marble is worth 5807 points: high score is 37305
	marble(476, 71657)     // 476 players; last marble is worth 71657 points
	marble(476, 71657*100) // 476 players; last marble is worth 7,165,700 points
}

func marble(num_players int, max_marble int) {
	score := make(map[int]int)
	list := create_list()
	player := 0
	//zero := list.current

	for i := 1; i <= max_marble; i++ {
		if i%23 == 0 {
			remarble := list.current
			for b := 0; b < 7; b++ {
				remarble = remarble.prev
			}

			list.current = remarble.next
			remarble.prev.next = remarble.next
			remarble.next.prev = remarble.prev
			score[player+1] += i + remarble.value
		} else {
			list.insert(i)
		}

		//print(player, zero, &list)
		player = (player + 1) % num_players
	}

	high_score := 0
	for _, v := range score {
		//fmt.Printf("[%d] = %v\n", k, v)
		if v > high_score {
			high_score = v
		}
	}

	fmt.Printf("%d players; last marble is worth %d points: high score is %d\n", num_players, max_marble, high_score)
}
