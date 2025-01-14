// using System.Globalization;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Converters;
//
// namespace i18n;
//
// public class GlobalDateTimeConverter : DateTimeConverterBase
// {
//     private const string DefaultDateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK";
//
//     private DateTimeStyles _dateTimeStyles = DateTimeStyles.RoundtripKind;
//     private string? _dateTimeFormat;
//     private CultureInfo? _culture;
//
//     /// <summary>
//     /// Gets or sets the date time styles used when converting a date to and from JSON.
//     /// </summary>
//     /// <value>The date time styles used when converting a date to and from JSON.</value>
//     public DateTimeStyles DateTimeStyles
//     {
//         get => _dateTimeStyles;
//         set => _dateTimeStyles = value;
//     }
//
//     /// <summary>
//     /// Gets or sets the date time format used when converting a date to and from JSON.
//     /// </summary>
//     /// <value>The date time format used when converting a date to and from JSON.</value>
//     public string? DateTimeFormat
//     {
//         get => _dateTimeFormat ?? string.Empty;
//         set => _dateTimeFormat = (StringUtils.IsNullOrEmpty(value)) ? null : value;
//     }
//
//     /// <summary>
//     /// Gets or sets the culture used when converting a date to and from JSON.
//     /// </summary>
//     /// <value>The culture used when converting a date to and from JSON.</value>
//     public CultureInfo Culture
//     {
//         get => _culture ?? CultureInfo.CurrentCulture;
//         set => _culture = value;
//     }
//
//     /// <summary>
//     /// Writes the JSON representation of the object.
//     /// </summary>
//     /// <param name="writer">The <see cref="JsonWriter"/> to write to.</param>
//     /// <param name="value">The value.</param>
//     /// <param name="serializer">The calling serializer.</param>
//     public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
//     {
//         string text;
//
//         if (value is DateTime dateTime)
//         {
//             if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
//                 || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
//             {
//                 dateTime = dateTime.ToUniversalTime();
//             }
//
//             text = dateTime.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
//         }
// #if HAVE_DATE_TIME_OFFSET
//             else if (value is DateTimeOffset dateTimeOffset)
//             {
//                 if ((_dateTimeStyles & DateTimeStyles.AdjustToUniversal) == DateTimeStyles.AdjustToUniversal
//                     || (_dateTimeStyles & DateTimeStyles.AssumeUniversal) == DateTimeStyles.AssumeUniversal)
//                 {
//                     dateTimeOffset = dateTimeOffset.ToUniversalTime();
//                 }
//
//                 text = dateTimeOffset.ToString(_dateTimeFormat ?? DefaultDateTimeFormat, Culture);
//             }
// #endif
//         else
//         {
//             throw new JsonSerializationException("Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {0}.".FormatWith(CultureInfo.InvariantCulture, ReflectionUtils.GetObjectType(value)!));
//         }
//
//         writer.WriteValue(text);
//     }
//
//     /// <summary>
//     /// Reads the JSON representation of the object.
//     /// </summary>
//     /// <param name="reader">The <see cref="JsonReader"/> to read from.</param>
//     /// <param name="objectType">Type of the object.</param>
//     /// <param name="existingValue">The existing value of object being read.</param>
//     /// <param name="serializer">The calling serializer.</param>
//     /// <returns>The object value.</returns>
//     public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
//     {
//         bool nullable = ReflectionUtils.IsNullableType(objectType);
//         if (reader.TokenType == JsonToken.Null)
//         {
//             if (!nullable)
//             {
//                 throw JsonSerializationException.Create(reader, "Cannot convert null value to {0}.".FormatWith(CultureInfo.InvariantCulture, objectType));
//             }
//
//             return null;
//         }
//
// #if HAVE_DATE_TIME_OFFSET
//             Type t = (nullable)
//                 ? Nullable.GetUnderlyingType(objectType)!
//                 : objectType;
// #endif
//
//         if (reader.TokenType == JsonToken.Date)
//         {
// #if HAVE_DATE_TIME_OFFSET
//                 if (t == typeof(DateTimeOffset))
//                 {
//                     return (reader.Value is DateTimeOffset) ? reader.Value : new DateTimeOffset((DateTime)reader.Value!);
//                 }
//
//                 // converter is expected to return a DateTime
//                 if (reader.Value is DateTimeOffset offset)
//                 {
//                     return offset.DateTime;
//                 }
// #endif
//
//             return reader.Value;
//         }
//
//         if (reader.TokenType != JsonToken.String)
//         {
//             throw JsonSerializationException.Create(reader, "Unexpected token parsing date. Expected String, got {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
//         }
//
//         string? dateText = reader.Value?.ToString();
//
//         if (StringUtils.IsNullOrEmpty(dateText) && nullable)
//         {
//             return null;
//         }
//
//         MiscellaneousUtils.Assert(dateText != null);
//
// #if HAVE_DATE_TIME_OFFSET
//             if (t == typeof(DateTimeOffset))
//             {
//                 if (!StringUtils.IsNullOrEmpty(_dateTimeFormat))
//                 {
//                     return DateTimeOffset.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
//                 }
//                 else
//                 {
//                     return DateTimeOffset.Parse(dateText, Culture, _dateTimeStyles);
//                 }
//             }
// #endif
//
//         if (!StringUtils.IsNullOrEmpty(_dateTimeFormat))
//         {
//             return DateTime.ParseExact(dateText, _dateTimeFormat, Culture, _dateTimeStyles);
//         }
//         else
//         {
//             return DateTime.Parse(dateText, Culture, _dateTimeStyles);
//         }
//     }
// }