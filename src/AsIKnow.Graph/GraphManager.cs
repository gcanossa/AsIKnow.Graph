using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public class GraphManager
    {
        public TypeManager Manager { get; private set; }
        public GraphManager(TypeManager manager)
        {
            Manager = manager;
        }

        public Node CreateNode(object obj = null)
        {
            return obj == null ? new Node(Manager) : new Node(Manager, obj);
        }
        public Relationship CreateRelationship(object obj = null)
        {
            return obj == null ? new Relationship(Manager) : new Relationship(Manager, obj);
        }
    }
}
