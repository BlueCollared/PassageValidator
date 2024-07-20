using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NamedPipeHandler
{
    public interface IRequest { }
    public interface IResponse { }
    internal class RequestConverter : JsonConverter
    {
        private readonly string typePropertyName = "$type";
        private readonly Dictionary<string, Type> typeMapping;

        public RequestConverter(Dictionary<string, Type> typeMapping)
        {
            this.typeMapping = typeMapping; 
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IRequest).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            var typeName = (string)jo[typePropertyName];

            if (typeMapping.TryGetValue(typeName, out Type type))
            {
                return jo.ToObject(type, serializer);
            }

            throw new Exception($"Unknown type: {typeName}");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JObject jo = JObject.FromObject(value, serializer);
            jo.AddFirst(new JProperty(typePropertyName, value.GetType().Name));
            jo.WriteTo(writer);
        }
    }
}
