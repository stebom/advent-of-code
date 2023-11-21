import { readFileSync } from "fs";

function make_apply_func(line) {
  return function(state) {
    if (line.startsWith('turn on ')) { return state + 1; }
    else if (line.startsWith('turn off ')) { return Math.max(state - 1, 0); }
    else if (line.startsWith('toggle ')) { return state + 2; }
  } 
}

const grid = new Array(1000);
for (let i = 0; i < grid.length; ++i) {
  grid[i] = new Array(1000);
  for (let y = 0; y < grid.length; ++y) {
    grid[i][y] = 0;
  }
}

const data = readFileSync("2015_day_6.txt", "ascii");
for (let line of data.split('\n')) {
  if (line === '') { continue; }
  const tokens = line.replace('turn on ', '')
                        .replace('turn off ', '')
                        .replace('toggle ', '')
                        .replace(' through ', ' ')
                        .split(/[ ,]/)
                        .map(val => parseInt(val));

  const apply = make_apply_func(line);
  const [y_start, x_start, y_end, x_end] = tokens;
  for (let y = y_start; y <= y_end; ++y) {
    for (let x = x_start; x <= x_end; ++x) {
      grid[y][x] = apply(grid[y][x]);
    }
  }
}

let count = 0;
let sum = 0;
for (let y = 0; y < grid.length; ++y) {
  for (let x = 0; x < grid.length; ++x) {
    sum += grid[y][x];
    if (grid[y][x] > 0) {
      count += 1;
    }
  }
}

console.log(count);
console.log(sum);
