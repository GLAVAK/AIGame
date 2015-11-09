using GameServer.DataEntities;
using Jurassic;
using Jurassic.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.GameLogic.JSClasses
{
    public class CellRepair : Cell
    {
        public override CellTypesEnum type
        {
            get { return CellTypesEnum.Repair; }
        }

        [JSProperty(Name = "type", IsConfigurable = false)]
        public override int getIntType { get { return base.getIntType; } }

        [JSProperty(Name = "weaponId", IsConfigurable = false)]
        public int weaponId { get; set; }

        [JSProperty(Name = "health", IsConfigurable = false)]
        public override int Health { get { return status.health; } }

        [JSProperty(Name = "energy", IsConfigurable = false)]
        public override int Energy { get { return status.energy; } }

        [JSProperty(Name = "stepsToReady", IsConfigurable = false)]
        public override int StepsToReady { get { return status.stepsToReady; } }

        [JSFunction(Name = "repair")]
        public int Repair(int x, int y)
        {
            if (!isYours) throw new Exception("trying to manipulate enemy cell");

            if (status.stepsToReady == 0 && this.parent.cells[y][x].Health < 10)
            {
                this.parent.cells[y][x].status.health++;

                status.stepsToReady = 5;
            }

            return 0;
        }

        [JSFunction(Name = "power")]
        public override int Power(int energy)
        {
            return base.Power(energy);
        }

        public CellRepair(Ship parent, int roomId)
            : base(parent, roomId) { }

        public override Cell copyCell()
        {
            return new CellRepair(parent, roomId) { status = this.status, isYours = false };
        }
    }
}