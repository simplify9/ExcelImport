using System;
using System.Text;
using System.Globalization;

namespace SW.ExcelImport
{
    public static class Converter
    {
        public static object GetDefault(this Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
        public static bool TryCreate(object value, Type resultType, out object result)
        {
            if (resultType == null) throw new ArgumentNullException(nameof(resultType));
            result = null;
            
            if(value == null)
            {
                if(!resultType.IsValueType)
                    return true;
                
                if(Activator.CreateInstance(resultType) == null)
                    return true;
                else 
                    return false;
            }
                
            
            var sourceType = value.GetType();

            if (sourceType == resultType)
            {
                result = value;
                return true;
            }
            if (sourceType == typeof(DateTime) && resultType == typeof(DateTime?))
            {
                result = value;
                return true;
            }

            var rawValue = value.ToString();

            if (resultType.Equals(typeof(string)))
            {
                result = rawValue;
                return true;
            }

            if (rawValue == string.Empty)
            {
                if (resultType.IsValueType) result = Activator.CreateInstance(resultType);
                else result = null;
                return true;
            }

            if (resultType.Equals(typeof(bool)) || resultType.Equals(typeof(bool?)))
            {
                bool boolResult;
                var correct = bool.TryParse(rawValue, out boolResult);
                result = boolResult;
                return correct;
            }

            if (resultType.Equals(typeof(decimal)) || resultType.Equals(typeof(decimal?)))
            {
                decimal typedResult;
                var correct = decimal.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(double)) || resultType.Equals(typeof(double?)))
            {
                double typedResult;
                var correct = double.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(float)) || resultType.Equals(typeof(float?)))
            {
                float typedResult;
                var correct = float.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(int)) || resultType.Equals(typeof(int?)))
            {
                int typedResult;
                var correct = int.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(long)) || resultType.Equals(typeof(long?)))
            {
                long typedResult;
                var correct = long.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(char)) || resultType.Equals(typeof(char?)))
            {
                char typedResult;
                var correct = char.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(Guid)) || resultType.Equals(typeof(Guid?)))
            {
                Guid typedResult;
                var correct = Guid.TryParse(rawValue, out typedResult);
                result = typedResult;
                return correct;
            }

            if (resultType.Equals(typeof(DateTime)) || resultType.Equals(typeof(DateTime?)))
            {
                try
                {
                    var formats = new[] { "dd/MM/yyyy", "o" };
                    result = DateTime.ParseExact(rawValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None);
                    return true;
                }
                catch (FormatException)
                {
                    result = null;
                    return false;
                }
            }

            if (resultType.Equals(typeof(string[])))
            {

                result = rawValue?.Split(new char[] { ';' });
                return true;
            }
            result = null;
            return false;
        }
    }
}