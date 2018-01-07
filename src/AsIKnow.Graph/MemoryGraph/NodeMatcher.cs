using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public class NodeMatcher : LabelledEntityMatcher
    {
        public List<RelationshipMatcher> EnteringRelationships { get; } = new List<RelationshipMatcher>();
        public List<RelationshipMatcher> ExitingRelationships { get; } = new List<RelationshipMatcher>();
    }
}
