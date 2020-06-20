using System.Linq;
using System;
using ExcelDataReader;
using System.Collections.Generic;

namespace SW.ExcelImport
{
    
    public static class TypeParsingExtensions
    {
        public static Type GetEnumerablePropertyType(this Type payloadType,string propertyName, JsonNamingStrategy namingStrategy)
        {
            var property = payloadType.GetProperties().Where(x=> x.Name == propertyName.Transform(namingStrategy)).FirstOrDefault();
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
        
    }
}