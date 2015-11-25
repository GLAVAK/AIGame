using GameServer.DataEntities;
using GameServer.GameLogic.JSClasses;
using Jurassic;
using Jurassic.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.GameLogic
{
    public class Radar : ObjectInstance
    {
        User owner;

        public Radar(User owner)
            : base(owner.engine)
        {
            this.owner = owner;

            this.PopulateFunctions();
        }

        [JSProperty(Name = "type", IsConfigurable = false)]
        public int type
        {
            get
            {
                if (owner.enemyShip != null) return 1;
                else if (owner.spaceStation != null) return 2;
                else return 0;
            }
        }

        [JSFunction(Name = "getCell")]
        public Cell getCell(int x, int y)
        {
            if (owner.enemyShip == null) return null;

            Cell a = owner.enemyShip.cells[y][x].copyCell();
            object asd = a.GetPropertyValue("health");
            return a;
        }

        [JSFunction(Name = "getHeight")]
        public int getHeight()
        {
            if (owner.enemyShip == null) return -1;

            return owner.enemyShip.cells[0].Length;
        }

        [JSFunction(Name = "getWidth")]
        public int getWidth()
        {
            if (owner.enemyShip == null) return -1;

            return owner.enemyShip.cells.Length;
        }
    }
}