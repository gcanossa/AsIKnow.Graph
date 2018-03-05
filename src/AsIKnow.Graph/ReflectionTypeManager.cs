using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AsIKnow.Graph
{
    public class ReflectionTypeManager : TypeManager
    {
        public ReflectionTypeManager(TypeManagerOptions options) : base(options)
        {

        }
        public override string GetLabel(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
            
            return Options.UseFullNameInLables ? type.FullName : type.Name;
        }
        public override IEnumerable<Type> GetTypesFromLabels(IEnumerable<string> labels)
        {
            if (labels == null)
                throw new ArgumentNullException(nameof(labels));

            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(p => p.GetTypes());

            return labels.Select(p => types.FirstOrDefault(x => Options.UseFullNameInLables ? x.FullName == p : x.Name == p));
        }
        public override IEnumerable<string> GetLabels(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            HashSet<string> result = new HashSet<string>();
            result.Add(GetLabel(type));

            foreach (Type item in type.GetInterfaces())
            {
                result.Add(GetLabel(item));
            }

            while (type != typeof(object))
            {
                type = type.BaseType;
                if(type != typeof(object))
                    result.Add(GetLabel(type));
            }

            return result;
        }
        public override T AsType<T>(T obj, Dictionary<string, object> properties)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            if (properties == null)
                throw new ArgumentNullException(nameof(properties));

            foreach (PropertyInfo pinfo in obj.GetType().GetProperties().Where(p => p.CanWrite && properties.Keys.Contains(p.Name)))
            {
                if (!pinfo.PropertyType.IsAssignableFrom(properties[pinfo.Name].GetType()))
                    throw new ArgumentException($"Type mismatch for property {pinfo.Name}.", nameof(properties));
                pinfo.SetValue(obj, properties[pinfo.Name]);
            }

            return obj;
        }
        public override Dictionary<string, object> FromType(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (PropertyInfo pinfo in obj.GetType().GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                result.Add(pinfo.Name, pinfo.GetValue(obj));
            }

            return result;
        }

        public override IEnumerable<string> GetPropertyNames(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            List<string> result = new List<string>();

            foreach (PropertyInfo pinfo in type.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                result.Add(pinfo.Name);
            }

            return result;
        }
        public override bool CheckObjectInclusion(object obj, object included)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (included == null)
                return false;

            Type type = obj.GetType();
            foreach (PropertyInfo pinfo in included.GetType().GetProperties().Where(p=>p.CanRead))
            {
                PropertyInfo tmp = type.GetProperty(pinfo.Name);
                if (
                    tmp == null || 
                    pinfo.PropertyType != tmp.PropertyType  || 
                    (pinfo.GetValue(included) == null && tmp.GetValue(obj) != null) ||
                    (pinfo.GetValue(included) != null && tmp.GetValue(obj) == null) ||
                    (pinfo.GetValue(included) != null && tmp.GetValue(obj) != null && !pinfo.GetValue(included).Equals(tmp.GetValue(obj))))
                    return false;
            }
            return true;
        }

        public override object GetInstanceOfMostSpecific(IEnumerable<Type> types)
        {
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            Type type = null;
            foreach (Type t in types.Where(p=>!p.IsInterface && !p.IsAbstract && p.GetConstructor(new Type[0])!=null))
            {
                if (type == null || type.IsAssignableFrom(t))
                    type = t;
            }

            if (type == null)
                throw new InvalidOperationException("None of the provided types can be used for instantiation");

            return Activator.CreateInstance(type);
        }
    }
}
