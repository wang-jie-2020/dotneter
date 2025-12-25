using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace I18n.LocalizationExtensions.Samples;

public class DateTimeConverter : JsonConverter
{
    private readonly string _format;

    public DateTimeConverter(string format = "yyyy-MM-dd HH:mm:ss")
    {
        _format = format;
    }
    
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || 
               objectType == typeof(DateTime?);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        
        if (reader.TokenType == JsonToken.Date)
            return reader.Value;
        
        if (reader.TokenType == JsonToken.String)
        {
            if (DateTime.TryParseExact(
                    (string)reader.Value, 
                    _format, 
                    null, 
                    System.Globalization.DateTimeStyles.None, 
                    out DateTime result))
            {
                return result;
            }
        }
        
        throw new JsonSerializationException($"无效的日期格式，预期格式: {_format}");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value == null)
        {
            writer.WriteNull();
            return;
        }
        
        DateTime dateTime = (DateTime)value;
        
        DateTime timeZoneDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTime, "Europe/Paris");
        writer.WriteValue(timeZoneDateTime.ToString(_format));
    }
}