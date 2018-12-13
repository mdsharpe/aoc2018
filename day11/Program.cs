using System;

namespace day11
{
    class Program
    {
        internal static readonly int DefaultWidth = 300;
        internal static readonly int DefaultHeight = 300;

        static void Main(string[] args)
        {
            new Grid(8).Run().WriteLineAndAssertPowerLevel(3, 5, 4);
            new Grid(57).Run().WriteLineAndAssertPowerLevel(122, 79, -5);
            new Grid(39).Run().WriteLineAndAssertPowerLevel(217, 196, 0);
            new Grid(71).Run().WriteLineAndAssertPowerLevel(101, 153, 4);
            new Grid(18).Run().WriteLineAndAssertMaxCoords(33, 45);
            new Grid(42).Run().WriteLineAndAssertMaxCoords(21, 61);
            Console.WriteLine();
            new Grid(1718).Run().WriteLine();
        }
    }
}
