using System;
using System.Collections.Generic;

namespace AsIKnow.Graph
{
    public class Node : LabelledEntity
    {
        public Node(TypeManager typeManager) : base(typeManager)
        {
        }
        public Node(TypeManager typeManager, object obj) : base(typeManager, obj)
        {
        }

        public HashSet<Relationship> EnteringRelationships { get; set; } = new HashSet<Relationship>();
        public HashSet<Relationship> ExitingRelationships { get; set; } = new HashSet<Relationship>();
    }
}
