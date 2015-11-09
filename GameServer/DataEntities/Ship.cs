using GameServer.GameLogic.JSClasses;
using GameServer.JSONConverters;
using Newtonsoft.Json;
using System;

namespace GameServer.DataEntities
{
    [JsonConverter(typeof(ShipJsonConverter))]
    public class Ship
    {
        public User owner;

        public Cell[][] cells;
        public CellStatus[][] status;

        public ShipPreset preset;

        public Ship(User owner, int width, int height)
        {
            this.owner = owner;

            cells = new Cell[height][];
            status = new CellStatus[height][];
            for (int i = 0; i < height; i++)
            {
                cells[i] = new Cell[width];
                status[i] = new CellStatus[width];
            }
        }

        private int energyRemain = 10;
        public void makeStep()
        {
            // To every room make step only once
            for (int i = 0; i < cells.Length; i++)
                for (int j = 0; j < cells[i].Length; j++)
                    status[i][j].stepMade = false;

            for (int i = 0; i < cells.Length; i++)
                for (int j = 0; j < cells[i].Length; j++)
                    cells[i][j].makeStep();
        }

        public void useEnergy(int amount)
        {
            if (energyRemain < amount) throw new Exception("You ship have not enough energy");

            energyRemain -= amount;
        }

        public bool IsBroken()
        {
            for (int i = 0; i < cells.Length; i++)
                for (int j = 0; j < cells[i].Length; j++)
                    if (cells[i][j].Health > 0) return false;
            return true;
        }

        public void Repair()
        {
            for (int i = 0; i < cells.Length; i++)
                for (int j = 0; j < cells[i].Length; j++)
                    if (!(cells[i][j] is CellEmpty)) cells[i][j].status.health = 10;

            owner.IsDead = false;
        }
    }
}