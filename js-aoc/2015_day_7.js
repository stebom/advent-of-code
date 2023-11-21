"use strict";
import { readFileSync } from "fs";

const operators = ['AND','NOT','OR','LSHIFT','RSHIFT'];

class Wire {
  constructor(id, expr) {
    this.id = id;
    this.expr = expr;
    this.value = undefined;
  }
};

function get_value(wires, expr) {
  if (/^\d+$/.test(expr)) {
    return parseInt(expr);
  }
  else if (wires.has(expr)) {
    return wires.get(expr).value;
  }
  return undefined;
}

function solve_a(unsolved) {
  const wires = new Map();
  while (unsolved.length > 0) {
    for (const wire of unsolved) {
      let solved = false;
      if (wire.expr.includes('AND')) {
        const [a,b] = wire.expr.split(' AND ');
        const wire_a = get_value(wires, a);
        const wire_b = get_value(wires, b);
        if (wire_a !== undefined && wire_b !== undefined) {
          wire.value = (wire_a & wire_b) & 0xFFFF;
          solved = true;
        }
      }
      else if (wire.expr.includes('NOT')) {
        const a = wire.expr.replace('NOT ', '');
        const wire_a = get_value(wires, a);
        if (wire_a !== undefined) {
          wire.value = ~wire_a & 0xFFFF;
          solved = true;
        }
      }
      else if (wire.expr.includes('OR')) {
        const [a,b] = wire.expr.split(' OR ');
        const wire_a = get_value(wires, a);
        const wire_b = get_value(wires, b);
        if (wire_a !== undefined && wire_b !== undefined) {
          wire.value = (wire_a | wire_b) & 0xFFFF;
          solved = true;
        }
      }
      else if (wire.expr.includes('LSHIFT')) {
        const [a,b] = wire.expr.split(' LSHIFT ');
        const wire_a = get_value(wires, a);
        if (wire_a !== undefined) {
            wire.value = (wire_a << parseInt(b)) & 0xFFFF;
            solved = true;
        }
      }
      else if (wire.expr.includes('RSHIFT')) {
        const [a,b] = wire.expr.split(' RSHIFT ');
        const wire_a = get_value(wires, a);
        if (wire_a !== undefined) {
          wire.value = (wire_a >> parseInt(b)) & 0xFFFF;
          solved = true;
        }
      }
      else {
        const wire_a = get_value(wires, wire.expr);
        if (wire_a !== undefined) {
          wire.value = wire_a;
          solved = true;
        }
      }
  
      if (solved) {
        unsolved.splice(unsolved.indexOf(wire), 1);
        wires.set(wire.id, wire);
      }
    }
  }

  return wires.get('a');
}

const data = readFileSync("2015_day_7.txt", "ascii");
const unsolved = new Array();

for (let line of data.split('\n')) {
  if (line == '') { continue; }
  const [expr, id] = line.split(' -> ');
  unsolved.push(new Wire(id, expr));
}

const part_1 = solve_a([...unsolved]);
console.log(part_1);

unsolved.find(wire => wire.id == 'b').expr = part_1.value + '';
const part_2 = solve_a([...unsolved]);
console.log(part_2);
