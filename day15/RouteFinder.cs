using System;
using System.Collections.Generic;
using System.Linq;

namespace day15
{
    internal class RouteFinder
    {
        private readonly Coordinate _origin;
        private readonly ISet<Coordinate> _walls;
        private readonly Stack<Coordinate> _stack = new Stack<Coordinate>();
        private readonly HashSet<Coordinate> _visited = new HashSet<Coordinate>();
        private Coordinate _target;
        private Coordinate[] _route;

        public RouteFinder(Coordinate origin, ISet<Coordinate> walls)
        {
            _origin = origin;
            _walls = walls;
        }

        public Coordinate[] FindRoute(Coordinate to)
        {
            _target = to;
            _stack.Clear();

            VisitRecursive(_origin);

            _target = null;
            _stack.Clear();
            var route = _route;
            _route = null;
            return route;
        }

        private void VisitRecursive(Coordinate c)
        {
            _stack.Push(c);

            _visited.Add(c);

            if (c.Equals(_target))
            {
                var route = _stack.Reverse().Skip(1).ToArray();
                if (GetIsRouteBetter(route)) _route = route;
            }
            else if (_route == null || _stack.Count < _route.Length - 2)
            {
                var possibleSteps = c.EnumerateAdjacent()
                    .Where(o => !_walls.Contains(o))
                    .Where(o => !_visited.Contains(o))
                    .ToArray();

                foreach (var step in possibleSteps)
                {
                    VisitRecursive(step);
                }
            }

            _visited.Remove(c);

            _stack.Pop();
        }

        private bool GetIsRouteBetter(Coordinate[] route)
        {
            if (_route == null) return true;
            if (route.Length < _route.Length) return true;

            if (route.Length == _route.Length)
            {
                if (route[0].Y < _route[0].Y) return true;
                if (route[0].Y == _route[0].Y && route[0].X < _route[0].X) return true;
            }

            return false;
        }
    }
}