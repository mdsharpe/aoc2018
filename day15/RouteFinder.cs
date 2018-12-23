using System;
using System.Collections.Generic;
using System.Linq;

namespace day15
{
    internal class RouteFinder
    {
        private readonly Coordinate _origin;
        private readonly ISet<Coordinate> _obstructions;
        private readonly Stack<Coordinate> _stack = new Stack<Coordinate>();
        private readonly HashSet<Coordinate> _visited = new HashSet<Coordinate>();
        private Coordinate _target;

        public RouteFinder(Coordinate origin, IEnumerable<Coordinate> obstructions)
        {
            _origin = origin;
            _obstructions = new HashSet<Coordinate>(obstructions);
        }

        public Coordinate[] Route { get; private set; }

        public RouteFinder Clear()
        {
            _stack.Clear();
            _visited.Clear();
            _target = null;
            Route = null;
            return this;
        }

        public RouteFinder RouteTo(Coordinate to)
        {
            Clear();
            _target = to;

            VisitRecursive(_origin);

            return this;
        }

        private void VisitRecursive(Coordinate c)
        {
            var isOrigin = c.Equals(_origin);
            var isTarget = c.Equals(_target);

            if (isTarget)
            {
                var route = _stack.Reverse().ToArray();
                if (GetIsRouteBetter(route)) Route = route;
            }
            else if (Route == null || _stack.Count < Route.Length)
            {
                _visited.Add(c);

                if (!isOrigin) _stack.Push(c);

                var possibleSteps = c.EnumerateAdjacent()
                    .Where(o => !_obstructions.Contains(o))
                    .Where(o => !_visited.Contains(o))
                    .ToArray();

                foreach (var step in possibleSteps)
                {
                    VisitRecursive(step);
                }

                _visited.Remove(c);
                if (!isOrigin) _stack.Pop();
            }
        }

        private bool GetIsRouteBetter(Coordinate[] route)
        {
            if (Route == null) return true;
            if (route.Length < Route.Length) return true;

            if (route.Length > 0 && route.Length == Route.Length)
            {
                if (route[0].Y < Route[0].Y) return true;
                if (route[0].Y == Route[0].Y && route[0].X < Route[0].X) return true;
            }

            return false;
        }
    }
}