using GameServer.Abstract;
using GameServer.GameLogic;
using GameServer.GameLogic.JSClasses;
using GameServer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Linq;

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

        public void Jump(IUserRepository repository)
        {
            owner.Events.Add(new Event("jump", new string[] { }));

            // Clear connection with previous enemy or station
            if (owner.enemyShip != null)
            {
                owner.enemyShip.owner.enemyShip = null;
                owner.enemyShip = null;
            }
            else if (owner.spaceStation != null)
            {
                owner.spaceStation = null;
            }

            // Found another enemy, with some chance
            User nextEnemy;
            Random r = new Random();
            if (r.NextDouble() <= 0.3)
            {
                nextEnemy = repository.Users.ToList()[r.Next(repository.Users.Count())];
                if (nextEnemy.enemyShip == null && nextEnemy.spaceStation == null && nextEnemy != owner)
                {
                    nextEnemy.enemyShip = this;
                    this.owner.enemyShip = nextEnemy.ship;
                }
            }
            // Else, with some chance, jump to the space station
            else if (r.NextDouble() <= 0.3)
            {
                // TODD: delegate space station creation to something else
                SpaceStation station = new SpaceStation();
                owner.spaceStation = station;
            }
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