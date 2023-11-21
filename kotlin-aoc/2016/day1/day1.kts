import java.io.File
import kotlin.math.abs

enum class Turn { R, L }
enum class Direction { N, E, S, W }
fun Direction.left() = Direction.values()[(4 + this.ordinal - 1) % 4]
fun Direction.right() = Direction.values()[(this.ordinal + 1) % 4]
fun Direction.turn(turn : Turn) = when (turn) {
    Turn.R -> this.right()
    Turn.L -> this.left()
}

fun Char.toTurn() = when (this) {
    'R' -> Turn.R
    'L' -> Turn.L
    else -> throw Exception("Bad direction ${this}")
}

data class Position(val x : Int, val y : Int)

fun Position.distance() = abs(this.x) + abs(this.y)

fun Position.walk(facing : Direction) = when (facing) {
    Direction.N -> Position(this.x, this.y + 1)
    Direction.E -> Position(this.x + 1, this.y)
    Direction.S -> Position(this.x, this.y - 1)
    Direction.W -> Position(this.x - 1, this.y)
}

data class Instruction(val turn : Turn, val steps : Int)

val instructions = File("2016/day1/input.txt")
    .readText()
    .filterNot { it.isWhitespace() }
    .split(",")
    .map { Instruction(it[0].toTurn(), it.substring(1).toInt()) }
    .toList()

fun part1() {
    var facing = Direction.N
    var position = Position(0,0)
    for (it in instructions){
        facing = facing.turn(it.turn)
        for (i in 1..it.steps) {
            position = position.walk(facing)
        }
    }

    println("part1: ${position} ${position.distance()}")
}

fun part2() {
    var facing = Direction.N
    var position = Position(0,0)
    var visited = mutableSetOf(position)

    for (it in instructions) {
        facing = facing.turn(it.turn)
        for (i in 1..it.steps) {
            position = position.walk(facing)

            if (position in visited) {
                println("part2: ${position} ${position.distance()}")
                return
            }
            visited.add(position)
        }
    }

    println("part2: ${position} ${position.distance()}")
}

part1()
part2()
