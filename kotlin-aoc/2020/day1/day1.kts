package day1

import java.io.File

val numbers = File("2020/day1/input.txt")
    .readLines()
    .filterNot { it == "" }
    .map { it.toInt() }

val needle = 2020

fun part1() {
    val set = numbers.toSet()
    val match = set.mapNotNull {
        val complement = (needle - it)
        if (complement in set) (complement to it) else null
    }
    .first()

    val product = match.first * match.second
    println("part1: ${match.first} * ${match.second} = ${product}")
}

fun part2() {
    for (i in 0..numbers.size-1) {
        val s = mutableSetOf<Int>()
        val current_sum = needle - numbers[i]

        for (j in (i+1)..numbers.size-1) {
            if ((current_sum - numbers[j]) in s) {
                val product = numbers[i] * numbers[j] * (current_sum - numbers[j])
                println("part2: ${numbers[i]} * ${numbers[j]} * ${current_sum - numbers[j]} = ${product}")
            }
            s.add(numbers[j])
        }
    }
}

part1()
part2()
