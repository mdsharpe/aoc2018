using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace day12
{
    class Rule
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

        public bool? Evaluate(BitArray state, int index)
        {
            var evals = from i in Enumerable.Range(0, _sequence.Length)
                        let stateIndex = index + (i - _evaluateRangeOffset)
                        let stateVal = (stateIndex >= 0 && stateIndex < state.Length) ? state[stateIndex] : false
                        select stateVal == _sequence[i];

            if (evals.All(o => o))
            {
                return _result;
            }

            return null;
        }
    }
}