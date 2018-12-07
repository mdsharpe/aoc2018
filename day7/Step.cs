using System;
using System.Linq;

namespace day7 {
    class Step {
        public string Name { get; set; }

        public ISet<Step> Prerequisites { get; } = new HashSet<Step>();
    }
}
