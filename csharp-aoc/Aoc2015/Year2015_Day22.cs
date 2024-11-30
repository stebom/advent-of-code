using System.Diagnostics;

namespace AdventOfCode;

internal class Year2015_Day22
{

    /*
     * 1) Magic Missile costs 53 mana.
     *      It instantly does 4 damage.
     * 2) Drain costs 73 mana.
     *      It instantly does 2 damage and heals you for 2 hit points.
     * 3) Shield costs 113 mana.
     *      It starts an effect that lasts for 6 turns.
     *      While it is active, your armor is increased by 7.
     * 4) Poison costs 173 mana.
     *      It starts an effect that lasts for 6 turns.
     *      At the start of each turn while it is active, it deals the boss 3 damage.
     * 5) Recharge costs 229 mana.
     *      It starts an effect that lasts for 5 turns.
     *      At the start of each turn while it is active, it gives you 101 new mana.
     */

    record State(int Turn, int ShieldCounter, int PoisonCounter, int RechargeCounter, int Health, int Mana, int Boss);

    static int Simulate()
    {
        const int bossDamage = 9;

        var queue = new PriorityQueue<State, int>();
        queue.Enqueue(new State(Turn: 0, ShieldCounter: 0, PoisonCounter: 0, RechargeCounter: 0, Health: 50, Mana: 500, Boss: 58), 0);

        while (queue.Count > 0)
        {
            if (!queue.TryDequeue(out var state, out var priority))continue;

            var playersTurn = (state.Turn & 1) == 0;
            var health = state.Health;

            if (playersTurn)
            {
                health--;
                if (health == 0) { continue; }
            }

            var power = bossDamage;
            var boss = state.Boss;
            var mana = state.Mana;
            var shieldCounter = state.ShieldCounter;
            var poisonCounter = state.PoisonCounter;
            var rechargeCounter = state.RechargeCounter;

            // Pop counters
            if (state.ShieldCounter > 0) { power -= 7; shieldCounter--; }
            if (state.PoisonCounter > 0) { boss -= 3; poisonCounter--; }
            if (state.RechargeCounter > 0) { mana += 101; rechargeCounter--; }

            if (boss <= 0)
            {
                return priority;
            }

            var nextTurn = state.Turn + 1;

            if (playersTurn)
            {
                // 1) Magic Missile costs 53 mana.
                //      It instantly does 4 damage.
                if (mana >= 53)
                {
                    queue.Enqueue(new State(Turn: nextTurn,
                        ShieldCounter: shieldCounter,
                        PoisonCounter: poisonCounter,
                        RechargeCounter: rechargeCounter,
                        Health: health,
                        Mana: mana - 53,
                        Boss: boss - 4),
                        priority + 53);
                }

                // 2) Drain costs 73 mana.
                //      It instantly does 2 damage and heals you for 2 hit points.
                if (mana >= 73)
                {
                    queue.Enqueue(new State(Turn: nextTurn,
                        ShieldCounter: shieldCounter,
                        PoisonCounter: poisonCounter,
                        RechargeCounter: rechargeCounter,
                        Health: health + 2,
                        Mana: mana - 73,
                        Boss: boss - 2),
                        priority + 73);
                }

                // 3) Shield costs 113 mana.
                //      It starts an effect that lasts for 6 turns.
                //      While it is active, your armor is increased by 7.
                if (mana >= 113 && shieldCounter == 0)
                {
                    queue.Enqueue(new State(Turn: nextTurn,
                            ShieldCounter: 6,
                            PoisonCounter: poisonCounter,
                            RechargeCounter: rechargeCounter,
                            Health: health,
                            Mana: mana - 113,
                            Boss: boss),
                            priority + 113);
                }

                // 4) Poison costs 173 mana.
                //      It starts an effect that lasts for 6 turns.
                //      At the start of each turn while it is active, it deals the boss 3 damage.
                if (mana >= 173 && poisonCounter == 0)
                {
                    queue.Enqueue(new State(Turn: nextTurn,
                            ShieldCounter: shieldCounter,
                            PoisonCounter: 6,
                            RechargeCounter: rechargeCounter,
                            Health: health,
                            Mana: mana - 173,
                            Boss: boss),
                            priority + 173);
                }

                // 5) Recharge costs 229 mana.
                //      It starts an effect that lasts for 5 turns.
                //      At the start of each turn while it is active, it gives you 101 new mana.
                if (mana >= 229 && rechargeCounter == 0) { 
                    queue.Enqueue(new State(Turn: nextTurn,
                            ShieldCounter: shieldCounter,
                            PoisonCounter: poisonCounter,
                            RechargeCounter: 5,
                            Health: health,
                            Mana: mana - 229,
                            Boss: boss),
                            priority + 229);
                }
            }
            else
            {
                if (health - power <= 0) { continue; }
                queue.Enqueue(new State(Turn: nextTurn,
                                        ShieldCounter: shieldCounter,
                                        PoisonCounter: poisonCounter,
                                        RechargeCounter: rechargeCounter,
                                        Health: health - power,
                                        Mana: mana,
                                        Boss: boss),
                                        priority);
            }
        }

        return int.MaxValue;
    }

    internal static void Solve()
    {
        Console.WriteLine($"Part 1: {Simulate()}");
    }
}