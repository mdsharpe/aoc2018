using System;

namespace day9
{
    class Program
    {
        static void Main(string[] args)
        {
            new Game(9, 25).Run(verbose: true).WriteLineAndAssertScore(32);
            new Game(10, 1618).Run().WriteLineAndAssertScore(8317);
            new Game(13, 7999).Run().WriteLineAndAssertScore(146373);
            new Game(17, 1104).Run().WriteLineAndAssertScore(2764);
            new Game(21, 6111).Run().WriteLineAndAssertScore(54718);
            new Game(30, 5807).Run().WriteLineAndAssertScore(37305);
            Console.WriteLine();
            new Game(473, 70904).Run().WriteLine();
            Console.WriteLine();
            new Game(473, 70904 * 100).Run().WriteLine();
        }
    }
}
