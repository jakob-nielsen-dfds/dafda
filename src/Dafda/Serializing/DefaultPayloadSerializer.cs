using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dafda.Serializing
{
    /// <summary>
    /// The default <see cref="T:System.Text.Json"/> payload serializer.
    /// </summary>
    public class DefaultPayloadSerializer : IPayloadSerializer
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        /// <summary>
        /// Initialize an instance of the class
        /// </summary>
        public DefaultPayloadSerializer()
        {
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = false,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }

        /// <inheritdoc />
        public string PayloadFormat { get; } = "application/json";

        /// <inheritdoc />
        public Task<string> Serialize(PayloadDescriptor payloadDescriptor)
        {
            var result = JsonSerializer.Serialize(
                value: ConvertToMessagePayload(payloadDescriptor),
                options: _jsonSerializerOptions
            );

            return Task.FromResult(result);
        }

        private object ConvertToMessagePayload(PayloadDescriptor descriptor)
        {
            // NOTE: due to a bug in system.text.json an additional conversion 
            // is made to have proper dictionary key casing - on envelope keys only though!
            // For reference: https://github.com/dotnet/runtime/issues/31849

            string MakeKeyFrom(string key) => _jsonSerializerOptions.DictionaryKeyPolicy.ConvertName(key);

            var envelope = new Dictionary<string, object>();
            envelope.Add(MakeKeyFrom("MessageId"), descriptor.MessageId);
            envelope.Add(MakeKeyFrom("Type"), descriptor.MessageType);

            foreach (var (key, value) in descriptor.MessageHeaders)
            {
                envelope.Add(MakeKeyFrom(key), value);
            }

            envelope.Add(MakeKeyFrom("Data"), descriptor.MessageData);

            return envelope;
        }
    }
}