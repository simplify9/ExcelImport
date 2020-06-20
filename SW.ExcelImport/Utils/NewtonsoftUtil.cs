using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    public static class JsonUtil
    {
        public static JsonSerializerSettings GetSettings(JsonNamingStrategy strategy) =>
            strategy switch
            {
                JsonNamingStrategy.CamelCase => new JsonSerializerSettings 
                    { ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }},
                JsonNamingStrategy.SnakeCase => new JsonSerializerSettings 
                    { ContractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() }},
                _ => new JsonSerializerSettings { }
            };
        
        
        
    }
}