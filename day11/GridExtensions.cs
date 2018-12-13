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

        public static Grid WriteLineAndAssertMax(this Grid @this, int size, int expectedX, int expectedY)
        {
            @this.FindMaxSquare(size, out var x, out var y);

            Console.Write(Format(@this));
            Console.Write($"; max {size}x{size} power square at {x}, {y} ");
            Console.Write((x == expectedX && y == expectedY) ? '✓' : '❌');
            Console.WriteLine();
            return @this;
        }

        public static Grid WriteLineAndAssertMaxAnySize(this Grid @this, int expectedSize, int expectedX, int expectedY)
        {
            @this.FindMaxSquareAnySize(out var size, out var x, out var y);

            Console.Write(Format(@this));
            Console.Write($"; max {size}x{size} power square at {x}, {y} ");
            Console.Write((size == expectedSize && x == expectedX && y == expectedY) ? '✓' : '❌');
            Console.WriteLine();
            return @this;
        }

        public static Grid WriteLineAndFindMax(this Grid @this, int size)
        {
            @this.FindMaxSquare(size, out var x, out var y);

            Console.Write(Format(@this));
            Console.Write($"; max {size}x{size} power square at {x}, {y} ");
            Console.WriteLine();
            return @this;
        }

        public static Grid WriteLineAndFindMaxAnySize(this Grid @this)
        {
            @this.FindMaxSquareAnySize(out var size, out var x, out var y);

            Console.Write(Format(@this));
            Console.Write($"; max {size}x{size} power square at {x}, {y} ");
            Console.WriteLine();
            return @this;
        }

        private static string Format(Grid grid)
            => $"Grid #{grid.SerialNumber}";
    }
}