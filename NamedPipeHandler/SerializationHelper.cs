namespace NamedPipeHandler
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public static class SerializationHelper
    {
        //private static Dictionary<string, Type> typeMapping = new Dictionary<string, Type>();

        //public static void RegisterType<T>() where T : IRequest
        //{
        //    var type = typeof(T);
        //    typeMapping[type.Name] = type;
        //}

        public static string SerializeRequest(IRequest request, JsonSerializerSettings settings)
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.Converters.Add(new RequestConverter(typeMapping));
            return JsonConvert.SerializeObject(request, settings);
        }

        public static IRequest DeserializeRequest(string json, JsonSerializerSettings settings)
        {
            //JsonSerializerSettings settings = new JsonSerializerSettings();
            //settings.Converters.Add(new RequestConverter(typeMapping));
            return JsonConvert.DeserializeObject<IRequest>(json, settings);
        }
    }
}
