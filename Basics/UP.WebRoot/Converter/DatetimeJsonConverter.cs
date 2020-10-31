// /**************************************************************************
// *
// *   创 建 者    ： S1mple  
// *   文 件 名 字   ： DatetimeJsonConverter.cs
// *   所 属 项 目    ： UP.Root
// *   创建日期    ： 2020/02/26 14:00
// *   功能描述    ：
// *   使用说明    ：
// *   =================================
// ***************************************************************************/

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace UP.WebRoot.Converter
{
    public class DatetimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                if (DateTime.TryParse(reader.GetString(), out DateTime date))
                    return date;
            }
            return reader.GetDateTime();
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}