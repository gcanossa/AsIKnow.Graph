using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class GraphQuery
    {
        protected Dictionary<string, object> Symbols { get; } = new Dictionary<string, object>();
        public abstract List<SubGraph> ExecuteQuery(SubGraph graph);
    }
}
