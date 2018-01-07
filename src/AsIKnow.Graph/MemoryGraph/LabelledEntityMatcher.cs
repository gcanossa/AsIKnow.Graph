using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class LabelledEntityMatcher
    {
        public List<List<string>> Labels { get; } = new List<List<string>>();

        public Func<LabelledEntity, bool> Where { get; set; }

        public bool Match(LabelledEntity entity)
        {
            bool labels = false;
            bool where = false;

            if (Labels.Count > 0)
            {
                foreach (List<string> item in Labels)
                {
                    int c = entity.Labels.Intersect(item).Count();
                    if (c == entity.Labels.Count() && c == item.Count)
                    {
                        labels = true;
                        break;
                    }
                }
            }
            else
                labels = true;

            where = Where?.Invoke(entity) ?? true;

            return labels && where;
        }
    }
}
