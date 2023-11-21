import java.io.File
import kotlin.math.abs

data class Triplet(val x : Int, val y : Int, val z : Int)

fun part1() {
    val count = File("2016/day3/input.txt")
        .readLines()
        .filterNot { it == "" }
        .map { it.split(" ").filterNot { it == "" }.map { it.toInt() } }
        .map { Triplet(it[0], it[1], it[2]) }
        .filter { t -> t.x + t.y > t.z && t.y + t.z > t.x && t.x + t.z > t.y }
        .count()
    println("part1: ${count}")
}

fun part2() {
    val count = File("2016/day3/input.txt")
        .readLines()
        .filterNot { it == "" }
        .map { it.split(" ").filterNot { it == "" }.map { it.toInt() } }
        .chunked(3)
        .map { chunk -> listOf(Triplet(chunk[0][0], chunk[1][0], chunk[2][0]),
                               Triplet(chunk[0][1], chunk[1][1], chunk[2][1]),
                               Triplet(chunk[0][2], chunk[1][2], chunk[2][2]))
        }
        .flatten()
        .filter { t -> t.x + t.y > t.z && t.y + t.z > t.x && t.x + t.z > t.y }
        .count()

    println("part2: ${count}")
}

part1()
part2()
