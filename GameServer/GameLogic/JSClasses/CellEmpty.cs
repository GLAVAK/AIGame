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
        [JSProperty(Name = "type", IsConfigurable = false)]
        public override CellTypesEnum type
        {
            get { return CellTypesEnum.NoCell; }
        }

        public CellEmpty(Ship parent)
            : base(parent, 0) { }
    }
}