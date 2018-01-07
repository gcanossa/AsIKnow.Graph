using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AsIKnow.Graph
{
    public abstract class TypeManager
    {
        protected TypeManagerOptions Options { get; set; }
        public TypeManager(TypeManagerOptions options)
        {
            Options = options;
        }

        public string GetLablel(object obj)
        {
            return GetLabel(obj?.GetType());
        }
        public string GetLabel<T>()
        {
            return GetLabel(typeof(T));
        }
        public abstract string GetLabel(Type type);

        public List<string> GetLabels(object obj)
        {
            return GetLabels(obj?.GetType());
        }
        public List<string> GetLabels<T>()
        {
            return GetLabels(typeof(T));
        }
        public abstract List<Type> GetTypesFromLabels(IEnumerable<string> labels);
        public abstract List<string> GetLabels(Type type);
        public List<string> GetPropertyNames(object obj)
        {
            return GetPropertyNames(obj?.GetType());
        }
        public List<string> GetPropertyNames<T>()
        {
            return GetPropertyNames(typeof(T));
        }
        public abstract List<string> GetPropertyNames(Type type);

        public abstract object GetInstanceOfMostSpecific(IEnumerable<Type> types);

        public T AsType<T>(Dictionary<string, object> properties) where T : new()
        {
            return AsType(new T(), properties);
        }
        public T AsType<T>(T obj, Dictionary<string, object> properties)
        {
            return AsType(obj, properties);
        }
        public abstract object AsType(object obj, Dictionary<string, object> properties);
        public abstract Dictionary<string, object> FromType(object obj);

        public abstract bool CheckObjectInclusion(object obj, object included);
    }
}
