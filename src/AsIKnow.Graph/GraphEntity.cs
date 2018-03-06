using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class GraphEntity : IEnumerable<KeyValuePair<string, object>>
    {
        protected TypeManager TypeManager { get; set; }
        public GraphEntity(TypeManager typeManager)
        {
            TypeManager = typeManager;
        }
        public GraphEntity(TypeManager typeManager, object obj) : this(typeManager)
        {
            SetFromObject(obj);
        }

        public IEnumerable<string> PropertiesKeys { get { return Properties.Keys; } }
        protected Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public string EntityType { get; protected set; }

        public object this[string key]
        {
            get
            {
                return Properties[key];
            }
            set
            {
                Properties[key] = value;
            }
        }

        public GraphEntity OfType<T>()
        {
            OfType(typeof(T));

            return this;
        }
        public virtual GraphEntity OfType(Type type)
        {
            EntityType = TypeManager.GetLabel(type);

            return this;
        }

        public virtual GraphEntity AddProperties<T>()
        {
            AddProperties(typeof(T));

            return this;
        }
        public virtual GraphEntity AddProperties(Type type)
        {
            TypeManager.GetPropertyNames(type).ToList().ForEach(p => { if (!Properties.ContainsKey(p)) Properties.Add(p, null); });

            return this;
        }

        public virtual GraphEntity RemoveProperties<T>()
        {
            RemoveProperties(typeof(T));

            return this;
        }
        public virtual GraphEntity RemoveProperties(Type type)
        {
            TypeManager.GetPropertyNames(type).ToList().ForEach(p => Properties.Remove(p));

            return this;
        }

        public virtual GraphEntity RemoveProps<T>(Expression<Func<T, object>> expr)
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
        public virtual GraphEntity SetProps<T>(Expression<Func<T, object>> expr, object value)
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

                if (!Properties.ContainsKey(info.Name))
                    Properties.Add(info.Name, value);
                else
                    Properties[info.Name] = value;
            }

            return this;
        }
        public virtual GraphEntity SetProps(IEnumerable<KeyValuePair<string, object>> values)
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            foreach (KeyValuePair<string, object> kv in values)
            {
                if (!Properties.ContainsKey(kv.Key))
                    Properties.Add(kv.Key, kv.Value);
                else
                    Properties[kv.Key] = kv.Value;
            }

            return this;
        }
        public virtual GraphEntity SetFromObject(object value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            Type type = value.GetType();
            OfType(type);
            foreach (PropertyInfo pinfo in type.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                Properties[pinfo.Name] = pinfo.GetValue(value);
            }

            return this;
        }

        public virtual T FillObject<T>(T obj = null) where T : class, new()
        {
            obj = obj ?? new T();

            List<Type> types = TypeManager.GetTypesFromLabels(new[] { EntityType }).ToList();

            object tmp = TypeManager.GetInstanceOfMostSpecific(types);

            if (!typeof(T).IsAssignableFrom(tmp.GetType()))
                throw new InvalidCastException($"Unable to cast object of type {tmp.GetType().FullName}  to {typeof(T).FullName}");

            return TypeManager.AsType<T>(obj, Properties);
        }

        public D As<D>() where D : GraphEntity
        {
            return this as D;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
