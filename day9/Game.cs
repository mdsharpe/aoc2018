using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace day9
{
    class Game
    {
        private readonly int _numberOfPlayers;
        private readonly int _lastMarbleWorth;
        private readonly Dictionary<int, long> _scores = new Dictionary<int, long>();

        public Game(int numberOfPlayers, int lastMarbleWorth)
        {
            _numberOfPlayers = numberOfPlayers;
            _lastMarbleWorth = lastMarbleWorth;
        }

        public int NumberOfPlayers => _numberOfPlayers;
        public int lastMarbleWorth => _lastMarbleWorth;
        public Dictionary<int, long> Scores => _scores;
        public long HighScore
        {
            get
            {
                return _scores.Values.DefaultIfEmpty().Max();
            }
        }

        public Game Run(bool verbose = false)
        {
            ResetScores();

            var circle = new LinkedList<int>();
            var node = circle.AddLast(0);
            var current = 0;

            var currentPlayer = 0;

            Timer timer;
            if (!verbose)
            {
                timer = new Timer(_ =>
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write(current);
                }, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            }
            else { timer = null; }

            void log()
            {
                if (!verbose)
                {
                    return;
                }

                var output = new StringBuilder();
                output.Append(currentPlayer > 0 ? $"[{currentPlayer}]" : "[-]");

                var nodeToPrint = circle.First;
                while (nodeToPrint != null) {
                    output.Append('\t');
                    if (nodeToPrint == node) { output.Append('('); }
                    output.Append(nodeToPrint.Value);
                    if (nodeToPrint == node) { output.Append(')'); }
                    nodeToPrint = nodeToPrint.Next;
                }

                Console.WriteLine(output);
            }

            log();

            var marbleEnumerator = Enumerable.Range(1, _lastMarbleWorth).GetEnumerator();

            while (marbleEnumerator.MoveNext())
            {
                current = marbleEnumerator.Current;
                currentPlayer++;
                if (currentPlayer > _numberOfPlayers) { currentPlayer = 1; }

                if (current % 23 != 0)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        node = node.Next ?? circle.First;
                    }

                    node = circle.AddBefore(node, current);
                }
                else
                {
                    _scores[currentPlayer] += current;

                    for (var i = 0; i < 7; i++)
                    {
                        node = node.Previous ?? circle.Last;
                    }

                    _scores[currentPlayer] += node.Value;
                    var remainingNode = node.Next;
                    circle.Remove(node);
                    node = remainingNode;
                }

                log();
            }

            if (!verbose)
            {
                timer.Dispose();
                Console.SetCursorPosition(0, Console.CursorTop);
            }

            return this;
        }

        private void ResetScores()
        {
            _scores.Clear();
            for (var i = 1; i <= _numberOfPlayers; i++)
            {
                _scores.Add(i, 0L);
            }
        }
    }
}