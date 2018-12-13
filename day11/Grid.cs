using System;
using System.Linq;

namespace day11
{
    class Grid
    {
        public Grid(int serialNumber)
            : this(serialNumber, 300)
        { }

        public Grid(int serialNumber, int size)
        {
            SerialNumber = serialNumber;
            Size = size;
            Cells = new int[size, size];
        }

        public int SerialNumber { get; }
        public int Size { get; }
        public int[,] Cells { get; }

        public Grid Run()
        {
            for (var y = 0; y < Cells.GetLength(1); y++)
            {
                for (var x = 0; x < Cells.GetLength(0); x++)
                {
                    Cells[x, y] = CalculatePowerLevel(x, y);
                }
            }

            return this;
        }

        public int FindMaxSquareAnySize(out int maxSize, out int maxX, out int maxY)
        {
            var max = Enumerable.Range(1, Size - 1)
                .AsParallel()
                .Select(size =>
                {
                    var power = FindMaxSquare(size, out var x, out var y);
                    return new
                    {
                        Size = size,
                        X = x,
                        Y = y,
                        Power = power
                    };
                })
                .OrderByDescending(o => o.Power)
                .First();

            maxSize = max.Size;
            maxX = max.X;
            maxY = max.Y;
            return max.Power;
        }

        public int FindMaxSquare(int size, out int maxX, out int maxY)
        {
            maxX = -1;
            maxY = -1;
            var maxPower = 0;

            for (var y = 0; y < Cells.GetLength(1) - (size - 1); y++)
            {
                for (var x = 0; x < Cells.GetLength(0) - (size - 1); x++)
                {
                    var power = 0;
                    for (var iy = 0; iy < size; iy++)
                    {
                        for (var ix = 0; ix < size; ix++)
                        {
                            power += Cells[x + ix, y + iy];
                        }
                    }

                    if (power > maxPower)
                    {
                        maxX = x;
                        maxY = y;
                        maxPower = power;
                    }
                }
            }

            return maxPower;
        }

        private int CalculatePowerLevel(int x, int y)
        {
            var rackId = x + 10;
            var powerLevel = rackId * y;
            powerLevel += SerialNumber;
            powerLevel *= rackId;

            if (powerLevel >= 100)
            {
                var powerLevelStr = powerLevel.ToString();
                powerLevel = (int)char.GetNumericValue(powerLevelStr[powerLevelStr.Length - 3]);
            }
            else
            {
                powerLevel = 0;
            }

            powerLevel -= 5;

            return powerLevel;
        }
    }
}
