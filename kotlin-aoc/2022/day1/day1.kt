package day1

import java.io.File

fun solve() {
    fun test1() {   
        val lines = File("2022/day1/input.txt").readLines()
        var list = mutableListOf<Int>()
        var current = 0
        for (line in lines) {
            when (line) {
                "" -> { list.add(current); current = 0 }
                else -> current += line.toInt()
            }
        }

        list.sortDescending()

        println(list.take(3).sum())
    }

    fun test2() {   
        val lines = File("2022/day1/input.txt").readText()

        val sum = lines.split("\n\n")
            .map {
                it.split("\n")
                .filterNot { i -> i == "" }
                .sumOf { i -> i.toInt() }
            }
            .sortedDescending()
            .take(3)
            .sum()
        
            println(sum)
        }

    test1()
    test2()
}

fun main() {
    solve()
}
