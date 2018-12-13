using System;

namespace day11
{
    class Grid
    {
        public Grid(int serialNumber)
            : this(serialNumber, Program.DefaultWidth, Program.DefaultHeight)
        { }

        public Grid(int serialNumber, int width, int height)
        {
            SerialNumber = serialNumber;
            Cells = new int[width, height];
        }

        public int SerialNumber { get; }
        public int[,] Cells { get; }
        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        public Grid Run()
        {
            PopulateGrid();
            FindMaxSquare();

            return this;
        }

        private void PopulateGrid()
        {
            for (var y = 0; y < Cells.GetLength(1); y++)
            {
                for (var x = 0; x < Cells.GetLength(0); x++)
                {
                    Cells[x, y] = CalculatePowerLevel(x, y);
                }
            }
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

        private void FindMaxSquare()
        {
            MaxX = -1;
            MaxY = -1;
            var max = 0;

            for (var y = 0; y < Cells.GetLength(1) - 2; y++)
            {
                for (var x = 0; x < Cells.GetLength(0) - 2; x++)
                {
                    var total = 0;
                    for (var iy = 0; iy < 3; iy++)
                    {
                        for (var ix = 0; ix < 3; ix++)
                        {
                            total += Cells[x + ix, y + iy];
                        }
                    }

                    if (total > max)
                    {
                        MaxX = x;
                        MaxY = y;
                        max = total;
                    }
                }
            }
        }
    }
}
