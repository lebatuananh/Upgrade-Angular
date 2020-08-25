using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Base.Logging.StaticConfig
{
    public static class NewtonJsonSerializerSettings
    {
        public static JsonSerializerSettings CamelIgnoreNullOutput = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),

            }
        };
        public static readonly JsonSerializerSettings SNAKE = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        public static readonly JsonSerializerSettings CAMEL = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };
    }
}
