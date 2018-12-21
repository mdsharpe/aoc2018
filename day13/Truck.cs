using System;
using System.Collections.Generic;

namespace day13
{
    internal class Truck
    {
        private static readonly IReadOnlyList<bool?> TurnSequence = new List<bool?> {
            false, // Left
            null, // Straight
            true // Right
        }.AsReadOnly();

        private int _nextTurnSequenceIndex;

        public int X { get; set; }
        public int Y { get; set; }
        public Direction Direction { get; set; }

        public (int x, int y) GetNextLocation()
        {
            switch (Direction)
            {
                case Direction.Up:
                    return (X, Y - 1);
                case Direction.Right:
                    return (X + 1, Y);
                case Direction.Down:
                    return (X, Y + 1);
                case Direction.Left:
                    return (X - 1, Y);
                default:
                    return (X, Y);
            }
        }

        public Direction GetRotatedDirection(bool clockwise)
        {
            switch (Direction)
            {
                case Direction.Up:
                    return clockwise ? Direction.Right : Direction.Left;
                case Direction.Right:
                    return clockwise ? Direction.Down : Direction.Up;
                case Direction.Down:
                    return clockwise ? Direction.Left : Direction.Right;
                case Direction.Left:
                    return clockwise ? Direction.Up : Direction.Down;
                default:
                    return Direction;
            }
        }

        public Direction GetNextTurnDirection()
        {
            var turn = GetNextTurn();

            if (turn == null)
            {
                return Direction;
            }

            return Direction.GetTurnDirection(turn.Value);
        }

        private bool? GetNextTurn()
        {
            var turn = TurnSequence[_nextTurnSequenceIndex];

            _nextTurnSequenceIndex++;
            if (_nextTurnSequenceIndex > TurnSequence.Count - 1)
            {
                _nextTurnSequenceIndex = 0;
            }

            return turn;
        }
    }
}