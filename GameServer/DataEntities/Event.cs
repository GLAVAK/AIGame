using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.DataEntities
{
    public class Event
    {
        public string type;
        public IEnumerable<string> data;

        public Event(string type, IEnumerable<string> data)
        {
            this.type = type;
            this.data = data;
        }
    }
}
