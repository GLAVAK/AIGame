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

        [JSProperty(Name = "weaponId", IsConfigurable = false)]
        public int weaponId { get; set; }

        [JSFunction(Name = "shoot")]
        public int Shoot(double x, double y)
        {
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
    }
}