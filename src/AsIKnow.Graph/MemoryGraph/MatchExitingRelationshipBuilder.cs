using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public class MatchExitingRelationshipBuilder : MatchLabelledEntityBuilder<GraphQueryBuilder.IMatchExitingRelationshipBuilder>, GraphQueryBuilder.IMatchExitingRelationshipBuilder
    {
        public GraphQueryBuilder.IMatchNodeBuilder WithFromNode()
        {
            MatchNodeBuilder r = new MatchNodeBuilder()
            {
                Builder = Builder,
                Matcher = Query.AddNodeMatcher(new NodeMatcher())
            };

            ((NodeMatcher)r.Matcher).ExitingRelationships.Add(Query.AddRelationshipMatcher((RelationshipMatcher)Matcher));

            return r;
        }

        public GraphQueryBuilder.IMatchNodeBuilder WithFromNode(string symbol)
        {
            MatchNodeBuilder r = new MatchNodeBuilder()
            {
                Builder = Builder,
                Matcher = (NodeMatcher)Builder.Query.MemorySymbols[symbol]
            };

            if(!((NodeMatcher)r.Matcher).ExitingRelationships.Contains((RelationshipMatcher)Matcher))
                ((NodeMatcher)r.Matcher).ExitingRelationships.Add(Query.AddRelationshipMatcher((RelationshipMatcher)Matcher));

            return r;
        }
    }
}
