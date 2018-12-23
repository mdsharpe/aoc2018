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
            _visited.Clear();
            _visited.Add(_origin);

            VisitRecursive(_origin);

            _target = null;
            _stack.Clear();
            _visited.Clear();

            var route = _route;
            _route = null;
            return route;
        }

        private void VisitRecursive(Coordinate c)
        {
            if (c.Equals(_target))
            {
                var route = _stack.Reverse().ToArray();
                if (GetIsRouteBetter(route)) _route = route;
            }
            else if (_route == null || _stack.Count < _route.Length)
            {
                var possibleSteps = c.EnumerateAdjacent()
                    .Where(o => !_walls.Contains(o))
                    .Where(o => !_visited.Contains(o))
                    .ToArray();

                foreach (var step in possibleSteps)
                {
                    _stack.Push(step);
                    _visited.Add(step);
                    VisitRecursive(step);
                    _visited.Remove(step);
                    _stack.Pop();
                }
            }
        }

        private bool GetIsRouteBetter(Coordinate[] route)
        {
            if (_route == null) return true;
            if (route.Length < _route.Length) return true;

            if (route.Length > 0 && route.Length == _route.Length)
            {
                if (route[0].Y < _route[0].Y) return true;
                if (route[0].Y == _route[0].Y && route[0].X < _route[0].X) return true;
            }

            return false;
        }
    }
}