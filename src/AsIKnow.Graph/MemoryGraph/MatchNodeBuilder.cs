using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public class MatchNodeBuilder : MatchLabelledEntityBuilder<GraphQueryBuilder.IMatchNodeBuilder>, GraphQueryBuilder.IMatchNodeBuilder
    {
        public GraphQueryBuilder Build()
        {
            Builder.Query = Query;

            return Builder;
        }

        public GraphQueryBuilder.IMatchExitingRelationshipBuilder WithEntering()
        {
            return new MatchExitingRelationshipBuilder()
            {
                Builder = Builder,
                Matcher = Query.AddRelationshipMatcher(new RelationshipMatcher() { ToNode = Query.AddNodeMatcher((NodeMatcher)Matcher) })
            };
        }

        public GraphQueryBuilder.IMatchExitingRelationshipBuilder WithEntering(string symbol)
        {
            MatchExitingRelationshipBuilder r = new MatchExitingRelationshipBuilder()
            {
                Builder = Builder,
                Matcher = (RelationshipMatcher)Builder.Query.MemorySymbols[symbol]
            };

            ((RelationshipMatcher)r.Matcher).ToNode = Query.AddNodeMatcher((NodeMatcher)Matcher);

            return r;
        }

        public GraphQueryBuilder.IMatchEnteringRelationshipBuilder WithExiting()
        {
            return new MatchEnteringRelationshipBuilder()
            {
                Builder = Builder,
                Matcher = Query.AddRelationshipMatcher(new RelationshipMatcher() { FromNode = Query.AddNodeMatcher((NodeMatcher)Matcher) })
            };
        }

        public GraphQueryBuilder.IMatchEnteringRelationshipBuilder WithExiting(string symbol)
        {
            MatchEnteringRelationshipBuilder r = new MatchEnteringRelationshipBuilder()
            {
                Builder = Builder,
                Matcher = (RelationshipMatcher)Builder.Query.MemorySymbols[symbol]
            };

            ((RelationshipMatcher)r.Matcher).FromNode = Query.AddNodeMatcher((NodeMatcher)Matcher);

            return r;
        }
    }
}
