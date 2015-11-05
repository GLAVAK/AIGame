using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.DataEntities
{
    public class ShipPreset
    {
        public string ImageFilename { get; set; }
        public string LayoutFilename { get; set; }
        public double[] LayoutOffset { get; set; }

        public int[,] RoomIds { get; set; }
        public int[] DefaultLayout { get; set; }

        public Weapon[] weapons { get; set; }

        public Action<Ship> afterShipBuilt { get; set; }

        /// <summary>
        /// Get maximum roomId in ship preset
        /// </summary>
        /// <returns></returns>
        public int GetRoomIdsCount()
        {
            int maxId = 0;
            for (int i = 0; i < RoomIds.GetLength(0); i++)
            {
                for (int j = 0; j < RoomIds.GetLength(1); j++)
                {
                    if (RoomIds[i,j] > maxId) maxId = RoomIds[i,j];
                }
            }
            return maxId;
        }
    }
}