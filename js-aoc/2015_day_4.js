import { createHash } from 'node:crypto';

function md5(str) {
  return createHash("md5").update(str).digest('hex');
}

const key_start = 'ckczppom';

// part 1
for (let i = 0; i < 999999; ++i) {
  const content = key_start + i.toString().padStart(6, '0');
  const checksum = md5(content);
  if (checksum.startsWith('00000')) {
    console.log('found solution 1 ' + checksum + ' after iteration ' + i);
    break;
  }
}

// part 2
for (let i = 0; i < 9999999; ++i) {
  const content = key_start + i.toString().padStart(6, '0');
  const checksum = md5(content);
  if (checksum.startsWith('000000')) {
    console.log('found solution 2 ' + checksum + ' after iteration ' + i);
    break;
  }
}
