using System;
using Newtonsoft.Json;

namespace SW.ExcelImport
{
    public class ObjectSerialized
    {
        public string AssemblyQualifiedTypeName { get; set; }
        public string Data { get; set; }
    }
    public static class Extensions
    {
        public static Type OnType(this TypedParseToJsonOptions options)
        {
            return Type.GetType(options.TypeAssemblyQualifiedName);
        }

        public static ObjectSerialized ToObjectSerialized(this object obj)
        {
            return new ObjectSerialized
            {
                AssemblyQualifiedTypeName = obj.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(obj)
            };
        }

        public static Object ToObject(this ObjectSerialized objectSerialized)
        {
            return JsonConvert.DeserializeObject(objectSerialized.Data,Type.GetType(objectSerialized.AssemblyQualifiedTypeName));
        }
        
    }
}
