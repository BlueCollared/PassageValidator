using System.Text.Json;
namespace CrossCutting
{
    public class Message
    {
        public string Type { get; set; }
        public byte[] Data { get; set; }
    }

    public static class SerializationHelper
    {
        public static byte[] Serialize<T>(T obj)
        {
            var type = typeof(T).Name;
            var data = JsonSerializer.SerializeToUtf8Bytes(obj);
            var message = new Message { Type = type, Data = data };
            return JsonSerializer.SerializeToUtf8Bytes(message);
        }

        public static object Deserialize(byte[] bytes)
        {
            var message = JsonSerializer.Deserialize<Message>(bytes);
            return message.Type switch
            {
                nameof(MyObject) => JsonSerializer.Deserialize<MyObject>(message.Data),
                nameof(MyObject2) => JsonSerializer.Deserialize<MyObject2>(message.Data),
                _ => throw new InvalidOperationException("Unknown type")
            };
        }
    }

}
