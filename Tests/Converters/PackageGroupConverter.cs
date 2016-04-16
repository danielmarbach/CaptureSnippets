using System;
using CaptureSnippets;
using Newtonsoft.Json;

class PackageGroupConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteValue("null");
            return;
        }

        var snippetGroup = (PackageGroup)value;
        writer.WriteStartObject();
        writer.WritePropertyName("Package");
        writer.WriteValue(snippetGroup.Package);
        writer.WritePropertyName("versions");
        serializer.Serialize(writer, snippetGroup.Versions);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(PackageGroup);
    }
}