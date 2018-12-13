using System;

namespace day11
{
    internal static class GridExtensions
    {
        public static Grid WriteLineAndAssertPowerLevel(this Grid @this, int x, int y, int expectedPowerLevel)
        {
            Console.Write(Format(@this));
            var actualPowerLevel = @this.Cells[x, y];
            Console.Write($"; power level at {x}, {y} = {actualPowerLevel} ");
            Console.Write(actualPowerLevel == expectedPowerLevel ? '✓' : '❌');
            Console.WriteLine();
            return @this;
        }

        public static Grid WriteLineAndAssertMaxCoords(this Grid @this, int expectedX, int expectedY)
        {
            Console.Write(Format(@this));
            Console.Write(' ');
            Console.Write((@this.MaxX == expectedX && @this.MaxY == expectedY) ? '✓' : '❌');
            Console.WriteLine();
            return @this;
        }

        public static Grid WriteLine(this Grid @this)
        {
            Console.WriteLine(Format(@this));
            return @this;
        }

        private static string Format(Grid grid)
            => $"Grid #{grid.SerialNumber}; max 3x3 power square at {grid.MaxX}, {grid.MaxY}";
    }
}