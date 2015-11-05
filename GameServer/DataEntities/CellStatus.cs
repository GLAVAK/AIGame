using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.DataEntities
{
    public class CellStatus
    {
        public int health { get; set; }
        public int energy { get; set; }
        public int stepsToReady { get; set; }

        public int cellsInRoomCount { get; set; }

        public bool stepMade { get; set; }
    }
}