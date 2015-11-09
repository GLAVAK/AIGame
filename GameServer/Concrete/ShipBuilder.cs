using GameServer.Abstract;
using GameServer.DataEntities;
using GameServer.GameLogic.JSClasses;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace GameServer.Concrete
{
    public class ShipBuilder
    {
        /// <summary>
        /// Builds new Ship from preset, user, and cellTypes
        /// </summary>
        /// <param name="owner">User, who owns the Ship</param>
        /// <param name="preset">Ship preset</param>
        /// <param name="cellTypes">cellTypes[i] - type of room with id = i</param>
        /// <returns>Built Ship</returns>
        public Ship BuildShip(User owner, ShipPreset preset, IEnumerable<int> cellTypes)
        {
            Ship ship = new Ship(owner, preset.RoomIds.GetLength(1), preset.RoomIds.GetLength(0));
            ship.preset = preset;

            currentWeaponId = 0;
            assignedWeaponIds = new Dictionary<int, int>();

            for (int i = 0; i < ship.cells.Length; i++)
            {
                for (int j = 0; j < ship.cells[i].Length; j++)
                {
                    int roomId = preset.RoomIds[i, j];
                    ship.cells[i][j] = buildCell(ship, cellTypes.ToList()[roomId], roomId);
                    if (currentWeaponId >= preset.weapons.Length) currentWeaponId = 0;
                }
            }

            assignesStatuses = new Dictionary<int, CellStatus>();
            InitCellStatuses(ship, preset);

            if (preset.afterShipBuilt != null)
                preset.afterShipBuilt.Invoke(ship);
                    
            return ship;
        }

        // assignesStatuses[i] - status of room with id = i
        private Dictionary<int, CellStatus> assignesStatuses;

        /// <summary>
        /// Sets status field for every Cell in ship.cells
        /// Room with the same roomId should have same staus
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="preset"></param>
        private void InitCellStatuses(Ship ship, ShipPreset preset)
        {
            for (int i = 0; i < ship.cells.Length; i++)
            {
                for (int j = 0; j < ship.cells[i].Length; j++)
                {
                    int roomId = preset.RoomIds[i, j];

                    if (!assignesStatuses.ContainsKey(roomId))
                        assignesStatuses[roomId] = new CellStatus() { energy = 0, health = 10 };

                    assignesStatuses[roomId].cellsInRoomCount++;
                    ship.cells[i][j].status =
                        ship.status[i][j] = 
                        assignesStatuses[roomId];
                }
            }
        }

        private int currentWeaponId;
        // assignedWeaponIds[i] - weaponId for room with id = i
        private Dictionary<int, int> assignedWeaponIds;

        private Cell buildCell(Ship parent, int cellType, int roomId)
        {
            switch (cellType)
            {
                case 1:
                    return new Cell(parent, roomId);
                case 2:
                    if (!assignedWeaponIds.ContainsKey(roomId))
                        assignedWeaponIds[roomId] = currentWeaponId++;

                    return new CellWeapon(parent, roomId) { weaponId = assignedWeaponIds[roomId] };
                case 3:
                    return new CellEngine(parent, roomId);
                case 4:
                    return new CellRepair(parent, roomId);
                default:
                    return new CellEmpty(parent);
            }
        }
    }
}