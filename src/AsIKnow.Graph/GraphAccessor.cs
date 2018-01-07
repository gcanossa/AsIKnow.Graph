using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class GraphAccessor
    {
        public abstract GraphQueryBuilder Query();

        public abstract int MergeWithGraph(SubGraph graph);
    }
}
