using System;
using System.Collections.Generic;
using System.Text;

namespace AsIKnow.Graph
{
    public class RelationshipMatcher : LabelledEntityMatcher
    {
        public NodeMatcher FromNode { get; set; }
        public NodeMatcher ToNode { get; set; }
    }
}
