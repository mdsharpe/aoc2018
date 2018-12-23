using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day15
{
    internal class RouteFinder
    {
        private readonly Coordinate _origin;
        private readonly ISet<Coordinate> _obstructions;
        private readonly object _routeLock = new object();
        private Coordinate _target;

        public RouteFinder(Coordinate origin, IEnumerable<Coordinate> obstructions)
        {
            _origin = origin;
            _obstructions = new HashSet<Coordinate>(obstructions);
        }

        public Coordinate[] Route { get; private set; }

        public RouteFinder Clear()
        {
            _target = null;
            Route = null;
            return this;
        }

        public RouteFinder RouteTo(Coordinate to)
        {
            Clear();
            _target = to;

            var routes = _origin.EnumerateAdjacent()
                .Where(o => !_obstructions.Contains(o) || o.Equals(_target))
                .ToArray();

            Parallel.ForEach(routes, c => 
            {
                VisitRecursive(c, new Stack<Coordinate>(), new HashSet<Coordinate>());
            });

            return this;
        }

        private void VisitRecursive(Coordinate c, Stack<Coordinate> stack, HashSet<Coordinate> visited)
        {
            if (c.Equals(_target))
            {
                lock (_routeLock)
                {
                    var route = stack.Reverse().ToArray();
                    if (GetIsRouteBetter(route)) Route = route;
                }
            }
            else if (Route == null || stack.Count < Route.Length)
            {
                visited.Add(c);
                stack.Push(c);

                var possibleSteps = c.EnumerateAdjacent()
                    .Where(o => !_obstructions.Contains(o) || o.Equals(_target))
                    .Where(o => !visited.Contains(o))
                    .Where(o => !o.Equals(_origin))
                    .ToArray();

                foreach (var step in possibleSteps)
                {
                    VisitRecursive(step, stack, visited);
                }

                stack.Pop();
                visited.Remove(c);
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