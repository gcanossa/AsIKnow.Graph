using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class GraphLabelledEntity : GraphEntity
    {
        public GraphLabelledEntity(TypeManager typeManager):base(typeManager)
        {
        }
        public GraphLabelledEntity(TypeManager typeManager, object obj) : this(typeManager)
        {
            SetFromObject(obj);
        }
        public IEnumerable<string> Labels { get; } = new List<string>();

        public override GraphEntity OfType(Type type)
        {
            ((List<string>)Labels).Clear();

            base.OfType(type);

            AddLabel(type);

            return this;
        }

        public GraphLabelledEntity AddLabels<T>()
        {
            AddLabels(typeof(T));

            return this;
        }
        public GraphLabelledEntity AddLabels(Type type)
        {
            TypeManager.GetLabels(type).ToList().ForEach(p => { if (!((List<string>)Labels).Contains(p)) ((List<string>)Labels).Add(p); });

            return this;
        }

        public GraphLabelledEntity AddLabel<T>()
        {
            AddLabel(typeof(T));

            return this;
        }
        public GraphLabelledEntity AddLabel(Type type)
        {
            string label = TypeManager.GetLabel(type);
            if (!((List<string>)Labels).Contains(label)) ((List<string>)Labels).Add(label);

            return this;
        }
        public GraphLabelledEntity RemoveLabels<T>()
        {
            RemoveLabels(typeof(T));

            return this;
        }
        public GraphLabelledEntity RemoveLabels(Type type)
        {
            TypeManager.GetLabels(type).ToList().ForEach(p => ((List<string>)Labels).Remove(p));

            return this;
        }
        public GraphLabelledEntity RemoveLabel<T>()
        {
            RemoveLabel(typeof(T));

            return this;
        }
        public GraphLabelledEntity RemoveLabel(Type type)
        {
            string label = TypeManager.GetLabel(type);
            ((List<string>)Labels).Remove(label);

            return this;
        }
        
        public override GraphEntity SetFromObject(object value)
        {
            base.SetFromObject(value);
            AddLabels(value.GetType());

            return this;
        }
        public GraphLabelledEntity SetFromObjectOnly(object value)
        {
            base.SetFromObject(value);
            AddLabel(value.GetType());

            return this;
        }

        public override T FillObject<T>(T obj = null)
        {
            obj = obj ?? new T();

            List<Type> types = TypeManager.GetTypesFromLabels(Labels).ToList();

            object tmp = TypeManager.GetInstanceOfMostSpecific(types);

            if (!typeof(T).IsAssignableFrom(tmp.GetType()))
                throw new InvalidCastException($"Unable to cast object of type {tmp.GetType().FullName}  to {typeof(T).FullName}");

            return TypeManager.AsType<T>(obj, Properties);
        }
    }
}
