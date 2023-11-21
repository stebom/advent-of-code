const fs = require('fs');

// length l, width w, and height h
function surface(l, w, h) {
  return 2*l*w + 2*w*h + 2*h*l;
}

function smallest_side(l, w, h) {
  return Math.min(l*w, Math.min(w*h, h*l));
}

// A present with dimensions 2x3x4 requires 2+2+3+3 = 10 feet of ribbon to wrap
// the present plus 2*3*4 = 24 feet of ribbon for the bow, for a total of 34 feet.

// A present with dimensions 1x1x10 requires 1+1+1+1 = 4 feet of ribbon to wrap
// the present plus 1*1*10 = 10 feet of ribbon for the bow, for a total of 14 feet.
function calc_ribbon(l, w, h) {
  let sides = [l, w, h].sort((a, b) => a-b);
  return (l * w * h) + (sides[0] * 2) + (sides[1] * 2);
}

try {
  const data = fs.readFileSync('2015_day_2.txt', 'ascii');

  let sum = 0;
  let ribbon = 0;

  for (let line of data.split('\n')) {
    const dimensions = line.split('x');
    if (dimensions.length !== 3) continue;

    let [l, w, h] = [+dimensions[0], +dimensions[1], +dimensions[2]];
    sum += surface(l, w, h) + smallest_side(l, w, h);
    ribbon += calc_ribbon(l, w, h);
  }

  console.log('sum: ' + sum);
  console.log('ribbon: ' + ribbon);

} catch (err) {
  console.error(err);
}
