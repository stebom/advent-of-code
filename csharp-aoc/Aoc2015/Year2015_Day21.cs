using System.Diagnostics;

namespace AdventOfCode;
/*
--- Day 21: RPG Simulator 20XX ---
Little Henry Case got a new video game for Christmas. It's an RPG, and he's stuck on a boss.
He needs to know what equipment to buy at the shop. He hands you the controller.

In this game, the player (you) and the enemy (the boss) take turns attacking.
The player always goes first. Each attack reduces the opponent's hit points by at least 1.
The first character at or below 0 hit points loses.

Damage dealt by an attacker each turn is equal to the attacker's damage score minus the defender's armor score.
An attacker always does at least 1 damage. So, if the attacker has a damage score of 8,
and the defender has an armor score of 3, the defender loses 5 hit points.
If the defender had an armor score of 300, the defender would still lose 1 hit point.

Your damage score and armor score both start at zero.
They can be increased by buying items in exchange for gold.
You start with no items and have as much gold as you need.
Your total damage or armor is equal to the sum of those stats from all of your items.
You have 100 hit points.

Here is what the item shop is selling:

Weapons:    Cost  Damage  Armor
Dagger        8     4       0
Shortsword   10     5       0
Warhammer    25     6       0
Longsword    40     7       0
Greataxe     74     8       0

Armor:      Cost  Damage  Armor
Leather      13     0       1
Chainmail    31     0       2
Splintmail   53     0       3
Bandedmail   75     0       4
Platemail   102     0       5

Rings:      Cost  Damage  Armor
Damage +1    25     1       0
Damage +2    50     2       0
Damage +3   100     3       0
Defense +1   20     0       1
Defense +2   40     0       2
Defense +3   80     0       3

You must buy exactly one weapon; no dual-wielding.
Armor is optional, but you can't use more than one.
You can buy 0-2 rings (at most one for each hand).
You must use any items you buy.
The shop only has one of each item, so you can't buy, for example, two rings of Damage +3.

For example,
    suppose you have 8 hit points, 5 damage, and 5 armor,
    and that the boss has 12 hit points, 7 damage, and 2 armor:

The player deals    5-2 = 3 damage; the boss goes down to       9 hit points.
The boss deals      7-5 = 2 damage; the player goes down to     6 hit points.
The player deals    5-2 = 3 damage; the boss goes down to       6 hit points.
The boss deals      7-5 = 2 damage; the player goes down to     4 hit points.
The player deals    5-2 = 3 damage; the boss goes down to       3 hit points.
The boss deals      7-5 = 2 damage; the player goes down to     2 hit points.
The player deals    5-2 = 3 damage; the boss goes down to       0 hit points.
In this scenario, the player wins! (Barely.)

You have 100 hit points. The boss's actual stats are in your puzzle input.

What is the least amount of gold you can spend and still win the fight?

Puzzle input:
  Hit Points: 103
  Damage: 9
  Armor: 2

*/

internal class Year2015_Day21
{
    record struct Item(int Cost, int Armor, int Damage);
    static Item Weapon(string _, int cost, int damage, int armor) => new(cost, armor, damage);
    static Item Armor(string _, int cost, int damage, int armor) => new(cost, armor, damage);
    static Item Ring(string _, int cost, int damage, int armor) => new(cost, armor, damage);

    static readonly Item[] Items = [
        // Weapons:  Cost  Damage  Armor
        Weapon("Dagger",      8, 4, 0),     // 0
        Weapon("Shortsword", 10, 5, 0),     // 1
        Weapon("Warhammer",  25, 6, 0),     // 2
        Weapon("Longsword",  40, 7, 0),     // 3
        Weapon("Greataxe",   74, 8, 0),     // 4
        // Armor:    Cost  Damage  Armor
        Armor("Leather",     13, 0, 1),     // 5
        Armor("Chainmail",   31, 0, 2),     // 6
        Armor("Splintmail",  53, 0, 3),     // 7
        Armor("Bandedmail",  75, 0, 4),     // 8
        Armor("Platemail",  102, 0, 5),     // 9
        // Rings:      Cost  Damage Armor
        Ring("Defense +1",   20, 0, 1),     // 13
        Ring("Damage +1",    25, 1, 0),     // 10
        Ring("Defense +2",   40, 0, 2),     // 14
        Ring("Damage +2",    50, 2, 0),     // 11
        Ring("Defense +3",   80, 0, 3),     // 15
        Ring("Damage +3",   100, 3, 0),     // 12
    ];

    static IEnumerable<(int Cost, int Armor, int Damage)> Combine()
    {
        // Try all weapons
        for (var i = 0; i < 5; i++)
        {
            yield return (Items[i].Cost, Items[i].Armor, Items[i].Damage);

            // Try ring #1
            for (var k = 10; k < 16; k++)
            {
                yield return (Items[i].Cost +  Items[k].Cost,
                              Items[i].Armor + Items[k].Armor,
                              Items[i].Damage + Items[k].Damage);

                // Try ring #2
                for (var l = k + 1; l < 16; l++)
                {
                    yield return (Items[i].Cost + Items[k].Cost + Items[l].Cost,
                                  Items[i].Armor + Items[k].Armor + Items[l].Armor,
                                  Items[i].Damage +  Items[k].Damage + Items[l].Damage);
                }
            }


            // Try all armors
            for (var j = 5; j < 10; j++)
            {
                yield return (Items[i].Cost + Items[j].Cost,
                              Items[i].Armor + Items[j].Armor,
                              Items[i].Damage + Items[j].Damage);

                // Try ring #1
                for (var k = 10; k < 16; k++)
                {
                    yield return (Items[i].Cost + Items[j].Cost + Items[k].Cost,
                                  Items[i].Armor + Items[j].Armor + Items[k].Armor,
                                  Items[i].Damage + Items[j].Damage + Items[k].Damage);

                    // Try ring #2
                    for (var l = k + 1; l < 16; l++)
                    {
                        yield return (Items[i].Cost + Items[j].Cost + Items[k].Cost + Items[l].Cost,
                                      Items[i].Armor + Items[j].Armor + Items[k].Armor + Items[l].Armor,
                                      Items[i].Damage + Items[j].Damage + Items[k].Damage + Items[l].Damage);
                    }
                }
            }
        }
    }

    static bool PlayerWins(int playerHealth, int bossHealth, int playerPower, int bossPower)
    {
        while (true)
        {
            bossHealth -= playerPower;
            if (bossHealth <= 0) return true;
            playerHealth -= bossPower;
            if (playerHealth <= 0) return false;
        }
    }

    static IEnumerable<int> Simulate(bool wins = true)
    {
        const int playerHealth = 100;

        // Hit Points: 103
        const int bossHealth = 103;
        // Damage: 9
        const int bossDamage = 9;
        // Armor: 2
        const int bossArmor = 2;

        foreach (var playerStats in Combine())
        {
            var playerPower = Math.Max(1, playerStats.Damage - bossArmor);
            var bossPower = Math.Max(1, bossDamage - playerStats.Armor);

            var playerHits = Math.Ceiling(bossHealth / (float)playerPower);
            var bossHits = Math.Ceiling(playerHealth / (float)bossPower);

            var playerWins = playerHits <= bossHits;
            if (playerWins == wins)
            {
                yield return playerStats.Cost;
            }
        }
    }

    internal static void Solve()
    {
        var least = Simulate().Order().First();
        Console.WriteLine($"Part 1: {least}");

        var most = Simulate(false).OrderDescending().First();
        Console.WriteLine($"Part 2: {most}");

    }
}