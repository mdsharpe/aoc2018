using System;
using System.Linq;

namespace day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Run("input.example.txt");
            Run("input.txt");
        }

        private static void Run(string filename)
        {
            var sky = Sky.Parse(filename);
            var exit = false;

            do
            {
                Console.Clear();
                Console.WriteLine($"{filename} @ {sky.Time} seconds");
                sky.Print();
                Console.WriteLine("Press [Esc] to end, arrow keys to navigate in time.");

                while (!HandleKeyPress(sky, out exit)) { }
            } while (!exit);
        }

        private static bool HandleKeyPress(Sky sky, out bool exit)
        {
            exit = false;
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.Escape:
                    exit = true;
                    return true;
                case ConsoleKey.UpArrow:
                    sky.MoveAll(60);
                    return true;
                case ConsoleKey.DownArrow:
                    sky.MoveAll(-60);
                    return true;
                case ConsoleKey.LeftArrow:
                    sky.MoveAll(-1);
                    return true;
                case ConsoleKey.RightArrow:
                    sky.MoveAll(1);
                    return true;
                default:
                    return false;
            }
        }
    }
}
