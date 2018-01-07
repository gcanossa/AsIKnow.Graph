using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public class MemoryGraphQueryBuilder : GraphQueryBuilder
    {        
        public MemoryGraphQueryBuilder(TypeManager typeManager) : base(typeManager)
        {
        }
        
        internal List<MatchBuilder> Matcher { get; } = new List<MatchBuilder>();
        internal TypeManager MemoryTypeManager { get { return TypeManager; } }

        internal MemoryGraphQuery Query { get; set; } = new MemoryGraphQuery();

        public override IMatchBuilder Match()
        {
            return new MatchBuilder() { Builder = this };
        }
        public override GraphQuery Build()
        {
            return Query;
        }
    }
}
