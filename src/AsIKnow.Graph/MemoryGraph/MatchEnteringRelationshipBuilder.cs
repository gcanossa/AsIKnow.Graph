using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public class MatchEnteringRelationshipBuilder : MatchLabelledEntityBuilder<GraphQueryBuilder.IMatchEnteringRelationshipBuilder>, GraphQueryBuilder.IMatchEnteringRelationshipBuilder
    {
        public GraphQueryBuilder.IMatchNodeBuilder WithToNode()
        {
            MatchNodeBuilder r = new MatchNodeBuilder()
            {
                Builder = Builder,
                Matcher = Query.AddNodeMatcher(new NodeMatcher())
            };

            ((NodeMatcher)r.Matcher).EnteringRelationships.Add(Query.AddRelationshipMatcher((RelationshipMatcher)Matcher));

            return r;
        }

        public GraphQueryBuilder.IMatchNodeBuilder WithToNode(string symbol)
        {
            MatchNodeBuilder r = new MatchNodeBuilder()
            {
                Builder = Builder,
                Matcher = (NodeMatcher)Builder.Query.MemorySymbols[symbol]
            };

            if (!((NodeMatcher)r.Matcher).EnteringRelationships.Contains((RelationshipMatcher)Matcher))
                ((NodeMatcher)r.Matcher).EnteringRelationships.Add(Query.AddRelationshipMatcher((RelationshipMatcher)Matcher));

            return r;
        }
    }
}
