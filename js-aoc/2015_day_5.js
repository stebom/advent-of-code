import { readFileSync } from "fs";

function tester_part_1() {
  const vowels = 'aeiou';
  const bad_strings = ['ab', 'cd', 'pq', 'xy'];

  return function(line) {
    let num_vowels = 0;
    for (const letter of line) {
      if (vowels.includes(letter)) {
        num_vowels += 1;
      }
    }
    if (num_vowels < 3) {
      return false;
    }

    let has_reoccuring_character = false;
    for (let i = 0; i < line.length - 1; ++i) {
      if (line[i] == line[i+1]) {
        has_reoccuring_character = true;
        break;
      }
    }
    if (!has_reoccuring_character) {
      return false;
    }

    let contains_bad_string = false;
    for (const bad_string of bad_strings) {
      if (line.includes(bad_string)) {
        contains_bad_string = true;
        break;
      }
    }
    return !contains_bad_string;
  }
}


function tester_part_2() {
  return function(line) {
    console.log('testing line ' + line);

    // It contains a pair of any two letters that appears at least twice in
    // the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like
    // aaa (aa, but it overlaps).
    let test_1 = false;
    for (let i = 0; i < line.length - 2; i++) {
      const pair = line[i] + line[i+1];
      if (line.indexOf(pair, i+2) != -1) {
        console.log('  found reoccuring pair: ' + pair + ' ' + line.indexOf(pair, i+2) + ' ' + i);
        test_1 = true;
        //break;
      }
    }

    // It contains at least one letter which repeats with exactly one letter between them,
    // like xyx, abcdefeghi (efe), or even aaa.
    let test_2 = false;
    for (let i = 0; i < line.length - 2; ++i) {
      if (line[i] === line[i+2]) {
        console.log('  found reoccuring letter: ' + line[i] + ' ' + i);
        test_2 = true;
        break;
      }
    }

    const is_nice = test_1 && test_2;
    if (is_nice) {
      console.log('is nice!');
    }
    return is_nice;
  }
}

try {
  const data = readFileSync("2015_day_5.txt", "ascii");
  const lines = data.split('\n');

  let counter_p1 = 0;
  let counter_p2 = 0;
  const part_1 = tester_part_1();
  const part_2 = tester_part_2();

  for (const line of lines) {
    counter_p1 += part_1(line) ? 1 : 0;
    counter_p2 += part_2(line) ? 1 : 0;
  }

  console.log('part 1: ' + counter_p1);
  console.log('part 2: ' + counter_p2);

} catch (err) {
  console.error(err);
}
