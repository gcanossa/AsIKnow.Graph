using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public class Relationship : GraphLabelledEntity
    {
        public Relationship(TypeManager typeManager) : base(typeManager)
        {
        }
        public Relationship(TypeManager typeManager, object obj) : base(typeManager, obj)
        {
        }

        public Node FromNode { get; set; }
        public Node ToNode { get; set; }
    }
}
