using System;

namespace day9
{
    internal static class GameExtensions
    {
        public static Game WriteLineAndAssertScore(this Game @this, int expectedScore)
        {
            Console.Write(Format(@this));
            Console.Write(' ');
            Console.Write(@this.HighScore == expectedScore ? '✓' : '❌');
            Console.WriteLine();
            return @this;
        }

        public static Game WriteLine(this Game @this)
        {
            Console.WriteLine(Format(@this));
            return @this;
        }

        private static string Format(Game game)
            => $"{game.NumberOfPlayers} players; last marble is worth {game.lastMarbleWorth} points: high score is {game.HighScore}";
    }
}