using Jurassic.Library;
using Jurassic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameServer.DataEntities;

namespace GameServer.GameLogic.JSClasses
{
    public class CellEmpty : Cell
    {
        public override CellTypesEnum type
        {
            get { return CellTypesEnum.NoCell; }
        }

        [JSProperty(Name = "type", IsConfigurable = false)]
        public override int getIntType { get { return base.getIntType; } }

        [JSProperty(Name = "health", IsConfigurable = false)]
        public override int Health { get { return 0; } }

        [JSProperty(Name = "energy", IsConfigurable = false)]
        public override int Energy { get { return 0; } }

        [JSProperty(Name = "stepsToReady", IsConfigurable = false)]
        public override int StepsToReady { get { return 0; } }

        public CellEmpty(Ship parent)
            : base(parent, 0) { }

        public override Cell copyCell()
        {
            return new CellEmpty(parent) { status = this.status, isYours = false };
        }
    }
}