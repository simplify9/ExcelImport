using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace SW.ExcelImport
{
    public class PropertyPath
    {
        private readonly Type _rootType;
        private readonly PropertyInfo[] _properties;
        


        public object Evaluate(object obj)
        {
            foreach (var prop in _properties)
            {
                if (obj == null) return null;
                obj = prop.GetValue(obj);
            }
            return obj;
        }
        
        public void Set(object obj, object value)
        {
            if (_properties.Length < 1) throw new NotSupportedException("property path must have at least one property, otherwise setting a value is not supported");
            var container = obj;
            var propQ = new Queue<PropertyInfo>(Properties);
            while (propQ.Count > 0)
            {
                var prop = propQ.Dequeue();
                if (propQ.Count == 0)
                {
                    // --> leaf property, set requested value
                    prop.SetValue(container, value);
                }
                else
                {
                    // --> container property, initialize if not initialized yet
                    var propValue = prop.GetValue(container);
                    if (propValue == null)
                    {
                        propValue = Activator.CreateInstance(prop.PropertyType);
                        prop.SetValue(container, propValue);
                    }
                    container = propValue;
                }
            }
        }
        

        public static PropertyPath FromExpression(Expression propertySelectorBody)
        {
            var propStack = new Stack<PropertyInfo>();
            
            var next = propertySelectorBody;
            while (!(next is ParameterExpression))
            {
                var eProp = next as MemberExpression;
                propStack.Push(eProp.Member as PropertyInfo);
                next = eProp.Expression;
            }
            return new PropertyPath(next.Type, propStack);
        }

        public static PropertyPath TryParse(Type rootType, string pathString)
        {
            var stringParts = pathString.Split('.');
            var propPath = new PropertyPath(rootType);
            foreach (var part in stringParts)
            {
                propPath = propPath.TryGoDown(part);
                if (propPath == null) return null;
            }
            return propPath;
        }
        
        public Type PropertyType
        {
            get
            {
                return _properties.LastOrDefault()?.PropertyType ?? _rootType;
            }
        }

        public Expression CreateMemberExpression(Expression expr)
        {
            foreach (var prop in _properties) expr = Expression.Property(expr, prop);
            return expr;
        }
        
        public PropertyPath GoDown(PropertyInfo property)
        {
            if (!property.DeclaringType.IsAssignableFrom(PropertyType))
            {
                throw new ArgumentException("Property does not belong to type");
            }

            var newProperties = _properties.Concat(new[] { property });
            return new PropertyPath(_rootType, newProperties);
        }

        public PropertyPath Extend(PropertyPath extension)
        {
            if (extension.RootType != PropertyType)
            {
                throw new ArgumentException("extension path is not compatible with self");
            }
            return new PropertyPath(_rootType, _properties.Concat(extension._properties));
        }

        public PropertyPath TryGoDown(string propertyName)
        {
            var property = PropertyType.GetProperty(propertyName);
            if (property == null) return null;
            else return GoDown(property);
        }

        public PropertyPath GoDown(string propertyName)
        {
            
            var property = PropertyType.GetProperty(propertyName);
            if (property == null)
            {
                var msg = string.Format("Property '{0}' does not exist in type '{1}'", propertyName, PropertyType.FullName);
                throw new ArgumentException(msg);
            }
            else return GoDown(property);
        }

        public PropertyPath GoUp()
        {
            if (!_properties.Any())
            {
                throw new InvalidOperationException("Property path is at root");
            }
            var newProperties = _properties.Except(new[] { _properties.Last() });
            return new PropertyPath(_rootType, newProperties);
        }

        public PropertyPath(Type rootType)
        {
            _rootType = rootType;
            _properties = new PropertyInfo[] { };
        }
        
        private PropertyPath(Type rootType, IEnumerable<PropertyInfo> properties)
        {
            _rootType = rootType;
            _properties = properties.ToArray();
        }

        public Type RootType { get { return _rootType; } }

        public override string ToString()
        {
            return string.Join(".", _properties.Select(p => p.Name));
        }

        public override bool Equals(object obj)
        {
            var otherPath = obj as PropertyPath;
            if (otherPath == null) return false;
            return RootType.Equals(otherPath.RootType) && ToString().Equals(otherPath.ToString());        
        }

        public override int GetHashCode()
        {
            return (_rootType.FullName + ":" + ToString()).GetHashCode();
        }

        public PropertyInfo[] Properties { get { return _properties; } }
    }
}