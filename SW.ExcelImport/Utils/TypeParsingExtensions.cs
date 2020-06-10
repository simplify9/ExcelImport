using System.Linq;
using System;
using ExcelDataReader;
using System.Collections.Generic;

namespace SW.ExcelImport
{
    
    public static class TypeParsingExtensions
    {
        public static Type GetEnumerablePropertyType(this Type payloadType,string propertyName)
        {
            var property = payloadType.GetProperties().Where(x=> x.Name == propertyName.ToPascalCase()).FirstOrDefault();
            if(property == null)
                return null;
            if(property.PropertyType == typeof(string))
                return null;
            
            var type = property.PropertyType;

            if(type.IsArray)
                return type.GetElementType();

            return type.GetInterfaces()
            .Where(t => t.IsGenericType
                && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            .Select(t => t.GetGenericArguments() [0]).FirstOrDefault();


            

        }
        public static int[] ParsePayloadMap(this Type payloadType, string[] header)
        {
            if (payloadType == null) throw new ArgumentNullException(nameof(payloadType));
            if (header == null) throw new ArgumentNullException(nameof(header));


            var invalidColumns = new List<int>();
            var map = new Dictionary<string,int>();

            for (int i = 0; i < header.Length; i++)
            {
                var propertyName = header[i];
                if(string.IsNullOrEmpty(propertyName))
                    invalidColumns.Add(i);
                else
                {
                    var propertyPath = PropertyPath.TryParse(payloadType, propertyName.ToPascalCase());
                    if(propertyPath == null)
                        invalidColumns.Add(i);
                    
                }    
            }
            return invalidColumns.ToArray();
        }

        
    }
}