using System;
using System.Linq;
using System.Collections.Generic;

namespace day7
{
    class Step
    {
        public Step(char letter, int baseDuration)
        {
            this.Letter = letter;
            this.Duration = baseDuration + (int)(Char.ToUpperInvariant(letter) - 'A') + 1;
        }

        public char Letter { get; }
        public int Duration { get; }

        public ISet<Step> IsBefore { get; } = new HashSet<Step>();
    }
}
