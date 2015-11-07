using Jurassic.Library;
using Jurassic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameServer.DataEntities;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using GameServer.JSONConverters;

namespace GameServer.GameLogic.JSClasses
{
    [JsonConverter(typeof(CellJsonConverter))]
    public class Cell : ObjectInstance
    {
        protected Ship parent;
        public int roomId;
        public bool isYours = true;

        public CellStatus status;

        [JSProperty(Name = "health", IsConfigurable = false)]
        public int Health { get { return status.health; } }

        [JSProperty(Name = "energy", IsConfigurable = false)]
        public int Energy { get { return status.energy; } }

        [JSProperty(Name = "stepsToReady", IsConfigurable = false)]
        public int StepsToReady { get { return status.stepsToReady; } }

        [JSProperty(Name = "type", IsConfigurable = false)]
        public virtual int getIntType { get { return (int)this.type; } }

        public virtual CellTypesEnum type
        {
            get { return CellTypesEnum.NoSystem; }
        }

        [JSFunction(Name = "power")]
        public virtual int Power(int energy)
        {
            if (!isYours) throw new Exception("trying to manipulate enemy cell");
            int deltaEnergy = energy - status.energy;
            parent.useEnergy(deltaEnergy);
            status.energy = energy;
            return 1;
        }

        public Cell(Ship parent, int roomId)
            : base(parent.owner.engine)
        {
            this.parent = parent;
            this.roomId = roomId;

            this.PopulateFunctions();
        }

        public void makeStep()
        {
            if (!status.stepMade && status.stepsToReady > 0)
            {
                status.stepsToReady -= status.energy;
                if (status.stepsToReady < 0) status.stepsToReady = 0;
                status.stepMade = true;
            }
        }

        public virtual Cell copyCell()
        {
            return new Cell(parent, roomId) { status = this.status, isYours = false };
        }
    }
}