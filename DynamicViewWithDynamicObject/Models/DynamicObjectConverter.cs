using DynamicViewWithDynamicObject.ExtensiveMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DynamicViewWithDynamicObject.Models
{
    internal class DynamicObjectConverter : JsonConverter<CustomObject>
    {
        public override CustomObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var item = new CustomObject();
            while (reader.Read())
            {
                
            }
            return item;
        }

        public override void Write(Utf8JsonWriter writer, CustomObject value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();/*{*/
            foreach (var kvp in value.dictionary)
            {
                var type = kvp.GetPropertyType();
                if(type.Name == "Boolean")
                {
                    writer.WriteBoolean(kvp.Key, bool.Parse(kvp.Value.ToString()));
                }
                else if (type.Name == "Int32")
                {
                    writer.WriteNumber(kvp.Key, int.Parse(kvp.Value.ToString()));
                }
                else
                {
                    writer.WriteString(kvp.Key, kvp.Value.ToString());
                }
            }


            writer.WriteEndObject();/*}*/
        }
    }
}
