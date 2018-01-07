using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public class MatchBuilder : GraphQueryBuilder.IMatchBuilder
    {
        internal MemoryGraphQueryBuilder Builder { get; set; }
        internal MemoryGraphQuery Query { get; set; } = new MemoryGraphQuery();
        public GraphQueryBuilder.IMatchNodeBuilder StartNode()
        {
            return new MatchNodeBuilder() { Builder = Builder, Matcher = Query.AddNodeMatcher(new NodeMatcher()) };
        }
    }
}
