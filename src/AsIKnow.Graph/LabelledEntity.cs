using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class LabelledEntity
    {
        protected TypeManager TypeManager { get; set; }
        public LabelledEntity(TypeManager typeManager)
        {
            TypeManager = typeManager;
        }
        public LabelledEntity(TypeManager typeManager, object obj) : this(typeManager)
        {
            SetFromObject(obj);
        }
        public IEnumerable<string> Labels { get; } = new List<string>();
        public IEnumerable<string> PropertiesKeys { get { return Properties.Keys; } }
        protected Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public LabelledEntity AddLabel<T>()
        {
            AddLabel(typeof(T));

            return this;
        }
        public LabelledEntity AddLabel(Type type)
        {
            TypeManager.GetLabels(type).ForEach(p => { if (!((List<string>)Labels).Contains(p)) ((List<string>)Labels).Add(p); });
            TypeManager.GetPropertyNames(type).ForEach(p => { if(!Properties.ContainsKey(p)) Properties.Add(p, null); });

            return this;
        }
        public LabelledEntity RemoveLabel<T>()
        {
            RemoveLabel(typeof(T));

            return this;
        }
        public LabelledEntity RemoveLabel(Type type)
        {
            TypeManager.GetLabels(type).ForEach(p => ((List<string>)Labels).Remove(p));
            TypeManager.GetPropertyNames(type).ForEach(p => Properties.Remove(p));

            return this;
        }
        public LabelledEntity RemoveProperty<T>(Expression<Func<T, object>> expr)
        {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));

            NewExpression nexpr = expr.Body as NewExpression;

            if (nexpr != null)
            {
                foreach (PropertyInfo pinfo in nexpr.Members)
                {
                    Properties.Remove(pinfo.Name);
                }
            }
            else
            {
                Properties.Remove((expr.Body as MemberExpression ?? ((UnaryExpression)expr.Body).Operand as MemberExpression).Member.Name);
            }

            return this;
        }
        public LabelledEntity SetProperty<T>(Expression<Func<T, object>> expr, object value)
        {
            if (expr == null)
                throw new ArgumentNullException(nameof(expr));

            NewExpression nexpr = expr.Body as NewExpression;

            if (nexpr != null)
            {
                if (value == null)
                {
                    foreach (PropertyInfo pinfo in nexpr.Members)
                    {
                        Properties.Remove(pinfo.Name);
                    }
                }
                else
                {
                    foreach (PropertyInfo pinfo in nexpr.Members)
                    {
                        object tmp = pinfo.GetValue(value);
                        if (tmp == null)
                            Properties.Remove(pinfo.Name);
                        if (!tmp.GetType().IsAssignableFrom(pinfo.PropertyType))
                            throw new ArgumentException($"Type mismatch for property {pinfo.Name}.", nameof(expr));

                        if (!Properties.ContainsKey(pinfo.Name))
                            Properties.Add(pinfo.Name, tmp);
                        else
                            Properties[pinfo.Name] = tmp;
                    }
                }
            }
            else
            {
                MemberInfo info = (expr.Body as MemberExpression ?? ((UnaryExpression)expr.Body).Operand as MemberExpression).Member;

                if (value == null)
                    Properties.Remove(info.Name);
                if (!value.GetType().IsAssignableFrom(((PropertyInfo)info).PropertyType))
                    throw new ArgumentException($"Type mismatch for property {info.Name}.", nameof(expr));

                if (!Properties.ContainsKey(info.Name))
                    Properties.Add(info.Name, value);
                else
                    Properties[info.Name] = value;
            }

            return this;
        }
        public LabelledEntity SetFromObject(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Type type = value.GetType();
            AddLabel(type);
            foreach (PropertyInfo pinfo in type.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                Properties[pinfo.Name] = pinfo.GetValue(value);
            }

            return this;
        }

        public T GetObject<T>()
        {
            List<Type> types = TypeManager.GetTypesFromLabels(Labels);

            object obj = TypeManager.GetInstanceOfMostSpecific(types);

            if (!typeof(T).IsAssignableFrom(obj.GetType()))
                throw new InvalidCastException($"Unable to cast object of type {obj.GetType().FullName}  to {typeof(T).FullName}");

            return (T)TypeManager.AsType(obj, Properties);
        }
        public T FillObject<T>(T obj)
        {
            List<Type> types = TypeManager.GetTypesFromLabels(Labels);

            object tmp = TypeManager.GetInstanceOfMostSpecific(types);

            if (!typeof(T).IsAssignableFrom(tmp.GetType()))
                throw new InvalidCastException($"Unable to cast object of type {tmp.GetType().FullName}  to {typeof(T).FullName}");

            return TypeManager.AsType<T>(obj, Properties);
        }
    }
}
