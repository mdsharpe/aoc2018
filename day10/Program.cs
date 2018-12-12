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

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"{filename} @ {sky.Time} seconds");
                Console.WriteLine($"({sky.TopLeft.X}, {sky.TopLeft.Y}) - ({sky.BottomRight.X}, {sky.BottomRight.Y})");
                sky.Print();
                Console.WriteLine("Press [Esc] to end, any other key to continue.");

                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    break;
                }

                sky.MoveAll();
            }
        }
    }
}
