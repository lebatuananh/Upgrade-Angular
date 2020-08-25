namespace DVG.WIS.Utilities.Serialization
{
    using System.IO;
    using Newtonsoft.Json;
    using System.Runtime.Serialization;

    public class JsonTextSerializer : ITextSerializer
    {
        private readonly JsonSerializer serializer;
        public JsonTextSerializer()
            : this(JsonSerializer.Create(new JsonSerializerSettings
            {
                // Allows deserializing to the actual runtime type
                TypeNameHandling = TypeNameHandling.Objects,
                Formatting = Formatting.Indented
            }))
        {
        }

        public JsonTextSerializer(JsonSerializer serializer)
        {
            this.serializer = serializer;
        }

        public void Serialize<T>(TextWriter writer, T graph)
        {
            var jsonWriter = new JsonTextWriter(writer);

            this.serializer.Serialize(jsonWriter, graph);

            // We don't close the stream as it's owned by the message.
            writer.Flush();
        }

        public T Deserialize<T>(TextReader reader)
        {
            var jsonReader = new JsonTextReader(reader);

            try
            {
                return this.serializer.Deserialize<T>(jsonReader);
            }
            catch (JsonSerializationException e)
            {
                // Wrap in a standard .NET exception.
                throw new SerializationException(e.Message, e);
            }
        }
    }
}
