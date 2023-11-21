import { readFileSync } from "fs";

const move = (pos, s) => {
  let [x,y] = pos;
  switch (s) {
    case "v":
      y += 1;
      break;
    case "^":
      y -= 1;
      break;
    case "<":
      x -= 1;
      break;
    case ">":
      x += 1;
      break;
    default:
      throw new Error(c);
  }
  return [x,y];
};

const contains = (arr, pos) => arr.findIndex(elem => elem[0] === pos[0] && elem[1] === pos[1]) !== -1;

try {
  const data = readFileSync("2015_day_3.txt", "ascii");

  let santa = [0,0];
  let robo = [0,0]

  let visited = [santa];

  for (let i = 0; i < data.length; i += 2) {
    santa = move(santa, data[i]);
    robo = move(robo, data[i+1]);

    if (!contains(visited, santa)) visited.push(santa);
    if (!contains(visited, robo)) visited.push(robo);
  }

  console.log(visited);
  console.log(visited.length);
} catch (err) {
  console.error(err);
}
