using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public class MemoryGraphQuery : GraphQuery
    {
        internal Dictionary<string, object> MemorySymbols { get { return Symbols; } }
        protected List<NodeMatcher> Nodes { get; } = new List<NodeMatcher>();
        protected List<RelationshipMatcher> Relationships { get; } = new List<RelationshipMatcher>();

        public NodeMatcher AddNodeMatcher(NodeMatcher matcher)
        {
            if (!Nodes.Contains(matcher))
                Nodes.Add(matcher);
            return matcher;
        }
        public RelationshipMatcher AddRelationshipMatcher(RelationshipMatcher matcher)
        {
            if (!Relationships.Contains(matcher))
                Relationships.Add(matcher);
            return matcher;
        }

        protected void FindStartingPoints(SubGraph graph, out Dictionary<NodeMatcher, List<Tuple<string, Node>>> nodes, out Dictionary<RelationshipMatcher, List<Tuple<string, Relationship>>> rels)
        {
            nodes = Symbols
                    .Where(p => p.Value is NodeMatcher)
                    .ToDictionary(
                        p => (NodeMatcher)p.Value,
                        p => graph.Nodes.Where(t => 
                            ((NodeMatcher)p.Value).Match(t)
                            && ((NodeMatcher)p.Value).EnteringRelationships.All(x => t.EnteringRelationships.Any(q=>x.Match(q)))
                            && ((NodeMatcher)p.Value).ExitingRelationships.All(x => t.ExitingRelationships.Any(q => x.Match(q)))
                        )
                        .Select(t => new Tuple<string, Node>(p.Key, t))
                        .ToList()
                );
            foreach (var el in Nodes
                    .Where(p => !Symbols.Values.Contains(p))
                    .Select(p => new KeyValuePair<NodeMatcher, List<Tuple<string, Node>>>(
                        p,
                        graph.Nodes.Where(t => 
                            p.Match(t)
                            && p.EnteringRelationships.All(x=> t.EnteringRelationships.Any(q=>x.Match(q)))
                            && p.ExitingRelationships.All(x => t.ExitingRelationships.Any(q => x.Match(q)))
                        )
                        .Select(t => new Tuple<string, Node>(null, t))
                        .ToList())
                ))
            {
                nodes.Add(el.Key, el.Value);
            }

            rels = Symbols
                .Where(p => p.Value is RelationshipMatcher)
                .ToDictionary(
                    p => (RelationshipMatcher)p.Value,
                    p => graph.Relationships.Where(t => 
                        ((RelationshipMatcher)p.Value).Match(t)
                        && ((RelationshipMatcher)p.Value).FromNode.Match(t.FromNode)
                        && ((RelationshipMatcher)p.Value).ToNode.Match(t.ToNode)
                    )
                    .Select(t => new Tuple<string, Relationship>(p.Key, t))
                    .ToList()
            );
            foreach (var el in Relationships
                    .Where(p => !Symbols.Values.Contains(p))
                    .Select(p => new KeyValuePair<RelationshipMatcher, List<Tuple<string, Relationship>>>(
                        p,
                        graph.Relationships.Where(t => 
                            p.Match(t)
                            && p.FromNode.Match(t.FromNode)
                            && p.ToNode.Match(t.ToNode)
                        )
                        .Select(t => new Tuple<string, Relationship>(null, t))
                        .ToList())
                ))
            {
                rels.Add(el.Key, el.Value);
            }
        }

        protected void ReduceEdgesAndArcs(Dictionary<NodeMatcher, List<Tuple<string, Node>>> nodes, Dictionary<RelationshipMatcher, List<Tuple<string, Relationship>>> rels)
        {
            //foreach (var nm in nodes)
            //{
            //    foreach (var n in nm.Value.ToArray())
            //    {
            //        if (!nm.Key.EnteringRelationships.All(p => n.Item2.EnteringRelationships.Any(t => p.Match(t)))
            //            ||
            //            !nm.Key.ExitingRelationships.All(p => n.Item2.ExitingRelationships.Any(t => p.Match(t))))
            //        {
            //            nm.Value.Remove(n);
            //        }
            //    }
            //}
        }

        public override List<SubGraph> ExecuteQuery(SubGraph graph)
        {
            Dictionary<NodeMatcher, List<Tuple<string, Node>>> nodes;
            Dictionary<RelationshipMatcher, List<Tuple<string, Relationship>>> rels;

            FindStartingPoints(graph, out nodes, out rels);

            //TODO: finisci

            List<SubGraph> result = new List<SubGraph>();

            return result;
        }
    }
}
