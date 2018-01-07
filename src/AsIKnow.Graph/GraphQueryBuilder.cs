using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class GraphQueryBuilder
    {
        #region nested types

        public interface IMatchBuilder
        {
            IMatchNodeBuilder StartNode();
        }
        public interface IMatchLabelledEntityBuilder<K>
        {
            K WithLabel<T>();
            K WithLabels<T1, T2>();
            K WithLabels<T1, T2, T3>();
            K WithLabels<T1, T2, T3, T4>();
            K WithLabels<T1, T2, T3, T4, T5>();
            K WithLabels<T1, T2, T3, T4, T5, T6>();
            K WithProperties<T>(Expression<Func<T, bool>> expr = null);
            K WithSymbol(string name);
        }
        public interface IMatchNodeBuilder : IMatchLabelledEntityBuilder<IMatchNodeBuilder>
        {
            IMatchEnteringRelationshipBuilder WithExiting();
            IMatchEnteringRelationshipBuilder WithExiting(string symbol);
            IMatchExitingRelationshipBuilder WithEntering();
            IMatchExitingRelationshipBuilder WithEntering(string symbol);
            GraphQueryBuilder Build();
        }
        public interface IMatchEnteringRelationshipBuilder : IMatchLabelledEntityBuilder<IMatchEnteringRelationshipBuilder>
        {
            IMatchNodeBuilder WithToNode();
            IMatchNodeBuilder WithToNode(string symbol);
        }
        public interface IMatchExitingRelationshipBuilder : IMatchLabelledEntityBuilder<IMatchExitingRelationshipBuilder>
        {
            IMatchNodeBuilder WithFromNode();
            IMatchNodeBuilder WithFromNode(string symbol);
        }

        #endregion

        protected TypeManager TypeManager { get; set; }
        public GraphQueryBuilder(TypeManager typeManager)
        {
            TypeManager = typeManager;
        }

        public abstract IMatchBuilder Match();

        public abstract GraphQuery Build();
    }
}
