using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public static class StringParser
    {
        public static bool TryParse(string rawValue, Type resultType, out object result)
        {
            if (rawValue == null) throw new ArgumentNullException("rawValue");
            if (resultType == null) throw new ArgumentNullException("resultType");

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

            if (resultType.IsEnum || 
                (   resultType.IsGenericType && 
                    resultType.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                    resultType.GetGenericArguments()[0].IsEnum))
            {
                var unwrappedEnumType = resultType.IsGenericType ?
                    resultType.GetGenericArguments()[0] :
                    resultType;
                try
                {
                    result = Enum.Parse(unwrappedEnumType, rawValue);
                    return true;
                }
                catch (Exception)
                {
                    result = null;
                    return false;
                }
            }

            var msg = string.Format("Parsing a string into type '{0}' is not supported", resultType.FullName);
            throw new NotSupportedException(msg);
        } 
    }

}
