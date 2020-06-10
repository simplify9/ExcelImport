using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public static class AnnotationExtensions
    {
        private static readonly Dictionary<PropertyPath, List<Attribute>> _runtimeAnnotations
            = new Dictionary<PropertyPath, List<Attribute>>();

        public static IEnumerable<Attribute> GetAnnotations(this PropertyPath path)
        {
            IEnumerable<Attribute> attributes;
            if (path.Properties.Any()) attributes = path.Properties.Last().GetCustomAttributes(false).OfType<Attribute>();
            else attributes = path.RootType.GetCustomAttributes(false).OfType<Attribute>();
            List<Attribute> v;
            if (_runtimeAnnotations.TryGetValue(path, out v))
            {
                attributes = attributes.Concat(v);
            }
            return attributes;
        }

        public static IEnumerable<Attribute> GetAnnotations(this Type t)
        {
            var path = new PropertyPath(t);
            return path.GetAnnotations();
        }

        public static IEnumerable<Attribute> GetAnnotations(this PropertyInfo property)
        {
            var path = new PropertyPath(property.DeclaringType).GoDown(property);
            return path.GetAnnotations();
        }

        public static PropertyPath Annotate(this PropertyPath path, Attribute a)
        {
            List<Attribute> v;
            if (!_runtimeAnnotations.TryGetValue(path, out v))
            {
                v = new List<Attribute>();
                _runtimeAnnotations[path] = v;
            }
            v.Add(a);
            return path;
        }
    }
}
