using System;
using System.Linq;
using System.Collections.Generic;

namespace day7 {
    class Step {
        public Step(char letter)
        {
            this.Letter = letter;
        }

        public char Letter { get; private set; }

        public ISet<Step> IsBefore { get; } = new HashSet<Step>();
    }
}
