using Newtonsoft.Json;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

public class TimeOnlyConverter : Newtonsoft.Json.JsonConverter<TimeOnly>
{
        public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string value = (string)reader.Value;
            return TimeOnly.Parse(value);
        }

        public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
}

