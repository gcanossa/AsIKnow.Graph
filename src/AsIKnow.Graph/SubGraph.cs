using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public class SubGraph : GraphAccessor
    {
        protected TypeManager TypeManager { get; set; }
        public SubGraph(TypeManager typeManager)
        {
            TypeManager = typeManager;
        }
        public Expression<Func<object>> LabelledEntityKeySelector { get; set; } = () => null;

        public IEnumerable<KeyValuePair<string, LabelledEntity>> Symbols { get; } = new Dictionary<string, LabelledEntity>();

        public IEnumerable<Node> Nodes { get; } = new List<Node>();
        public IEnumerable<Relationship> Relationships { get; } = new List<Relationship>();

        protected bool ContainsNode(Node node)
        {
            object obj = LabelledEntityKeySelector?.Compile()?.Invoke();

            return Nodes.Contains(node) || obj != null && Nodes.Any(p => TypeManager.CheckObjectInclusion(p, obj));
        }
        protected bool ContainsRelationship(Relationship relationship)
        {
            object obj = LabelledEntityKeySelector?.Compile()?.Invoke();

            return Relationships.Contains(relationship) || obj != null && Relationships.Any(p => TypeManager.CheckObjectInclusion(p, obj));
        }

        public int AddNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            List<Node> nodes = (List<Node>)Nodes;
            int count = 0;
            if (!ContainsNode(node))
            {
                nodes.Add(node);
                count++;

                node.EnteringRelationships?.ForEach(p=> count += AddRelationship(p));
                node.ExitingRelationships?.ForEach(p => count += AddRelationship(p));
            }

            return count;
        }
        public int RemoveNode(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            List<Node> nodes = (List<Node>)Nodes;
            int count = 0;
            if (ContainsNode(node))
            {
                nodes.Remove(node);
                count++;

                node.EnteringRelationships?.ForEach(p => count += RemoveRelationship(p));
                node.ExitingRelationships?.ForEach(p => count += RemoveRelationship(p));
            }

            return count;
        }
        public int AddRelationship(Relationship relationship)
        {
            if (relationship == null)
                throw new ArgumentNullException(nameof(relationship));

            List<Relationship> rels = (List<Relationship>)Relationships;
            int count = 0;
            if (!ContainsRelationship(relationship))
            {
                rels.Add(relationship);
                count++;

                count += AddNode(relationship.FromNode);
                count += AddNode(relationship.ToNode);
            }

            return count;
        }
        public int RemoveRelationship(Relationship relationship)
        {
            if (relationship == null)
                throw new ArgumentNullException(nameof(relationship));

            List<Relationship> rels = (List<Relationship>)Relationships;
            int count = 0;
            if (ContainsRelationship(relationship))
            {
                rels.Remove(relationship);
                count++;

                count += RemoveNode(relationship.FromNode);
                count += RemoveNode(relationship.ToNode);
            }

            return count;
        }

        public override GraphQueryBuilder Query()
        {
            throw new NotImplementedException();
        }

        public override int MergeWithGraph(SubGraph graph)
        {
            if (graph == null)
                return 0;

            int count = 0;
            foreach (Node item in graph.Nodes)
            {
                count += AddNode(item);
            }
            foreach (Relationship item in graph.Relationships)
            {
                count += AddRelationship(item);
            }

            return count;
        }
    }
}
