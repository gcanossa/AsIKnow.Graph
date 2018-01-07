using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class MatchLabelledEntityBuilder<K> : GraphQueryBuilder.IMatchLabelledEntityBuilder<K>
    {
        public MemoryGraphQueryBuilder Builder { get; set; }
        public MemoryGraphQuery Query { get; set; }
        public LabelledEntityMatcher Matcher { get; set; }
        protected K Continue { get; set; }

        protected void AndLabel<T>()
        {
            int count = Matcher.Labels.Count;
            if (count == 0)
                Matcher.Labels.Add(new List<string>());
            Matcher.Labels[count - 1].AddRange(Builder.MemoryTypeManager.GetLabels<T>());
        }
        protected void OrLabel<T>()
        {
            Matcher.Labels.Add(new List<string>());
            AndLabel<T>();
        }
        public K WithLabel<T>()
        {
            AndLabel<T>();

            return Continue;
        }

        public K WithLabels<T1, T2>()
        {
            AndLabel<T1>();
            AndLabel<T2>();

            return Continue;
        }

        public K WithLabels<T1, T2, T3>()
        {
            AndLabel<T1>();
            AndLabel<T2>();
            AndLabel<T3>();

            return Continue;
        }

        public K WithLabels<T1, T2, T3, T4>()
        {
            AndLabel<T1>();
            AndLabel<T2>();
            AndLabel<T3>();
            AndLabel<T4>();

            return Continue;
        }

        public K WithLabels<T1, T2, T3, T4, T5>()
        {
            AndLabel<T1>();
            AndLabel<T2>();
            AndLabel<T3>();
            AndLabel<T4>();
            AndLabel<T5>();

            return Continue;
        }

        public K WithLabels<T1, T2, T3, T4, T5, T6>()
        {
            AndLabel<T1>();
            AndLabel<T2>();
            AndLabel<T3>();
            AndLabel<T4>();
            AndLabel<T5>();
            AndLabel<T6>();

            return Continue;
        }

        public K WithProperties<T>(Expression<Func<T, bool>> expr = null)
        {
            if (expr != null)
            {
                Func<T, bool> m = expr.Compile();
                Matcher.Where = p => m(p.GetObject<T>());
            }

            return Continue;
        }

        public K WithSymbol(string name)
        {
            if (Builder.Query.MemorySymbols.ContainsKey(name))
                throw new ArgumentException($"Duplicate symbol: {name}", nameof(name));
            Builder.Query.MemorySymbols.Add(name, Matcher);

            return Continue;
        }
    }
}
