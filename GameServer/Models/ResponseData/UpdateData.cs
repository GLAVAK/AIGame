using GameServer.DataEntities;
using System.Collections.Generic;

namespace GameServer.Models.ResponseData
{
    public class UpdateData
    {
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<IEnumerable<CellStatus>> ShipStatus { get; set; }
        public IEnumerable<string> Log { get; set; }

        public RadarType RadarType { get; set; }
        public object RadarData { get; set; }

        public bool IsDead { get; set; }
        public int Credits { get; set; }

        public UpdateData()
        {
            RadarType = ResponseData.RadarType.Nothing;
        }
    }

    public enum RadarType
    {
        Nothing = 0,
        EnemyShip = 1
    }
}