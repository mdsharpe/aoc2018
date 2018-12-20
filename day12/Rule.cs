using System;
using System.Linq;

namespace day12
{
    internal class Rule
    {
        private readonly bool[] _sequence;
        private readonly bool _result;

        private readonly int _evaluateRangeOffset;

        public Rule(bool[] sequence, bool result)
        {
            _sequence = sequence;
            _result = result;
            _evaluateRangeOffset = (sequence.Length - 1) / 2;
        }

        public bool Evaluate(bool[] state, int index)
            => state.Skip(index - _evaluateRangeOffset)
                .Take(_sequence.Length)
                .SequenceEqual(_sequence)
                ? _result : false;
    }
}