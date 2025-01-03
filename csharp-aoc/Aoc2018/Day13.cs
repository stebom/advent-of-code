using System.Text;

namespace Aoc2018;

static class Day13
{
    enum Turn { Left, Straight, Right };
    enum Direction { Up, Right, Down, Left };

    record Cart(Direction Direction, int X, int Y, Turn Turn);

    static string PrintTrack(char[][] grid, Dictionary<int, Cart> carts)
    {
        var sb = new StringBuilder();
        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[0].Length; x++)
            {
                
                if (!carts.Any(c => c.Value.X == x && c.Value.Y == y))
                {
                    sb.Append(grid[y][x]);
                }
                else
                {
                    var cart = carts.Single(c => c.Value.X == x && c.Value.Y == y).Value;
                    var dir = cart.Direction switch
                    {
                        Direction.Up => '^',
                        Direction.Right => '>',
                        Direction.Down => 'v',
                        Direction.Left => '<',
                        _ => throw new Exception(cart.Direction.ToString())
                    };
                    sb.Append(dir);
                }
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static void Run()
    {
        var grid = File.ReadAllLines(@"day13_input.txt").Select(s => s.ToCharArray()).ToArray();
        var carts = FindCarts(grid);

        Console.Write(PrintTrack(grid, carts));

        var c1 = carts.ToDictionary();
        var c2 = carts.ToDictionary();

        Part1(grid, c1);
        Part2(grid, c2);
    }

    static void Part1(char[][] grid, Dictionary<int, Cart> carts)
    {
        while (true)
        {
            var moveOrder = carts.OrderBy(c => c.Value.Y).ThenBy(c => c.Value.X).Select(c => c.Key);

            foreach (var id in moveOrder)
            {
                var movedCart = MoveAndTurnCart(grid, carts[id]);

                if (carts.Any(c => c.Value.X == movedCart.X && c.Value.Y == movedCart.Y))
                {
                    Console.WriteLine($"Part 1: Collision first detected at {movedCart.X},{movedCart.Y}");
                    return;
                }

                carts[id] = movedCart;
            }
        }
    }

    static void Part2(char[][] grid, Dictionary<int, Cart> carts)
    {
        while (true)
        {
            var moveOrder = carts.OrderBy(c => c.Value.Y).ThenBy(c => c.Value.X).Select(c => c.Key);

            foreach (var id in moveOrder)
            {
                if (!carts.ContainsKey(id)) continue;

                var movedCart = MoveAndTurnCart(grid, carts[id]);

                var collisions = carts.Where(c => c.Value.X == movedCart.X && c.Value.Y == movedCart.Y);
                if (collisions.Any())
                {
                    var collisionId = carts.Single(c => c.Value.X == movedCart.X && c.Value.Y == movedCart.Y).Key;
                    carts.Remove(id);
                    carts.Remove(collisionId);
                    continue;
                }

                carts[id] = movedCart;
            }

            if (carts.Count == 1)
            {
                Console.WriteLine($"Part 2: Location of last cart is {carts.Single().Value.X},{carts.Single().Value.Y}");
                return;
            }
        }
    }

    static Cart MoveAndTurnCart(char[][] grid, Cart cart) => TurnCart(grid, MoveCart(cart));

    static Cart MoveCart(Cart cart) => cart.Direction switch
    {
        Direction.Up    => cart with { Y = cart.Y - 1 },
        Direction.Down  => cart with { Y = cart.Y + 1 },
        Direction.Right => cart with { X = cart.X + 1 },
        Direction.Left  => cart with { X = cart.X - 1 },
        _ => throw new Exception($"{cart}")
    };

    static Cart TurnCart(char[][] grid, Cart cart)
    {
        var track = grid[cart.Y][cart.X];

        if (track is '+')
        {
            var nextTurn = (Turn)(((int)cart.Turn + 1) % 3);

            if (cart.Turn == Turn.Straight)
            {
                return cart with { Turn = nextTurn };
            }

            return cart.Direction switch
            {
                Direction.Up       => cart.Turn switch
                                      {
                                          Turn.Left => cart with { Direction = Direction.Left, Turn = nextTurn },
                                          Turn.Right => cart with { Direction = Direction.Right, Turn = nextTurn },
                                          _ => throw new Exception(cart.ToString())
                                      },
                Direction.Down     => cart.Turn switch
                                      {
                                          Turn.Left => cart with { Direction = Direction.Right, Turn = nextTurn },
                                          Turn.Right => cart with { Direction = Direction.Left, Turn = nextTurn },
                                          _ => throw new Exception(cart.ToString())
                                      },
                Direction.Right    => cart.Turn switch
                                      {
                                          Turn.Left => cart with { Direction = Direction.Up, Turn = nextTurn },
                                          Turn.Right => cart with { Direction = Direction.Down, Turn = nextTurn },
                                          _ => throw new Exception(cart.ToString())
                                      },
                Direction.Left     => cart.Turn switch
                                      {
                                          Turn.Left => cart with { Direction = Direction.Down, Turn = nextTurn },
                                          Turn.Right => cart with { Direction = Direction.Up, Turn = nextTurn },
                                          _ => throw new Exception(cart.ToString())
                                      },
                _ => throw new Exception(cart.ToString())
            };
        }
        else if (track is '\\')
        {
            return cart.Direction switch
            {
                Direction.Up => cart with { Direction = Direction.Left },
                Direction.Down => cart with { Direction = Direction.Right },
                Direction.Right => cart with { Direction = Direction.Down },
                Direction.Left => cart with { Direction = Direction.Up },
                _ => throw new Exception(cart.ToString())
            };
        }
        else if (track is '/')
        {
            return cart.Direction switch
            {
                Direction.Up => cart with { Direction = Direction.Right },
                Direction.Down => cart with { Direction = Direction.Left },
                Direction.Right => cart with { Direction = Direction.Up },
                Direction.Left => cart with { Direction = Direction.Down },
                _ => throw new Exception(cart.ToString())
            };
        }

        return cart;
    }

    static Dictionary<int, Cart> FindCarts(char[][] grid)
    {
        var id = 0;
        var carts = new Dictionary<int, Cart>();
        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[0].Length; x++)
            {
                if (grid[y][x] is '^' or '<' or '>' or 'v')
                {
                    var direction = grid[y][x] switch
                    {
                        '^' => Direction.Up,
                        '>' => Direction.Right,
                        'v' => Direction.Down,
                        '<' => Direction.Left,
                        _ => throw new Exception(grid[y][x].ToString())
                    };
                    carts.Add(id++, new Cart(direction, x, y, Turn.Left));
                }
                grid[y][x] = grid[y][x] switch
                {
                    '^' or 'v' => '|',
                    '<' or '>' => '-',
                    _ => grid[y][x],
                };
            }
        }
        return carts;
    }
}