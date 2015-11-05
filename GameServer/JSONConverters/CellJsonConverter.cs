using GameServer.GameLogic.JSClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.JSONConverters
{
    public class CellJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value,
            JsonSerializer serializer)
        {
            var cell = (Cell)value;

            writer.WriteStartObject();

            writer.WritePropertyName("roomId");
            serializer.Serialize(writer, cell.roomId);

            writer.WritePropertyName("type");
            serializer.Serialize(writer, cell.type);

            if (cell.status != null)
            {
                writer.WritePropertyName("health");
                serializer.Serialize(writer, cell.status.health);
                writer.WritePropertyName("energy");
                serializer.Serialize(writer, cell.status.energy);
                writer.WritePropertyName("stepsToReady");
                serializer.Serialize(writer, cell.status.stepsToReady);
            }

            if (cell is CellWeapon)
            {
                writer.WritePropertyName("weaponId");
                serializer.Serialize(writer, ((CellWeapon)cell).weaponId);
            }

            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Cell);
        }
    }
}