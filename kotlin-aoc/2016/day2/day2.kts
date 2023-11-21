import java.io.File
import kotlin.math.abs

enum class Button { Up, Down, Left, Right }

data class Position(var x : Int, var y : Int)

// calculate the manhattan distance between `a` and `b`
fun manhattan_distance(a : Position, b : Position) : Int = abs(a.x - b.x) + abs(a.y - b.y)

fun Char.toButton() = when (this) {
    'U' -> Button.Up
    'D' -> Button.Down
    'R' -> Button.Right
    'L' -> Button.Left
    else -> throw Exception("Bad button '${this}'!")
}

fun Position.press(button : Button) : Position {
    val new_pos = this.copy()
    when (button) {
        Button.Up -> new_pos.y -= 1
        Button.Down -> new_pos.y += 1
        Button.Right -> new_pos.x += 1
        Button.Left -> new_pos.x -= 1
    }
    return new_pos
}

val instructions = File("2016/day2/input.txt")
    .readLines()
    .map { it.trim().map { it.toButton() } }
        
fun part1() {
    val code = instructions.map {
            var pos = Position(0,0)
            for (button in it) {
                val new_pos = pos.press(button)
                pos = when {
                    new_pos.y < -1 || new_pos.y > 1 -> pos
                    new_pos.x < -1 || new_pos.x > 1 -> pos
                    else -> new_pos
                }
            }

            // find index of position and add 1
            //  [ 1, 2, 3 ]
            //  [ 4, 5, 6 ]
            //  [ 7, 8, 9 ]
            (pos.y + 1) * 3 + (pos.x + 1) + 1
    }
    println("part1: ${code.joinToString("")}")
}

fun part2() {
    //  [     1     ]
    //  [   2 3 4   ]
    //  [ 5 6 7 8 9 ]
    //  [   A B C   ]
    //  [     D     ]

    val start = Position(0,0)

    // Build a list of valid cells and order them by Y,X in ascending order
    // We can use this list as a look-up table to map a cell to button.
    // N.B. There is a off-by-one so one as buttons start at '1'!
    //
    // Usage:
    //   positions.indexOf(Position( 0, -2)) // returns 0  (button 1)
    //   positions.indexOf(Position(-1,  0)) // returns 5  (button 6)
    //   positions.indexOf(Position( 0,  0)) // returns 6  (button 7)
    //   positions.indexOf(Position( 0, -1)) // returns 12 (button C)
    // 
    val positions = (-2..2).map { x -> (-2..2).map { y -> Position(x,y) } }
            .flatten()
            .filter { manhattan_distance(start, it) <= 2 }
            .sortedWith(compareBy({ it.y }, { it.x }))

    val code = instructions.map {
            var pos = Position(0,0)
            for (button in it) {
                val new_pos = pos.press(button)

                // use the manhattan distance to determine if new_pos is
                // within valid grid
                pos = if (manhattan_distance(start, new_pos) <= 2) new_pos else pos
            }

            val i = positions.indexOf(pos) + 1
            i.toString(16).uppercase()
    }
    println("part2: ${code.joinToString("")}")
}

part1()
part2()
