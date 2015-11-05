using GameServer.DataEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.JSONConverters
{
    public class ShipJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            var ship = (Ship)value;

            writer.WriteStartObject();

            writer.WritePropertyName("cells");
            serializer.Serialize(writer, ship.cells);

            writer.WritePropertyName("layoutOffset");
            serializer.Serialize(writer, ship.preset.LayoutOffset);

            writer.WritePropertyName("weapons");
            serializer.Serialize(writer, ship.preset.weapons);

            writer.WritePropertyName("isDead");
            serializer.Serialize(writer, ship.owner.IsDead);

            writer.WritePropertyName("owner");
            serializer.Serialize(writer, ship.owner.Login);

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Ship);
        }
    }
}