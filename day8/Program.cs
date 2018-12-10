using System;
using System.Linq;

namespace day8
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("./input.txt")
                .Split(' ')
                .Select(o => int.Parse(o));

            var root = Node.Parse(input.GetEnumerator());

            Console.WriteLine($"Metadata sum: {root.GetMetadataSum()}");
            Console.WriteLine($"Value of root note: {root.GetValue()}");
        }
    }
}
