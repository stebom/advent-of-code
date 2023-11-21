package main

import "fmt"

// Passwords must include one increasing straight of at least three letters, like abc, bcd, cde, and so on, up to xyz. They cannot skip letters; abd doesn't count.
// Passwords may not contain the letters i, o, or l, as these letters can be mistaken for other characters and are therefore confusing.
// Passwords must contain at least two different, non-overlapping pairs of letters, like aa, bb, or zz.
func test(str string) bool {
	test1 := false
	test2 := true
	test3 := false
	pair1 := -1
	pair2 := -1

	for i := 0; i < len(str); i++ {
		if len(str)-i-1 >= 2 && !test1 {
			v1 := int(str[i])
			v2 := int(str[i+1]) - 1
			v3 := int(str[i+2]) - 2
			test1 = v1 == v2 && v2 == v3
		}
		if str[i] == 'i' || str[i] == 'o' || str[i] == 'l' {
			test2 = false
			break
		}
		if len(str)-i-1 >= 1 && !test3 {
			if str[i] == str[i+1] {
				if pair1 == -1 {
					pair1 = i
				} else if i > pair1+1 {
					pair2 = i
				}
				test3 = pair1 != -1 && pair2 != -1
			}
		}
	}

	//fmt.Printf("%s %t %t %t\n", str, test1, test2, test3)
	return test1 && test2 && test3
}

func increment(str string) string {
	out := []byte(str)
	index := len(out) - 1
	for {
		if out[index] == 'z' {
			out[index] = 'a'
			index--
		} else {
			out[index] = out[index] + 1
			break
		}
	}

	return string(out)
}

func find_next(str string) string {
	s := increment(str)
	for !test(s) {
		s = increment(s)
	}
	fmt.Printf("The next password after %s is %s\n", str, s)
	return s
}

func solve_2015_day_11() {
	find_next("abcdefgh")
	find_next("ghijklmn")
	find_next(find_next("hepxcrrq"))
}
