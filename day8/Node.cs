using System;
using System.Collections.Generic;
using System.Linq;

namespace day8
{
    class Node
    {
        private Node(List<Node> childNodes, List<int> metadata)
        {
            ChildNodes = childNodes;
            Metadata = metadata;
        }

        public List<Node> ChildNodes { get; }
        public List<int> Metadata { get; }

        public int GetMetadataSum()
        {
            return Metadata.DefaultIfEmpty().Sum()
                + ChildNodes.Select(o => o.GetMetadataSum()).DefaultIfEmpty().Sum();
        }

        public int GetValue()
        {
            if (!ChildNodes.Any())
            {
                return Metadata.DefaultIfEmpty().Sum();
            }
            else
            {
                return (from m in Metadata
                        where m > 0
                        let i = m - 1
                        where i < ChildNodes.Count
                        let n = ChildNodes.ElementAt(i)
                        select n.GetValue())
                            .DefaultIfEmpty()
                            .Sum();
            }
        }

        public static Node Parse(IEnumerator<int> input)
        {
            input.MoveNext();
            var childNodeCount = input.Current;
            input.MoveNext();
            var metadataCount = input.Current;

            var childNodes = Enumerable.Range(0, childNodeCount)
                .Select(o => Node.Parse(input))
                .ToList();

            var metadata = Enumerable.Range(0, metadataCount)
                .Select(o =>
                {
                    input.MoveNext();
                    return input.Current;
                })
                .ToList();

            return new Node(childNodes, metadata);
        }
    }
}