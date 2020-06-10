using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public static class TypeExtensions
    {
        public static string ToPascalCase(this string s)
        {
            var yourString = s.ToLower().Replace("_", " ");
            var info = CultureInfo.CurrentCulture.TextInfo;
            return info.ToTitleCase(yourString).Replace(" ", string.Empty);
        }
        public static object CreateFromDictionary(this Type t, IDictionary<string,object> valueMap)
        {
            var obj = Activator.CreateInstance(t);
            foreach (var pair in valueMap)
            {
                var propertyPath = PropertyPath.TryParse(t, pair.Key.ToPascalCase());
                if (propertyPath == null)
                {
                    var msg = string.Format("Property path '{0}' is invalid", pair.Key);
                    throw new ArgumentException(msg);
                }

                
                propertyPath.Set(obj, pair.Value);
                
            }
            return obj;
        }
        
        public static bool IsBuiltIn(this Type t)
        {
            return (t.Namespace == "System" || t.Namespace.StartsWith("System") || t.Module.ScopeName == "CommonLanguageRuntimeLibrary");
            
        }

        public static IEnumerable<Type> GetInterfacesRecursive(this Type t)
        {
            var parent = t;
            while (parent != null && parent != typeof(object))
            {
                var interfaces = parent.GetInterfaces();
                foreach (var i in interfaces) yield return i;
                parent = parent.BaseType;
            }
        }

        private static IEnumerable<PropertyPath> IterateProperties(PropertyPath baseProp, int maxNesting = 10)
        {
            var publicProps = baseProp.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var prop in publicProps)
            {
                if (prop.PropertyType.IsBuiltIn())
                {
                    yield return baseProp.GoDown(prop);
                }
                else
                {
                    var subProps = IterateProperties(baseProp.GoDown(prop), maxNesting);
                    foreach (var subProp in subProps) yield return subProp;
                }
            }
        }

        public static IEnumerable<PropertyPath> GetPropertiesRecursive(this Type t, int maxNesting = 10)
        {
            return IterateProperties(new PropertyPath(t), maxNesting);
        }

        public static Type GetEnumerableTypeArgument(this Type t, bool includeString = false)
        {
            if (!includeString && t.Equals(typeof(string))) return null;
            return GetInterfacesRecursive(t).Where(x =>
                x.IsGenericType &&
                x.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                .Select(x => x.GetGenericArguments()[0]).FirstOrDefault();
        }
    }
}
