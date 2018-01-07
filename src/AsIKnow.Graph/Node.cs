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

        public List<Relationship> EnteringRelationships { get; set; }
        public List<Relationship> ExitingRelationships { get; set; }
    }
}
