using GameServer.DataEntities;
using Jurassic;
using Jurassic.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameServer.GameLogic.JSClasses
{
    public class CellWeapon : Cell
    {
        public override CellTypesEnum type
        {
            get { return CellTypesEnum.Weapon; }
        }

        [JSProperty(Name = "type", IsConfigurable = false)]
        public override int getIntType { get { return base.getIntType; } }

        [JSProperty(Name = "weaponId", IsConfigurable = false)]
        public int weaponId { get; set; }

        [JSProperty(Name = "health", IsConfigurable = false)]
        public int Health { get { return status.health; } }

        [JSProperty(Name = "energy", IsConfigurable = false)]
        public int Energy { get { return status.energy; } }

        [JSProperty(Name = "stepsToReady", IsConfigurable = false)]
        public int StepsToReady { get { return status.stepsToReady; } }

        [JSFunction(Name = "shoot")]
        public int Shoot(double x, double y)
        {
            if (!isYours) throw new Exception("trying to manipulate enemy cell");
            Ship enemyShip = this.parent.owner.enemyShip;
            if (enemyShip != null && status.stepsToReady == 0)
            {
                this.parent.owner.Events.Add(
                    new Event("shoot", new string[] { x.ToString(), y.ToString(), this.weaponId.ToString() }));

                enemyShip.owner.Events.Add(
                    new Event("shootIncoming", new string[] { x.ToString(), y.ToString(), this.weaponId.ToString() }));

                status.stepsToReady = 5;
            }

            return 0;
        }

        [JSFunction(Name = "power")]
        public override int Power(int energy)
        {
            return base.Power(energy);
        }

        public CellWeapon(Ship parent, int roomId)
            : base(parent, roomId)
        {
            weaponId = 0;
        }

        public override Cell copyCell()
        {
            return new CellWeapon(parent, roomId) { status = this.status, isYours = false };
        }
    }
}