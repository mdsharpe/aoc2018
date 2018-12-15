using System;
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

        public bool? Evaluate(bool[] state, int index)
        {
            var evals = from i in Enumerable.Range(0, _sequence.Length)
                        let ruleVal = _sequence[i]
                        let stateIndex = index + (i - _evaluateRangeOffset)
                        let stateVal = (stateIndex >= 0 && stateIndex < state.Length) ? state[stateIndex] : false
                        select stateVal == ruleVal;

            if (evals.All(o => o))
            {
                return _result;
            }

            return null;
        }
    }
}