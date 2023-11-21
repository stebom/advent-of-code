const fs = require('fs');

try {
  const data = fs.readFileSync('2015_day_1.txt', 'ascii');
  
  let position = 1;
  let level = 0;
  for (let s of data) {
    	switch (s) {
        case '(':
          level++;
          break;
        case ')':
          level--;
          if (level === -1) {
            console.log('part 2: ' + position);
          }
          break;
      }
      position += 1;
  }
  console.log('answer: ' + level)
} catch (err) {
  console.error(err);
}
