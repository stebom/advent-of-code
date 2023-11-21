package day2

import java.io.File

enum class Shape { ROCK, PAPER, SCISSORS }

data class Round(val opponent : Shape, val me : Shape)

val shapeValues = Shape.values()
fun Shape.scoreOf() = this.ordinal + 1
fun Shape.previous() = shapeValues[(3 + this.ordinal - 1) % 3]
fun Shape.next() = shapeValues[(this.ordinal + 1) % 3]

fun Char.shapeOf() = when (this) {
    'A','X' -> Shape.ROCK
    'B','Y' -> Shape.PAPER
    'C','Z' -> Shape.SCISSORS
    else -> throw Exception("Bad shape ${this}")
}

fun Round.score() = this.me.scoreOf() + when {
    // 0 if you lost, 3 if the round was a draw, and 6 if you won
    // Rock defeats Scissors, Scissors defeats Paper, and Paper defeats Rock.
    // If both players choose the same shape, the round instead ends in a draw.
    this.opponent == this.me -> 3
    this.me == Shape.ROCK && this.opponent == Shape.SCISSORS -> 6    // C X
    this.me == Shape.SCISSORS && this.opponent == Shape.PAPER  -> 6  // B Z
    this.me == Shape.PAPER && this.opponent == Shape.ROCK -> 6       // A Y
    else -> 0
}

val lines = File("2022/day2/input.txt").readLines()

val part1 = lines.sumOf { Round(it[0].shapeOf(), it[2].shapeOf()).score() }

println("part1: ${part1}")

val part2 = lines.sumOf {
    val opponent = it[0].shapeOf()
    val prediction = when(it[2]) {
        'X' -> opponent.previous()  // X means you need to lose
        'Y' -> opponent             // Y means you need to end the round in a draw
        'Z' -> opponent.next()      // Z means you need to win
        else -> throw Exception("No prediction possible")
    }
    Round(opponent, prediction).score()
}

println("part2: ${part2}")
